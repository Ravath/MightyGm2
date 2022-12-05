using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MightyGm2.RPG.L5R4.Data;
using L5R.Model.Trait;
using MightyGm2.Engine.Process;
using MightyGm2.Engine.RpgDatabase;
using MightyGm2.RPG.L5R4.Model;

namespace L5R4.Control
{
	public class AdvantageStep : IProcessStep
	{
		private PersonnageProcess _process;

		public PersonnageParameters Params { get { return (PersonnageParameters)_process.Parameters; } }

		public IEnumerable<RpgDataElement> Advantages { get; private set; }
		public IEnumerable<RpgDataElement> Disadvantages { get; private set; }

		public List<AvantageModel> ChosenAdvantages { get; } = new List<AvantageModel>();
		public List<AvantageModel> ChosenDisadvantages { get; } = new List<AvantageModel>();

		public bool CanProgress(out string ErrorMessageTag)
		{
			int diff = ChosenAdvantages.Sum(a => a.Cout) - Params.MaxAvantagePoints;
			if (diff > 0)
			{
				ErrorMessageTag = String.Format("You must choose {0} less advantage points", diff);
				return false;
			}

			diff = ChosenDisadvantages.Sum(a => a.Cout) - Params.MaxDesavantagePoints;
			if (diff > 0 && !Params.CanOvertakeDesavantagePointsLimit)
			{
				ErrorMessageTag = String.Format("You must choose {0} less disadvantage points", diff);
				return false;
			}

			var gi = ChosenAdvantages.GroupBy(a => a.Groupe_Tag);
			foreach (var item in gi)
			{
				if (item.Count() > 1)
				{
					ErrorMessageTag = String.Format("2 Advantages can't be of the same group", diff);
					return false;
				}
			}

			var gd = ChosenDisadvantages.GroupBy(a => a.Groupe_Tag);
			foreach (var item in gd)
			{
				if (item.Count() > 1)
				{
					ErrorMessageTag = String.Format("2 Disadvantages can't be of the same group", diff);
					return false;
				}
			}

			ErrorMessageTag = "Ok";
			return true;
		}

		public void Init(IProcess process)
		{
			_process = (PersonnageProcess)process;
            Advantages = process.Data.GetElements((int)L5R4_DataCategory.Advantage);
            Disadvantages = process.Data.GetElements((int)L5R4_DataCategory.Disadvantage);
            //Advantages = process.Data.GetTable<AvantageModel>();
            //Disadvantages = process.Data.GetTable<DesavantageModel>();
        }

        private List<Avantage> _applied = new List<Avantage>();

		public void PreprossNext()
		{
			_applied.Clear();

			foreach (var item in ChosenAdvantages)
			{
                Avantage a = ModelFactory.Factory.InstantiateAdvantage(item);
				_applied.Add(a);
				_process.Personnage.Avantages.AddTrait(a);
			}

			foreach (var item in ChosenDisadvantages)
            {
                Avantage a = ModelFactory.Factory.InstantiateAdvantage(item);
                _applied.Add(a);
				_process.Personnage.Avantages.AddTrait(a);
			}
		}

		public void PreprossReset()
		{
			foreach (var item in _applied)
			{
				_process.Personnage.Avantages.RemoveTrait(item);
			}
		}

		public void Reset()
		{
			ChosenAdvantages.Clear();
			ChosenDisadvantages.Clear();
		}

		public string GetStepMessage()
		{
			return "Choose advantages and disadvantages";
		}
	}
}
