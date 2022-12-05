using Godot;
using MightyGm2.Engine.Control;
using MightyGm2.Engine.Database;
using System;
using System.IO;


public class DatabaseImportPanel : Control
{
	private Tree tree;
	private FileDialog fileDialog;
	private TextEdit folderPathTextEdit;
	private Label ErrorLabel;

	//main tree nodes
	private TreeItem root, cRes, cTag;

	//current DB importation
	private DatabaseImportResult _dataFolder;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fileDialog = GetNode<FileDialog>("FileDialog");
		folderPathTextEdit = GetNode<TextEdit>("MarginContainer/VBoxContainer/HBoxContainer/FolderPathTextEdit");
		ErrorLabel = GetNode<Label>("MarginContainer/VBoxContainer/ErrorLabel");
		tree = GetNode<Tree>("MarginContainer/VBoxContainer/Tree");
		_on_FileDialog_dir_selected("");
	}

	public void Clear()
	{
		folderPathTextEdit.Text = "";
		SetDbFolder(null);
	}

	public void SetDbFolder(DatabaseImportResult dataFolder)
	{
		_dataFolder = dataFolder;
		tree.Clear();
		if (dataFolder != null)
		{
			root = tree.CreateItem();
			cRes = tree.CreateItem(root);
			cTag = tree.CreateItem(root);

			tree.HideRoot = false;
			root.SetText(0, "DB:");
			root.SetText(1, dataFolder.Database.Name);
			root.SetEditable(1, true);
			root.SetText(2, dataFolder.Database.Path);
			tree.SetColumnMinWidth(0, 100);
			tree.SetColumnExpand(0, false);

			cRes.SetText(0, "Files");
			cTag.SetText(0, "Tags");
			foreach (var item in dataFolder.Resources)
			{
				TreeItem node = tree.CreateItem(cRes);
				node.SetText(0, String.Format("[{0}]", item.Type));
				node.SetText(1, item.Name);
				node.SetEditable(1, true);
				node.SetText(2, item.RelativePath);

				node.SetMetadata(0, new GodotResource(item));
			}
			foreach (var item in dataFolder.Tags)
			{
				TreeItem node = tree.CreateItem(cTag);
				node.SetCellMode(0, TreeItem.TreeCellMode.Check);
				node.SetChecked(0, true);
				node.SetEditable(0, true);
				node.SetText(1, item.Name);
				node.SetEditable(1, true);

				node.SetMetadata(0, new GodotTag(item));
			}
		}
	}
	
	public void ImportDb()
	{
		if(_dataFolder != null)
			ApplicationControl.Control.Database.Save(_dataFolder);
	}

	private void _on_SelectDbFolderButton_pressed()
	{
		fileDialog.Visible = true;
	}
	
	private void _on_FileDialog_dir_selected(String dir)
	{
		folderPathTextEdit.Text = dir;
		_on_FolderPathTextEdit_text_changed();
	}

	private void _on_FolderPathTextEdit_text_changed()
	{
		if (String.IsNullOrWhiteSpace(folderPathTextEdit.Text))
		{
			ErrorLabel.Visible = false;
			SetDbFolder(null);
			return;
		}

		DirectoryInfo directory = null;
		try
		{
			directory = new DirectoryInfo(folderPathTextEdit.Text);
		}
		catch (Exception ex)
		{
			ErrorLabel.Text = ex.Message;
			ErrorLabel.Visible = true;
			SetDbFolder(null);
			return;
		}

		DatabaseImportResult importRes = ApplicationControl.Control.Database.StartImport(directory);

		ErrorLabel.Text = importRes.ErrorMessage;
		ErrorLabel.Visible = !importRes.IsFolderEligible;

		SetDbFolder(!importRes.IsFolderEligible ? null : importRes);

	}

	private void _on_Tree_item_edited()
	{
		TreeItem ed = tree.GetEdited();
		if (ed == root)
			_dataFolder.Database.Name = ed.GetText(1);
		else if (ed.GetParent() == cRes)
		{
			ResourceFile f = ((GodotResource)ed.GetMetadata(0)).File;
			f.Name = ed.GetText(1);
		}
		else if (ed.GetParent() == cTag)
		{
			Tag t = ((GodotTag)ed.GetMetadata(0)).Tag;
			if (tree.GetEditedColumn() == 0)
			{
				_dataFolder.ImportTag(t, ed.IsChecked(0));
			}
			else // == 1
			{
				t.Name = ed.GetText(1);
			}
		}
		else
			GD.PrintErr("Wrong cell edited");
	}
}
