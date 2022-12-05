using System;
using MightyGm2.RPG.L5R4.Data;
using L5R.Model.Skill;
using MightyGm2.Engine.Process;

namespace L5R4.Control
{
	/// <summary>
	/// Spend XP within limits if PJ.
	///		Specific from usal XP spending : 
	///			- 40 at start (can change with parameters).
	///			- Can buy Avantages or desavantages, or Ancesters if allowed.
	///			- Specific limits on objects.
	///	Else, wathever if MJ/Monster
	///		-	Powers and capacities if monster.
	/// </summary>
	public class XPStep : IProcessStep
	{
		public PersonnageProcess Process { get; private set; }

		public bool CanProgress(out string ErrorMessageTag)
		{
			throw new NotImplementedException();
		}

		public void Init(IProcess process)
		{
			Process = (PersonnageProcess)process;

			EcoleModel selectedSchool = ((ClanStep)Process.GetStep(0)).SelectedSchool;
			foreach (var item in selectedSchool.Competences)
			{
				Process.Personnage.Competences.AddCompetence(new Competence(item));
			}

			////init attributes
			//foreach (var item in _params.Personnage.Attributs.AllAttributs)
			//{
			//	item.BaseValue = 2;
			//}
			//_params.Personnage.Attributs.MaxVide.BaseValue = 2;
			////set data
			//fiche.SetData(_params.Data);
		}

		public void PreprossNext()
		{
			throw new NotImplementedException();
		}

		public void PreprossReset()
		{
			throw new NotImplementedException();
		}

		public void Reset()
		{
			EcoleModel selectedSchool = ((ClanStep)Process.GetStep(0)).SelectedSchool;
			foreach (var item in selectedSchool.Competences)
            {
                Process.Personnage.Competences.RemoveCompetenceByTag(item.Competence_Tag);
			}
		}

		public string GetStepMessage()
		{
			return "Spend your XP";
		}
	}
}
