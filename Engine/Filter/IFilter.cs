using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Filter
{

	public interface IFilter<T>
	{
		IEnumerable<T> Filter(IEnumerable<T> toFilter);

		bool ValidItem(T item);
	}

	public abstract class AbsDefaultFilter<T> : IFilter<T>
	{
		public IEnumerable<T> Filter(IEnumerable<T> toFilter)
		{
			foreach (var item in toFilter)
			{
				if (ValidItem(item))
					yield return item;
			}
		}

		public abstract bool ValidItem(T item);
	}

	/// <summary>
	/// A default Filter which does not filter anything.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class NoFilter<T> : IFilter<T>
	{
		public IEnumerable<T> Filter(IEnumerable<T> toFilter)
		{
			return toFilter;
		}

		public bool ValidItem(T item)
		{
			return true;
		}
	}
}
