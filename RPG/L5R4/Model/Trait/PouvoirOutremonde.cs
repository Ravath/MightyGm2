using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;

namespace L5R.Model.Trait {
	public class PouvoirOutremonde : L5R_Trait {

		public TypePouvoirOutremonde Type { get; private set; }

		public void SetDataModel( PouvoirOutremondeModel model )
        {
            Tag = model.Tag;
			Name = model.Name;
			Description = model.Description;
			Type = model.TypePouvoir;
            AddEffect(ModelFactory.Factory.InstantiateShadowlandPower(model.Tag));
        }
    }
}
