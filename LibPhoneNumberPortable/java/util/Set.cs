using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util
{
    public class Set<T> : System.Collections.Generic.HashSet<T>
    {
        internal Set()
        {
        }

        internal Set(Set<T> set)
            : base(set)
        {
        }

        internal Set(T[] set)
            : base(set)
        {
        }

        public Set(int capacity)
        {
            // TODO ??
        }

        internal void add(T value)
        {
            Add(value);
        }

        internal int size()
        {
            return this.Count;
        }

        internal bool contains(T item)
        {
            return this.Contains(item);
        }

        internal bool remove(T item)
        {
            if (this.Contains(item))
            {
                this.Remove(item);
                return true;
            }
            return false;
        }

        internal void addAll(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                this.Add(item);
        }
    }
}
