using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MightyGm2.Engine.Process;

namespace L5R4.Control
{
	public class PersonnageParameters : IProcessParameters
	{
		/* Clan restrictions */
		[Category("Clan")]
		public bool MajorClanAllowed { get; set; }
		[Category("Clan")]
		public bool MinorClanAllowed { get; set; }
		[Category("Clan")]
		public bool ImperialClanAllowed { get; set; }
		[Category("Clan")]
		public bool SpiderClanAllowed { get; set; }
		[Category("Clan")]
		public bool MonkAllowed { get; set; }
		[Category("Clan")]
		public bool RoninAllowed { get; set; }

		/* School Restrictions */
		[Category("Family")]
		public bool BushiAllowed { get; set; }
		[Category("Family")]
		public bool CourtierAllowed { get; set; }
		[Category("Family")]
		public bool ShugenjaAllowed { get; set; }
		//Ninja : Bushi
		//Craftman : Courtier
		//If monk allowed, monks school are always allowed
		//Ronins are always bushi? / They seem to have SchoolKeywords too

		/* XP restrictions */
		[Category("Customisation")]
		public int StartXP { get; set; }
		[Category("Customisation")]
		public bool CanOvertakeDesavantagePointsLimit { get; set; }
		[Category("Customisation")]
		public int MaxDesavantagePoints { get; set; }
		[Category("Customisation")]
		public int MaxAvantagePoints { get; set; }
		[Category("Customisation")]
		public bool AncestorsAllowed { get; set; }

		//TODO Nezumi and other PJable monsters

		public PersonnageParameters()
		{
			MajorClanAllowed = true;
			BushiAllowed = true;
			CourtierAllowed = true;
			ShugenjaAllowed = true;
			StartXP = 40;
			CanOvertakeDesavantagePointsLimit = false;
			MaxDesavantagePoints = 10;
			MaxAvantagePoints = 15;
			AncestorsAllowed = false;
		}
	}
}
