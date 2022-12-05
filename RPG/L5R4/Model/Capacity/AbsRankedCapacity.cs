using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;
using System.Collections.Generic;

namespace L5R.Model.Capacity {

	public interface IImplementedCapacity {
		void AffectSelf( Agent.Agent target );
		void AffectTarget( Agent.Agent caster, Agent.Agent target );
	}

	public class AbsRankedCapacity : ICapaciteActive<Agent.Agent> {
		#region Properties
		public string Name { get; protected set; }
		public string Description { get; protected set; }
        public int Rank { get; protected set; }

		public virtual TargetType TargetType { get; }
		public virtual double Range { get; }
		public virtual bool CanAffectMultipleTargets { get; }
		public IImplementedCapacity Delegate { get; internal set; }
		#endregion

		#region Init
		public AbsRankedCapacity() {}
		#endregion

		public void AffectSelf( Agent.Agent target ) {
			if(Delegate != null)
				Delegate.AffectSelf(target);
        }

		public void AffectTarget( Agent.Agent caster, Agent.Agent target ) {
			if(Delegate != null)
				Delegate.AffectTarget(caster, target);
		}

		public void AffectTargets( Agent.Agent caster, IEnumerable<Agent.Agent> targets ) {
			foreach(var item in targets) {
				AffectTarget(caster, item);
			}
		}
	}

	public class AbsElementalCapacity : AbsRankedCapacity {
		public Anneau Anneau { get; set; }
	}
}
