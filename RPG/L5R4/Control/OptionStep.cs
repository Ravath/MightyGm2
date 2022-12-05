using System.Collections.Generic;
using L5R.Model.Skill;
using L5R.Model.Object;
using System.Data.Entity;
using MightyGm2.Engine.Process;
using MightyGm2.Engine.Database;
using MightyGm2.RPG.L5R4.Model;

namespace L5R4.Control
{
	/// <summary>
	/// Options to choose
	///		- School skills
	///		- School items
	/// </summary>
	public class OptionStep : IProcessStep
	{
		#region Members
		private List<OptCompetence> _cpts = new List<OptCompetence>();
		private List<OptEquipment> _eqps = new List<OptEquipment>();
		#endregion

		#region Properties
		public IEnumerable<OptCompetence> OptionsCompetence { get { return _cpts; } }
		public IEnumerable<OptEquipment> OptionsEquipment { get { return _eqps; } }
		public MightyDb Data { get { return Process.Data; } }
		public PersonnageProcess Process { get; private set; }
		#endregion

		public bool CanProgress(out string ErrorMessageTag)
		{
			foreach (OptCompetence opt in _cpts)
			{
				if (!opt.IsChoiceValid)
				{
					ErrorMessageTag = opt.Number + "x " + opt.Description;
					return false;
				}
			}

			foreach (OptEquipment opt in _eqps)
			{
				if (!opt.IsChoiceValid)
				{
					ErrorMessageTag = opt.Number + "x " + opt.Description;
					return false;
				}
			}

			ErrorMessageTag = "OK";
			return true;
		}

		public string GetStepMessage()
		{
			return "Resolve all your school options";
		}

		public void Init(IProcess process)
		{
			Process = (PersonnageProcess)process;

			ClanStep cs = Process.GetStep(0) as ClanStep;

			_cpts.Clear();
			foreach (var item in cs.SelectedSchool.CompetencesOpt)
			{
				_cpts.Add(OptCompetenceInstantiate.Instanciate(item));
			}

			_eqps.Clear();
			foreach (var item in cs.SelectedSchool.EquipementsOpt)
			{
				_eqps.Add(ModelFactory.Factory.InstantiateEquipmentOption(item));
			}
		}

		public void PreprossNext()
		{
			foreach (var item in _cpts)
			{
				item.ApplyChoices(Process.Personnage);
			}
			foreach (var item in _eqps)
			{
				item.ApplyChoices(Process.Personnage);
			}
		}

		public void PreprossReset()
		{
			foreach (var item in _cpts)
			{
				item.UnapplyChoices(Process.Personnage);
			}
			foreach (var item in _eqps)
			{
				item.UnapplyChoices(Process.Personnage);
			}
		}

		public void Reset()
		{
			_cpts.Clear();
			_eqps.Clear();
		}
	}
}
