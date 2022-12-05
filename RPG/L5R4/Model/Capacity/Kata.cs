using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;

namespace L5R.Model.Capacity {

	public class Kata : AbsElementalCapacity {
		#region Properties
		public override double Range {
			get { return 0; }
		}
		public override bool CanAffectMultipleTargets {
			get { return false; }
		}
		public override TargetType TargetType {
			get { return TargetType.Self; }
		}
		#endregion

		public void SetDataModel( KataModel model ) {
			Name = model.Name;
            Description = model.Description;
            Anneau = model.Anneau;
			Rank = model.Maitrise;
			Delegate = ModelFactory.Factory.InstantiateKata(model.Tag);
		}
	}
}
