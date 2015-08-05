using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util
{
    public class SortedSet<T> : System.Collections.Generic.SortedSet<T>
    {
        public SortedSet(IEnumerable<T> b)
            : base(b)
        {
        }

        public SortedSet()
        {
        }

        internal int size()
        {
            return this.Count;
        }

        internal T last()
        {
            return this.Last();
        }

        internal void add(T value)
        {
            this.Add(value);
        }

        internal SortedSet<T> headSet(T value)
        {
            SortedSet<T> temp = new SortedSet<T>(this.TakeWhile((T item) => { return Comparer.Compare(item, value) < 0; })); // ???
            return temp;
        }

        internal void toArray(T[] descriptionPool)
        {
            this.CopyTo(descriptionPool);
        }
    }
}
