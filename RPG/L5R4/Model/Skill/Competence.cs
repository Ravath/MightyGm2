using L5R.Model.Attribute;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MightyGm2.RPG.L5R4.Data;
using Engine.RpgLogic;
using L5R4.JdrCore;
using MightyGm2.Engine.RpgDatabase;

namespace L5R.Model.Skill {

	public class Competence : IEquatable<Competence>, INamed, ITag {

		#region Members
		private string _nom;
		private Value _rank = new Value(0) { Label = "Rang" };
		private RollAndKeep _pool;
		private Attribut _att;
		private IndirectValue _keepValue = new IndirectValue(new Value(0)) { Label = "Attribut" };
		private List<Specialisation> _specialite = new List<Specialisation>();
		private List<Maitrise> _maitrise = new List<Maitrise>();
		private GroupeCompetence _groupe;
		#endregion

		#region Properties
		public bool Degradante { get; protected set; }
		public bool Noble { get; protected set; }
		public bool Sociale { get; protected set; }
		public bool Martiale { get; protected set; }
		public string Tag { get; set; }
		public string Global_Tag { get; set; }

        public string Name {
			get { return _nom; }
			set {
				TextInfo info = CultureInfo.CurrentCulture.TextInfo;
				_nom = info.ToTitleCase(value);
			}
		}
		public string Description { get; set; }

		public GroupeCompetence Groupe {
			get { return _groupe; }
			set {
				_groupe = value;
				if(_groupe == GroupeCompetence.Noble)
					Noble = true;
				if(_groupe == GroupeCompetence.Degradante)
					Degradante = true;
			}
		}
		public TraitCompetence Trait { get; protected set; }
		public TraitCompetence? TraitAlternatif { get; protected set; }

		/// <summary>
		/// Used for competence tests.
		/// Don't forget to adapt reroll of 1 and 10 if the competence is trained or specialised.
		/// </summary>
		public RollAndKeep Pool { get { return _pool; } }

		public int Rank {
			get { return _rank.BaseValue; }
			set { _rank.BaseValue = value; }
		}

		public IEnumerable<Specialisation> Specialisations {
			get { return _specialite; }
		}
		/// <summary>
		/// Return the unlocked masteries.
		/// </summary>
		public IEnumerable<Maitrise> CurrentMaitrises {
			get { return _maitrise.Where(m => m.Rang <= _pool.RollValue.BaseValue); }
		}
		/// <summary>
		/// Return all the masteries.
		/// </summary>
		public IEnumerable<Maitrise> AllMaitrises {
			get { return _maitrise; }
		}
		#endregion

		#region Init
		public Competence() { _pool = new RollAndKeep(_rank, _keepValue); }
		public Competence(CompetenceModel model) : this() { SetCompetence(model); }
        //TODO use Competence Status, whatever that is
		public Competence(CompetenceStatus model) //: this(model.Competence)
        {
            //Rang = model.Rang;
			//if(!string.IsNullOrWhiteSpace(model.Specialite_Tag))
			//	AddSpecialisation(new Specialisation(model.Specialite));
		}

		/// <summary>
		/// Set the attribute used for this competence.
		/// </summary>
		/// <param name="attribut"></param>
		public void SetAttribut( Attribut attribut ) {
			if(_att == attribut) { return; }

			/* remove former */
			UnsetAttribute();

			/* set */
			_att = attribut;
			_keepValue.SetValue(_att);

			/* add new */
			if (attribut != null)
			{
				_pool.RollValue.AddModifier(_att);
			}
		}

		public void UnsetAttribute()
		{
			if (_att != null)
			{
				_pool.RollValue.RemoveModifier(_att);
				_att = null;
			}
		}

		public void SetAttribut( Attribute.Attributs attribut ) {
			SetAttribut(attribut.GetAttribut(Trait));
		}
		#endregion

		#region Model Setters
		public void SetCompetence( CompetenceModel model, CompetenceGlobaleModel globale = null ) {
			/* val de base */
			if (globale != null)
			{
                Name = globale.Name+" : "+model.Name;
			}
			else
			{
				Name = model.Name;
			}
			Degradante = model.Degradante;
			Sociale = model.Sociale;
			Martiale = model.Martiale;
			Groupe = model.Groupe;
			Tag = model.Tag;
            Global_Tag = model.Global_Tag;
            Description = model.Description;
			/* attributs */
			Trait = model.TraitAssocie;
			TraitAlternatif = model.TraitAlternatif;
			/* ajouter les maitrise */
			_maitrise.Clear();
            foreach(MaitriseModel maitrise in model.Maitrises.OrderBy(m=>m.RangRequis)) {
				_maitrise.Add(MaitriseInstantiate.Instanciate(maitrise));
            }
		}

		public void AddSpecialisation(Specialisation specialisation)
		{
			_specialite.Add(specialisation);
		}

		public void RemoveSpecialisation(Specialisation specialisation)
		{
			_specialite.Remove(specialisation);
		}

		public bool HasSpeciality(string speTag)
		{
			foreach (var item in _specialite)
			{
				if(item.Tag == speTag) { return true; }
			}
			return false;
		}
		#endregion

		#region Operators
		public override bool Equals(object other)
		{
			if (other is CompetenceModel)
				return Equals(other as CompetenceModel);
			return Equals(other as Competence);
		}

		public bool Equals(Competence other)
		{
			if (other is null) return false;
			if (other is null) return true;
			return Tag == other.Tag;
		}

		public bool Equals(CompetenceModel other)
		{
			if (other is null) return false;
			return Tag == other.Tag;
		}

		public override int GetHashCode()
		{
			return 1005349675 + EqualityComparer<string>.Default.GetHashCode(Tag);
		}

		public static bool operator !=(Competence value1, Competence value2)
		{
			return !(value1 == value2);
		}
		public static bool operator ==(Competence value1, Competence value2)
		{
			// Check for null.
			if (value1 is null)
			{
				if (value2 is null) return true;

				// Only the left side is null.
				return false;
			}

			return value1.Equals(value2);
		}
		#endregion

	}
}
