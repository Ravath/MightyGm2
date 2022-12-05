using System;
using System.Collections.Generic;
using System.Linq;
using MightyGm2.RPG.L5R4.Data;
using Engine.Filter;
using L5R.Model.Agent;
using System.Data.Entity;
using MightyGm2.Engine.Database;
using MightyGm2.Engine.RpgDatabase;

namespace L5R.Model.Skill
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class OptCharacterCreation<T>
	{
		public int Number { get; set; }
		public String Description { get; set; }

        public OptCharacterCreation()
        {

        }

		public OptCharacterCreation(int number, string description)
		{
			Number = number;
			Description = description;
		}

		public abstract IEnumerable<T> GetChoices(IEnumerable<T> data);
		public abstract void SetChoice(IEnumerable<T> choices);

		public abstract bool IsChoiceValid { get; }

		public abstract void ApplyChoices(Personnage character);
		public abstract void UnapplyChoices(Personnage character);
	}

	/// <summary>
	/// An optionnal skill from a school at character creation.
	/// </summary>
	public abstract class OptCompetence : OptCharacterCreation<Competence>
	{
	}

	/// <summary>
	/// When have to choose among a skill set.
	/// </summary>
	public class DefaultOptCompetence : OptCompetence
	{
		private List<Competence> _choices = new List<Competence>();

		public override bool IsChoiceValid { get { return _choices.Count() == Number; } }
		public int CompetenceRank { get; set; }

		public IFilter<Competence> Filter { get; internal set; }

		public override IEnumerable<Competence> GetChoices(IEnumerable<Competence> data)
		{
			return Filter.Filter(data);
		}

		public override void SetChoice(IEnumerable<Competence> choices)
		{
			_choices.Clear();
            _choices.AddRange(choices);
		}

		public override void ApplyChoices(Personnage character)
		{
			foreach (var item in _choices)
			{
				item.Rank = CompetenceRank;
				character.Competences.AddCompetence(item);
			}
		}

		public override void UnapplyChoices(Personnage character)
		{
			foreach (var item in _choices)
			{
				character.Competences.RemoveCompetence(item);
			}
		}
	}

	/// <summary>
	/// When have to choose a specialization from a specific skill.
	/// </summary>
	public class DefaultOptSpecialisation : OptCompetence
    {
		private List<Competence> _choices = new List<Competence>();

		public override bool IsChoiceValid { get { return _choices.Count() == Number; } }

		public IFilter<Competence> Filter { get; internal set; }

		public override IEnumerable<Competence> GetChoices(IEnumerable<Competence> data)
        {
            List<Competence> _specialisations = new List<Competence>();
            foreach (var cpt in Filter.Filter(data))
            {
                foreach (var item in cpt.Specialisations)
                {
                    Competence newC = new Competence()
                    {
                        Name = cpt.Name
                    };
                    newC.AddSpecialisation(item);
                    _specialisations.Add(newC);
                }
            }
            return _specialisations;
		}

		public override void SetChoice(IEnumerable<Competence> choices)
		{
			_choices.Clear();
            _choices.AddRange(choices);
		}

		public override void ApplyChoices(Personnage character)
		{
			foreach (var item in _choices)
			{
				character.Competences.GetCompetenceByTag(item.Tag).AddSpecialisation(item.Specialisations.ElementAt(0));
			}
		}

		public override void UnapplyChoices(Personnage character)
		{
			foreach (var item in _choices)
			{
				character.Competences.GetCompetenceByTag(item.Tag).RemoveSpecialisation(item.Specialisations.ElementAt(0));
			}
		}
	}
}
