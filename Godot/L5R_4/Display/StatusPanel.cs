using Engine.RpgLogic;
using Godot;
using L5R.Model.Agent;
using System;

public class StatusPanel : Control
{
	Agent _character;

	private ProgressBar _honnor;
	private ProgressBar _glory;
	private ProgressBar _status;
	private ProgressBar _taint;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_honnor = GetNode<ProgressBar>("VBoxContainer/MarginContainer/HonnorDisplay");
		_glory = GetNode<ProgressBar>("VBoxContainer/MarginContainer2/GloryDisplay");
		_status = GetNode<ProgressBar>("VBoxContainer/MarginContainer3/StatusDisplay");
		_taint = GetNode<ProgressBar>("VBoxContainer/MarginContainer4/TaintDisplay");
	}

	public void SetCharacter(Agent character)
	{
		// Remove previous character events
		if (_character != null)
		{
			_character.Honneur.ValueChanged -= Honneur_ValueChanged;
			_character.Gloire.ValueChanged -= Gloire_ValueChanged;
			_character.Status.ValueChanged -= Status_ValueChanged;
			_character.Souillure.ValueChanged -= Souillure_ValueChanged;
		}

		// Set new character
		_character = character;

		// Link new character events
		_character.Honneur.ValueChanged += Honneur_ValueChanged;
		_character.Gloire.ValueChanged += Gloire_ValueChanged;
		_character.Status.ValueChanged += Status_ValueChanged;
		_character.Souillure.ValueChanged += Souillure_ValueChanged;

		// Init values
		Honneur_ValueChanged(/*not used values ->*/null, 0, 0);
		Gloire_ValueChanged(/*not used values ->*/null, 0, 0);
		Status_ValueChanged(/*not used values ->*/null, 0, 0);
		Souillure_ValueChanged(/*not used values ->*/null, 0, 0);
	}

	private void Souillure_ValueChanged(IValue ival, int newValue, int oldValue)
	{
		_taint.Value = _character.Souillure.BaseValue;
		_taint.HintTooltip = _character.Souillure.BaseValue + "." + _character.Souillure.RemainingPoints;
	}

	private void Status_ValueChanged(IValue ival, int newValue, int oldValue)
	{
		_status.Value = _character.Status.BaseValue;
		_status.HintTooltip = _character.Status.BaseValue + "." + _character.Status.RemainingPoints;
	}

	private void Gloire_ValueChanged(IValue ival, int newValue, int oldValue)
	{
		_glory.Value = _character.Gloire.BaseValue;
		_glory.HintTooltip = _character.Gloire.BaseValue + "." + _character.Gloire.RemainingPoints;
	}

	private void Honneur_ValueChanged(IValue ival, int newValue, int oldValue)
	{
		_honnor.Value = _character.Honneur.BaseValue;
		_honnor.HintTooltip = _character.Honneur.BaseValue + "." + _character.Honneur.RemainingPoints;
	}
}
