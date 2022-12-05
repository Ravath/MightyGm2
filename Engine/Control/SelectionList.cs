using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.Control
{
    public class SelectionList<T> : IEnumerable<T>
    {
        public delegate void isSelected(T newSelection);
        public event isSelected OnSelect;

        private List<T> _list = new List<T>();

        public T Selection { get; private set; }

        public T Select(int index)
        {
            Selection = _list[index];
            OnSelect?.Invoke(Selection);
            return Selection;
        }

        public IEnumerable<T> this[int i]
        {
            get
            {
                return _list;
            }
        }

        public void Clear() {
            _list.Clear();
            Selection = default(T);
            OnSelect?.Invoke(Selection);
        }

        public void AddRange(IEnumerable<T> range) {
            _list.AddRange(range);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
