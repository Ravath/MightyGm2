using L5R.Model.Agent;
using System.Linq;
using Engine.RpgLogic;

namespace L5R.Model.Attribute {
	public class RangReputation : DerivedValue {

		private Personnage _perso;

		public RangReputation( Personnage perso ) {
			_perso = perso;
			/* il y a bcp de dépendences, donc on le fait à la main */
			/* - les anneaux */
			AddDerivedValue( _perso.Attributs.MaxVide );
			AddDerivedValue( _perso.Attributs.FourRings.ToArray() );
			/* - les compétences */
			AddDerivedValue(_perso.Competences.Competences.Select(c=>c.Pool.KeepValue).ToArray());
			_perso.Competences.NewCompetences += Competences_NewCompetences;
			_perso.Competences.RemovedCompetences += Competences_RemovedCompetences;
		}

		private void Competences_RemovedCompetences( Skill.ListeCompetences emiter, params Skill.Competence[] competences ) {
			RemoveDerivedValue(competences.Select(c => c.Pool.KeepValue).ToArray());
		}
		private void Competences_NewCompetences( Skill.ListeCompetences emiter, params Skill.Competence[] competences ) {
			AddDerivedValue(competences.Select(c => c.Pool.KeepValue).ToArray());
        }

		public int Reputation {
			get {
				int sum = 0;
				foreach(int ring in _perso.Attributs.FourRings.Select(a => a.BaseValue))
					sum += ring;
				sum += _perso.Attributs.MaxVide.BaseValue;
				sum *= 10;
				foreach(int rangCompetence in _perso.Competences.Competences.Select(c => c.Rank))
					sum += rangCompetence;
				return sum;
			}
		}
		public override int BaseValue {
			get { return GetRank(Reputation); }
		}

		public static int GetRank( int reputation ) {
			if(reputation < 150)
				return 1;
			return (reputation - 150) / 25 +2;
		}
	}
}
