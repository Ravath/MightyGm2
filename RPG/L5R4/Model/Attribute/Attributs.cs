using System;
using System.Collections.Generic;
using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;

namespace L5R.Model.Attribute {
	public class Attributs : AgentPart<Agent.Agent> {
		#region Properties : Attributs
		public Attribut Constitution { get; }
		public Attribut Volonte { get; }
		public Attribut Force { get; }
		public Attribut Perception { get; }
		public Attribut Reflexes { get; }
		public Attribut Intuition { get; }
		public Attribut Agilite { get; }
		public Attribut Intelligence { get; }

		public Anneau Terre { get; }
		public Anneau Eau { get; }
		public Anneau Air { get; }
		public Anneau Feu { get; }

		public CompositeJauge Vide { get; }
		public Attribut MaxVide { get; }
		#endregion
		#region Properties : Accessors
		public IEnumerable<Anneau> FourRings {
			get {
				yield return Terre;
				yield return Eau;
				yield return Air;
				yield return Feu;
			}
		}
		public IEnumerable<Attribut> AllAttributs {
			get {
				yield return Constitution;
				yield return Volonte;
				yield return Force;
				yield return Perception;
				yield return Reflexes;
				yield return Intuition;
				yield return Agilite;
				yield return Intelligence;
			}
		}
		#endregion

		#region Init
		public Attributs( Agent.Agent agent ) : base(agent) {
			//attributs
			Constitution = new Attribut() { Label = "Constitution" };
			Volonte = new Attribut() { Label = "Volonte" };
			Force = new Attribut() { Label = "Force" };
			Perception = new Attribut() { Label = "Perception" };
			Reflexes = new Attribut() { Label = "Reflexes" };
			Intuition = new Attribut() { Label = "Intuition" };
			Agilite = new Attribut() { Label = "Agilite" };
			Intelligence = new Attribut() { Label = "Intelligence" };
			//4 anneaux base
			Terre = new Anneau(Constitution, Volonte) { Label = "Terre" };
			Eau = new Anneau(Force, Perception) { Label = "Eau" };
			Air = new Anneau(Reflexes, Intuition) { Label = "Air" };
			Feu = new Anneau(Agilite, Intelligence) { Label = "Feu" };
			//vide
			MaxVide = new Attribut() { Label = "Vide" };
			Vide = new CompositeJauge(0, MaxVide, 0);
		}

        public void SetPersonnage( PersonnageModel perso ) {
			Constitution.BaseValue = perso.Constitution;
			Volonte.BaseValue = perso.Volonte;
			Force.BaseValue = perso.Force;
			Perception.BaseValue = perso.Perception;
			Reflexes.BaseValue = perso.Reflexes;
			Intuition.BaseValue = perso.Intuition;
			Agilite.BaseValue = perso.Agilite;
			Intelligence.BaseValue = perso.Intelligence;
            MaxVide.BaseValue = perso.Vide ?? 0;
		} 
		#endregion

		public Attribut GetAttribut( TraitCompetence trait ) {
			switch(trait) {
				case TraitCompetence.Reflexes:
				return Reflexes;
				case TraitCompetence.Intuition:
				return Intuition;
				case TraitCompetence.Agilite:
				return Agilite;
				case TraitCompetence.Intelligence:
				return Intelligence;
				case TraitCompetence.Perception:
				return Perception;
				case TraitCompetence.Force:
				return Force;
				case TraitCompetence.Volonte:
				return Volonte;
				case TraitCompetence.Constitution:
				return Constitution;
				case TraitCompetence.Vide:
				return MaxVide;
				default:
				return null;
			}
		}
		public Attribut GetAttribut(MightyGm2.RPG.L5R4.Data.Trait trait ) {
			switch(trait) {
				case MightyGm2.RPG.L5R4.Data.Trait.Reflexes:
				return Reflexes;
				case MightyGm2.RPG.L5R4.Data.Trait.Intuition:
				return Intuition;
				case MightyGm2.RPG.L5R4.Data.Trait.Agilite:
				return Agilite;
				case MightyGm2.RPG.L5R4.Data.Trait.Intelligence:
				return Intelligence;
				case MightyGm2.RPG.L5R4.Data.Trait.Perception:
				return Perception;
				case MightyGm2.RPG.L5R4.Data.Trait.Force:
				return Force;
				case MightyGm2.RPG.L5R4.Data.Trait.Volonte:
				return Volonte;
				case MightyGm2.RPG.L5R4.Data.Trait.Constitution:
				return Constitution;
				default:
				return null;
			}
        }

        public IValue GetRing(MightyGm2.RPG.L5R4.Data.Anneau ring)
        {
            switch (ring)
            {
                case MightyGm2.RPG.L5R4.Data.Anneau.Feu:
                    return Feu;
                case MightyGm2.RPG.L5R4.Data.Anneau.Air:
                    return Air;
                case MightyGm2.RPG.L5R4.Data.Anneau.Terre:
                    return Terre;
                case MightyGm2.RPG.L5R4.Data.Anneau.Eau:
                    return Eau;
                case MightyGm2.RPG.L5R4.Data.Anneau.Vide:
                    return MaxVide;
                default:
                    return null;
            }
        }

        public void ResetAttributes()
		{
			foreach (var item in AllAttributs)
			{
				item.BaseValue = 2;
			}
		}
    }
}
