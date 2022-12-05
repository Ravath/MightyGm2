using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;

namespace L5R.Model.School {
	public class Clan : INamed {
		public string Tag { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }

		public void SetModel( ClanModel model ) {
			Tag = model.Tag;
			Name = model.Name;
			Description = model.Description;
		}
	}
}
