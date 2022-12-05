using Engine.Units;
using MightyGm2.Engine.RpgDatabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MightyGm2.RPG.L5R4.Data
{

    public class JsonDatabase
    {
        #region Singleton
        private static JsonDatabase _data;
        public static JsonDatabase Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new JsonDatabase();
                }
                return _data;
            }
        }
        #endregion

        private JsonDatabase() { }

        public void Load()
        {
            Clan.Load();
            Family.Load();
            Ancestor.Load();
            TableHeritage.Load();

            Ecole.Load();
            Technique.Load();
            EcoleAvancee.Load();
            TechniqueAvancee.Load();
            VoieAlternative.Load();

            AgentCondition.Load();
            EquipementOptionnel.Load();
            OptionnalTraining.Load();

            GloryGain.Load();
            HonourGain.Load();
            InfoGloire.Load();
            InfoStatus.Load();
            InfoInfamie.Load();
            Intrigue.Load();

            CombatAction.Load();
            CombatCondition.Load();
            OpportuniteHeroique.Load();

            Objet.Load();
            Arme.Load();
            Armure.Load();
            SpecialObjet.Load();

            CompetenceGlobale.Load();
            Competence.Load();

            Sort.Load();
            Mahou.Load();
            AugmentationSort.Load();

            GroupeAvantage.Load();
            Avantage.Load();
            Desavantage.Load();
            Kata.Load();
            Kiho.Load();
            TatouageTogashi.Load();
            PouvoirOutremonde.Load();

            Figurant.Load();
            PouvoirNaturel.Load();
            Joueur.Load();
        }

        public JsonTagList<ClanModel> Clan { get; } = new JsonTagList<ClanModel>() { File = new FileInfo("L5R4/Clan.json") };
        public JsonTagList<FamilleModel> Family { get; } = new JsonTagList<FamilleModel>() { File = new FileInfo("L5R4/Famille.json") };
        public JsonTagList<AncetreModel> Ancestor { get; } = new JsonTagList<AncetreModel>() { File = new FileInfo("L5R4/Ancetre.json") };
        public JsonTagList<TableHeritageModel> TableHeritage { get; } = new JsonTagList<TableHeritageModel>() { File = new FileInfo("L5R4/TableHeritage.json") };

        public JsonTagList<EcoleModel> Ecole { get; } = new JsonTagList<EcoleModel>() { File = new FileInfo("L5R4/Ecole.json") };
        public JsonTagList<TechniqueModel> Technique { get; } = new JsonTagList<TechniqueModel>() { File = new FileInfo("L5R4/Technique.json") };
        public JsonTagList<EcoleAvanceeModel> EcoleAvancee { get; } = new JsonTagList<EcoleAvanceeModel>() { File = new FileInfo("L5R4/EcoleAvancee.json") };
        public JsonTagList<TechniqueModel> TechniqueAvancee { get; } = new JsonTagList<TechniqueModel>() { File = new FileInfo("L5R4/TechniqueAvancee.json") };
        public JsonTagList<VoieAlternativeModel> VoieAlternative { get; } = new JsonTagList<VoieAlternativeModel>() { File = new FileInfo("L5R4/VoieAlternative.json") };

        public JsonTagList<AgentConditionModel> AgentCondition { get; } = new JsonTagList<AgentConditionModel>() { File = new FileInfo("L5R4/AgentCondition.json") };
        public JsonTagList<EquipementOptionnelModel> EquipementOptionnel { get; } = new JsonTagList<EquipementOptionnelModel>() { File = new FileInfo("L5R4/EquipementOptionnel.json") };
        public JsonTagList<ApprentissageOptionnelModel> OptionnalTraining { get; } = new JsonTagList<ApprentissageOptionnelModel>() { File = new FileInfo("L5R4/ApprentissageOptionnel.json") };

        public JsonList<GainGloire> GloryGain { get; } = new JsonList<GainGloire>() { File = new FileInfo("L5R4/GainGloire.json") };
        public JsonList<GainHonneur> HonourGain { get; } = new JsonList<GainHonneur>() { File = new FileInfo("L5R4/GainHonneur.json") };
        public JsonList<ScaleTable> InfoGloire { get; } = new JsonList<ScaleTable>() { File = new FileInfo("L5R4/InfoGloire.json") };
        public JsonList<ScaleTable> InfoStatus { get; } = new JsonList<ScaleTable>() { File = new FileInfo("L5R4/InfoStatus.json") };
        public JsonList<ScaleTable> InfoInfamie { get; } = new JsonList<ScaleTable>() { File = new FileInfo("L5R4/InfoInfamie.json") };
        public JsonList<IntrigueModel> Intrigue { get; } = new JsonList<IntrigueModel>() { File = new FileInfo("L5R4/Intrigue.json") };

        public JsonTagList<ActionCombatModel> CombatAction { get; } = new JsonTagList<ActionCombatModel>() { File = new FileInfo("L5R4/ActionCombat.json") };
        public JsonTagList<CombatConditionModel> CombatCondition { get; } = new JsonTagList<CombatConditionModel>() { File = new FileInfo("L5R4/CombatCondition.json") };
        public JsonTagList<OpportuniteHeroiqueModel> OpportuniteHeroique { get; } = new JsonTagList<OpportuniteHeroiqueModel>() { File = new FileInfo("L5R4/OpportuniteHeroique.json") };

        public JsonTagList<ObjetModel> Objet { get; } = new JsonTagList<ObjetModel>() { File = new FileInfo("L5R4/Objet.json") };
        public JsonTagList<ArmeModel> Arme { get; } = new JsonTagList<ArmeModel>() { File = new FileInfo("L5R4/Arme.json") };
        public JsonTagList<ArmureModel> Armure { get; } = new JsonTagList<ArmureModel>() { File = new FileInfo("L5R4/Armure.json") };
        public JsonTagList<SpecialObjetModel> SpecialObjet { get; } = new JsonTagList<SpecialObjetModel>() { File = new FileInfo("L5R4/SpecialObjet.json") };

        public JsonTagList<CompetenceGlobaleModel> CompetenceGlobale { get; } = new JsonTagList<CompetenceGlobaleModel>() { File = new FileInfo("L5R4/CompetenceGlobale.json") };
        public JsonTagList<CompetenceModel> Competence { get; } = new JsonTagList<CompetenceModel>() { File = new FileInfo("L5R4/Competence.json") };

        public JsonTagList<SortModel> Sort { get; } = new JsonTagList<SortModel>() { File = new FileInfo("L5R4/Sort.json") };
        public JsonTagList<SortModel> Mahou { get; } = new JsonTagList<SortModel>() { File = new FileInfo("L5R4/Mahou.json") };
        public JsonTagList<AugmentationSortModel> AugmentationSort { get; } = new JsonTagList<AugmentationSortModel>() { File = new FileInfo("L5R4/AugmentationSort.json") };

        public JsonTagList<GroupeAvantageModel> GroupeAvantage { get; } = new JsonTagList<GroupeAvantageModel>() { File = new FileInfo("L5R4/GroupeAvantage.json") };
        public JsonTagList<AvantageModel> Avantage { get; } = new JsonTagList<AvantageModel>() { File = new FileInfo("L5R4/Avantage.json") };
        public JsonTagList<AvantageModel> Desavantage { get; } = new JsonTagList<AvantageModel>() { File = new FileInfo("L5R4/Desavantage.json") };
        public JsonTagList<KataModel> Kata { get; } = new JsonTagList<KataModel>() { File = new FileInfo("L5R4/Kata.json") };
        public JsonTagList<KihoModel> Kiho { get; } = new JsonTagList<KihoModel>() { File = new FileInfo("L5R4/Kiho.json") };
        public JsonTagList<TatouageTogashiModel> TatouageTogashi { get; } = new JsonTagList<TatouageTogashiModel>() { File = new FileInfo("L5R4/Tatouage.json") };
        public JsonTagList<PouvoirOutremondeModel> PouvoirOutremonde { get; } = new JsonTagList<PouvoirOutremondeModel>() { File = new FileInfo("L5R4/PouvoirOutremonde.json") };

        public JsonTagList<FigurantModel> Figurant { get; } = new JsonTagList<FigurantModel>() { File = new FileInfo("L5R4/Figurant.json") };
        public JsonTagList<PouvoirNaturelModel> PouvoirNaturel { get; } = new JsonTagList<PouvoirNaturelModel>() { File = new FileInfo("L5R4/PouvoirNaturel.json") };

        public JsonList<PersonnageJoueurModel> Joueur { get; } = new JsonList<PersonnageJoueurModel>() { File = new FileInfo("L5R4/Joueur.json") };
    }

    public class L5R_Dice
    {
        public int Roll { get; set; }
        public int Keep { get; set; }
        public L5R_Dice() { }
        public L5R_Dice(int r, int k)
        {
            Roll = r;
            Keep = k;
        }
    }

    #region Models
    public class ClanModel : RpgDataModel
    {
        public TypeClan TypeClan { get; set; }
    }

    public class FamilleModel : RpgDataModel
    {
        public bool FamilleDisparue { get; set; }
        public string Clan_Tag { get; set; }
        public TraitCompetence BonusInitial { get; set; }
    }

    public class EcoleModel : RpgDataModel
    {
        public string Clan_Tag { get; set; }

        public BaliseEcole Balise { get; set; }
        public TraitCompetence BonusInitial { get; set; }
        public int ArgentInitial { get; set; }
        public Decimal Honneur { get; set; }

        //public List<TechniqueModel> Techniques { get; set; } = new List<TechniqueModel>();
        public List<CompetenceStatus> Competences { get; set; } = new List<CompetenceStatus>();
        public List<ApprentissageOptionnelExemplar> CompetencesOpt { get; set; } = new List<ApprentissageOptionnelExemplar>();
        public List<DataExemplaire<AbsObjetModel>> Equipements { get; set; } = new List<DataExemplaire<AbsObjetModel>>();
        public List<EquipementOptionnelExemplar> EquipementsOpt { get; set; } = new List<EquipementOptionnelExemplar>();
        // Shugenja
        public SortsEcole Sorts { get; set; }
        // Moine
        public Devotion? Devotion { get; set; }
    }
    public class EcoleExemplar : DataExemplaire<EcoleModel>
    {
        public int Rang { get; set; }
    }

    public class SortsEcole
    {
        public int NbrSortFeu { get; set; }
        public int NbrSortVide { get; set; }
        public int NbrSortAir { get; set; }
        public int NbrSortEau { get; set; }
        public int NbrSortTerre { get; set; }
        public Affinite Affinite { get; set; }
        public Affinite Deficience { get; set; }
    }

    public class EcoleAvanceeModel : RpgDataModel
    {
        public string Clan_Tag { get; set; }
        public BaliseEcole Balise { get; set; }
        public List<CompetenceStatus> CompetencesRequises { get; set; } = new List<CompetenceStatus>();
        public List<TechniqueModel> Techniques { get; set; } = new List<TechniqueModel>();
        public List<AgentConditionExemplar> Conditions { get; set; } = new List<AgentConditionExemplar>();
    }
    public class EcoleAvanceeExemplar : DataExemplaire<EcoleAvanceeModel>
    {
        public int Rang { get; set; }
    }
    #endregion

    #region Info
    public class GainGloire
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GainHonneur
    {
        public string Acte { get; set; }
        public int Gain0 { get; set; }
        public int Gain1 { get; set; }
        public int Gain2 { get; set; }
        public int Gain3 { get; set; }
        public int Gain4 { get; set; }
        public int Gain5 { get; set; }
        public GainHonneur() { }
        public GainHonneur(int g0, int g1, int g2, int g3, int g4, int g5)
        {
            Gain0 = g0;
            Gain1 = g1;
            Gain2 = g2;
            Gain3 = g3;
            Gain4 = g4;
            Gain5 = g5;
        }
    }

    public class ScaleTable
    {
        public int Rang { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
    }

    public class IntrigueModel : RpgDataModel
    {
        public List<string> Acteurs { get; set; } = new List<string>();
    }
    #endregion

    #region Conversion
    public class ActionCombatModel : RpgDataModel
    {
        public TypeActionCombat Type { get; set; }
        public int CoutMin { get; set; }
        public int CoutMax { get; set; }
    }

    public class CombatConditionModel : RpgDataModel
    {
        public TypeCombatCondition Type { get; set; }
    }

    public class OpportuniteHeroiqueModel : RpgDataModel
    {
        public string Action { get; set; }
    }

    public class AncetreModel : RpgDataModel
    {
        public string Clan_Tag { get; set; }
        public int Cout { get; set; }
        public string Exigences { get; set; }
    }

    public class TableHeritageModel : RpgDataModel
    {
        public int Chances { get; set; }
        public string Clan_Tag { get; set; }

        public List<TableHeritageConsequencesModel> Consequences { get; set; } = new List<TableHeritageConsequencesModel>();
    }

    public class TableHeritageConsequencesModel : RpgDataModel
    {
        public int Chances { get; set; }
    }

    public class ApprentissageOptionnelModel : RpgDataModel
    {
        //NOTHING
    }
    public class ApprentissageOptionnelExemplar : DataExemplaire<ApprentissageOptionnelModel>
    {
        public int Nombre { get; set; }
    }

    public abstract class AbsObjetModel : RpgDataModel
    {
        public UnitValue<int, Monnaie> Cout { get; set; }

        public List<SpecialObjetExemplar> Special { get; set; } = new List<SpecialObjetExemplar>();
    }

    public class SpecialObjetModel : RpgDataModel
    {
    }
    public class SpecialObjetExemplar : DataExemplaire<SpecialObjetModel>
    {
    }

    public class ObjetModel : AbsObjetModel
    {
        public int? CoutMax { get; set; }
        public ObjectType Type { get; set; }
        public bool NecessaireVoyage { get; set; }
    }

    public class ArmeModel : AbsObjetModel
    {
        public TypeArme Type { get; set; }
        public L5R_Dice VD { get; set; }
        public TailleArme Taille { get; set; }
        public bool Paysan { get; set; }
        public bool Samurai { get; set; }
    }

    public class ArmureModel : AbsObjetModel
    {
        public int BonusND { get; set; }
        public int Reduction { get; set; }
    }

    public class CompetenceGlobaleModel : RpgDataModel
    {
        public List<CompetenceModel> Specifiques { get; set; } = new List<CompetenceModel>();
    }
    public class CompetenceModel : RpgDataModel
    {
        public GroupeCompetence Groupe { get; set; }
        public TraitCompetence TraitAssocie { get; set; }
        public TraitCompetence? TraitAlternatif { get; set; }
        public bool Degradante { get; set; }
        public bool Sociale { get; set; }
        public bool Martiale { get; set; }
        public bool Noble { get; set; }
        public string Global_Tag { get; set; }

        public List<MaitriseModel> Maitrises { get; set; } = new List<MaitriseModel>();
        public List<SpecialisationModel> Specialisations { get; set; } = new List<SpecialisationModel>();
    }
    public class CompetenceExemplar : DataExemplaire<CompetenceModel>
    {
        public int Rang { get; set; }
        public List<string> SpecialisationsChoisies_Tag { get; set; } = new List<string>();
    }
    public class CompetenceStatus
    {
        public int Rang { get; set; }
        public string Competence_Tag { get; set; }
        public string Specialite_Tag { get; set; }
    }
    public class MaitriseModel : RpgDataModel
    {
        public int RangRequis { get; set; }
    }
    public class SpecialisationModel
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public bool Degradante { get; set; }
    }

    public class SortModel : RpgDataModel
    {
        public ElementSort Element { get; set; }
        public int Maitrise { get; set; }

        public bool Concentration { get; set; }

        //TODO use specific class {factor, rank, unit} => SpellUnitValue
        public Portee Portee { get; set; }
        public ZoneEffet ZoneEffet { get; set; }
        public Duree Duree { get; set; }

        public decimal FacteurPortee { get; set; }
        public decimal FacteurZone { get; set; }
        public decimal FacteurDuree { get; set; }

        public bool PorteeXRang { get; set; }
        public bool ZoneXRang { get; set; }
        public bool DureeXRang { get; set; }

        public List<AugmentationSortExemplar> Augmentations { get; set; } = new List<AugmentationSortExemplar>();

        public List<MotClefSort> MotClefs { get; set; } = new List<MotClefSort>();
    }

    public class AugmentationSortModel : RpgDataModel
    {

    }
    public class AugmentationSortExemplar : DataExemplaire<AugmentationSortModel>
    {
    }

    public class AttaqueFigurant
    {
        public string Name { get; set; }
        public Action Action { get; set; }
        public L5R_Dice Toucher { get; set; }
        public L5R_Dice Degats { get; set; }
    }

    public class SeuilBlessure
    {
        public int Seuil { get; set; }
        public int Malus { get; set; }
    }

    public class PouvoirNaturelModel : RpgDataModel
    {

    }

    public class PouvoirNaturelExemplar : DataExemplaire<PouvoirNaturelModel>
    {
    }

    public class GroupeAvantageModel : RpgDataModel
    {
        public List<AvantageModel> Avantages { get; set; } = new List<AvantageModel>();
    }
    public class AvantageModel : RpgDataModel
    {
        public string Groupe_Tag { get; set; }
        public SousTypeAvantage SousType { get; set; }
        public int Cout { get; set; }
        public int RangMax { get; set; }
    }

    public class AvantageExemplar : DataExemplaire<AvantageModel>
    {
        public int NbrRangs { get; set; }
    }

    public class KataModel : RpgDataModel
    {
        public Anneau Anneau { get; set; }
        public int Maitrise { get; set; }

        public List<DataExemplaire<EcoleModel>> Ecoles { get; set; } = new List<DataExemplaire<EcoleModel>>();
    }

    public class KihoModel : RpgDataModel
    {
        public TypeKiho Type { get; set; }
        public Anneau Anneau { get; set; }
        public int Maitrise { get; set; }
        public bool UseAtemi { get; set; }
    }

    public class VoieAlternativeModel : RpgDataModel
    {
        public string NomTechnique { get; set; }
        public int Rang { get; set; }
        public BaliseEcole? Balise { get; set; }
        public string ClanRequis_Tag { get; set; }
        public string DescriptionTechnique { get; set; }

        public List<CompetenceStatus> CompetencesRequises { get; set; } = new List<CompetenceStatus>();
        public List<AgentConditionExemplar> Conditions { get; set; } = new List<AgentConditionExemplar>();
    }

    public class TechniqueModel : RpgDataModel
    {
        public string Ecole_Tag { get; set; }
        public int Rang { get; set; }
    }

    public class TatouageTogashiModel : RpgDataModel
    {

    }

    public class TatouageTogashiExemplar : DataExemplaire<TatouageTogashiModel>
    {
    }

    public class PouvoirOutremondeModel : RpgDataModel
    {
        public TypePouvoirOutremonde TypePouvoir { get; set; }
    }

    public class AgentConditionModel : RpgDataModel
    {
    }
    public class AgentConditionExemplar : DataExemplaire<AgentConditionModel>
    {
    }

    public class EquipementOptionnelModel : RpgDataModel
    {
    }
    public class EquipementOptionnelExemplar : DataExemplaire<EquipementOptionnelModel>
    {
        public int Nombre { get; set; }
    }
    #endregion

    #region Characters
    public abstract class PersonnageModel : RpgDataModel
    {
        public int Reflexes { get; set; }
        public int Intuition { get; set; }
        public int Agilite { get; set; }
        public int Intelligence { get; set; }
        public int Perception { get; set; }
        public int Force { get; set; }
        public int Volonte { get; set; }
        public int Constitution { get; set; }
        public int? Vide { get; set; }
        public decimal Status { get; set; }
        public decimal Honneur { get; set; }
        public decimal Gloire { get; set; }
        public decimal Infamie { get; set; }
        public decimal Souillure { get; set; }

        public string Clan_Tag { get; set; }
        public string Famille_Tag { get; set; }
        public string Ancetre_Tag { get; set; }

        public List<AvantageExemplar> Avantages { get; set; } = new List<AvantageExemplar>();
        public List<AvantageExemplar> Desavantages { get; set; } = new List<AvantageExemplar>();
        public List<EcoleExemplar> Ecoles { get; set; } = new List<EcoleExemplar>();
        public List<EcoleAvanceeExemplar> EcolesAvancees { get; set; } = new List<EcoleAvanceeExemplar>();
        public List<VoieAlternativeModel> VoieAlternatives { get; set; } = new List<VoieAlternativeModel>();
        public List<SortModel> Sorts { get; set; } = new List<SortModel>();
        public List<SortModel> Mahou { get; set; } = new List<SortModel>();
        public List<KataModel> Katas { get; set; } = new List<KataModel>();
        public List<KihoModel> Kihos { get; set; } = new List<KihoModel>();
        public List<PouvoirNaturelExemplar> Pouvoirs { get; set; } = new List<PouvoirNaturelExemplar>();
        public List<ArmeModel> Armes { get; set; } = new List<ArmeModel>();
        public ArmureModel Armure { get; set; }
        public List<ObjetModel> Inventaire { get; set; } = new List<ObjetModel>();
    }
    public abstract class PersonnageExemplar : DataExemplaire<PersonnageModel>
    {
        public int DegatsSubis { get; set; }
        public int DepenseVide { get; set; }
    }

    public class PersonnageJoueurModel : PersonnageModel
    {
        public int XpTotal { get; set; }
        public int XpDepense { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public List<CompetenceExemplar> Competences { get; set; } = new List<CompetenceExemplar>();
    }
    public class PersonnageJoueurExemplar : PersonnageExemplar
    {
    }

    public class FigurantModel : PersonnageModel
    {
        public int NDArmure { get; set; }
        public int Reduction { get; set; }
        public L5R_Dice Initiative { get; set; }
        public bool VieHumaine { get; set; }
        public int VieMax { get; set; }
        public List<SeuilBlessure> Seuils { get; set; } = new List<SeuilBlessure>();
        public List<AttaqueFigurant> Attaques { get; set; } = new List<AttaqueFigurant>();
        public List<CompetenceStatus> Competences { get; set; } = new List<CompetenceStatus>();
    }
    public class FigurantExemplar : PersonnageExemplar
    {
    }
    #endregion
}
