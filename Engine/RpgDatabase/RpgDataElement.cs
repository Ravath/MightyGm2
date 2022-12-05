using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.RpgDatabase
{
    public class RpgDataElement
    {
        [Key]
        public int Id { get; set; }
        public string Tag { get; set; }
        public int DataType { get; set; } // enums or specific table in 'RpgDataType' if not implemented
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Specific to one RPG.
        /// </summary>
        public int RpgId { get; set; }
        public Rpg Rpg { get; set; }

        public List<RpgDataElementToRpgImplementationElement> Implementations { get; } = new List<RpgDataElementToRpgImplementationElement>();

        [InverseProperty("From")]
        public List<RpgDataElementToRpgDataElement> SelfReferencesTo { get; } = new List<RpgDataElementToRpgDataElement>();
        [InverseProperty("To")]
        public List<RpgDataElementToRpgDataElement> SelfReferencesFrom { get; } = new List<RpgDataElementToRpgDataElement>();
    }

    public class RpgDataType
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public int RpgId { get; set; }
        public Rpg Rpg { get; set; }
    }

    //TODO remove after Debug
    public class DataDescription<T> { }
    public class DataRelation<T, U> { }
    public class DataValue<T, U> { }

    /// <summary>
    /// Warning : specific to a rpg implementation.
    /// </summary>
    public class RpgImplementationElement
    {
        [Key]
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public ImplementationType Type { get; set; }
    }
    public enum ImplementationType
    {
        Condition, // A condition of the associated trait.
        Effect, // Does an effect on a target affected by the associated trait.
        Descriptor, // A tag or precision (generally to the associated trait)
    }

    /// ===========================================================
    ///TODO maybe? or maybe not?
    /// ===========================================================
    public class RpgTargetElement
    {
        public int Id { get; set; }
        public bool LineOfSight { get; set; }
        public int RangeFactor { get; set; }
        public Range Range { get; set; }
        public TargetType TargetType { get; set; }
        public int ZoneOfEffectFactor { get; set; }
        public ZoneOfEffect ZoneOfEffect { get; set; }
        public int DurationFactor { get; set; }
        public Duration Duration { get; set; }
    }
    public enum Range
    {
        Personnel, Contact, Metres, Kilometres, Unlimited, DefaultUnit, Special
    }

    public enum TargetType
    {
        People, Ennemy, Ally, Object, Weapon, Armor, Cadaver, Undead, Place, Spot
    }

    public enum ZoneOfEffect
    {
        Cube, Cone, Ray, Circle, Sphere
    }

    public enum Duration
    {
        Instant, Permanent, Round, Minute, Hour, Day, Week, Month, Year, Special, Undefined
    }

}
