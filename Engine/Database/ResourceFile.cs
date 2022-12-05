using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MightyGm2.Engine.Control;

namespace MightyGm2.Engine.Database
{
	public enum ResourceFileType
	{
		Misc,
		Picture,
		Soundtrack,
		Text,
		Archive,
		Pdf,
		Video
	}

	public class ResourceFile
    {
        [Key]
        public int Id { get; set; }
		[Required]
		public String Name { get; set; }
		[Required]
		public String RelativePath { get; set; }
		public ResourceFileType Type { get; set; }

		public virtual List<ResourceFilesToTags> ResourceFilesToTags { set; get; } = new List<ResourceFilesToTags>();

		public int DatabaseId { get; set; }
		[Required]
		public ResourceFolder Database { get; set; }

		public FileInfo Info {
			get
			{
				return new FileInfo(Database.Path + "/" + RelativePath);
			}
		}

		public void AddTag(Tag newTag)
		{
			ResourceFilesToTags.Add(new ResourceFilesToTags
			{
				File = this,
				Tag = newTag
			});
        }

        public void AddTagIfNotAlreadyHave(Tag newTag)
        {
            ResourceFilesToTags rft = ResourceFilesToTags.FirstOrDefault(fileTag => fileTag.Tag.Id == newTag.Id);
            if (rft == null)
            {
                ResourceFilesToTags.Add(new ResourceFilesToTags
                {
                    File = this,
                    Tag = newTag
                });
            }
        }

        internal void RemoveTagIfFound(Tag tag)
		{
			ResourceFilesToTags rft = ResourceFilesToTags.FirstOrDefault(fileTag => fileTag.Tag == tag);
			if(rft != null)
			{
				ResourceFilesToTags.Remove(rft);
			}
		}

		internal void ReplaceTagIfFound(Tag oldTag, Tag newTag)
		{
			ResourceFilesToTags rft = ResourceFilesToTags.FirstOrDefault(fileTag => fileTag.Tag == oldTag);
			if (rft != null)
			{
				rft.Tag = newTag;
			}
		}
	}

	public class ResourceFilesToTags
    {
        public int FileId { get; set; }
		public ResourceFile File { get; set; }

        public int TagId { get; set; }
		public Tag Tag { get; set; }
	}

	public class Tag
    {
        [Key]
        public int Id { get; set; }
		[Required]
		[Index(IsUnique=true)]
		public String Name { get; set; }
	}
}
