using Engine.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Units {
	public enum WeightUnity {
		g, kg, T
	}
	public class Weight : UnitValue<int, WeightUnity> {

		public Weight() { }
		public Weight( int val, WeightUnity unit ) : base(val, unit){ }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="u"></param>
		/// <param name="v"></param>
		/// <returns>u/v</returns>
		public override double Ratio( WeightUnity u, WeightUnity v ) {
			if(u == v)
				return 1;
			switch(u) {
				case WeightUnity.g:
				if(v == WeightUnity.kg)
					return 1e-3;
				return 1e-6;//Tonne
				case WeightUnity.kg:
				if(v == WeightUnity.g)
					return 1e3;
				return 1e-3;//Tonne
				case WeightUnity.T:
				if(v == WeightUnity.g)
					return 1e6;
				return 1e3;//kg
				default:
				return 0;
			}
		}
	}
}
