using Engine.RpgLogic;
using Godot;
using System;

public class LabelNamedDisplay : Label, INamedDisplay
{
	public Control Control => this;

	public void SetDisplay(INamed toDisplay)
	{
		Text = toDisplay.Name;
	}
}
