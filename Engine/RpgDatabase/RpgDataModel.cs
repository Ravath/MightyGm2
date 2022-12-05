using Engine.RpgLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MightyGm2.Engine.RpgDatabase
{

    public class RpgDataModel : ITag, INamed
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class DataExemplaire<T> where T : RpgDataModel
    {
        public string Complement { get; set; }
        public string Model_Tag { get; set; }
        //TODO remove?
        [JsonIgnore]
        public T Model { get; set; }
    }
}
