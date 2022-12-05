using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.RpgDatabase
{

    /// <summary>
    /// Self referencing itself.
    /// </summary>
    public class RpgDataElementToRpgDataElement
    {
        public int FromId { get; set; }
        [InverseProperty("SelfReferencesTo")]
        public RpgDataElement From { get; set; }

        public int ToId { get; set; }
        [InverseProperty("SelfReferencesFrom")]
        public RpgDataElement To { get; set; }

        public string Arguments { get; set; }
    }

    public class RpgDataElementToRpgImplementationElement
    {
        public int DataId { get; set; }
        public RpgDataElement From { get; set; }

        public int CapacityId { get; set; }
        public RpgImplementationElement Capacity { get; set; }

        public string Arguments { get; set; }
        public CapacityTarget Target { get; set; }
    }
    public enum CapacityTarget
    {
        Trait, // Affects the Trait itself.
        Bearer, // Affects the bearer of the associated Trait. (as in : just need to be possessed)
        Wearer, // Affects the wearer of the associated Trait. (as in : needs a kind of activation)
        Target, // Affects the target of the associated Trait.
    }
}
