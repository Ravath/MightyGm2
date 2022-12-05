using Engine.RpgLogic;
using Godot;
using L5R.Model.Attack;
using System;

public class AttackDisplay : Control, INamedDisplay
{
	private Label _name;
	private ValueDisplay _touchRoll;
	private ValueDisplay _touchKeep;
	private ValueDisplay _damageRoll;
	private ValueDisplay _damageKeep;

    public Control Control => this;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_name = GetNode<Label>("VBoxContainer/NameLabel");
		_touchRoll = GetNode<ValueDisplay>("VBoxContainer/HBoxContainer/AttRoll");
		_touchKeep = GetNode<ValueDisplay>("VBoxContainer/HBoxContainer/AttKeep");
		_damageRoll = GetNode<ValueDisplay>("VBoxContainer/HBoxContainer/DamagesRoll");
		_damageKeep = GetNode<ValueDisplay>("VBoxContainer/HBoxContainer/DamageKeep");
	}

	public void SetDisplay(INamed toDisplay)
	{
		IAttaque att = (IAttaque)toDisplay;
		_name.Text = string.Format("{0} ({1})", att.Name, att.Action);
		_touchRoll.SetValue(att.JetAttaque.RollValue);
		_touchKeep.SetValue(att.JetAttaque.KeepValue);
		_damageRoll.SetValue(att.Degats.RollValue);
		_damageKeep.SetValue(att.Degats.KeepValue);
	}
}
