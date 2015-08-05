using java.lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util
{

    public class LinkedHashMap<K, V>
        : LinkedList<LinkedHashMap<K, V>.Entry>
    {
        Func<boolean> removeFunc;
        private int _size;

        public LinkedHashMap(int size, float p2, bool p3, Func<boolean> removeFunc)
        {
            this.removeFunc = removeFunc;
            this._size = size;
        }

        public class Entry
        {
            internal K key;
            internal V value;

            internal Entry(K key, V value)
            {
                this.key = key;
                this.value = value;
            }

            public override bool Equals(object obj)
            {
                var temp = obj as Entry;
                if (temp != null)
                {
                    return this.key.Equals(temp.key);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return this.key.GetHashCode();
            }
        }

        internal V get(K key)
        {
            foreach (var item in this)
            {
                if (item.key.Equals(key))
                {
                    this.Remove(item);
                    this.AddFirst(item);
                    return item.value;
                }
            }
            return default(V);
        }

        internal void put(K key, V value)
        {
            this.AddFirst(new Entry(key, value));
            if (removeFunc())
            {
                this.RemoveLast();
            }
        }

        internal boolean containsKey(K key)
        {
            return this.Contains(new Entry(key, default(V)));
        }

        internal int size()
        {
            return this.Count;
        }
    }
}
