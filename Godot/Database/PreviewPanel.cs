using Godot;
using MightyGm2.Engine.Control;
using MightyGm2.Engine.Database;
using NAudio.Wave;
using System;
using System.IO;

public class PreviewPanel : Control
{
	private AudioPanel audioPanel;
	private VideoPlayer videoPanel;
	private TextureRect imagePanel;
	private Label textPanel;
	private Label errorPanel;

	private ResourceFile _currentFile;

	private TrackControl _currentTrack;
    public TrackControl CurrentTrack { get => _currentTrack; }

	public bool AutoPlay { get; set; } = false;

	public override void _Ready()
	{
		audioPanel = GetNode<AudioPanel>("AudioPanel");
		videoPanel = GetNode<VideoPlayer>("VideoPanel");
		imagePanel = GetNode<TextureRect>("ImagePanel");
		textPanel = GetNode<Label>("TextPanel");
		errorPanel = GetNode<Label>("ErrorPanel");

		HideChildren();
	}

	private void HideChildren()
	{
		audioPanel.Visible = false;
		videoPanel.Visible = false;
		imagePanel.Visible = false;
		textPanel.Visible = false;
		errorPanel.Visible = false;
	}

	private void PrintErrorMessage(string message)
	{
		errorPanel.Text = message;
		errorPanel.Visible = true;
	}

	public void SetPreview(ResourceFile file)
	{
		if (_currentFile == file) return;

		HideChildren();

		_currentFile = file;
		if (_currentFile == null) return;
		FileInfo fileInfo = file.Info;

		switch (file.Type)
		{
			case ResourceFileType.Misc:
				textPanel.Text = file.Name;
				textPanel.Text += "\r\nMisc Type Ressource";
				textPanel.Visible = true;
				break;
			case ResourceFileType.Picture:

				Image image = new Image();
				Error error = image.Load(fileInfo.FullName);

				if(error == Error.Ok)
				{
					ImageTexture imageTexture = new ImageTexture();
					imageTexture.CreateFromImage(image);
					imagePanel.Texture = imageTexture;
					imagePanel.Visible = true;
				}
				else
				{
					PrintErrorMessage(String.Format("Can't Open File {0} : {1}", fileInfo.FullName, error));
				}

				break;
			case ResourceFileType.Soundtrack:
				_currentTrack?.Dispose();
				_currentTrack = ApplicationControl.Control.Audio.GetTrack(file.Info);
				audioPanel.SetTrack(_currentTrack);
				if(AutoPlay)
					audioPanel.Play();
				audioPanel.Visible = true;
				break;
			case ResourceFileType.Text:
				textPanel.Text = file.Info.OpenText().ReadToEnd();
				textPanel.Visible = true;
				break;
			case ResourceFileType.Archive:
				textPanel.Text = file.Name;
				textPanel.Text += "\r\nArchive Type Ressource";
				textPanel.Visible = true;
				break;
			case ResourceFileType.Pdf:
				textPanel.Text = file.Name;
				textPanel.Text += "\r\nPDF Type Ressource";
				textPanel.Visible = true;
				break;
			case ResourceFileType.Video:
				textPanel.Text = file.Name;
				textPanel.Text += "\r\nVideo Type Ressource";
				textPanel.Visible = true;
				//videoPanel.Visible = true;
				break;
			default:
				break;
		}
	}
}
