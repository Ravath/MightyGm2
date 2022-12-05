using System;
using System.Collections.Generic;
using System.Linq;
using MightyGm2.RPG.L5R4.Data;
using L5R.Model.Capacity;
using MightyGm2.Engine.Process;
using MightyGm2.Engine.RpgDatabase;

namespace L5R4.Control
{
	/// <summary>
	/// Selection of spells if shugenja.
	/// else, nothing.
	/// The Monks have 3 free Kihos, but have to be selected at XP spending
	/// because they have to meet the conditions.
	/// </summary>
	public class SpellStep : IProcessStep
	{
		private PersonnageProcess _process;
		private List<Sort> _applied = new List<Sort>();

		public IEnumerable<RpgDataElement> Spells { get; private set; }
		public List<SortModel> ChosenSpells { get; set; } = new List<SortModel>();

		public bool CanProgress(out string ErrorMessageTag)
		{
			foreach (Anneau item in Enum.GetValues(typeof(Anneau)))
			{
				int diff = GetSpellNumber(item) - ChosenSpells.Where(s => s.Element == item.Convert()).Count();
				if(diff != 0)
				{
					ErrorMessageTag = String.Format("You must still choose {0} spells of {1} element.", diff, item);
					return false;
				}
			}

			ErrorMessageTag = "OK";
			return true;
		}

		public void Init(IProcess process)
		{
			_process = (PersonnageProcess)process;

            Spells = _process.Data.GetElements((int)L5R4_DataCategory.Spell);
            //TODO //.Where(s => s.Maitrise == 1);
		}

		public void PreprossNext()
		{
			_applied.Clear();
			foreach (var item in ChosenSpells)
			{
				Sort s = new Sort(item);
				_process.Personnage.Sorts.AddCapacity(s);
				_applied.Add(s);
			}
		}

		public void PreprossReset()
		{
			foreach (var item in _applied)
			{
				_process.Personnage.Sorts.RemoveCapacity(item);
			}
			_applied.Clear();
		}

		public void Reset()
		{
			ChosenSpells.Clear();
		}

		public string GetStepMessage()
		{
			return "Choose your shugenja spells";
		}

		public int GetSpellNumber(Anneau item)
		{
			ClanStep cs = (ClanStep)_process.GetStep(0);
            SortsEcole sorts = cs.SelectedSchool.Sorts;
            if (sorts == null) return 0;
			switch (item)
			{
				case Anneau.Feu:
					return sorts.NbrSortFeu;
				case Anneau.Air:
					return sorts.NbrSortAir;
				case Anneau.Terre:
					return sorts.NbrSortTerre;
				case Anneau.Eau:
					return sorts.NbrSortEau;
				case Anneau.Vide:
					return sorts.NbrSortVide;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
