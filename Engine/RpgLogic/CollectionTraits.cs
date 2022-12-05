using System.Collections.Generic;
using System.Linq;

namespace Engine.RpgLogic {

	public class TraitCollection<C,T> : AgentPart<C> , INamedCollection
			where C : class, IAgent
			where T : IModifier<C>{

		public TraitCollection(C agent): base(agent) {}

		#region Traits
		private List<T> _traits = new List<T>();

		public event SelectionChangedHandler SelectionChanged;

		public IEnumerable<T> Traits {
			get { return _traits; }
		}

		public IEnumerable<INamed> NamedCollection {
			get {
				return Traits.Cast<INamed>();
            }
		}

		public void AddTrait( T t ) {
			_traits.Add(t);
			t.AffectAgent(Agent);
			SelectionChanged?.Invoke(this);
		}
		public void RemoveTrait( T t ) {
			if(_traits.Remove(t))
				t.UnaffectAgent(Agent);
			SelectionChanged?.Invoke(this);
		}
		public void Clear() {
			foreach(var t in _traits) {
				t.UnaffectAgent(Agent);
			}
			_traits.Clear();
			SelectionChanged?.Invoke(this);
		}

		public void RemoveNamed( INamed item ) {
			RemoveTrait((T)item);
		}

		public void AddNamed( INamed item ) {
			AddTrait((T)item);
		}
		#endregion
	}
}
