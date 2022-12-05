using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Filter
{
	public abstract class AbsPropFilter<T, V> : IFilter<T>
	{
		public V Value { get; set; }
		public PropertyInfo Property { get; }

		public AbsPropFilter(string propName, V value = default(V)) : this(typeof(T).GetProperty(propName), value)
		{
		}

		public AbsPropFilter(PropertyInfo prop, V value = default(V))
		{
			if (typeof(V).IsAssignableFrom(prop.GetType()))
			{
				throw new ArgumentException("The compared property must be of the implemented type");
			}
			Property = prop;
		}

		public IEnumerable<T> Filter(IEnumerable<T> toFilter)
		{
			foreach (var item in toFilter)
			{
				V val = (V)Property.GetValue(item);
				if (IsValidValue(val))
					yield return item;
			}
		}

		public bool ValidItem(T item)
		{
			V val = (V)Property.GetValue(item);
			return IsValidValue(val);
		}

		protected abstract bool IsValidValue(V val);
	}
}
