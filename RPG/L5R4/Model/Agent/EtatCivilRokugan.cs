using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;

namespace L5R.Model.Agent {
	public class EtatCivilRokugan : EtatCivil {

		public void SetPersonnage(PersonnageModel perso)
		{
			Name = perso.Name;
			Description = perso.Description;
		}
	}
}
