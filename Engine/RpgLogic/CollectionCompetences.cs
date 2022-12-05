using Engine.Dice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Engine.RpgLogic {
	/// <summary>
	/// Une collection de compétences
	/// </summary>
	public class CompetenceCollection : IEnumerable<ICompetence>, INamedCollection{

		#region Properties
		public IAgent Personnage { get; private set; }

		public IEnumerable<INamed> NamedCollection {
			get {
				return this.Cast<INamed>();
			}
		}
		#endregion

		#region Init
		public CompetenceCollection( IAgent perso) {
			Personnage = perso;
        }
		#endregion

		#region Competences Collection
		private List<ICompetence> _competences = new List<ICompetence>();

		public event SelectionChangedHandler SelectionChanged;

		public virtual void Add( ICompetence cpt ) {
			_competences.Add(cpt);
			if(SelectionChanged != null)
				SelectionChanged(this);
		}
		public virtual void Remove( ICompetence cpt ) {
			_competences.Remove(cpt);
			if(SelectionChanged != null)
				SelectionChanged(this);
		}

		public IEnumerator<ICompetence> GetEnumerator() {
			return _competences.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return _competences.GetEnumerator();
		}

		public IEnumerable<ICompetence> GetAll {
			get {
				return _competences;
			}
		}
		public void RemoveNamed( INamed item ) {
			Remove((ICompetence)item);
		}
		public void AddNamed( INamed item ) {
			Add((ICompetence)item);
		}
		#endregion
	}
}
