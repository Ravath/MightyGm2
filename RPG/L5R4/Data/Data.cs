using L5R4.Data;
using MightyGm2.Engine.RpgDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.RPG.L5R4.Data
{
    public partial class ClanModel : RpgDataModel
    {
        public TypeClan TypeClan { get; set; }
    }

    public partial class FamilleModel : RpgDataModel
    {
        public TraitCompetence BonusInitial { get; set; }
        public ClanModel Clan { get; set; }
        public bool FamilleDisparue { get; set; }
    }

    public partial class EcoleModel : RpgDataModel
    {
        public ClanModel Clan { get; set; }

        public BaliseEcole Balise { get; set; }
        public TraitCompetence BonusInitial { get; set; }
        public decimal Honneur { get; set; }
        public decimal ArgentInitial { get; set; }
        public List<CompetenceStatus> Competences { get; } = new List<CompetenceStatus>();
        public List<ApprentissageOptionnelExemplar> CompetencesOpt { get; } = new List<ApprentissageOptionnelExemplar>();

        public List<AbsObjetModel> Equipement { get; } = new List<AbsObjetModel>();
        public List<EquipementOptionnelExemplar> EquipementsOpt { get; } = new List<EquipementOptionnelExemplar>();
        public bool NecessaireDeVoyage { get; set; }
        public List<TechniqueModel> Techniques { get; } = new List<TechniqueModel>();

        public Affinite? Affinite { get; set; }
        public Affinite? Deficience { get; set; }
        public int? NbrSortTerre { get; set; }
        public int? NbrSortFeu { get; set; }
        public int? NbrSortEau { get; set; }
        public int? NbrSortAir { get; set; }
        public int? NbrSortVide { get; set; }
        public Devotion? Devotion { get; set; }
    }

    public partial class EcoleExemplar : DataExemplaire<EcoleModel>
    {
        public int Rang { get; set; }
    }
}
