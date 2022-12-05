using Engine.RpgLogic;
using MightyGm2.RPG.L5R4.Data;
using L5R4.JdrCore;
using L5R.Model.Trait;
using System.Collections.Generic;

namespace L5R.Model.Object {
	public class L5R_Object : L5R_Trait
    {
		public ValeurMonetaire Valeur { get; protected set; } = new ValeurMonetaire();
        public ValeurMonetaire ValeurMax { get; set; }

        private readonly List<ObjectSpecial> _specs = new List<ObjectSpecial>();

        public IEnumerable<ObjectSpecial> Specials
        {
            get { return _specs; }
        }

        public void AddObjectSpecial(ObjectSpecial newSpe)
        {
            _specs.Add(newSpe);
        }

    }
}
