using Engine.RpgLogic;
using Godot;
using L5R.Model.Skill;
using System;
using System.Linq;
using System.Text;

public class SkillDetailsDisplay : Control, INamedDisplay
{
	private Button _attribute;
	private Label _mastery;
	private Label _specialty;
	private Button _rollButton;

	public Control Control => this;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_attribute = GetNode<Button>("HBoxContainer/AttributeLabel");
		_mastery = GetNode<Label>("HBoxContainer/MasteryLabel");
		_specialty = GetNode<Label>("HBoxContainer/SpecialtyLabel");
		_rollButton = GetNode<Button>("HBoxContainer/RollButton");
	}

	public void SetDisplay(INamed toDisplay)
	{
		Competence cpt = (Competence)toDisplay;
		_attribute.Text = cpt.Trait.ToString();
		StringBuilder sb = new StringBuilder();

		if (cpt.CurrentMaitrises.Count() > 0)
		{
			sb.AppendLine("Maitrises :");
			foreach (var item in cpt.CurrentMaitrises)
			{
				sb.AppendFormat(" - Rang {0} : {1}", item.Rang, item.Description);
			}
		}
		_mastery.Text = sb.ToString();
		sb.Clear();

		if (cpt.Specialisations.Count() > 0)
		{
			sb.AppendLine("Spécialités :");
			foreach (var item in cpt.Specialisations)
			{
				sb.AppendFormat(" - {0}", item.Name);
			}
		}
		_specialty.Text = sb.ToString();
	}

	private void _on_RollButton_pressed()
	{
		Godot.GD.Print("Not Implemented yet (SkillDetailsDisplay.cs)");
	}
}

