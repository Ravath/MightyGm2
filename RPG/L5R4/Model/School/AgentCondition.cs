using L5R4.JdrCore;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;
using System;

namespace L5R.Model.School
{
	public class AgentCondition
    {
        private string _name;
        private readonly string _descModel;
        private object[] _args;

		#region Prop
		public string Name
		{
			get { return _name; }
			set { _name = value; }
        }

        public string Description
        {
            get
            {
                if (_args.Length == 0) return _descModel;
                return String.Format(_descModel, _args);
            }
        }
        #endregion

        public AgentCondition(AgentConditionModel model)
        {
            Name = model.Name;
            _descModel = model.Description;
        }

        public void SetExemplar(AgentConditionExemplar ae)
        {
            _args = GetComplement(new FiveRingsComplementParser(ae.Complement));
        }

        public virtual object[] GetComplement(FiveRingsComplementParser cp)
        {
            return cp.Values;
        }

        public virtual bool ConditionIsMet(Agent.Agent agent) { return true; }
    }

    // ===============================================
    // ===             IMPLEMENTATIONS             ===
    // ===============================================

    public class GMApprobation : AgentCondition
    {
        public GMApprobation(AgentConditionModel model) : base(model) { }
        //TODO maybe something
    }

    public class RingNeed : AgentCondition
    {
        public Anneau Ring { get; set; }
        public int Min { get; set; }

        public RingNeed(Anneau ring, AgentConditionModel model) : base(model)
        {
            Ring = ring;
        }

        public override object[] GetComplement(FiveRingsComplementParser cp)
        {
            Min = cp.GetInt(0, 2);
            return new object[] { Min };
        }

        public override bool ConditionIsMet(Agent.Agent agent)
        {
            return agent.Attributs.GetRing(Ring).BaseValue >= Min;
        }
    }

    public class AttributeNeed : AgentCondition
    {
        public MightyGm2.RPG.L5R4.Data.Trait Trait { get; set; }
        public int Min { get; set; }

        public AttributeNeed(MightyGm2.RPG.L5R4.Data.Trait trait, AgentConditionModel model) : base(model)
        {
            Trait = trait;
        }

        public override object[] GetComplement(FiveRingsComplementParser cp)
        {
            Min = cp.GetInt(0, 2);
            return new object[] { Min };
        }

        public override bool ConditionIsMet(Agent.Agent agent)
        {
            return agent.Attributs.GetAttribut(Trait).BaseValue >= Min;
        }
    }

    public class SchoolBaliseNeed : AgentCondition
    {
        private BaliseEcole Balise { get; set; }

        public SchoolBaliseNeed(BaliseEcole balise, AgentConditionModel model) : base(model) { }

        public override object[] GetComplement(FiveRingsComplementParser cp)
        {
            Balise = cp.GetEnum<BaliseEcole>(0);
            return new object[] { Balise };
        }

        public override bool ConditionIsMet(Agent.Agent agent)
        {
            if (agent is Agent.Personnage perso)
            {
                foreach (var item in perso.Ecoles)
                {
                    if (item.MotClef == Balise)
                        return true;
                }
            }

            return false;
        }
    }

    public class SkillNeed : AgentCondition
    {
        public CompetenceModel Skill { get; set; }
        public int Min { get; set; }

        public SkillNeed(AgentConditionModel model) : base(model) { }

        public override object[] GetComplement(FiveRingsComplementParser cp)
        {
            Skill = JsonDatabase.Data.Competence.Get(cp.GetAt(0));
            Min = cp.GetInt(1, 1);
            return new object[] { Skill.Name, Min };
        }

        public override bool ConditionIsMet(Agent.Agent agent)
        {
            var sk = agent.Competences.GetCompetenceByTag(Skill.Tag);
            return sk.Rank >= Min;
        }
    }

    public class ClanNeed : AgentCondition
    {
        public ClanModel Clan { get; set; }

        public ClanNeed(AgentConditionModel model) : base(model) { }

        public override object[] GetComplement(FiveRingsComplementParser cp)
        {
#if DEBUG
            if (!JsonDatabase.Data.Clan.ContainsKey(cp.GetAt(0))){
                ModelFactory.ReportError("Can't find Clan key : " + cp.GetAt(0));
                return new object[] { "Clan Not Found" };
            }
#endif
            Clan = JsonDatabase.Data.Clan.Get(cp.GetAt(0));
            return new object[] { Clan.Name };
        }

        public override bool ConditionIsMet(Agent.Agent agent) {
            if (agent is Agent.Personnage perso)
            {
                return perso.Clan.Tag == Clan.Tag;
            }
            else
                return false;
        }
    }

    public abstract class MinRankNeed : AgentCondition
    {
        public int Min { get; set; }

        public MinRankNeed(AgentConditionModel model) : base(model) { }

        public override object[] GetComplement(FiveRingsComplementParser cp)
        {
            Min = cp.GetInt(0, 1);
            return new object[] { Min };
        }
    }

    public class HonnorMin : MinRankNeed
    {
        public HonnorMin(AgentConditionModel model) : base(model) { }

        public override bool ConditionIsMet(Agent.Agent agent)
        {
            return agent.Honneur.BaseValue >= Min;
        }
    }

    public class GloryMin : MinRankNeed
    {
        public GloryMin(AgentConditionModel model) : base(model) { }

        public override bool ConditionIsMet(Agent.Agent agent)
        {
            return agent.Gloire.BaseValue >= Min;
        }
    }

    public class StatusMin : MinRankNeed
    {
        public StatusMin(AgentConditionModel model) : base(model) { }

        public override bool ConditionIsMet(Agent.Agent agent)
        {
            return agent.Status.BaseValue >= Min;
        }
    }

    public class TaintMin : MinRankNeed
    {
        public TaintMin(AgentConditionModel model) : base(model) { }

        public override bool ConditionIsMet(Agent.Agent agent)
        {
            return agent.Souillure.BaseValue >= Min;
        }
    }

    public class SexeNeed : AgentCondition
    {
        public bool NeedMale { get; set; }
        public SexeNeed(bool male, AgentConditionModel model) : base(model) { NeedMale = male; }

        public override bool ConditionIsMet(Agent.Agent agent)
        {
            return agent.EtatCivil.Male == NeedMale;
        }
    }
}
