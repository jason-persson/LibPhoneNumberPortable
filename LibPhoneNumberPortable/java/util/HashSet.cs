using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util
{
    public class HashSet<T> : Set<T>
    {
        public HashSet()
        {
        }

        public HashSet(Set<T> set)
            : base(set)
        {
        }

        public HashSet(int capacity)
            : base(capacity)
        {
        }
    }
}
