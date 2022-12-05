using System.Collections.Generic;
using Engine.RpgLogic;
using L5R.Model.Agent;
using L5R.Model.Attack;
using MightyGm2.RPG.L5R4.Data;
using L5R4.JdrCore;
using MightyGm2.RPG.L5R4.Model;
using L5R.Model.Trait;

namespace L5R.Model.Object {

	public class Arme : L5R_Object
    {
		private AttaqueArme _attaque = new AttaqueArme();

        public RollAndKeep Degats { get; set; }
		public TypeArme Type { get; set; }
		public TailleArme Taille { get; set; }
		public bool Brisee { get; set; }

		public bool ArmePaysan { get; set; }
		public bool ArmeSamurai { get; set; }

		public Arme() { }

		public void Equiper( IAgent agent ) {
			Personnage personnage = (Personnage)agent;
            _attaque.SetArme(this, personnage);
			personnage.Armes.AddAttack(_attaque);
		}

		public void Desequiper( IAgent agent )
		{
			Personnage personnage = (Personnage)agent;
			personnage.Armes.RemoveAttack(_attaque);
			_attaque = null;
		}
	}
}
