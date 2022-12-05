using Godot;
using System;

public class TechniquePanel : VBoxContainer
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetTechnique();
	}

	public void SetTechnique()
	{
		GetNode<Label>("Name").Text = "Kage";
		GetNode<Label>("Description").Text = "Become invisible!!";
	}
}
