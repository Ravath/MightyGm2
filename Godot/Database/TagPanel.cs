using Godot;
using MightyGm2.Engine.Database;
using System;

public class TagPanel : Button
{
	private Tag _tag;
	public Tag Tag
	{
		get { return _tag; }
		set
		{
			_tag = value;
			Text = value.Name;
		}
	}

	[Signal]
	public delegate void RemoveTag(TagPanel tag);
	[Signal]
	public delegate void ExcludeTag(TagPanel tag);

	internal void _on_Button_pressed()
	{
		EmitSignal(nameof(RemoveTag), this);
	}

	internal void _on_Label_pressed()
	{
		EmitSignal(nameof(ExcludeTag), this);
	}
}
