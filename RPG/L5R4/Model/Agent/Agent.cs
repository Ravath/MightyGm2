using L5R.Model.Skill;
using L5R.Model.Attribute;
using L5R.Model.Attack;
using L5R.Model.Object;
using L5R.Model.Trait;
using L5R.Model.Capacity;
using Engine.RpgLogic;
using L5R4.JdrCore;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model.Attribute;

namespace L5R.Model.Agent {
	public abstract class Agent : IAgent, INamed
    {

		#region Members
		public delegate void ModelChanged(Agent agent);
		public event ModelChanged OnModelChanged;
        #endregion

        #region Properties : Composants
        public string Name { get => EtatCivil.Name; set { EtatCivil.Name = value; } }
        public EtatCivilRokugan EtatCivil { get; private set; }
		public Attribute.Attributs Attributs { get; }
		public ListeCompetences Competences { get; }
		public SeuilVie Vie { get; protected set; }
		#region Objects
		public Inventaire Inventaire { get; }
		public InventaireArmure Armures { get; }
		public InventaireArme Armes { get; }
		#endregion
		#region Traits
		public ListeAvantages Avantages { get; }
		public ListeTraitsCreature TraitsCreature { get; }
		public ListePouvoirsOutremonde PouvoirsOutremonde { get; }
		public ListeTechniques Techniques { get; }
		#endregion
		#region Capacites
		public ListeKatas Katas { get; }
		public ListeKihos Kihos { get; }
		public ListeSorts Sorts { get; }
		#endregion
		#endregion

		#region Properties : Caractéristiques
		public Movement Movement { get; }
		public RollAndKeep Initiative { get; protected set; }
		public FactorValue RecuperationRate { get; }
        private readonly Value pointsHonneur = new Value(0);
        private readonly Value pointsGloire = new Value(0);
        private readonly Value pointsStatus = new Value(0);
        private readonly Value pointsSouillure = new Value(0);
        public RankedCarac Honneur { get; }
        public RankedCarac Gloire { get; }
        public RankedCarac Status { get; }
        public RankedCarac Souillure { get; }
		public TargetType TargetType {
			get { return TargetType.Agent; }
		}

		public bool IsDead {
			get {
				return Vie.IsDead;
			}
		}
		#endregion

		#region Init
		public Agent() {
			//composants
			EtatCivil = new EtatCivilRokugan();
			Attributs = new Attributs(this);
			Competences = new ListeCompetences(Attributs, this);
			//objects
			Inventaire = new Inventaire(this);
			Armes = new InventaireArme(this);
			Armures = new InventaireArmure(this);
			//traits
			Avantages = new ListeAvantages(this);
			TraitsCreature = new ListeTraitsCreature(this);
			PouvoirsOutremonde = new ListePouvoirsOutremonde(this);
			Techniques = new ListeTechniques(this);
			//capacités
			Katas = new ListeKatas(this);
			Kihos = new ListeKihos(this);
			Sorts = new ListeSorts(this);
			// caractéristiques
            Movement = new Movement(Attributs.Eau);
			RecuperationRate = new FactorValue(Attributs.Constitution, 2) { Label = "Récuperation" };
            Honneur = new RankedCarac(pointsHonneur) { Label = "Honneur" };
            Gloire = new RankedCarac(pointsGloire) { Label = "Gloire" };
            Status = new RankedCarac(pointsStatus) { Label = "Status" };
            Souillure = new RankedCarac(pointsSouillure) { Label = "Souillure" };
		}

		private void SetPersonnage(PersonnageModel perso)
		{
			Attributs.SetPersonnage(perso);
			Competences.SetPersonnage(perso);
			EtatCivil.SetPersonnage(perso);
			OnModelChanged?.Invoke(this);
        }

		public virtual void SetPersonnage(FigurantModel perso) { SetPersonnage((PersonnageModel)perso); }
		public virtual void SetPersonnage(PersonnageJoueurModel perso) { SetPersonnage((PersonnageModel)perso); }
		#endregion

	}
}
