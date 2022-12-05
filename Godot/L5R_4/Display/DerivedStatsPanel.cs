using Engine.RpgLogic;
using Godot;
using L5R.Model.Agent;
using System;

public class DerivedStatsPanel : Control
{
	private ValueDisplay _initRoll;
	private ValueDisplay _initKeep;
	private ValueDisplay _armorND;
	private ValueDisplay _armorRed;
	private ValueDisplay _recuperation;
	private ValueDisplay _freeMovement;
	private ValueDisplay _simpleMovement;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_initRoll = GetNode<ValueDisplay>("VBoxContainer/HBoxContainer/InitRollValue");
		_initKeep = GetNode<ValueDisplay>("VBoxContainer/HBoxContainer/InitKeepValue");
		_armorND = GetNode<ValueDisplay>("VBoxContainer2/HBoxContainer/ArmorValue");
		_armorRed = GetNode<ValueDisplay>("VBoxContainer2/HBoxContainer/ReductionValue");
		_recuperation = GetNode<ValueDisplay>("VBoxContainer/RecuperationValue");
		_freeMovement = GetNode<ValueDisplay>("VBoxContainer2/HBoxContainer2/FreeValue");
		_simpleMovement = GetNode<ValueDisplay>("VBoxContainer2/HBoxContainer2/SimpleValue");
	}

	public void SetCharacter(Agent character)
	{
		_initRoll.SetValue(character.Initiative.RollValue);
		_initKeep.SetValue(character.Initiative.KeepValue);
		_armorND.SetValue(character.Armures.ND);
		_armorRed.SetValue(character.Armures.Reduction);
		_recuperation.SetValue(character.RecuperationRate);
		_freeMovement.SetValue(character.Movement.FreeMovement);
		_simpleMovement.SetValue(character.Movement.SimpleMovement);
	}
}
