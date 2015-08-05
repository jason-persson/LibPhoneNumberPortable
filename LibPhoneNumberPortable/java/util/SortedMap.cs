using JavaPort.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util
{
    public class SortedMap<T1, T2> : SortedDictionary<T1, T2>
    {
        internal int size()
        {
            return this.Count;
        }

        internal IEnumerable<T1> keySet()
        {
            return this.Keys;
        }

        internal IEnumerable<T2> values()
        {
            return this.Values;
        }

        internal T1 lastKey()
        {
            return this.Keys.Last();
        }

        internal T2 get(T1 key)
        {
            return this[key];
        }

        internal IEnumerable<Entry<T1, T2>> entrySet()
        {
            Entry<T1, T2>[] result = new Entry<T1,T2>[this.Count];
            int index = 0;
            foreach (var item in this)
            {
                result[index++] = item;
            }
            return result;
        }

        internal void put(T1 p, T2 hashSet)
        {
            this.Add(p, hashSet);
        }
    }
}
