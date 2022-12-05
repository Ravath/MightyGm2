using System.Collections.Generic;
using System.Linq;
using L5R.Model.School;
using L5R.Model.Agent;
using MightyGm2.Engine.Process;
using MightyGm2.Engine.RpgDatabase;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;

namespace L5R4.Control
{
	/// <summary>
	/// Choose a Clan, a Familly and school.
	/// </summary>
	public class ClanStep : IProcessStep
	{
		#region Members
		private ClanModel _selectedClan;
		private PersonnageProcess _pross;
		#endregion

		#region Properties
		public IEnumerable<RpgDataElement> SelectableClan { get; private set; }
		public ClanModel SelectedClan
		{
			get
			{
				return _selectedClan;
			}
			set
			{
				if (_selectedClan != value)
				{
					_selectedClan = value;
					//Reset dependant
					SelectedFamily = null;
					SelectedSchool = null;
				}
			}
		}
		public IEnumerable<RpgDataElement> SelectableFamily { get; private set; }
		public FamilleModel SelectedFamily { get; set; }
		public IEnumerable<RpgDataElement> SelectableSchool { get; private set; }
		public EcoleModel SelectedSchool { get; set; }
		private Personnage Personnage { get { return _pross.Personnage; } }
		public PersonnageParameters Params { get { return (PersonnageParameters)_pross.Parameters; } }
		#endregion

		public bool CanProgress(out string errorMessageTag)
		{
			bool error = false;
			errorMessageTag = "";
			if (SelectedClan == null)
			{
				errorMessageTag = "You must choose a Clan";
				error = true;
			}
			else if (SelectedFamily == null)
			{
				errorMessageTag = "You must choose a Family";
				error = true;
			}
			else if (SelectedSchool == null)
			{
				errorMessageTag = "You must choose a School";
				error = true;
			}
			return !error;
		}

		public void Init(IProcess process)
		{
			_pross = (PersonnageProcess)process;

			SelectedClan = null;
			SelectedFamily = null;
			SelectedSchool = null;

            //Clan filters
            SelectableClan = process.Data.GetElements((int)L5R4_DataCategory.Clan).OrderBy(n => n.Name);
            SelectableSchool = process.Data.GetElements((int)L5R4_DataCategory.School).OrderBy(n => n.Name);
            SelectableFamily = process.Data.GetElements((int)L5R4_DataCategory.Family).OrderBy(n => n.Name);
            //TODO
            //SelectableClan = process.Data.GetTable<ClanModel>()
			//	.Where(c=> c.Tag != "CLN0009" || Params.SpiderClanAllowed)
			//	.Where(c => c.Tag == "CLN0009" || c.TypeClan != TypeClan.Majeur || Params.MajorClanAllowed)
			//	.Where(c => c.TypeClan != TypeClan.Mineur || Params.MinorClanAllowed)
			//	.Where(c => c.TypeClan != TypeClan.Confrerie || Params.MonkAllowed)
			//	.Where(c => c.Tag != "CLN0000" || Params.RoninAllowed)
			//	.Where(c => c.Tag != "CLN0010" || Params.ImperialClanAllowed)
			//	.OrderBy(c => c.Tag);
			//SelectableSchool = process.Data.GetTable<EcoleModel>()
			//	.Where(s => s.Devotion == null || Params.MonkAllowed)
			//	.Where(s => (s.Balise != BaliseEcole.Bushi && s.Balise != BaliseEcole.Ninja) || Params.BushiAllowed)
			//	.Where(s => (s.Balise != BaliseEcole.Courtisan && s.Balise != BaliseEcole.Artisan) || Params.CourtierAllowed)
			//	.Where(s => s.Devotion != null || (s.Balise != BaliseEcole.Shugenja && s.Balise != BaliseEcole.Moine) || Params.ShugenjaAllowed)
			//	.OrderBy(n => n.Name);
			//SelectableFamily = process.Data.GetTable<FamilleModel>().OrderBy(n => n.Name);
		}

		public void Reset(){ /* Nothing */ }

		public void PreprossNext()
		{
			//clan
			Personnage.Clan.SetModel(SelectedClan);
			//famille
			Personnage.Famille.SetModel(SelectedFamily);
			Personnage.Attributs.GetAttribut(Personnage.Famille.BonusTrait).BaseValue++;
            //ecole
            Ecole e = ModelFactory.Factory.InstantiateSchool(SelectedSchool);
			Personnage.Ecoles.AddSchool(e, 1);
			Personnage.Attributs.GetAttribut(e.BonusTrait).BaseValue++;
			Personnage.Money.SetMoney(e.KokuInitial, e.BuInitial, e.ZeniInitial);
			// gloire, status et honneur
			Personnage.Status.SetRank(1, 0);
			Personnage.Gloire.SetRank(1, 0);
			Personnage.Souillure.SetRank(0, 0);
			Personnage.Honneur.SetRank(e.InitialHonnor);
		}

		public void PreprossReset()
		{
			Personnage.Ecoles.ClearSchools();
			Personnage.Attributs.ResetAttributes();
		}

		public string GetStepMessage()
		{
			return "Choose a Clan, Family and School";
		}
	}
}
