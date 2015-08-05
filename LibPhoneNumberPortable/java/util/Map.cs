using JavaPort.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util
{
    public class Map<T1, T2> : Dictionary<T1, T2> where T2 : class
    {
        public Map()
        {
        }

        public Map(int capacity)
            : base(capacity)
        {
        }

        internal void put(T1 key, T2 value)
        {
            Add(key, value);
        }

        internal T2 get(T1 key)
        {
            if (this.ContainsKey(key))
                return this[key];
            return null;
        }

        internal bool containsKey(T1 key)
        {
            return this.ContainsKey(key);
        }

        internal IEnumerable<T1> keySet()
        {
            return Keys;
        }

        internal IEnumerable<Entry<T1, T2>> entrySet()
        {
            List<Entry<T1, T2>> list = new List<Entry<T1, T2>>(this.Count);
            foreach (var item in this.AsEnumerable())
            {
                list.Add(new Entry<T1, T2>(item));
            }
            return list.AsEnumerable();
        }
    }
}
