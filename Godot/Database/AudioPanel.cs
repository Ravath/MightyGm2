using Godot;
using MightyGm2.Engine.Control;
using NAudio.Wave;
using System;
using System.IO;

public class AudioPanel : Control
{
	private TrackControl track;


	private Button playButton;
	private Label duration;
	private Label name;
	private HScrollBar timeline;

	/// <summary>
	/// Detect if the timeline is changed by user or script process.
	/// </summary>
	private Boolean isTimelinePressed = false;

	/// <summary>
	/// Called when the node enters the scene tree for the first time.
	/// </summary>
	public override void _Ready()
	{
		playButton = GetNode<Button>("VBox/HBox/PlayButton");
		duration = GetNode<Label>("VBox/HBox/Duration");
		name = GetNode<Label>("VBox/HBox/Name");
		timeline = GetNode<HScrollBar>("VBox/Timeline");
		SetTrack(null);
	}

	public override void _Process(float delta)
	{
		if (track?.PlaybackState == NAudio.Wave.PlaybackState.Playing)
		{
			UpdateTimeline();
		}
	}

	/// <summary>
	/// Load the stream to play. Pause by default
	/// </summary>
	/// <param name="stream"></param>
	public void SetTrack(TrackControl newTrack)
	{
		this.track = newTrack;
		if (track == null)
		{
			name.Text = "";
			duration.Text = "";
			playButton.Disabled = true;
			timeline.Ratio = 0;
		}
		else
		{
			name.Text = new FileInfo(track.File).Name;
			playButton.Disabled = false;
			Pause();
			UpdateTimeline();
		}
	}

	private void UpdateTimeline()
	{
		duration.Text = String.Format(@"{0:hh\:mm\:ss} / {1:hh\:mm\:ss}", track.CurrentTime, track.TotalTime);
		if (!isTimelinePressed)
			timeline.Ratio = track.CurrentTime.TotalSeconds / track.TotalTime.TotalSeconds;
	}

	public void Play()
	{
		playButton.Text = "||";

		track?.Play();
	}

	public void Pause()
	{
		playButton.Text = ">";
		track?.Pause();
	}

	#region Events
	private void _on_PlayButton_pressed()
	{
		if (track.PlaybackState == PlaybackState.Playing)
		{
			Pause();
		}
		else
		{
			Play();
		}
	}

	private void _on_Timeline_scrolling()
	{
		if(track != null)
		{
			track.SetPosition(timeline.Ratio);
		}
	}

	private void _on_Timeline_gui_input(object @event)
	{
		if(@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.IsPressed())
			{
				isTimelinePressed = true;
			}
			else
			{
				isTimelinePressed = false;
			}
		}
	}

	private void _on_Timeline_mouse_exited()
	{
		isTimelinePressed = false;
	}
	#endregion
}
