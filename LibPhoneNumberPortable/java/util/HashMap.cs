using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util
{
    class HashMap<T1, T2> : Map<T1, T2> where T2 : class
    {
        public HashMap()
        {
        }

        public HashMap(int capacity)
            : base(capacity)
        {
        }

        public HashMap(int p1, float p2)
        {
        }

        internal void putAll(Map<T1, T2> other)
        {
            foreach (var item in other)
                this.Add(item.Key, item.Value);
        }
    }
}
