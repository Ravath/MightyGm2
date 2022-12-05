
using Godot;
using L5R.Model.Agent;
using System;
using System.Text;

public class LifePanel : Control
{
	private Label _thresholdLabel;
	private ValueDisplay _woundsDisplay;
	private ValueDisplay _lifeDisplay;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_thresholdLabel = GetNode<Label>("VBoxContainer/ThresholdLabel");
		_woundsDisplay = GetNode<ValueDisplay>("VBoxContainer/HBoxContainer/WoundsDisplay");
		_lifeDisplay = GetNode<ValueDisplay>("VBoxContainer/HBoxContainer/MaxLifeDisplay");
	}

	public void SetLife(SeuilVie vie)
	{
		_woundsDisplay.SetValue(vie.DegatsValue);
		_lifeDisplay.SetValue(vie.DeathThresholdValue);
		StringBuilder sb = new StringBuilder();
		foreach (var item in vie.Seuils)
		{
			sb.AppendFormat("{0,2} : -{1,2}\n", item.Item1, item.Item2);
		}
		_thresholdLabel.Text = sb.ToString();
	}
}
