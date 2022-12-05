using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.RpgLogic {
	/// <summary>
	/// Deprecated for uselessness and not using Values
	/// </summary>
	public class XPaPoints {
		private int _totalXp, _xpDepense;

		public int TotalXp { get { return _totalXp; } }
		public int XpDepense { get { return _xpDepense; } }
		public int XpRestant { get { return _totalXp - _xpDepense; } }

		public void GagnerXP(int xp ) {
			_totalXp += xp;
		}

		public bool DepenserXp(int xp ) {
			if(xp > XpRestant)
				return false;
			_totalXp -= xp;
			_xpDepense += xp;
			return true;
		}

		public void Reset() {
			_totalXp = 0;
			_xpDepense = 0;
		}

		public override string ToString() {
			return TotalXp + " - " + XpRestant;
		}
	}
}
