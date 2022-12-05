using System;

namespace MightyGm2.Engine.RpgDatabase
{
    public class ComplementParser
    {
        private String[] _comp;
        public String[] Values
        {
            get { return _comp; }
        }
        public ComplementParser(String complement)
        {
            if (string.IsNullOrWhiteSpace(complement))
                _comp = new string[0];
            else
                _comp = complement.Split(';');
        }

        /// <summary>
        /// Get the value at the given index.
        /// </summary>
        /// <param name="index">The position of the element to found.</param>
        /// <returns>Empty String if not found.</returns>
        public string GetAt(int index)
        {
            if (_comp.Length > index)
            {
                return _comp[index];
            }
            else
            {
                return "";
            }
        }

        public int GetInt(int index, int defVal)
        {
            if (int.TryParse(GetAt(index), out int val))
            {
                return val;
            }
            else { return defVal; }
        }
        public double GetDouble(int index, double defVal)
        {
            if (double.TryParse(GetAt(index), out double val))
            {
                return val;
            }
            else { return defVal; }
        }

        public T GetEnum<T>(int v) where T : struct
        {
            Enum.TryParse(GetAt(v), out T res);
            return res;
        }
    }
}