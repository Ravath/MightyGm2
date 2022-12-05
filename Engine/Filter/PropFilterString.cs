using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Filter
{
	public enum StringComparison { EQUAL, CONTAIN, START_WITH, END_WITH }

	public class PropFilterString<T> : AbsPropFilter<T, string>
	{
		public StringComparison Comparison { get; set; }

		public PropFilterString(string propName, string value, StringComparison comparison = StringComparison.CONTAIN) : base(propName, value)
		{
			Comparison = comparison;
		}

		public PropFilterString(PropertyInfo prop, string value, StringComparison comparison = StringComparison.CONTAIN) : base(prop, value)
		{
			Comparison = comparison;
		}

		protected override bool IsValidValue(string val)
		{
			switch (Comparison)
			{
				case StringComparison.EQUAL:
					return val == Value;
				case StringComparison.CONTAIN:
					return val.Contains(Value);
				case StringComparison.START_WITH:
					return val.StartsWith(Value);
				case StringComparison.END_WITH:
					return val.EndsWith(Value);
				default:
					throw new NotImplementedException();
			}
		}
	}
}
