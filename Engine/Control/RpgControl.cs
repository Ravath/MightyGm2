using MightyGm2.Engine.RpgDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.Control
{
    public class RpgControl
    {
        DatabaseCtrl data;

        public SelectionList<Rpg> Rpgs { get; } = new SelectionList<Rpg>();
        public SelectionList<Campaign> Campaigns { get; } = new SelectionList<Campaign>();
        public SelectionList<Chapter> Chapters { get; } = new SelectionList<Chapter>();
        public SelectionList<Session> Sessions { get; } = new SelectionList<Session>();

        internal void LoadRpgData()
        {
            data = ApplicationControl.Control.Database;

            Rpgs.Clear();
            Campaigns.Clear();
            Chapters.Clear();
            Sessions.Clear();

            Rpgs.AddRange(data.DB.Rpgs);
            Campaigns.AddRange(data.DB.Campaigns);
            Chapters.AddRange(data.DB.Chapters);
            Sessions.AddRange(data.DB.Sessions);
            foreach (var rpg in Rpgs)
            {
                rpg.Campaigns.Clear();
                foreach (var campaign in rpg.GetCampaigns().ToList())
                {
                    rpg.Campaigns.Add(campaign);
                    campaign.Chapters.Clear();

                    foreach (var chapter in campaign.GetChapters().ToList())
                    {
                        campaign.Chapters.Add(chapter);
                        chapter.Sessions.Clear();

                        foreach (var session in chapter.GetSessions().ToList())
                        {
                            chapter.Sessions.Add(session);
                        }
                    }
                }
            }
        }
    }
}
