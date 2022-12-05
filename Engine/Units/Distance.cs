using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Units {
	/// <summary>
	/// Units of distance.
	/// The int value is n with "enum = 10^(n-1)"
	/// </summary>
	public enum DistanceUnity { mm=1,cm=2,dm=3,m=4,km=7 }
	/// <summary>
	/// A distance in space, with IRL Units.
	/// </summary>
	public class Distance : UnitValue<int, DistanceUnity> {
		public Distance() { }
		public Distance( int val, DistanceUnity unit ) : base(val, unit){ }
		public override double Ratio( DistanceUnity u, DistanceUnity v ) {
			return Math.Pow(10, u - v);
		}
	}
	/// <summary>
	/// Units of time.
	/// </summary>
	public enum TimeUnity {
		s, min, hour, day, week, month, year, century, millennium
	}
	/// <summary>
	/// A period of time, with IRL Units.
	/// </summary>
	public class TimePeriod : UnitValue<int, TimeUnity> {
		public TimePeriod() { }
		public TimePeriod( int val, TimeUnity unit ) : base(val, unit){ }
		/// <summary>
		/// Uses: 1 month = 3O days, and 1 month = 30/7 weeks
		/// </summary>
		/// <param name="u"></param>
		/// <param name="v"></param>
		/// <returns>u/v</returns>
		public override double Ratio( TimeUnity u, TimeUnity v ) {
			if(u == v) return 1;
			bool umin = u < v;
			TimeUnity min, max;
			double ratio = 1;
            if(umin) {
				min = u;
				max = v;
			} else {
				min = v;
				max = u;
			} while(min != max) {
				switch(min) {
					case TimeUnity.s:
					min = TimeUnity.min;
					ratio *= 60;
                    break;
					case TimeUnity.min:
					min = TimeUnity.hour;
					ratio *= 60;
					break;
					case TimeUnity.hour:
					min = TimeUnity.day;
					ratio *= 24;
					break;
					case TimeUnity.day:
					min = TimeUnity.week;
					ratio *= 7;
					break;
					case TimeUnity.week:
					min = TimeUnity.month;
					ratio *= 30d/7d;
					break;
					case TimeUnity.month:
					min = TimeUnity.year;
					ratio *= 12;
					break;
					case TimeUnity.year:
					min = TimeUnity.century;
					ratio *= 100;
					break;
					case TimeUnity.century:
					min = TimeUnity.millennium;
					ratio *= 10;
					break;
					case TimeUnity.millennium:
					return 0;//should not append
					default:
					return 0;//should not append
				}
			}

			return umin ? 1 / ratio : ratio;
        }
	}
}
