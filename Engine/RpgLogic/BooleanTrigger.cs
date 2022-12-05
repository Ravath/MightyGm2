using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic {
	/// <summary>
	/// Provide a true or false capacity or information to a BoolCapacity.
	/// </summary>
	public interface BooleanTrigger {

	}
	/// <summary>
	/// A capacity or information that can be provided by multiple different sources, and can only be true or false.
	/// </summary>
	public class BoolCapacity {
		private List<BooleanTrigger> _providers = new List<BooleanTrigger>();

		public void AddProvider( BooleanTrigger p ) {
			_providers.Add(p);
		}
		public void RemoveProvider( BooleanTrigger p ) {
			_providers.Remove(p);
		}
		/// <summary>
		/// True while at least one provider remains.
		/// </summary>
		public bool Accessible {
			get { return _providers.Count > 0; }
		}

		public override string ToString() {
			return Accessible ? "True" : "False";
		}
	}
}
