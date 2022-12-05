using MightyGm2.RPG.L5R4.Data;
using Engine.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5R4.JdrCore {
	public class ValeurMonetaire : UnitValue<int, Monnaie> {
		public ValeurMonetaire() { }
		public ValeurMonetaire( int val, Monnaie unit ) : base(val, unit) { }
		/// <summary>
		/// Returns the Ratio U/V
		/// </summary>
		/// <param name="u">>Unité de monnaie</param>
		/// <param name="v">>Unité de monnaie</param>
		/// <returns>U/V</returns>
		public override double Ratio( Monnaie u, Monnaie v ) {
			if(u == v)
				return 1;
			return GetValMonnaie(u) / GetValMonnaie(v);
        }
		/// <summary>
		/// Ramène la valeur de la monnaie àl'aquivalent en zeni.
		/// </summary>
		/// <param name="m">Unité de monnaie</param>
		/// <returns>La valeur en zeni</returns>
		private int GetValMonnaie( Monnaie m ) {
			switch(m) {
				case Monnaie.Zeni:
				return 1;
				case Monnaie.Bu:
				return 5;
				case Monnaie.Koku:
				return 50;
				default:
				throw new NotImplementedException("GetValMonnaie not implemented for "+m+" value.");
			}
		}
	}
}
