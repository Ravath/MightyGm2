using Engine.RpgLogic;
using Godot;
using L5R.Model.Agent;
using System;
using System.Text;

public class ValueDisplay : Control
{

	private Label valueLabel;
	private IValue _value;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		valueLabel = GetNode<Label>("ValueLabel");
	}

	public void SetValue(IValue newVal)
	{
		// Remove previous
		if (_value != null)
		{
			_value.ValueChanged -= _value_ValueChanged;
		}
		// Set new one
		_value = newVal;
		_value.ValueChanged += _value_ValueChanged;
		// Update display
		_value_ValueChanged(_value, /*Values here are not relevant ->*/0, 0);
	}

	private void _value_ValueChanged(IValue ival, int newValue, int oldValue)
	{
		valueLabel.Text = ival.TotalValue.ToString();
		StringBuilder sb = new StringBuilder();
		sb.AppendFormat("{0} : {1}\n", ival.Label, ival.BaseValue);
		foreach (var item in ival.Modifiers)
		{
			sb.AppendFormat(" - {0} : {1}\n", item.Label, item.TotalValue);
		}
		valueLabel.HintTooltip = sb.ToString();
	}
}
