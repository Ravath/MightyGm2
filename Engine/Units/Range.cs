using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Units {
	public class Range {

		#region Members
		private int _min;
		private int _max;
		#endregion

		#region Property
		/// <summary>
		/// If Max is less than  Min: Max = Min.
		/// </summary>
		public int Min {
			get { return _min; }
			set {
				_min = value;
				if(_min > _max)
					_max = _min;
			}
		}
		/// <summary>
		/// If Max is less than Min: Min = Max.
		/// </summary>
		public int Max {
			get { return _max; }
			set {
				_max = value;
				if(_min > _max)
					_min = _max;
			}
		}
		#endregion

		#region Init
		public Range() { }
		/// <summary>
		/// If min is > max, swap the values.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public Range(int min, int max) {
			if(min < max) {
				_min = min;
				_max = max;
			} else {
				_min = max;
				_max = min;
			}
		}
		#endregion

		public override string ToString() {
			if(Min == Max)
				return Min.ToString();
			return string.Format("{0}-{1}",Min,Max);
		}
	}
}
