using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace java.util
{
    public static class Collections
    {
        internal static List<T> unmodifiableList<T>(List<T> collection) where T : class
        {
            return collection;
        }

        internal static Set<T> unmodifiableSet<T>(Set<T> collection) where T : class
        {
            return collection;
        }

        internal static Map<T1, T2> unmodifiableMap<T1, T2>(Map<T1, T2> collection) where T2 : class
        {
            return collection;
        }

        internal static Map<T1, T2> synchronizedMap<T1, T2>(HashMap<T1, T2> collection) where T2 : class
        {
            return collection;
        }

        internal static SortedMap<T1, T2> unmodifiableSortedMap<T1, T2>(SortedMap<T1, T2> collection) where T2 : class
        {
            return collection;
        }
    }
}
