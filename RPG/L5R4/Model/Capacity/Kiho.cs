using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;

namespace L5R.Model.Capacity {
	public class Kiho : AbsElementalCapacity {
		#region Properties
		public bool Atemi { get; private set; }
		public TypeKiho Type { get; private set; }
		public override double Range {
			get { return 0; }
		}
		public override bool CanAffectMultipleTargets {
			get { return false; }
		}
		public override TargetType TargetType {
			get {
				if(Atemi)
					return TargetType.Agent;
				return TargetType.Self;
			}
		}
		#endregion

		public void SetDataModel( KihoModel model ) {
			Name = model.Name;
            Description = model.Description;
			Anneau = model.Anneau;
			Rank = model.Maitrise;
			Atemi = model.UseAtemi;
			Type = model.Type;
			Delegate = ModelFactory.Factory.InstantiateKiho(model.Tag);
        }
	}
}
