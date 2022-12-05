using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;

namespace L5R.Model.School {
	public class Famille : INamed {
		public string Tag { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public TraitCompetence BonusTrait { get; private set; }

		public void SetModel( FamilleModel model ) {
			Tag = model.Tag;
			Name = model.Name;
			Description = model.Description;
			BonusTrait = model.BonusInitial;
		}
	}
}
