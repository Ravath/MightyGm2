using Godot;
using L5R.Model.Agent;
using MightyGm2.RPG.L5R4.Data;
using System;
using System.Linq;

public class CharacterSheet : Control
{
	private Label _nameLabel;
	private AttributePanel _attributes;
	private StatusPanel _status;
	private LifePanel _life;
	private DerivedStatsPanel _derived;
	private Label _description;

	private NamedListDisplay _attacks;
	private NamedListDisplay _capacities;
	private NamedListDisplay _skills;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nameLabel = GetNode<Label>("VBoxLeft/HBoxContainer/NameLabel");
		_attributes = GetNode<AttributePanel>("VBoxLeft/AttributePanel");
		_status = GetNode<StatusPanel>("VBoxLeft/HBoxContainer2/StatusPanel");
		_life = GetNode<LifePanel>("VBoxLeft/HBoxContainer2/LifePanel");
		_derived = GetNode<DerivedStatsPanel>("VBoxLeft/HBoxContainer2/DerivedStatsPanel");
		_description = GetNode<Label>("VBoxLeft/MarginContainer/Description");
		_attacks = GetNode<NamedListDisplay>("AttackList");
		var attackDisplayScene = GD.Load<PackedScene>("res://Godot/L5R_4/Display/AttackDisplay.tscn");
		_attacks.ItemDisplayer = (AttackDisplay)attackDisplayScene.Instance(); ;
		_capacities = GetNode<NamedListDisplay>("CapacityList");
		((DataModelDisplay)_capacities.ItemDisplayer).DisplayName = false;
		_skills = GetNode<NamedListDisplay>("SkillList");
		var skillBasicDisplayScene = GD.Load<PackedScene>("res://Godot/L5R_4/Display/SkillBasicDisplay.tscn");
		_skills.ListItemTemplate = (SkillBasicDisplay)skillBasicDisplayScene.Instance();
		var skillDetailsDisplayScene = GD.Load<PackedScene>("res://Godot/L5R_4/Display/SkillDetailsDisplay.tscn");
		_skills.ItemDisplayer = (SkillDetailsDisplay)skillDetailsDisplayScene.Instance();
	}

	public void SetCharacter(Agent character)
	{
		_nameLabel.Text = character.EtatCivil.Name;

		_attributes.SetAttribute(character.Attributs);
		_status.SetCharacter(character);
		_life.SetLife(character.Vie);
		_derived.SetCharacter(character);
		_description.Text = character.EtatCivil.Description;

		_attacks.SetList(character.Armes.Attacks);
		_capacities.SetList(character.TraitsCreature.NamedCollection);
		_skills.SetList(character.Competences.Competences);
	}
}
