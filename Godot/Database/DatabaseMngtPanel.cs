using Godot;
using Microsoft.EntityFrameworkCore;
using MightyGm2.Engine.Control;
using MightyGm2.Engine.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DatabaseMngtPanel : Control
{
	private DatabaseCtrl data;

	private ItemList dbList;
	private PopupMenu dbListMenu;
	private AcceptDialog dbImportPopup;
	private DatabaseImportPanel dbImportPanel;
	private ConfirmationDialog confirmationDialog;

	public TagList tagList;

	private ResourceFileSearchPanel searchPanel;
	private FilePropertiesPanel propertiesPanel;
	private PreviewPanel previewPanel;
	private AudioPanel audioPanel;
	private TrackControl _currentAudio;
	private int _currentTrackIndex;
	private ResourceFile _currentTrack;

	private IEnumerable<ResourceFolder> _folders;

	private ContextMenuItems dbListMenuItems = new ContextMenuItems();

	[Export]
	private readonly Texture activeDb;
	[Export]
	private readonly Texture inactiveDb;

	public IEnumerable<ResourceFolder> SelectedDBs
	{
		get
		{
			int[] selDbIndex = dbList.GetSelectedItems();
			foreach (var item in selDbIndex)
			{
				yield return _folders.ElementAt(item);
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		data = ApplicationControl.Control.Database;

		const string dbPath = "HBoxContainer/TabContainer/DB/";
		dbList = GetNode<ItemList>(dbPath + "DatabaseList");
		dbListMenu = GetNode<PopupMenu>(dbPath + "DatabaseList/DbPopupMenu");

		tagList = GetNode<TagList>("HBoxContainer/TabContainer/TagList");

		searchPanel = GetNode<ResourceFileSearchPanel>("HBoxContainer/SearchPanel");
		audioPanel = GetNode<AudioPanel>("HBoxContainer/VBoxContainer/AudioPanel");
		previewPanel = GetNode<PreviewPanel>("HBoxContainer/VBoxContainer/PreviewPanel");
		propertiesPanel = GetNode<FilePropertiesPanel>("HBoxContainer/VBoxContainer/PropertiesPanel");

		dbImportPopup = GetNode<AcceptDialog>("DbImportDialog");
		dbImportPanel = GetNode<DatabaseImportPanel>("DbImportDialog/DatabaseImportPanel");

		confirmationDialog = GetNode<ConfirmationDialog>("DefaultConfirmationDialog");

		// Setup DB Context Menu
		dbListMenuItems.AddMenuItem(
			new ContextMenuItem() {
				Name = "Set Used",
				IsUsedCondition = DbMenu_AtLeastOneSelected,
				DoAction = DbMenu_activateDb,
			}, new ContextMenuItem() {
				Name = "Set Unused",
				IsUsedCondition = DbMenu_AtLeastOneSelected,
				DoAction = DbMenu_inactivateDb,
			}, new ContextMenuItem() {
				Name = "Refresh",
				IsUsedCondition = DbMenu_AtLeastOneSelected,
				DoAction = DbMenu_RefreshDb,
			}, new ContextMenuItem() {
				Name = "Show Properties",
				IsUsedCondition = DbMenu_OnlyOneSelected,
				DoAction = DbMenu_genericAction,
			}, new ContextMenuItem() {
				Name = "Rename DB",
				IsUsedCondition = DbMenu_OnlyOneSelected,
				DoAction = DbMenu_RenameDb,
			}, new ContextMenuItem() {
				Name = "Remove DB",
				IsUsedCondition = DbMenu_OnlyOneSelected,
				DoAction = DbMenu_RemoveDb,
		});

		RefreshDbList();
	}

	#region DbManagement
	private void RefreshDbList()
	{
		dbList.Clear();
		_folders = data.DB.Folders.OrderBy(d=>d.Id);
		foreach (var item in _folders)
		{
			dbList.AddItem(item.Name,
				item.IsActive ? activeDb : inactiveDb);
		}
	}

	/// <summary>
	/// When clicks on "Add New DB"
	/// Opens the import DB popup.
	/// </summary>
	private void _on_AddNewDbButton_pressed()
	{
		dbImportPanel.Clear();
		dbImportPopup.Visible = true;
	}

	/// <summary>
	/// When importation confirmed from the import DB popup
	/// import the DB.
	/// </summary>
	private void _on_DbImportDialog_confirmed()
	{
		dbImportPanel.ImportDb();
		dbImportPanel.Clear();
		RefreshDbList();
	}

	/// <summary>
	/// When Right Click on the Database list and one DB selected,
	/// show a context menu.
	/// </summary>
	/// <param name="index"></param>
	/// <param name="at_position"></param>
	private void _on_DatabaseList_item_rmb_selected(int index, Vector2 at_position)
	{
		dbListMenuItems.OpenMenu(dbListMenu);
	}

	private void _on_DbPopupMenu_focus_exited()
	{
		dbListMenu.Visible = false;
	}

	private void _on_DbPopupMenu_index_pressed(int index)
	{
		dbListMenuItems.DoAction(index);
	}

	private void DbMenu_genericAction()
	{
		Godot.GD.Print("Db Generic action : ", string.Concat(SelectedDBs.Select(tre => tre.Name + ", ")));
		//TODO DB actions
	}

	private void DbMenu_activateDb()
	{
		foreach (var item in SelectedDBs)
		{
			item.IsActive = true;
		}
		data.DB.SaveChanges();
		RefreshDbList();
		searchPanel.RefreshSearchResult();
	}

	private void DbMenu_inactivateDb()
	{
		foreach (var item in SelectedDBs)
		{
			item.IsActive = false;
		}
		data.DB.SaveChanges();
		RefreshDbList();
		searchPanel.RefreshSearchResult();
	}

	private void DbMenu_RefreshDb()
	{
		int[] selDbIndex = dbList.GetSelectedItems();
		foreach (var item in selDbIndex)
		{
			data.RefreshDb(_folders.ElementAt(item));
		}
		//TODO popup with list of files to tag, and list of removed files (to edit if have been moved)
	}

	private void DbMenu_RenameDb()
	{
		int[] selDbIndex = dbList.GetSelectedItems();
		ResourceFolder selDb = _folders.ElementAt(selDbIndex[0]);

		this.AskText("Rename DB '" + selDb.Name + "'",
			(u)=>
			{
				selDb.Name = u.InputText;
				data.DB.SaveChanges();
				RefreshDbList();
			});
	}

	private void DbMenu_RemoveDb()
	{
		int[] selDbIndex = dbList.GetSelectedItems();
		ResourceFolder selDb = _folders.ElementAt(selDbIndex[0]);

		AskConfirmation(
			"Remove DB ...",
			"Do you really want to remove the Database \"" + selDb.Name + "\" ?",
			() => {
				// remove DB
				ApplicationControl.Control.Database.RemoveDb(selDb);
				RefreshDbList();
				searchPanel.RefreshSearchResult();
			});
	}

	/// <summary>
	/// Compute a condition for the popup menu list.
	/// </summary>
	/// <returns>True if at least one item is selected in the db list.</returns>
	private bool DbMenu_AtLeastOneSelected()
	{
		int[] selItems = dbList.GetSelectedItems();
		return selItems.Length >= 1;
	}

	/// <summary>
	/// Compute a condition for the popup menu list.
	/// </summary>
	/// <returns>True if at ony one item is selected in the db list.</returns>
	private bool DbMenu_OnlyOneSelected()
	{
		int[] selItems = dbList.GetSelectedItems();
		return selItems.Length == 1;
	}
	#endregion

	#region TagManagement
	private void _on_TagList_TagActivated(GodotTag selTag)
	{
		searchPanel.AddTag(selTag.Tag);
	}
	#endregion

	#region Confirmation Window management
	private Action OnConfirmation;
	private void AskConfirmation(string popupName, string popupText, Action onConfirmation)
	{
		confirmationDialog.WindowTitle = popupName;
		confirmationDialog.DialogText = popupText;
		OnConfirmation = onConfirmation;

		confirmationDialog.PopupCentered();
	}
	private void _on_DefaultConfirmationDialog_confirmed()
	{
		OnConfirmation?.Invoke();
	}
	#endregion

	private void _on_SearchPanel_ResourceSelected(GodotResource file)
	{
		// No preview if soundtrack
		ResourceFile rf = (file.File.Type != ResourceFileType.Soundtrack) ? file.File : null;
		previewPanel.SetPreview(rf);

		propertiesPanel.SetFile(file.File);
	}

	private void _on_SearchPanel_ResourceActivated(GodotResource file)
	{
		if(file.File.Type == ResourceFileType.Soundtrack)
		{
			_currentTrackIndex = searchPanel.LastSelectedIndex;
			PlayTrack(file.File);
		}
	}

	private void PlayTrack(ResourceFile track)
	{
		_currentTrack = track;

		if(_currentAudio != null)
		{
			_currentAudio.PlaybackStopped -= _currentTrack_PlaybackStopped;
			_currentAudio.Dispose();
		}
		_currentAudio = ApplicationControl.Control.Audio.GetTrack(track.Info);
		audioPanel.SetTrack(_currentAudio);
		audioPanel.Play();

		_currentAudio.PlaybackStopped += _currentTrack_PlaybackStopped;
	}

	private void _currentTrack_PlaybackStopped(object sender, NAudio.Wave.StoppedEventArgs e)
	{
		var res = searchPanel.Results.Where(rf => rf.Type == ResourceFileType.Soundtrack);
		int resCount = res.Count();
		if(resCount > 0)
		{
			int startIndex = _currentTrackIndex;

			// if current track the single one in the list, stop playing
			if (resCount == 1 && _currentTrack == searchPanel.GetResultItem(0))
				return;

			// if result have changed, restart from the beginning
			if(_currentTrack != searchPanel.GetResultItem(startIndex))
			{
				startIndex = -1;
				// but still after the current track if it is now the first
				if (_currentTrack == searchPanel.GetResultItem(0))
					startIndex = 0;
			}

			// play next
			_currentTrackIndex = (startIndex + 1) % resCount;
			PlayTrack(searchPanel.GetResultItem(_currentTrackIndex));
		}
	}
}
