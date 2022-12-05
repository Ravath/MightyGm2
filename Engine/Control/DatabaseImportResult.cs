using MightyGm2.Engine.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.Control
{
	public class DatabaseImportResult
	{
		public Boolean IsFolderEligible { get; set; }
		public String ErrorMessage { get; set; }

		public ResourceFolder Database { get; set; }

		private List<ResourceFile> _resources = new List<ResourceFile>();
		public IEnumerable<ResourceFile> Resources { get { return _resources; } }

		private List<TagImportation> _tags = new List<TagImportation>();
		public IEnumerable<Tag> Tags { get { return _tags.Select(t => t.tag); } }
		public IEnumerable<TagImportation> TagImportations { get { return _tags; } }


		public void AddResource(ResourceFile resourceFile)
		{
			_resources.Add(resourceFile);
			Database.Elements.Add(resourceFile);
		}
		public void RemoveResource(ResourceFile resourceFile)
		{
			_resources.Remove(resourceFile);
			Database.Elements.Remove(resourceFile);
		}
		public void AddTag(Tag tag)
		{
			_tags.Add(new TagImportation(tag));
		}
		public void ImportTag(Tag tag, bool toImport)
		{
			_tags.Find(t => t.tag == tag).toImport = toImport;
		}
	}

	public class TagImportation
	{
		public Tag tag;
		public bool toImport = true;
		public TagImportation(Tag tag)
		{
			this.tag = tag;
		}

	}
}
