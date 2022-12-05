using L5R.Model.Attribute;
using L5R.Model.Trait;
using System.Collections.Generic;
using System.Linq;
using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;
using L5R.Model.Object;
using L5R.Model.Skill;

namespace L5R.Model.School {
	public class Ecole : INamed {

		private readonly Value pointsHonneur = new Value();

		public string Tag { get; set; }
		public string Name { get; set; }
		public BaliseEcole MotClef { get; set; }
		public SortsEcole Spells { get; set; }
		public Devotion? Devotion { get; set; }
		public string Description { get; set; }
		public RankedCarac InitialHonnor { get; }
		public Value Rank { get; set; }
		public int KokuInitial { get; set; }
		public int BuInitial { get; set; }
		public int ZeniInitial { get; set; }
        public TraitCompetence BonusTrait { get; set; }

		public List<Technique> Techniques { get; }
		public List<OptEquipment> OptEquipments { get; }
		public List<L5R_Trait> Equipments { get; }
		public List<Competence> Skills { get; }
		public List<OptCompetence> OptSkills { get; }
        

        public Ecole() {
			Techniques = new List<Technique>();
            OptEquipments = new List<OptEquipment>();
            Equipments = new List<L5R_Trait>();
            Skills = new List<Competence>();
            OptSkills = new List<OptCompetence>();
            InitialHonnor = new RankedCarac(pointsHonneur) { Label = "Honneur Initial" };
			Rank = new Value(1);
        }
    }

    public class AlternativeSchool : INamed
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public BaliseEcole? MotClef { get; set; }
        public string Description { get; set; }
        public int RequiredRank { get; set; }
        public Value Rank { get; set; }

        public List<Technique> Techniques { get; }
        public List<AgentCondition> Conditions { get; }

        public AlternativeSchool()
        {
            Techniques = new List<Technique>();
            Conditions = new List<AgentCondition>();
            Rank = new Value(1);
        }
    }
}
