using Engine.RpgLogic;
using Godot;
using L5R.Model.Agent;
using L5R.Model.Capacity;
using L5R.Model.Object;
using L5R.Model.School;
using L5R.Model.Skill;
using L5R.Model.Trait;
using L5R4;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model.Trait;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DataViewer : Control
{
	private CharacterSheet _characterSheet;
	private ClanPanel _clanPanel;
	private SchoolPanel _schoolPanel;
	private Control _defaultPanel;

	private Label _defaultNameLabel;
	private RichTextLabel _defaultSupplementLabel;

	public delegate string ConvertToSupplement(object data);
	private Dictionary<Type, ConvertToSupplement> _supplementConverter = new Dictionary<Type, ConvertToSupplement>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_characterSheet = GetNode<CharacterSheet>("CharacterSheet");
		_clanPanel = GetNode<ClanPanel>("ClanPanel");
		_schoolPanel = GetNode<SchoolPanel>("SchoolPanel");
		_defaultPanel = GetNode<Control>("DefaultPanel");
		_defaultNameLabel = GetNode<Label>("DefaultPanel/NameLabel");
		_defaultSupplementLabel = GetNode<RichTextLabel>("DefaultPanel/DescriptionLabel");

		AddConverter(typeof(Arme), GetWeaponSupplement);
		AddConverter(typeof(Armure), GetArmorSupplement);
		AddConverter(typeof(L5R_Object), GetObjectSupplement);
		AddConverter(typeof(Avantage), GetAdvantageSupplement);
		AddConverter(typeof(Kata), GetKataSupplement);
		AddConverter(typeof(Kiho), GetKihoSupplement);
		AddConverter(typeof(Sort), GetSpellSupplement);
		AddConverter(typeof(Tatoo), GetTatooSupplement);
		AddConverter(typeof(PouvoirOutremonde), GetPowerSupplement);
		AddConverter(typeof(Competence), GetSkillSupplement); 
		AddConverter(typeof(OpportuniteHeroiqueModel), GetHeroicOpportunitySupplement);
		AddConverter(typeof(TableHeritageModel), GetInheritanceSupplement);

	}

	public void AddConverter(Type type, ConvertToSupplement converter)
	{
		_supplementConverter.Add(type, converter);
	}

	public void SetData(object newData)
	{
		// Ensure every Panel is hidden
		foreach (Control item in GetChildren())
		{
			item.Visible = false;
		}

		if (newData == null)
			return;

		// Display panel corresponding to given type
		if (newData is Clan dataClan)
		{
			_clanPanel.SetClan(dataClan);
			_clanPanel.Visible = true;
		}
		else if (newData is Ecole school)
		{
			_schoolPanel.SetSchool(school);
			_schoolPanel.Visible = true;
		}
		else if (newData is AlternativeSchool avschool)
		{
			_schoolPanel.SetSchool(avschool);
			_schoolPanel.Visible = true;
		}
		else if (newData is Agent dataAgent)
		{
			_characterSheet.SetCharacter(dataAgent);
			_characterSheet.Visible = true;
		}
		else // Default
		{
			// Get Name and Description if has corresponding property
			_defaultNameLabel.Text = GetProperty("Name", newData);
			// Get Supplement information if has a corresponding coverter
			if (_supplementConverter.TryGetValue(newData.GetType(), out ConvertToSupplement converter))
			{
				_defaultSupplementLabel.BbcodeText = converter.Invoke(newData);
			}
			else
			{
				_defaultSupplementLabel.Text = GetProperty("Description", newData);
			}
			// Set visible
			_defaultPanel.Visible = true;
		}
	}

	private string GetProperty(string propName, object from)
	{
		var desProp = from.GetType().GetProperty(propName);
		return (desProp?.GetValue(from) as string) ?? "";
	}

	public string GetAdvantageSupplement(object data)
	{
		Avantage av = (Avantage)data;
		StringBuilder sb = new StringBuilder();

		// Cost
		if (av.RankMax > 1)
			sb.AppendFormat("[color=" + LocalContext.GOLD + "]Cout : [/color]{0} (Max. {1})\n\n", av.Cout, av.RankMax);
		else
			sb.AppendFormat("[color=" + LocalContext.GOLD + "]Cout : [/color]{0}\n\n", av.Cout);

		// Conditions if any
		if(av.AcqCondition.Count > 0)
		{
			sb.Append("[color=" + LocalContext.GOLD + "]Conditions : [/color]\n");
			foreach (var item in av.AcqCondition)
			{
				sb.AppendFormat(" - {0}\n", item.Description);
			}
			sb.AppendLine();
		}

		// Description
		sb.Append(av.Description);

		// Return
		return sb.ToString();
	}

	public string GetKataSupplement(object data)
	{
		Kata power = (Kata)data;
		StringBuilder sb = new StringBuilder();

		// Rank
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Anneau : [/color]{0} - {1}\n\n", power.Anneau, power.Rank);

		// Description
		sb.Append(power.Description);

		// Return
		return sb.ToString();
	}

	public string GetKihoSupplement(object data)
	{
		Kiho power = (Kiho)data;
		StringBuilder sb = new StringBuilder();

		// Type
		if(power.Atemi)
			sb.AppendFormat("[color=" + LocalContext.GOLD + "]{0} - Atemi[/color]\n", power.Type);
		else
			sb.AppendFormat("[color=" + LocalContext.GOLD + "]{0}[/color]\n", power.Type);

		// Rank
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Anneau : [/color]{0} - {1}\n\n", power.Anneau, power.Rank);

		// Description
		sb.Append(power.Description);

		// Return
		return sb.ToString();
	}

	public string GetSpellSupplement(object data)
	{
		Sort power = (Sort)data;
		StringBuilder sb = new StringBuilder();

		// Tags
		if(power.Keys.Count() > 0)
			sb.AppendFormat("[color=" + LocalContext.GOLD + "][ {0} ][/color]\n", string.Join(", ", power.Keys));

		// Rank
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Anneau : [/color]{0} - {1}\n\n", power.Element, power.Rank);

		// Stats
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Portee : [/color]{0}\n", power.UnitRange);
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Cibles : [/color]{0}\n", power.UnitZone);
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Durée : [/color]{0}\n", power.UnitDuration);
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Concentration : [/color]{0}\n\n", power.Concentration?"Oui":"Non");

		// Augmentations
		if (power.Augmentations.Count() > 0)
		{
			foreach (var item in power.Augmentations)
			{
				sb.AppendFormat(" - {0}\n", item.Description);
			}
			sb.AppendLine();
		}

		// Description
		sb.Append(power.Description);

		// Return
		return sb.ToString();
	}

	public string GetTatooSupplement(object data)
	{
		Tatoo power = (Tatoo)data;
		StringBuilder sb = new StringBuilder();

		// Description
		sb.Append(power.Description);

		// Return
		return sb.ToString();
	}

	public string GetPowerSupplement(object data)
	{
		PouvoirOutremonde power = (PouvoirOutremonde)data;
		StringBuilder sb = new StringBuilder();

		// Type
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Type : [/color]{0}\n\n", power.Type);

		// Description
		sb.Append(power.Description);

		// Return
		return sb.ToString();
	}

	private string GetWeaponSupplement(object data)
	{
		Arme arme = (Arme)data;
		StringBuilder sb = new StringBuilder();

		// Type and Tags
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]{0}[/color]\n", arme.Type);
		List<string> tags = new List<string>
		{
			arme.Taille.ToString()
		};
		if (arme.ArmePaysan)
			tags.Add("Paysan");
		if (arme.ArmeSamurai)
			tags.Add("Samurai");
		sb.AppendFormat("[color=" + LocalContext.GOLD + "][ {0} ][/color]\n\n", string.Join(", ", tags));

		// Dammage
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Degats : [/color]{0}\n\n", arme.Degats.ToString());

		// Specials
		if(arme.Specials.Count() > 0)
		{
			foreach (var item in arme.Specials)
			{
				sb.AppendFormat(" - {0}\n", item.Description);
			}
			sb.AppendLine();
		}

		// Cost
		PrintObjectCost(sb, arme);

		// Description
		sb.Append(arme.Description);

		// Return
		return sb.ToString();
	}

	public string GetArmorSupplement(object data)
	{
		Armure ar = (Armure)data;
		StringBuilder sb = new StringBuilder();

		// ND/Red
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]ND : [/color]{0}\n", ar.ND.BaseValue);
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Reduction : [/color]{0}\n\n", ar.Reduction.BaseValue);

		// Cost
		PrintObjectCost(sb, ar);

		// Specials
		if (ar.Specials.Count() > 0)
		{
			foreach (var item in ar.Specials)
			{
				sb.AppendFormat(" - {0}\n", item.Description);
			}
			sb.AppendLine();
		}

		// Description
		sb.Append(ar.Description);

		// Return
		return sb.ToString();
	}

	private static void PrintObjectCost(StringBuilder sb, L5R_Object obj)
	{
		if (obj.Valeur.Value == -1)
			sb.AppendFormat("[color=" + LocalContext.GOLD + "]Cout : [/color]{0}\n\n", "Spécial");
		else if(obj.ValeurMax != null)
			if(obj.ValeurMax.Value == -1)
				sb.AppendFormat("[color=" + LocalContext.GOLD + "]Cout : [/color]à partir de {0}\n\n", obj.Valeur);
			else
				sb.AppendFormat("[color=" + LocalContext.GOLD + "]Cout : [/color]de {0} à {1}\n\n", obj.Valeur, obj.ValeurMax);
		else
			sb.AppendFormat("[color=" + LocalContext.GOLD + "]Cout : [/color]{0}\n\n", obj.Valeur);
	}

	public string GetObjectSupplement(object data)
	{
		L5R_Object obj = (L5R_Object)data;
		StringBuilder sb = new StringBuilder();

		// Cost
		PrintObjectCost(sb, obj);

		// Specials
		if (obj.Specials.Count() > 0)
		{
			foreach (var item in obj.Specials)
			{
				sb.AppendFormat(" - {0}\n", item.Description);
			}
			sb.AppendLine();
		}

		// Description
		sb.Append(obj.Description);

		// Return
		return sb.ToString();
	}

	public string GetSkillSupplement(object data)
	{
		Competence competence = (Competence)data;
		StringBuilder sb = new StringBuilder();

		// Type
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]{0}[/color]\n\n", competence.Groupe);

		// Traits
		if(competence.TraitAlternatif != null)
			sb.AppendFormat("[color=" + LocalContext.GOLD + "]{0} / {1}[/color]\n\n", competence.Trait, competence.TraitAlternatif);
		else
			sb.AppendFormat("[color=" + LocalContext.GOLD + "]{0}[/color]\n\n", competence.Trait);

		// Tags
		List<string> tags = new List<string>();
		if (competence.Degradante)
			tags.Add("Degradante");
		if (competence.Noble)
			tags.Add("Noble");
		if (competence.Sociale)
			tags.Add("Sociale");
		if (competence.Martiale)
			tags.Add("Martiale");
		if(tags.Count > 0)
			sb.AppendFormat("[color=" + LocalContext.GOLD + "][{0}][/color]\n\n", string.Join(", ", tags));

		// Specialisations
		if(competence.Specialisations.Count() > 0)
		{
			sb.AppendFormat("[color=" + LocalContext.GOLD + "]Specialisations : [/color] {0}\n\n", string.Join(", ", competence.Specialisations.Select(s=>s.Name)));
		}

		// Maitrises
		if (competence.AllMaitrises.Count() > 0)
		{
			sb.Append("[color=" + LocalContext.GOLD + "]Maitrises : [/color]\n");
			foreach (var item in competence.AllMaitrises)
			{
				sb.AppendFormat(" - [color=" + LocalContext.GOLD + "]{1} :[/color] {0}\n", item.Description, item.Rang);
			}
			sb.AppendLine();
		}

		// Description
		sb.Append(competence.Description);

		// Return
		return sb.ToString();
	}

	public string GetHeroicOpportunitySupplement(object data)
	{
		OpportuniteHeroiqueModel opp = (OpportuniteHeroiqueModel)data;
		StringBuilder sb = new StringBuilder();

		// Type
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Action : [/color]{0}\n\n", opp.Action);

		// Description
		sb.Append(opp.Description);

		// Return
		return sb.ToString();
	}

	public string GetInheritanceSupplement(object data)
	{
		TableHeritageModel heritance = (TableHeritageModel)data;
		StringBuilder sb = new StringBuilder();

		//TODO use a decent type when available, and probably a specific panel

		// Type
		sb.AppendFormat("[color=" + LocalContext.GOLD + "]Chances : [/color]{0}\n\n", heritance.Chances);

		foreach (var item in heritance.Consequences)
		{
			sb.AppendFormat(" - [color=" + LocalContext.GOLD + "]{0} : [/color]{1}\n\n", item.Chances, item.Description);
		}

		// Description
		sb.Append(heritance.Description);

		// Return
		return sb.ToString();
	}
}
