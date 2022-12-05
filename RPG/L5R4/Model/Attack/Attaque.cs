using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L5R.Model.Skill;
using L5R.Model.Object;
using L5R4.JdrCore;
using MightyGm2.RPG.L5R4.Data;
using Engine.RpgLogic;

namespace L5R.Model.Attack {
	public interface IAttaque : INamed {
		RollAndKeep JetAttaque { get ;}
		RollAndKeep Degats { get; }
        MightyGm2.RPG.L5R4.Data.Action Action { get; }
	}

	public class AttaqueCreature : IAttaque {

		public string Name { get; protected set; }
		public RollAndKeep JetAttaque { get; private set; }
		public RollAndKeep Degats { get; private set; }
		public MightyGm2.RPG.L5R4.Data.Action Action { get; private set; }

		public AttaqueCreature(AttaqueFigurant att) {
			SetAttaqueCreature(att);
        }

		public void SetAttaqueCreature( AttaqueFigurant att )
		{
            JetAttaque = new RollAndKeep(att.Toucher);
            JetAttaque.RollValue.Label = "Attaque Lancés";
            JetAttaque.KeepValue.Label = "Attaque Gardés";
            Degats = new RollAndKeep(att.Degats);
            Degats.RollValue.Label = "Dégats Lancés";
            Degats.KeepValue.Label = "Dégats Gardés";
			Action = att.Action;
			Name = att.Name;
		}
	}

	public class AttaqueArme : IAttaque {

		public Arme Arme { get; private set; }

		public string Name { get { return Arme.Name; } }
		public Competence Competence { get; set; }
		public RollAndKeep JetAttaque { get { return Competence.Pool; } }
		public RollAndKeep Degats { get { return Arme.Degats; } }
		public MightyGm2.RPG.L5R4.Data.Action Action { get; private set; }

		public AttaqueArme() { }

		public void SetArme( Arme arme , Agent.Agent agent ) {
			Arme = arme;
			Action = MightyGm2.RPG.L5R4.Data.Action.Complexe;
			/* find used compétence */
			Competence = agent.Competences.GetCompetenceArme(arme.Type);
			//TODO spécialisation compétence
		}
	}
}
