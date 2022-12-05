using Engine.RpgLogic;
using L5R.Model.Agent;
using L5R.Model.Trait;
using MightyGm2.RPG.L5R4.Data;

namespace L5R.Model.Object {
	public class Armure : L5R_Object, IAgentEffect
    {
		public Value ND { get; set; }
		public Value Reduction { get; set; }

        public Armure() {
			ND = new Value(0) { Label = "Armure" };
			Reduction = new Value(0) { Label = "Armure" };
            AddEffect(this);
		}

		public void Affect(Agent.Agent personnage ) {
			Agent.Agent p = (Agent.Agent)personnage;
			p.Armures.ND.RemoveModifier(ND);
			p.Armures.Reduction.RemoveModifier(Reduction);
		}

		public void Unaffect(Agent.Agent personnage ) {
			Agent.Agent p = (Agent.Agent)personnage;
			p.Armures.ND.AddModifier(ND);
			p.Armures.Reduction.AddModifier(Reduction);
		}

        public void SetComplement(string complement)
        {
            // NOTHING
        }
    }
}
