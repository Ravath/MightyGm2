using Godot;
using L5R.Model.Agent;
using L5R.Model.Attribute;
using System;

public class AttributePanel : Control
{
	private ValueDisplay _void, _water, _air, _earth, _fire;
	private ValueDisplay _const, _will, _strenght, _per;
	private ValueDisplay _reflexes, _intuition, _agility, _intelligence;
	private GaugeDisplay _voidPoints;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		const string ringPath = "VBoxContainer/HBoxContainer/CenterContainer/RingValues/";
		_void = GetNode<ValueDisplay>(ringPath+"Void");
		_water = GetNode<ValueDisplay>(ringPath + "Water");
		_air = GetNode<ValueDisplay>(ringPath + "Air");
		_earth = GetNode<ValueDisplay>(ringPath + "Earth");
		_fire = GetNode<ValueDisplay>(ringPath + "Fire");
		const string leftPath = "VBoxContainer/HBoxContainer/VBoxContainerLeft/AttContainer";
		_const = GetNode<ValueDisplay>(leftPath + "1/AttDisplay");
		_will = GetNode<ValueDisplay>(leftPath + "2/AttDisplay");
		_strenght = GetNode<ValueDisplay>(leftPath + "3/AttDisplay");
		_per = GetNode<ValueDisplay>(leftPath + "4/AttDisplay");
		const string rightPath = "VBoxContainer/HBoxContainer/VBoxContainerRight/AttContainer";
		_reflexes = GetNode<ValueDisplay>(rightPath + "1/AttDisplay");
		_intuition = GetNode<ValueDisplay>(rightPath + "2/AttDisplay");
		_agility = GetNode<ValueDisplay>(rightPath + "3/AttDisplay");
		_intelligence = GetNode<ValueDisplay>(rightPath + "4/AttDisplay");

		_voidPoints = GetNode<GaugeDisplay>("VBoxContainer/MarginContainer/GaugeDisplay");
		Agent a = new Personnage();
		SetAttribute(a.Attributs);
	}


	public void SetAttribute(Attributs attributes)
	{
		// Rings
		_void.SetValue(attributes.MaxVide);
		_water.SetValue(attributes.Eau);
		_air.SetValue(attributes.Air);
		_earth.SetValue(attributes.Terre);
		_fire.SetValue(attributes.Feu);
		// Left attributes
		_const.SetValue(attributes.Constitution);
		_will.SetValue(attributes.Volonte);
		_strenght.SetValue(attributes.Force);
		_per.SetValue(attributes.Perception);
		// Right attributes
		_reflexes.SetValue(attributes.Reflexes);
		_intuition.SetValue(attributes.Intuition);
		_agility.SetValue(attributes.Agilite);
		_intelligence.SetValue(attributes.Intelligence);
		// Void Points
		_voidPoints.Visible = (attributes.Vide.MaxValue != 0);
		if(_voidPoints.Visible)
			_voidPoints.SetGauge(attributes.Vide);

	}
}
