using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;

namespace L5R.Model.Skill {
	public abstract class Maitrise : IModifier<Agent.Agent> {
		public int Rang { get; set; }
		public string Description { get; set; }

		public string Name { get; private set; }

		public Maitrise( MaitriseModel model ) {
			SetMaitrise(model);
        }

		public void SetMaitrise( MaitriseModel model ) {
			Rang = model.RangRequis;
			Description = model.Description;
			Name = model.Name;
        }

		public abstract void AffectAgent( Agent.Agent a );
		public abstract void UnaffectAgent( Agent.Agent a );
	}

	public class DefaultMaitrise : Maitrise {
		public DefaultMaitrise( MaitriseModel model ) : base(model) { }
		public override void AffectAgent( Agent.Agent a ) { }
		public override void UnaffectAgent( Agent.Agent a ) { }
	}
}