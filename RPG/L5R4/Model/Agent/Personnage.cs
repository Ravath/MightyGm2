using Engine.RpgLogic;
using L5R.Model.Attribute;
using L5R.Model.School;
using MightyGm2.RPG.L5R4.Data;
using L5R4.JdrCore;
using MightyGm2.Engine.RpgDatabase;

namespace L5R.Model.Agent {
	public class Personnage : Agent {

		#region Members
		#endregion

		#region Properties
		public Player Joueur { get; set; }
		public RangReputation Reputation { get; }
		public Famille Famille { get;}
		public Clan Clan { get;}
		public ListeEcoles Ecoles { get; }
		public Money Money { get; }
		#endregion

		#region Init
		public Personnage() {
			/* init les caractéristiques spécifiques */
			Reputation = new RangReputation(this);
			/* linker l'initiative */
			IndirectValue initRoll = new IndirectValue(Attributs.Reflexes);
			initRoll.AddModifier(Reputation);
			Initiative = new RollAndKeep(initRoll, Attributs.Reflexes);
			/* vie */
			Vie = new SeuilViePJ(this);
			//autres
			Famille = new Famille();
			Clan = new Clan();
			Ecoles = new ListeEcoles(this);
			Money = new Money();
        }
		public Personnage(PersonnageJoueurModel model) : this()
		{
			SetPersonnage(model);
		}
		#endregion

		public override void SetPersonnage(PersonnageJoueurModel model)
		{
			//TODO : init set pj
			/* clear everything */
			Ecoles.ClearSchools();
			/* set values */
            //TODO set Models
			//Famille.SetModel(model.Famille);
			//Clan.SetModel(model.Clan);

			Armes.SetPersonnageModel(model);

			// base settings + event
			base.SetPersonnage(model);
		}
	}
}
