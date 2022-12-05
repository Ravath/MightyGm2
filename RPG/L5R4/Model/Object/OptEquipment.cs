using System;
using System.Collections.Generic;
using System.Linq;
using MightyGm2.RPG.L5R4.Data;
using L5R.Model.Agent;
using L5R.Model.Skill;
using Engine.Filter;
using Engine.RpgLogic;
using MightyGm2.Engine.Database;
using MightyGm2.Engine.RpgDatabase;
using MightyGm2.RPG.L5R4.Model;

namespace L5R.Model.Object
{
	public class WeaponTypeFilter : AbsDefaultFilter<Arme>
	{
		public TypeArme Type { get; set; }
		public WeaponTypeFilter(TypeArme type) { Type = type; }

        public override bool ValidItem(Arme item) { return item.Type == Type; }
	}

	/// <summary>
	/// An optionnal equipment from a school at character creation.
	/// </summary>
	public abstract class OptEquipment : OptCharacterCreation<L5R_Object>
	{
	}

	public class DefaultOptEquipment : OptEquipment
	{
		private List<L5R_Object> _choices = new List<L5R_Object>();
		private List<IModifier<Agent.Agent>> _applied = new List<IModifier<Agent.Agent>>();

		public override bool IsChoiceValid { get { return _choices.Count() == Number; } }

		/// <summary>
		/// Null means "No Armor to choose"
		/// </summary>
		public IFilter<Armure> ArmorFilter { get; internal set; }
		/// <summary>
		/// Null means "No Weapon to choose"
		/// </summary>
		public IFilter<Arme> WeaponFilter { get; internal set; }

		public override IEnumerable<L5R_Object> GetChoices(IEnumerable<L5R_Object> data)
		{
			if(ArmorFilter != null)
			{
                return ArmorFilter.Filter(data.OfType<Armure>());
			}
			else
            {
                return WeaponFilter.Filter(data.OfType<Arme>());
			}
		}

		public override void SetChoice(IEnumerable<L5R_Object> choices)
		{
			_choices.Clear();
			foreach (var item in choices)
			{
				_choices.Add(item);
			}
		}

		public override void ApplyChoices(Personnage character)
		{
			_applied.Clear();
			foreach (var item in _choices)
			{
				if(item is Arme arme)
				{
					character.Armes.GetObject(arme);
					_applied.Add(arme);
				}
				else if(item is Armure armure)
                {
					character.Armures.GetObject(armure);
					_applied.Add(armure);
				}
			}
		}

		public override void UnapplyChoices(Personnage character)
		{
			foreach (var item in _applied)
			{
				if (item is Arme arme)
				{
					character.Armes.ThrowObject(arme);
				}
				else if (item is Armure armure)
				{
					character.Armures.ThrowObject(armure);
				}
			}
			_applied.Clear();
		}
	}
}
