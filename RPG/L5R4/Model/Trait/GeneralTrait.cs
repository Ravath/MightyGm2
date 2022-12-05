using Engine.RpgLogic;
using L5R.Model.Agent;
using L5R.Model.School;
using MightyGm2.Engine.RpgDatabase;
using System.Collections.Generic;

namespace L5R.Model.Trait
{
    public interface IAgentEffect
    {
        void Affect(Agent.Agent a);
        void Unaffect(Agent.Agent a);
        void SetComplement(string complement);
    }

    public abstract class DefaultAgentEffect : IAgentEffect
    {
        public abstract void Affect(Agent.Agent a);
        public abstract void Unaffect(Agent.Agent a);

        public virtual void SetComplement(string complement)
        {
            // NOTHING
            // TODO : default management of the complement string.
        }
    }

    public class DummyAgentEffect : IAgentEffect
    {
        public void Affect(Agent.Agent a) { /* NOTHING */ }
        public void Unaffect(Agent.Agent a) { /* NOTHING */ }
        public virtual void SetComplement(string complement) { /* NOTHING */ }
    }

    public class L5R_Trait : IModifier<Agent.Agent>, ITag
    {
		public string Tag { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
        public List<IAgentEffect> EffectImplementations { get; protected set; } = new List<IAgentEffect>();
        public List<AgentCondition> AcqCondition { get; protected set; } = new List<AgentCondition>();

        public void AffectAgent( Agent.Agent a ) {
            foreach (var effect in EffectImplementations)
            {
                effect.Affect(a);
            }
		}

		public void UnaffectAgent( Agent.Agent a )
        {
            foreach (var effect in EffectImplementations)
            {
                effect.Unaffect(a);
            }
		}

        public void SetDataModel(RpgDataModel dataModel)
        {
            Tag = dataModel.Tag;
            Name = dataModel.Name;
            Description = dataModel.Description;
        }

        public void AddEffect(IAgentEffect implementation)
        {
            EffectImplementations.Add(implementation);
        }

        public void AddCondition(AgentCondition condition)
        {
            AcqCondition.Add(condition);
        }

        public bool CanAcquire(Agent.Agent agent)
        {
            foreach (var cond in AcqCondition)
            {
                if (!cond.ConditionIsMet(agent))
                    return false;
            }
            return true;
        }

    }
}
