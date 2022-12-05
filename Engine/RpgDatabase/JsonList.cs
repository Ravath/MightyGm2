using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MightyGm2.Engine.RpgDatabase
{
    /// <summary>
    /// Manage a data collection extracted from a given JSON file.
    /// </summary>
    /// <typeparam name="T">The Type to serialize</typeparam>
    public class JsonList<T>
    {
        public FileInfo File { get; set; }
        private List<T> _items = new List<T>();
        public IEnumerable<T> Items { get { return _items; } }

        /// <summary>
        /// Save in the given file as json.
        /// </summary>
        public virtual void Save()
        {
            var options = new JsonSerializerOptions {
                // Use readable formating
                WriteIndented = true,
                // Use readable characters (like, any we can)
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                // Use enum names
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
            string jsonString = JsonSerializer.Serialize(_items, options);
            File.Directory.Create();
            using (StreamWriter ws = new StreamWriter(File.Create()))
            {
                ws.Write(jsonString);
            }
        }

        /// <summary>
        /// Load data from the given file, but only of the file exists.
        /// </summary>
        /// <returns>False if couldn't load or find the file.</returns>
        public virtual bool Load()
        {
            if (!File.Exists)
            {
                return false;
            }

            // Open file
            using (StreamReader sr = new StreamReader(File.OpenRead()))
            {
                string jsonString = sr.ReadToEnd();
                // Deseralize
                var options = new JsonSerializerOptions
                {
                    // Use readable formating
                    WriteIndented = true,
                    // Use readable characters (like, any we can)
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    // Use enum names
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                };
                _items = JsonSerializer.Deserialize<List<T>>(jsonString, options);
            }
            return true;
        }

        /// <summary>
        /// Add Item to the list.
        /// </summary>
        /// <param name="newItem">The item to add.</param>
        /// <returns>True.</returns>
        public virtual bool Add(T newItem)
        {
            _items.Add(newItem);
            return true;
        }
    }

    /// <summary>
    /// Manage a data collection of RpgDataModel extracted from a given JSON file.
    /// </summary>
    /// <typeparam name="T">The Type to serialize</typeparam>
    public class JsonTagList<T> : JsonList<T> where T : RpgDataModel
    {
        private Dictionary<string, T> _dic = new Dictionary<string, T>();

        /// <summary>
        /// Load data from the given file, but only of the file exists.
        /// </summary>
        /// <returns>False if couldn't load or find the file.</returns>
        public override bool Load()
        {
            if (!base.Load())
            {
                return false;
            }
            // Update Dictionary
            _dic.Clear();
            foreach (var item in Items)
            {
                _dic.Add(item.Tag, item);
            }
            return true;
        }

        /// <summary>
        /// Add Item to the list.
        /// </summary>
        /// <param name="newItem">The item to add.</param>
        /// <returns>False if already contains item with same tag, or item has no tag.</returns>
        public override bool Add(T newItem)
        {
            if (String.IsNullOrWhiteSpace(newItem.Tag)
                || _dic.ContainsKey(newItem.Tag))
                return false;
            if (base.Add(newItem))
            {
                _dic.Add(newItem.Tag, newItem);
            }
            return true;
        }

        /// <summary>
        /// Check i f contains the item by its tag.
        /// </summary>
        /// <param name="tag">The tag of the item to find.</param>
        /// <returns>True if contains the item.</returns>
        public bool ContainsKey(string tag)
        {
            return _dic.ContainsKey(tag);
        }

        /// <summary>
        /// Get the item by its RpgDataModel tag.
        /// </summary>
        /// <param name="tag">The tag of the item to find.</param>
        /// <returns>The item.</returns>
        public T Get(string tag)
        {
            return _dic[tag];
        }
    }
}
