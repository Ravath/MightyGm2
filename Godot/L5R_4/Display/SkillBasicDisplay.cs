using Engine.RpgLogic;
using Godot;
using L5R.Model.Skill;
using System;

public class SkillBasicDisplay : Control, INamedDisplay
{
	private Label _name;
	private ValueDisplay _roll;
	private ValueDisplay _keep;
	private TextureRect _infoIcon;

	public Control Control => this;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_name = GetNode<Label>("Name");
		_roll = GetNode<ValueDisplay>("VBoxContainer/RollValue");
		_keep = GetNode<ValueDisplay>("VBoxContainer/KeepValue");
		_infoIcon = GetNode<TextureRect>("Name/InfoIcon");
	}

	public void SetDisplay(INamed toDisplay)
	{
		Competence cpt = (Competence)toDisplay;
		_name.Text = cpt.Name;
		_roll.SetValue(cpt.Pool.RollValue);
		_keep.SetValue(cpt.Pool.KeepValue);
		_infoIcon.HintTooltip = (cpt.Description??"").WordWrap(100).ToString();

	}
}
