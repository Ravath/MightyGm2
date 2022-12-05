using L5R.Model.Agent;
using System.Collections.Generic;
using Engine.RpgLogic;
using System.Collections;
using System.Linq;

namespace L5R.Model.School {
	public class ListeEcoles : AgentPart<Personnage>, IEnumerable<Ecole> {

		private List<Ecole> _ecoles = new List<Ecole>();

		public Ecole this[int i]
		{
			get { return _ecoles[i]; }
		}

        public int Count { get => _ecoles.Count(); }

		public ListeEcoles(Personnage perso): base(perso) {

		}

		public void AddSchool(Ecole e, int rank ) {
			e.Rank.BaseValue = rank;
            _ecoles.Add(e);
			for(int i=0; i< rank; i++) {
				Agent.Techniques.AddTrait(e.Techniques[i]);
            }
			e.Rank.ValueChanged += V_ValueChanged;
		}

		private void V_ValueChanged( IValue ival, int newValue, int oldValue ) {
			foreach(Ecole item in _ecoles) {
				if(item.Rank == ival) {//find the sender
					if(newValue > oldValue) {
						// add new techniques
						for(int i = oldValue+1; i <=newValue; i++) {
							Agent.Techniques.AddTrait(item.Techniques[i]);
						}
					} else if(newValue < oldValue) {//should not be equal, but for security
						//remove old techniques
						for(int i = oldValue; i < newValue; i--) {
							Agent.Techniques.RemoveTrait(item.Techniques[i]);
						}
					}
				}
			}
		}

		public void ClearSchools() {
			Agent.Techniques.Clear();
            _ecoles.Clear();
        }

        public IEnumerator<Ecole> GetEnumerator()
        {
            return _ecoles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _ecoles.GetEnumerator();
        }
    }
}
