using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic {

	public class CapaciteCollection<C,T> : AgentPart<C>, INamedCollection
			where C : class, IAgent
			where T : ICapaciteActive<C> {

		#region Init
		public CapaciteCollection( C agent ) : base(agent) { }
		#endregion

		#region Traits
		private List<T> _capacities = new List<T>();

		public event SelectionChangedHandler SelectionChanged;

		public IEnumerable<T> Capacities {
			get { return _capacities; }
		}

		public IEnumerable<INamed> NamedCollection {
			get {
				return Capacities.Cast<INamed>();
			}
		}

		public void AddCapacity( T t ) {
			_capacities.Add(t);
			if(SelectionChanged != null)
				SelectionChanged(this);
        }
		public void RemoveCapacity( T t ) {
			_capacities.Remove(t);
			if(SelectionChanged != null)
				SelectionChanged(this);

		}
		public void ClearCapacities() {
			_capacities.Clear();
			if(SelectionChanged != null)
				SelectionChanged(this);
		}

		public void RemoveNamed( INamed item ) {
			RemoveCapacity((T)item);
		}
		public void AddNamed( INamed item ) {
			AddCapacity((T)item);
		}
		#endregion
	}
}
