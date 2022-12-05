using Godot;
using L5R.Model.School;
using L5R4;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SchoolPanel : Control
{
	private Label _name;
	private NamedListDisplay _techniques;
	private RichTextLabel _stats;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_name = GetNode<Label>("VBoxContainer/Name");
		_techniques = GetNode<NamedListDisplay>("VBoxContainer/TechniqueList");
		((DataModelDisplay)_techniques.ItemDisplayer).DisplayName = false;
		_stats = GetNode<RichTextLabel>("VBoxContainer/Stats");
	}

	public void SetSchool(Ecole school)
	{
		if(school == null)
		{
			_name.Text = "";
			_stats.Text = "";
			_techniques.SetList(null);
			return;
		}
		_name.Text = school.Name;
		_techniques.SetList(school.Techniques);
		StringBuilder sb = new StringBuilder();

		// Trait
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]{0}[/color]\n\n", school.BonusTrait);

		// Devotion if any
		if (school.Devotion != null)
		{
			sb.AppendFormat("[color=" + LocalContext.GOLD + "]Devotion : [/color]{0}\n\n", school.BonusTrait);
		}

		// Honnor
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Honneur : [/color]{0}\n\n", school.InitialHonnor.TotalValue);

		// Skills
		sb.AppendLine("[color=" + LocalContext.GOLD + "]Compétences[/color]\n");
		foreach (var item in school.Skills)
		{
			if(item.Specialisations.Count() == 0)
				sb.AppendFormat("- {1} {0}\n", item.Name, item.Rank);
			else
			{
				sb.AppendFormat("- {1} {0} [{2}]\n", item.Name, item.Rank, String.Join(", ", item.Specialisations.Select(s=>s.Name)));
			}
		}

		// Optional Skills
		foreach (var item in school.OptSkills)
		{
			sb.AppendFormat(" - {0}\n", item.Description);
		}
		sb.AppendLine();

		// Equipment
		sb.AppendLine("[color=" + LocalContext.GOLD + "]Equipement[/color]\n");
		foreach (var item in school.Equipments)
		{
			sb.AppendFormat(" - {0}\n", item.Name);
		}

		// Optional Equipment
		foreach (var item in school.OptEquipments)
		{
			sb.AppendFormat(" - {0}\n", item.Description);
		}

		// Gold
		List<string> money = new List<string>();
		if (school.KokuInitial > 0 )
			money.Add(school.KokuInitial + " Koku");
		if (school.BuInitial > 0)
			money.Add(school.BuInitial + " Bu");
		if (school.ZeniInitial > 0)
			money.Add(school.ZeniInitial + " Zeni");
		sb.AppendFormat(" - {0}\n\n", string.Join(", ", money));

		// Spells if any
		if (school.Spells != null)
		{
			sb.AppendLine("[color=" + LocalContext.GOLD + "]Sorts[/color]\n");
			sb.AppendFormat(" Affinité : {0}\n", school.Spells.Affinite);
			sb.AppendFormat(" Déficience : {0}\n", school.Spells.Deficience);
			if (school.Spells.NbrSortAir > 0)
				sb.AppendFormat(" Sorts d'Air : {0}\n", school.Spells.NbrSortAir);
			if (school.Spells.NbrSortEau > 0)
				sb.AppendFormat(" Sorts d'Eau : {0}\n", school.Spells.NbrSortEau);
			if (school.Spells.NbrSortFeu > 0)
				sb.AppendFormat(" Sorts de Feu : {0}\n", school.Spells.NbrSortFeu);
			if (school.Spells.NbrSortTerre > 0)
				sb.AppendFormat(" Sorts de Terre : {0}\n", school.Spells.NbrSortTerre);
			if (school.Spells.NbrSortVide > 0)
				sb.AppendFormat(" Sorts de Vide : {0}\n", school.Spells.NbrSortVide);
			sb.AppendLine();
		}

		// Description
		sb.Append(school.Description);

		_stats.BbcodeText = sb.ToString();
	}

	public void SetSchool(AlternativeSchool school)
	{
		if (school == null)
		{
			_name.Text = "";
			_stats.Text = "";
			_techniques.SetList(null);
			return;
		}
		_name.Text = school.Name;
		_techniques.SetList(school.Techniques);
		StringBuilder sb = new StringBuilder();

		// Balise
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]{0}[/color] - {1}\n\n", (school.MotClef?.ToString() ?? "Rang"), school.RequiredRank);

		// Conditions
		if(school.Conditions.Count > 0)
		{
			sb.Append("[color=" + LocalContext.GOLD + "]Conditions[/color]\n");
			foreach (var cdt in school.Conditions)
			{
				sb.AppendFormat("- {0}\n", cdt.Description);
			}
			sb.AppendLine();
		}

		// Description
		sb.Append(school.Description);

		_stats.BbcodeText = sb.ToString();
	}
}
