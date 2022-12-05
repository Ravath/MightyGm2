using Godot;
using System;

public class SchemaDot : Panel
{

	private bool can_grab = false;
	private Vector2 grabbed_offset;
	public bool InhibitDrag { get; set; }

	public bool IsGrabbed { get => can_grab; }

	private void _on_Panel_gui_input(object @event)
	{
		if (@event is InputEventMouseButton iemb)
		{
			can_grab = iemb.Pressed;
			grabbed_offset = RectPosition - GetGlobalMousePosition();
		}
	}

	public override void _Process(float delta)
	{
		if (!InhibitDrag && Input.IsMouseButtonPressed((int)ButtonList.Left) && can_grab)
		{
			RectPosition = GetGlobalMousePosition() + grabbed_offset;
		}
	}
}
