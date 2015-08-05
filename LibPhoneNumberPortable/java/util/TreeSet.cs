using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util
{
    class TreeSet<T> : SortedSet<T>
    {
        public TreeSet(Set<T> set)
            : base(set)
        {
        }

        public TreeSet()
        {
        }

        internal void clear()
        {
            this.Clear();
        }
    }
}
