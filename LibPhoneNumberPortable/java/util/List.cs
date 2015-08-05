using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util
{
    public class List<T> : System.Collections.Generic.List<T> where T : class
    {
        public List(int capacity)
            : base(capacity)
        {
        }

        public List()
        {
        }

        public List(IEnumerable<T> b) : base(b)
        {
        }

        internal void add(T value)
        {
            Add(value);
        }

        internal T get(int index)
        {
            return this[index];
        }

        internal int size()
        {
            return this.Count;
        }

        internal void clear()
        {
            this.Clear();
        }

        internal Iterator<T> iterator()
        {
            return new Iterator<T>(this);
        }

        internal bool isEmpty()
        {
            return this.Count == 0;
        }

        internal void addAll(IEnumerable<T> other)
        {
            foreach (T item in other)
            {
                Add(item);
            }
        }

        internal bool contains(T p)
        {
            return Contains(p);
        }

        internal void set(int index, T value)
        {
            this[index] = value;
        }
    }
}
