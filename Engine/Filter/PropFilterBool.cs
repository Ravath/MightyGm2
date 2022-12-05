using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Filter
{
	public class PropFilterBool<T> : AbsPropFilter<T, bool>
	{
		public PropFilterBool(string propName, bool value = true) : base(propName, value)
		{
		}

		public PropFilterBool(PropertyInfo prop, bool value = true) : base(prop, value)
		{
		}

		protected override bool IsValidValue(bool val)
		{
			return val == Value;
		}
	}
}
