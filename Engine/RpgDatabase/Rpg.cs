using MightyGm2.Engine.Control;
using MightyGm2.Engine.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.RpgDatabase
{
    public class Rpg
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Campaign> Campaigns = new List<Campaign>();

        [NotMapped]
        public IEnumerable<ResourceFile> Resources { get => RpgToResourceFiles.Select(rj => rj.File); }
        public List<RpgToResourceFile> RpgToResourceFiles { get; } = new List<RpgToResourceFile>();

        public IEnumerable<Campaign> GetCampaigns()
        {
            return ApplicationControl.Control.Database.DB.Campaigns.Where(c => c.RpgId == Id);
        }
    }
    public class Campaign
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int RpgId { get; set; }
        public Rpg Rpg { get; set; }

        public int GmId { get; set; }
        public Player Gm { get; set; }

        public List<Chapter> Chapters = new List<Chapter>();

        [NotMapped]
        public IEnumerable<Player> Players { get => CampaignToPlayers.Select(pj => pj.Player); }
        public List<CampaignToPlayer> CampaignToPlayers { get; } = new List<CampaignToPlayer>();

        [NotMapped]
        public IEnumerable<ResourceFile> Resources { get => CampaignToResourceFiles.Select(rj => rj.File); }
        public List<CampaignToResourceFile> CampaignToResourceFiles { get; } = new List<CampaignToResourceFile>();

        public void AddPlayer(Player newPlayer)
        {
            CampaignToPlayers.Add(new CampaignToPlayer()
            {
                Campaign = this,
                Player = newPlayer
            });
        }

        public IEnumerable<Chapter> GetChapters()
        {
            return ApplicationControl.Control.Database.DB.Chapters.Where(c => c.CampaignId == Id);
        }
    }
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        public List<Session> Sessions = new List<Session>();

        [NotMapped]
        public IEnumerable<ResourceFile> Resources { get => ChapterToResourceFiles.Select(rj => rj.File); }
        public List<ChapterToResourceFile> ChapterToResourceFiles { get; } = new List<ChapterToResourceFile>();

        public IEnumerable<Session> GetSessions()
        {
            return ApplicationControl.Control.Database.DB.Sessions.Where(c => c.ChapterId == Id);
        }
    }
    public class Session
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Resume { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }
    }
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Handbook
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int RpgId { get; set; }
        public Rpg Rpg { get; set; }
    }
}
