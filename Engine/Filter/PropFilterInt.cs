using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Filter
{
	public enum NUmberComparison { EQUAL, SUPERIOR, INFERIOR, SUPERIOR_OR_EQUAL, INFEROR_OR_EQUAL, DIFFERENT }

	public class PropFilterInt<T> : AbsPropFilter<T, int>
	{
		public NUmberComparison Comparison { get; set; }

		public PropFilterInt(string propName, int value = 0, NUmberComparison comparison = NUmberComparison.EQUAL) : base(propName, value)
		{
			Comparison = comparison;
		}

		public PropFilterInt(PropertyInfo prop, int value = 0, NUmberComparison comparison = NUmberComparison.EQUAL) : base(prop, value)
		{
			Comparison = comparison;
		}

		protected override bool IsValidValue(int val)
		{
			switch (Comparison)
			{
				case NUmberComparison.EQUAL:
					return val == Value;
				case NUmberComparison.SUPERIOR:
					return val == Value;
				case NUmberComparison.INFERIOR:
					return val == Value;
				case NUmberComparison.SUPERIOR_OR_EQUAL:
					return val == Value;
				case NUmberComparison.INFEROR_OR_EQUAL:
					return val == Value;
				case NUmberComparison.DIFFERENT:
					return val != Value;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
