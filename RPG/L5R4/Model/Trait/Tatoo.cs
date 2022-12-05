using L5R.Model.Trait;
using MightyGm2.RPG.L5R4.Data;

namespace MightyGm2.RPG.L5R4.Model.Trait
{
    public class Tatoo : L5R_Trait
    {
        public void SetDataModel(TatouageTogashiModel model)
        {
            Tag = model.Tag;
            Name = model.Name;
            Description = model.Description;
            AddEffect(ModelFactory.Factory.InstantiateTatooPower(model.Tag));
        }
    }
}
