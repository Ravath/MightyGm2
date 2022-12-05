using Godot;
using MightyGm2.Engine.Control;
using MightyGm2.Engine.Database;
using System;
using System.Collections.Generic;
using System.Linq;

public class TagList : VBoxContainer
{
	[Signal]
	public delegate void TagActivated(GodotTag tag);

	private DatabaseCtrl data;

	private LineEdit tagFilter;
	private ItemList tagList;
	private PopupMenu tagListMenu;
	private ContextMenuItems tagListMenuItems = new ContextMenuItems();

	public string TagFilterText { get; set; }
	private IEnumerable<Tag> _tags;
	private bool _refreshTagListFlag = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		data = ApplicationControl.Control.Database;

		tagFilter = GetNode<LineEdit>("LineEdit");
		tagList = GetNode<ItemList>("TagList");
		tagListMenu = GetNode<PopupMenu>("TagList/TagPopupMenu");

		// Setup Tag Context Menu
		tagListMenuItems.AddMenuItem(
			new ContextMenuItem() {
				Name = "Add Tag",
				Used = true,
				DoAction = TagMenu_NewTag,
			}, new ContextMenuItem() {
				Name = "Rename",
				IsUsedCondition = TagMenu_OnlyOneSelected,
				DoAction = TagMenu_RenameTag,
			}, new ContextMenuItem() {
				Name = "Merge",
				IsUsedCondition = TagMenu_MoreThanOneSelected,
				DoAction = TagMenu_MergeTag,
			}, new ContextMenuItem() {
				Name = "Remove",
				IsUsedCondition = TagMenu_AtLeastOneSelected,
				DoAction = TagMenu_RemoveTags,
			});

		// connect to tag events
		data.OnTagAdded += Data_OnTagDbChanged;
		data.OnTagRemoved += Data_OnTagDbChanged;
		data.OnTagChangedName += Data_OnTagDbChanged;

		_refreshTagList();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		data.OnTagAdded -= Data_OnTagDbChanged;
		data.OnTagRemoved -= Data_OnTagDbChanged;
		data.OnTagChangedName -= Data_OnTagDbChanged;
	}

	public override void _Process(float delta)
	{
		if (_refreshTagListFlag && Visible)
		{
			_refreshTagListFlag = false;
			_refreshTagList();
		}
	}

	public IEnumerable<Tag> SelectedTags
	{
		get
		{
			int[] selTagIndex = tagList.GetSelectedItems();
			foreach (var item in selTagIndex)
			{
				yield return _tags.ElementAt(item);
			}
		}
	}

	private void _refreshTagList()
	{
		tagList.Clear();

		if (string.IsNullOrWhiteSpace(TagFilterText))
		{
			_tags = data.DB.Tags.OrderBy(t => t.Id);
		}
		else
		{
			_tags = data.FindTagSuggestion(TagFilterText);
		}

		foreach (var item in _tags)
		{
			tagList.AddItem(item.Name);
		}
	}

	#region MiscEvents
	private void Data_OnTagDbChanged(params Tag[] newTags)
	{
		_refreshTagListFlag = true;
	}

	private void _on_LineEdit_text_changed(String new_text)
	{
		TagFilterText = new_text;
		_refreshTagListFlag = true;
	}

	private void _on_TagList_item_activated(int index)
	{
		Tag selTag = _tags.ElementAt(index);

		EmitSignal(nameof(TagActivated), new GodotTag(selTag));
	}
	#endregion

	#region Menu Actions
	
	/// <summary>
	/// Checks the user enters a valid tag name.
	/// - not empty
	/// - not already exists in DB
	/// </summary>
	/// <param name="input">Tag name to check</param>
	/// <param name="errorMessage">Message to feedback user.</param>
	/// <returns>True is tag can be used.</returns>
	public bool IsNewTagNameStringChecker(string input, ref string errorMessage)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			errorMessage = "Please enter some text...";
			return false;
		}
		errorMessage = "This tag name already exists...";
		var findTag = data.DB.Tags.FirstOrDefault(t => t.Name == input);
		return findTag == null;
	}

	/// <summary>
	/// Add a new Tag to the DB
	/// </summary>
	private void TagMenu_NewTag()
	{
		this.AskUser("Enter a new tag Name", IsNewTagNameStringChecker,
			(u) =>
			{
				Tag newTag = new Tag()
				{
					Name = u.InputText
				};
				data.AddTags(newTag);
			});
	}

	/// <summary>
	/// Rename the selected Tag
	/// </summary>
	private void TagMenu_RenameTag()
	{
		int[] selectedTags = tagList.GetSelectedItems();
		Tag t = _tags.ElementAt(selectedTags[0]);

		var popup = this.AskText("Rename Tag '" + t.Name + "'",
			(u) =>
			{
				data.ChangeTagName(t, u.InputText);

			});
		popup.OnCheck = IsNewTagNameStringChecker;
		popup.InputText = t.Name;
	}

	/// <summary>
	/// Mege the selected tags into a new one.
	/// The name is asked to the user.
	/// Resource files are taged with the new tag.
	/// </summary>
	private void TagMenu_MergeTag()
	{
		Tag[] mtags = SelectedTags.ToArray();
		UserInputPopup uip = this.AskText("New merged Tag name",
			(u) =>
			{
				data.ChangeTagName(mtags[0], u.InputText);
				data.MergeTags(mtags.Skip(1).ToArray(), mtags[0]);
			});
		//TODO should check the name not already exists, but with the merged tags excepted
		uip.InputText = mtags[0].Name;
	}

	private void TagMenu_RemoveTags()
	{
		var rtags = SelectedTags.ToArray();
		data.RemoveTags(rtags);
	} 
	#endregion

	#region Menu Conditions
	/// <summary>
	/// Compute a condition for the popup menu list.
	/// </summary>
	/// <returns>True if at least two items are selected in the tag list.</returns>
	private bool TagMenu_MoreThanOneSelected()
	{
		int[] selItems = tagList.GetSelectedItems();
		return selItems.Length > 1;
	}

	/// <summary>
	/// Compute a condition for the popup menu list.
	/// </summary>
	/// <returns>True if at least one item is selected in the tag list.</returns>
	private bool TagMenu_AtLeastOneSelected()
	{
		int[] selItems = tagList.GetSelectedItems();
		return selItems.Length >= 1;
	}

	/// <summary>
	/// Compute a condition for the popup menu list.
	/// </summary>
	/// <returns>True if at ony one item is selected in the tag list.</returns>
	private bool TagMenu_OnlyOneSelected()
	{
		int[] selItems = tagList.GetSelectedItems();
		return selItems.Length == 1;
	}
	#endregion

	#region Menu Events management
	private void _on_TagList_rmb_clicked(Vector2 at_position)
	{
		tagListMenuItems.OpenMenu(tagListMenu);
	}

	private void _on_TagList_item_rmb_selected(int index, Vector2 at_position)
	{
		tagListMenuItems.OpenMenu(tagListMenu);
	}

	private void _on_TagPopupMenu_focus_exited()
	{
		tagListMenu.Visible = false;
	}

	private void _on_TagPopupMenu_index_pressed(int index)
	{
		tagListMenuItems.DoAction(index);
	}
	#endregion
}
