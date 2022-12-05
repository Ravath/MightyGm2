using Engine.RpgLogic;
using MightyGm2.Engine.RpgDatabase;

namespace Engine.Filter
{
	public class TagFilter<T> : AbsDefaultFilter<T> where T : ITag
    {
		public string Tag { get; set; }
		public TagFilter(string tag) { Tag = tag; }

		public override bool ValidItem(T item) { return item.Tag == Tag; }
	}
}
