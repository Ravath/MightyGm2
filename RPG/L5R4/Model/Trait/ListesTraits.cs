using Engine.RpgLogic;
using L5R.Model.Capacity;
using System.Collections.Generic;
using System.Linq;

namespace L5R.Model.Trait {
	public class ListeAvantages : TraitCollection<Agent.Agent,Avantage>
    {
		public ListeAvantages(Agent.Agent agent) : base(agent) { }

		public IEnumerable<Avantage> Avantages { get { return base.Traits.Where(a => a.Desavantage == false); } }
		public IEnumerable<Avantage> Desavantages { get { return base.Traits.Where(a => a.Desavantage == true); } }
	}

	public class ListeTraitsCreature : TraitCollection<Agent.Agent, IModifier<Agent.Agent>>
    {
		public ListeTraitsCreature( Agent.Agent agent ) : base(agent) { }
	}

	public class ListeTechniques : TraitCollection<Agent.Agent, Technique>
    {
		public ListeTechniques( Agent.Agent agent ) : base(agent) { }
	}

	public class ListePouvoirsOutremonde : TraitCollection<Agent.Agent, PouvoirOutremonde>
    {
		public ListePouvoirsOutremonde( Agent.Agent agent ) : base(agent) { }
    }

    public class ListeKatas : CapaciteCollection<Agent.Agent, Kata>
    {
        public ListeKatas(Agent.Agent agent) : base(agent) { }
    }

    public class ListeSorts : CapaciteCollection<Agent.Agent, Sort>
    {
        public ListeSorts(Agent.Agent agent) : base(agent) { }
    }

    public class ListeKihos : CapaciteCollection<Agent.Agent, Kiho>
    {
        public ListeKihos(Agent.Agent agent) : base(agent) { }
    }
}
