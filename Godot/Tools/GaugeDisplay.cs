using Engine.RpgLogic;
using Godot;
using System;

public class GaugeDisplay : Control
{
	private ProgressBar _progressBar;
	private IJauge _jauge;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_progressBar = GetNode<ProgressBar>("ProgressBar");
	}

	public void SetGauge(IJauge newJauge)
	{
		// Remove previous
		if (_jauge != null)
		{
			_jauge.MaxValueChanged -= _value_MaxValueChanged;
			_jauge.MinValueChanged -= _value_MinValueChanged;
			_jauge.CurrentValueChanged -= _value_CurrentValueChanged;
		}
		// Set new one
		_jauge = newJauge;
		_jauge.MaxValueChanged += _value_MaxValueChanged;
		// Update display
		_progressBar.MaxValue = _jauge.MaxValue;
		_progressBar.MinValue = _jauge.MinValue;
		_progressBar.Value = _jauge.CurrentValue;
		UpdateTooltip();

	}

	private void UpdateTooltip()
	{
		_progressBar.HintTooltip = String.Format("{0}/{1}", _jauge.CurrentValue, _jauge.MaxValue);
	}

	private void _value_MaxValueChanged(IJauge ival, int newValue, int oldValue)
	{
		_progressBar.MaxValue = ival.MaxValue;
		UpdateTooltip();
	}

	private void _value_MinValueChanged(IJauge ival, int newValue, int oldValue)
	{
		_progressBar.MinValue = ival.MinValue;
		UpdateTooltip();
	}

	private void _value_CurrentValueChanged(IJauge ival, int newValue, int oldValue)
	{
		_progressBar.Value = ival.CurrentValue;
		UpdateTooltip();
	}
}
