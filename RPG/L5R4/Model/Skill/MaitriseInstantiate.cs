using MightyGm2.RPG.L5R4.Data;

namespace L5R.Model.Skill {
	public static class MaitriseInstantiate {

		public static Maitrise Instanciate(MaitriseModel model ) {
			switch(model.Tag) {
				default:
				return new DefaultMaitrise(model);
			}
		}

	}
}
