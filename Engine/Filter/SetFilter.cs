using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Filter
{
	public enum SetFilterOperation { CONJONCTION /* ET */, DISJONCTION /* OU */}

	public class SetFilter<T> : IFilter<T>
	{
		public SetFilterOperation Operation { get; set; }

		public SetFilter(IEnumerable<IFilter<T>> filters, SetFilterOperation operation = SetFilterOperation.DISJONCTION)
		{
			_filters.AddRange(filters);
			Operation = operation;
		}

		#region Filter collection
		private List<IFilter<T>> _filters = new List<IFilter<T>>();

		public IFilter<T> this[int i] { get { return _filters[i]; } }

		public void AddFilter(IFilter<T> newFilter)
		{
			_filters.Add(newFilter);
		}
		public void AddFilter(IEnumerable<IFilter<T>> newFilters)
		{
			_filters.AddRange(newFilters);
		} 
		#endregion

		public IEnumerable<T> Filter(IEnumerable<T> toFilter)
		{
			foreach (var item in toFilter)
			{
				if (ValidItem(item))
					yield return item;
			}
		}

		public bool ValidItem(T item)
		{
			switch (Operation)
			{
				case SetFilterOperation.CONJONCTION: // ET
					foreach (var filter in _filters)
					{
						if (!filter.ValidItem(item))
							return false;
					}
					return true;
				case SetFilterOperation.DISJONCTION: // OU
					foreach (var filter in _filters)
					{
						if (filter.ValidItem(item))
							return true;
					}
					return false;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
