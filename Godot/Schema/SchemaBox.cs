using Godot;
using System;

public class SchemaBox : Control
{
	private bool can_grab = false;
	private Vector2 grabbed_offset = new Vector2();

	private MarginContainer content;
	private SchemaDot tl;
	private SchemaDot tr;
	private SchemaDot bl;
	private SchemaDot br;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		content = GetNode<MarginContainer>("Content");
		tl = GetNode<SchemaDot>("TopLeftCorner");
		tr = GetNode<SchemaDot>("TopRightCorner");
		bl = GetNode<SchemaDot>("BottomLeftCorner");
		br = GetNode<SchemaDot>("BottomRightCorner");

		tl.InhibitDrag = tr.InhibitDrag = true;
		bl.InhibitDrag = br.InhibitDrag = true;
	}

	private void _on_Control_gui_input(object @event)
	{
		if (@event is InputEventMouseButton iemb)
		{
			can_grab = iemb.Pressed;
			grabbed_offset = RectPosition - GetGlobalMousePosition();
		}
	}

	public override void _Process(float delta)
	{
		if (Input.IsMouseButtonPressed((int)ButtonList.Left) && can_grab)
		{
			RectPosition = GetGlobalMousePosition() + grabbed_offset;
		}
		if (tl.IsGrabbed)
		{
			Vector2 diff = GetLocalMousePosition();
			RectGlobalPosition = GetGlobalMousePosition();
			MarginRight -= diff.x;
			MarginBottom -= diff.y;
		}
		if (tr.IsGrabbed)
		{
			MarginBottom -= GetLocalMousePosition().y;
			Vector2 newPos = RectGlobalPosition;
			newPos.y = GetGlobalMousePosition().y;
			RectGlobalPosition = newPos;

			MarginRight = GetGlobalMousePosition().x;
		}
		if (bl.IsGrabbed)
		{
			MarginRight -= GetLocalMousePosition().x;
			Vector2 newPos = RectGlobalPosition;
			newPos.x = GetGlobalMousePosition().x;
			RectGlobalPosition = newPos;

			MarginBottom = GetGlobalMousePosition().y;
		}
		if (br.IsGrabbed)
		{
			MarginRight = GetGlobalMousePosition().x;
			MarginBottom = GetGlobalMousePosition().y;
		}
	}

	public void SetContent(Control newChild)
	{
		content.RemoveChildren();
		content.AddChild(newChild);
		newChild.MouseFilter = MouseFilterEnum.Pass;
	}
}
