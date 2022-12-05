using Godot;
using MightyGm2.Engine.Database;
using System;
using System.IO;
using System.Linq;
using System.Text;

public class FilePropertiesPanel : Control
{
	private Label infoPanel;
	private VBoxContainer tagContainer;
	private PackedScene tagPanelScene;

	private ResourceFile _currentFile;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		infoPanel = GetNode<Label>("InfoTextLabel");
		tagContainer = GetNode<VBoxContainer>("LabelBoxContainer");
		tagPanelScene = GD.Load<PackedScene>("res://Godot/Database/TagPanel.tscn");
	}
	public void SetFile(ResourceFile file)
	{
		if (_currentFile == file) return;
		_currentFile = file;

		// Clean and init
		tagContainer.RemoveChildren();
		if(file == null)
		{
			infoPanel.Text = "";
			return;
		}

		// Display basic infos as text
		StringBuilder sb = new StringBuilder(file.Name);
		sb.AppendFormat("\r\n[{0}]\r\n", file.Type);
		sb.AppendFormat("{0}:{1}\r\n", file.Database.Name, file.RelativePath.Remove(file.RelativePath.Length - file.Name.Length, file.Name.Length));
		//TODO Type specific informations (dimensions, length, ...)
		sb.Append("TODO : Specific info");
		infoPanel.Text = sb.ToString();

		// Display tags
		foreach (Tag tag in file.ResourceFilesToTags.Select(t=>t.Tag))
		{
			TagPanel tagPan = (TagPanel)tagPanelScene.Instance();
			tagPan.Tag = tag;
            tagPan.Disabled = true;
            tagPan.Connect(nameof(TagPanel.RemoveTag), this, nameof(RemoveTagToCurrentFile));
            //TODO connect del button to remove from tagList of the item
            tagContainer.AddChild(tagPan);
		}
	}

    private void RemoveTagToCurrentFile(TagPanel tag)
    {
        _currentFile.RemoveTagIfFound(tag.Tag);
        tag.QueueFree();
    }
}
