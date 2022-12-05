using MightyGm2.Engine.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.RpgDatabase
{
    public class RpgToResourceFile
    {
        public int RpgId { get; set; }
        public Rpg Rpg { get; set; }

        public int FileId { get; set; }
        public ResourceFile File { get; set; }
    }
    public class CampaignToResourceFile
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        public int FileId { get; set; }
        public ResourceFile File { get; set; }
    }
    public class ChapterToResourceFile
    {
        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }

        public int FileId { get; set; }
        public ResourceFile File { get; set; }
    }
    public class CampaignToPlayer
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
