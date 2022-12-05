using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.Database
{
	public class ResourceFolder
    {
        [Key]
        public int Id { get; set; }
		public String Name { get; set; }
		public String Path { get; set; }
        public bool IsActive { get; set; }

        public List<ResourceFile> Elements { set; get; } = new List<ResourceFile>();

		public DirectoryInfo Info
		{
			get
			{
				return new DirectoryInfo(Path);
			}
		}
	}
}
