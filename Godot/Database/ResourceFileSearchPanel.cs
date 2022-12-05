using Godot;
using MightyGm2.Engine.Control;
using MightyGm2.Engine.Database;
using System;
using System.Collections.Generic;
using System.Linq;

public class ResourceFileSearchPanel : Control
{
	[Signal]
	public delegate void ResourceSelected(GodotResource file);
	[Signal]
	public delegate void ResourceActivated(GodotResource file);

	/// <summary>
	/// Access to the DataBase Controler
	/// </summary>
	private DatabaseCtrl data;

	// Nodes
	private LineEdit filterTextEdit;
	private HBoxContainer tagContainer;
	private ItemList resultList;
	private PopupMenu resultListMenu;
	private ContextMenuItems resultListMenuItems = new ContextMenuItems();
	private PackedScene tagScene;
	private ItemList autocompletionList;
	private PopupPanel tagPickerPanel;

	// Filter buttons
	private Button MiscButton;
	private Button SoundButton;
	private Button PictureButton;
	private Button TextButton;
	private Button PdfButton;
	private Button VideoButton;
	private Button ArchiveButton;

	/// <summary>
	/// Timer used to delay the research update when typing text in the research bar.
	/// </summary>
	private Timer researchTimer;
	/// <summary>
	/// Turnaround to avoid spaming the "RessourceSelected" event
	/// when using multiple selection in the reasearch results (typicaly 'Maj+Click' will spam eveny selected item).
	/// </summary>
	private Timer selectionTimer;

	/// <summary>
	/// The tags to research.
	/// </summary>
	private List<Tag> _researchTags = new List<Tag>();
	/// <summary>
	/// The tags to exclude from reasearch
	/// </summary>
	private List<Tag> _excludeTags = new List<Tag>();
	/// <summary>
	/// The research results.
	/// </summary>
	private List<ResourceFile> _searchResults = new List<ResourceFile>();
	/// <summary>
	/// The tags suggested by autocompletion.
	/// </summary>
	private IEnumerable<Tag> suggestedTags = null;

	private bool _updateSearchResultFlag;

