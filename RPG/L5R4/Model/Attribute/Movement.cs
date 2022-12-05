using Engine.RpgLogic;
using L5R.Model.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.RPG.L5R4.Model.Attribute
{
    public class Movement
    {
        public IndirectValue MovementFactor { get; set; }

        public FactorValue FreeMovement { get; set; }
        public FactorValue SimpleMovement { get; set; }

        public Movement(Anneau water)
        {
            MovementFactor = new IndirectValue(water) { Label = "Facteur de Mouvement" };
            FreeMovement = new FactorValue(MovementFactor, 1.5) { Label = "Mouvement Gratuit" };
            SimpleMovement = new FactorValue(MovementFactor, 3) { Label = "Mouvement Simple" };
        }
    }
}