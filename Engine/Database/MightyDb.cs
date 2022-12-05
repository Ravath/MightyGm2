using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MightyGm2.Engine.RpgDatabase;

namespace MightyGm2.Engine.Database
{
	public class MightyDb : DbContext
	{

		public MightyDb()
		{
		}

		public DbSet<ResourceFolder> Folders { get; set; }
		public DbSet<ResourceFile> Files { get; set; }
		public DbSet<ResourceFilesToTags> FilesToTags { get; set; }
		public DbSet<Tag> Tags { get; set; }

        public DbSet<Rpg> Rpgs { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Handbook> Handbooks { get; set; }
        public DbSet<CampaignToPlayer> CampaignToPlayers { get; set; }

        public DbSet<RpgDataElement> RpgDataElements { get; set; }
        public DbSet<RpgDataType> RpgDataTypes { get; set; }
        public DbSet<RpgImplementationElement> RpgImplementationElements { get; set; }
        public DbSet<RpgDataElementToRpgDataElement> RpgDataSelfReferences { get; set; }
        public DbSet<RpgDataElementToRpgImplementationElement> RpgDataImplementations { get; set; }

        public IEnumerable<RpgDataElement> GetElements(int rpgDataType)
        {
            return from e in RpgDataElements
                   where e.DataType == rpgDataType
                   select e;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            // Define Jointure Keys
            modelBuilder.Entity<ResourceFilesToTags>()
                .HasKey(cs => new { cs.FileId, cs.TagId });
            modelBuilder.Entity<RpgToResourceFile>()
                .HasKey(cs => new { cs.RpgId, cs.FileId });
            modelBuilder.Entity<CampaignToPlayer>()
                .HasKey(cs => new { cs.CampaignId, cs.PlayerId });
            modelBuilder.Entity<CampaignToResourceFile>()
                .HasKey(cs => new { cs.CampaignId, cs.FileId });
            modelBuilder.Entity<ChapterToResourceFile>()
                .HasKey(cs => new { cs.ChapterId, cs.FileId });
            modelBuilder.Entity<RpgDataElementToRpgDataElement>()
                .HasKey(cs => new { cs.FromId, cs.ToId});
            modelBuilder.Entity<RpgDataElementToRpgImplementationElement>()
                .HasKey(cs => new { cs.DataId, cs.CapacityId });
            //modelBuilder.Entity<ResourceFile>()
            //	.HasMany(b => b.Tags)
        }

		/// <summary>
		/// This connection string as an example, in case it should go to a app.config file instead;
		///<connectionStrings>
		///  <add name = "Jdr" connectionString="Host=localhost;Port=5432;Database=MightyGM;uid=postgres;Password=admin;" providerName="PostgreSQL" />
		///</connectionStrings>
		/// </summary>
		/// <param name="optionsBuilder"></param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MightyGm2;Username=postgres;Password=admin");
		}
	}
}