	/// <summary>
	/// Index of the last selected item.
	/// </summary>
	public int LastSelectedIndex { get; private set; }
	/// <summary>
	/// Number of found item in the search.
	/// </summary>
	public int ResultCount { get => _searchResults.Count; }
	public ResourceFile GetResultItem(int at) { return _searchResults.ElementAt(at); }
	public IEnumerable<ResourceFile> Results { get => _searchResults; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		data = ApplicationControl.Control.Database;

		filterTextEdit = GetNode<LineEdit>("VBoxContainer/FilterTextEdit");
		tagContainer = GetNode<HBoxContainer>("VBoxContainer/TagContainer");
		resultList = GetNode<ItemList>("VBoxContainer/ResultList");
		resultListMenu = GetNode<PopupMenu>("VBoxContainer/ResultList/ResultPopupMenu");
		tagScene = GD.Load<PackedScene>("res://Godot/Database/TagPanel.tscn");
		autocompletionList = GetNode<ItemList>("AutocompletionList");
		tagPickerPanel = GetNode<PopupPanel>("TagPickerPanel");

		const string buttonContainerPath = "VBoxContainer/HBoxContainer/";
		MiscButton = GetNode<Button>(buttonContainerPath + "MiscButton");
		SoundButton = GetNode<Button>(buttonContainerPath + "SoundButton");
		PictureButton = GetNode<Button>(buttonContainerPath + "PictureButton");
		TextButton = GetNode<Button>(buttonContainerPath + "TextButton");
		PdfButton = GetNode<Button>(buttonContainerPath + "PdfButton");
		VideoButton = GetNode<Button>(buttonContainerPath + "VideoButton");
		ArchiveButton = GetNode<Button>(buttonContainerPath + "ArchiveButton");

		researchTimer = GetNode<Timer>("VBoxContainer/TextFilterTimer");
		selectionTimer = GetNode<Timer>("VBoxContainer/SelectionTimer");

		// Since we use a lot of arrows and tabs, make it stay still.
		filterTextEdit.FocusPrevious = filterTextEdit.GetPath();
		filterTextEdit.FocusNext = filterTextEdit.GetPath();
		filterTextEdit.FocusNeighbourBottom = filterTextEdit.GetPath();
		filterTextEdit.FocusNeighbourTop = filterTextEdit.GetPath();

		// Setup resource file Context Menu
		resultListMenuItems.AddMenuItem(
			new ContextMenuItem() {
				IsUsedCondition = ResultSearch_AtLeastOneSelected,
				Name = "Add Tag",
				DoAction = ContextMenuOnClick_AddTag,
			}, new ContextMenuItem() {
				IsUsedCondition = ResultSearch_AtLeastOneSelected,
				Name = "Tag Management",
				DoAction = ContextMenuOnClick_ManageTags,
			}, new ContextMenuItem() {
				IsUsedCondition = ResultSearch_OnlyOneSelected,
				Name = "Open in Explorer",
				DoAction = ContextMenuOnClick_OpenInFolder,
		});
		
		data.OnTagRemoved += Data_OnTagRemoved;
		data.OnTagChangedName += Data_OnTagChangedName;

		// Update the typeFile filters
		RefreshTypeFilters();
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		if(_updateSearchResultFlag && Visible)
		{
			_updateSearchResultFlag = false;
			_updateSearchResult();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		data.OnTagRemoved -= Data_OnTagRemoved;
		data.OnTagChangedName -= Data_OnTagChangedName;
	}

	/// <summary>
	/// Synchonize the type filters of the research with the button states.
	/// </summary>
	private void RefreshTypeFilters()
	{
		data.GetMisc = MiscButton.Pressed;
		data.GetSound = SoundButton.Pressed;
		data.GetPicture = PictureButton.Pressed;
		data.GetText = TextButton.Pressed;
		data.GetPdf = PdfButton.Pressed;
		data.GetVideo = VideoButton.Pressed;
		data.GetArchive = ArchiveButton.Pressed;
	}

	/// <summary>
	/// When any typeButton filter is toggled, subsequently update the search criteria.
	/// </summary>
	/// <param name="button_pressed"></param>
	private void _on_FilterButton_toggled(bool button_pressed)
	{
		RefreshTypeFilters();
		RefreshSearchResult();
	}

	/// <summary>
	/// Refresh the research and update the display list.
	/// </summary>
	public void RefreshSearchResult()
	{
		_updateSearchResultFlag = true;
	}

	private void _updateSearchResult()
	{
		resultList.Clear();
		_searchResults.Clear();

		// Manage spaces as different search words
		IEnumerable<string> filterText = filterTextEdit.Text.Split(' ')
			.Where(s=>s.Length>=3);

		if (filterText.Count() > 0 || _researchTags.Count > 0)
		{
			// Delegate Research to controler
			var res = data.GetResearch(
				filterText,
				 _researchTags.Select(t => t.Id),
				 _excludeTags.Select(t => t.Id)
				);

			// Display result
			foreach (var file in res)
			{
				resultList.AddItem(file.Name);
				_searchResults.Add(file);
			}
		}
	}

	#region Tag Manipulation
	/// <summary>
	/// Add a tag to the filter.
	/// Will be displayed as a TagPanel under the text filter.
	/// </summary>
	/// <param name="newTag">Tag to add.</param>
	public void AddTag(Tag newTag)
	{
		// Don't add if already in lists
		if (_researchTags.Select(rt => rt.Name).Contains(newTag.Name)) return;
		if (_excludeTags.Select(rt => rt.Name).Contains(newTag.Name)) return;

		TagPanel tagPan = (TagPanel)tagScene.Instance();
		tagPan.Tag = newTag;
		tagPan.Connect(nameof(TagPanel.RemoveTag), this, nameof(RemoveTagPanel));
		tagPan.Connect(nameof(TagPanel.ExcludeTag), this, nameof(ExcludeTagPanel));

		_researchTags.Add(newTag);
		tagContainer.AddChild(tagPan);

		RefreshSearchResult();
	}

	/// <summary>
	/// Switch the tag : if is a wanted tag, swap as excluded, and vice versa.
	/// </summary>
	/// <param name="tagPanel">Panel of the tag to switch.</param>
	private void ExcludeTagPanel(TagPanel tagPanel)
	{
		if (tagPanel.Pressed)
		{
			_researchTags.Remove(tagPanel.Tag);
			_excludeTags.Add(tagPanel.Tag);
		}
		else
		{
			_excludeTags.Remove(tagPanel.Tag);
			_researchTags.Add(tagPanel.Tag);
		}

		RefreshSearchResult();
	}

	/// <summary>
	/// Remove the tag from the resarch list criteria.
	/// </summary>
	/// <param name="tagPanel">Panel of the tag to remove.</param>
	private void RemoveTagPanel(TagPanel tagPanel)
	{
		if (tagPanel.Pressed)
		{
			_excludeTags.Remove(tagPanel.Tag);
		}
		else
		{
			_researchTags.Remove(tagPanel.Tag);
		}
		tagPanel.QueueFree();


		RefreshSearchResult();
	}

	/// <summary>
	/// Get the Tag panel associated with the given tag.
	/// </summary>
	/// <param name="tag">Tag related to the researched panel.</param>
	/// <returns>Null if not found.</returns>
	public TagPanel GetTagPanel(Tag tag)
	{
		return tagContainer.GetChildren().OfType<TagPanel>()
			.SingleOrDefault(tp => tp.Tag.Id == tag.Id);
	}

	/// <summary>
	/// Remove the given tags from the research filters.
	/// Checks if the tags are actually used, and
	/// refresh the research if they are.
	/// </summary>
	/// <param name="removedTags">Tags to remove.</param>
	/// <returns>True if some panelTags have been removed.</returns>
	public bool RemoveSearchTags(params Tag[] removedTags)
	{
		bool changed = false;
		foreach (Tag t in removedTags)
		{
			// Find the relative panelTag
			TagPanel relTagpanel = GetTagPanel(t);
			//If there is one : remove
			if (relTagpanel != null)
			{
				changed = true;
				tagContainer.RemoveChild(relTagpanel);
				(relTagpanel.Pressed ? _excludeTags : _researchTags).Remove(t);
			}
		}
		// Update search if needed
		if(changed)
			_updateSearchResultFlag = true;
		return changed;
	}
	#endregion

	#region Text Filter update
	/// <summary>
	/// When a tag is selected from the autocompletion list,
	/// add it to the tag list and remove corresponding word from text research bar.
	/// </summary>
	/// <param name="index"></param>
	private void _on_AutocompletionList_item_activated(int index)
	{
		AddTagFromLastWord(index);
	}

	/// <summary>
	/// Remove the last word in the text research bar.
	/// Used when a tag is added from the text research bar.
	/// </summary>
	private void RemoveLastWordFromResearchBar()
	{
		IEnumerable<string> filterText = filterTextEdit.Text.Split(' ');
		var last = filterText.Last();

		string newString = "";
		foreach (var item in filterText)
		{
			if (item == last) continue;
			newString += item + " ";
		}

		// Tidy searchbar
		filterTextEdit.Text = newString;
		filterTextEdit.CaretPosition = newString.Length;
		autocompletionList.Visible = false;
	}

	/// <summary>
	/// Hide autocompletion tag list if lose focus
	/// </summary>
	private void _on_AutocompletionList_mouse_exited()
	{
		autocompletionList.Visible = false;
	}

	/// <summary>
	/// Process the use of non-characters keys from the text research bar.
	/// </summary>
	/// <param name="event"></param>
	private void _on_FilterTextEdit_gui_input(object @event)
	{
		if (@event is InputEventKey eventKey && eventKey.Pressed)
		{
			if (eventKey.Scancode == (int)KeyList.Escape)
				// Hide autocompletion list
				autocompletionList.Visible = false;
			else if (autocompletionList.Visible == true)
			{
				int selIndex = autocompletionList.IsAnythingSelected() ?
					autocompletionList.GetSelectedItems()[0] : 0;
				if (eventKey.Scancode == (int)KeyList.Space
					&& Input.IsKeyPressed((int)KeyList.Control))
				{
					// Auto-add new tag
					AddTagFromLastWord(selIndex);
				}
				else if(eventKey.Scancode == (int)KeyList.Up)
				{
					// Previous item in autocompletion list
					filterTextEdit.CaretPosition = filterTextEdit.Text.Length;
					autocompletionList.Select((selIndex + suggestedTags.Count() -1) % suggestedTags.Count());
				}
				else if (eventKey.Scancode == (int)KeyList.Down)
				{
					// Next item in autocompletion list
					filterTextEdit.CaretPosition = filterTextEdit.Text.Length;
					autocompletionList.Select((selIndex + 1) % suggestedTags.Count());
				}
			}
		}
	}

	/// <summary>
	/// Add the given tag from the autocompletion list to the tag list
	/// and remove corresponding word from text research bar.
	/// </summary>
	/// <param name="tagIndex">Tag Index in the autocompletion list.</param>
	private void AddTagFromLastWord(int tagIndex)
	{
		Tag selectedTag = suggestedTags.ElementAt(tagIndex);
		RemoveLastWordFromResearchBar();
		AddTag(selectedTag);//research result refreshed in 'AddTag'
	}

	/// <summary>
	/// When text entered in the research filter, start selay before updateing the search, to avoid unecessary over processing data.
	/// said differently : the search update is delayed some time after the last text edit.
	/// </summary>
	private void _on_FilterTextEdit_text_changed(String new_text)
	{
		// Text format
		new_text = new_text.Replace("  ", " ");
		filterTextEdit.Text = new_text;
		filterTextEdit.CaretPosition = new_text.Length;

		if(new_text.Length > 0)
		{
			// Get last character to decide what to do
			char lastLetter = filterTextEdit.Text.Last();
			if (lastLetter == ' ')
			{
				// prepare for a new word
				autocompletionList.Visible = false;
			}
			else
			{
				// Use last word for tag autocompletion

				// Manage spaces as different search words
				IEnumerable<string> filterText = filterTextEdit.Text.Split(' ');
				
				// Get suggestion from last word
				string potentialTag = filterText.Last();
				suggestedTags = data.FindTagSuggestion(potentialTag);

				// Update tag list
				autocompletionList.Clear();
				autocompletionList.Visible = suggestedTags.Count() != 0;
				autocompletionList.SetPosition(filterTextEdit.RectPosition + new Vector2(0, filterTextEdit.RectPosition.Length()));

				foreach (var item in suggestedTags)
				{
					autocompletionList.AddItem(item.Name);
				}

			}
		}

		// Update research result after a delay
		researchTimer.Start();
	}

	/// <summary>
	/// At end of timer, do the actual search.
	/// </summary>
	private void _on_TextFilterTimer_timeout()
	{
		RefreshSearchResult();
	} 
	#endregion

	#region Result List selection
	/// <summary>
	/// When items ar eselected in result list, starts a timer before sending selection signal.
	/// This is done to prevent the multi-selection events to flood the process.
	/// </summary>
	/// <param name="index"></param>
	/// <param name="selected"></param>
	private void _on_ResultList_multi_selected(int index, bool selected)
	{
		LastSelectedIndex = index;
		selectionTimer.Start();
	}

	/// <summary>
	/// At the end of timer, send the selection event with the index of the last selected item.
	/// </summary>
	private void _on_SelectionTimer_timeout()
	{
		EmitSignal(nameof(ResourceSelected), new GodotResource(_searchResults.ElementAt(LastSelectedIndex)));
	}

	/// <summary>
	/// Forward the Activated Item Event. 
	/// </summary>
	/// <param name="index"></param>
	private void _on_ResultList_item_activated(int index)
	{
		EmitSignal(nameof(ResourceActivated), new GodotResource(_searchResults.ElementAt(LastSelectedIndex)));
	}
	#endregion

	#region Context Menu Management
	/// <summary>
	/// Display ResultList Right-Click Menu.
	/// </summary>
	/// <param name="at_position"></param>
	private void _on_ResultList_rmb_clicked(Vector2 at_position)
	{
		resultListMenuItems.OpenMenu(resultListMenu);
	}

	/// <summary>
	/// Display ResultList Right-Click Menu.
	/// </summary>
	/// <param name="index"></param>
	/// <param name="at_position"></param>
	private void _on_ResultList_item_rmb_selected(int index, Vector2 at_position)
	{
		resultListMenuItems.OpenMenu(resultListMenu);
	}

	/// <summary>
	/// Hide ResultList Right-Click Menu.
	/// </summary>
	private void _on_ResultPopupMenu_focus_exited()
	{
		resultListMenu.Visible = false;
	}

	/// <summary>
	/// Manage ResultList Right-Click Menu:
	/// Process the selected action on the selected items.
	/// </summary>
	/// <param name="index"></param>
	private void _on_ResultPopupMenu_index_pressed(int index)
	{
		resultListMenuItems.DoAction(index);
	}

	private void ContextMenuOnClick_AddTag()
	{
		tagPickerPanel.PopupCentered();
	}

	#region TagPicker
	private void _on_TagPickerList_TagActivated(GodotTag tag)
	{
		tagPickerPanel.Hide();

		int[] selItems = resultList.GetSelectedItems();
		foreach (var item in selItems)
		{
			ResourceFile rf = _searchResults.ElementAt(item);
			rf.AddTagIfNotAlreadyHave(tag.Tag);
		}
		data.DB.SaveChanges();
	}

	private void _on_TagPickerPanel_gui_input(object @event)
	{
		if (@event is InputEventKey eventKey
			&& eventKey.Pressed
			&& eventKey.Scancode == (int)KeyList.Escape)
		{
			tagPickerPanel.Hide();
		}
	}
	#endregion

	private void ContextMenuOnClick_ManageTags()
	{
		int[] selItems = resultList.GetSelectedItems();
		List<ResourceFile> sel = new List<ResourceFile>();
		foreach (var item in selItems)
		{
			sel.Add(_searchResults.ElementAt(item));
		}
		Godot.GD.Print("ResourceFile Action ManageTags : ", string.Concat(sel.Select(tre=>tre.Name+", ")));
		//TODO manage tags
	}

	/// <summary>
	/// Open the selected file in the windows file explorer.
	/// </summary>
	private void ContextMenuOnClick_OpenInFolder()
	{
		int[] selItems = resultList.GetSelectedItems();
		ResourceFile resultFile = _searchResults.ElementAt(selItems[0]);
		if (resultFile.Info.Exists)
		{
			System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", resultFile.Info.FullName));
		}
	}

	/// <summary>
	/// Compute a condition for the popup menu list.
	/// </summary>
	/// <returns>True if at least one item is selected in the search result list.</returns>
	private bool ResultSearch_AtLeastOneSelected()
	{
		int[] selItems = resultList.GetSelectedItems();
		return selItems.Length >= 1;
	}

	/// <summary>
	/// Compute a condition for the popup menu list.
	/// </summary>
	/// <returns>True if at ony one item is selected in the search result list.</returns>
	private bool ResultSearch_OnlyOneSelected()
	{
		int[] selItems = resultList.GetSelectedItems();
		return selItems.Length == 1;
	}
	#endregion

	#region DB Tag Update management
	private void Data_OnTagChangedName(params Tag[] changedTags)
	{
		foreach (var item in changedTags)
		{
			// Find & Update the relative panelTag if exists
			TagPanel relTagpanel = GetTagPanel(item);
			if (relTagpanel != null)
			{
				relTagpanel.Text = item.Name;
			}
		}
	}

	private void Data_OnTagRemoved(params Tag[] removedTags)
	{
		RemoveSearchTags(removedTags);
	}
	#endregion

}
