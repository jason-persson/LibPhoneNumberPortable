using System.Collections.Generic;

namespace JavaPort.Collections
{
    public class Entry<T1, T2>
    {
        KeyValuePair<T1, T2> keyValuePair;

        public Entry(KeyValuePair<T1, T2> kvp)
        {
            this.keyValuePair = kvp;
        }

        internal T1 getKey()
        {
            return keyValuePair.Key;
        }

        internal T2 getValue()
        {
            return keyValuePair.Value;
        }

        public static implicit operator KeyValuePair<T1, T2>(Entry<T1, T2> entry)
        {
            return entry.keyValuePair;
        }

        public static implicit operator Entry<T1, T2>(KeyValuePair<T1, T2> keyValuePair)
        {
            return new Entry<T1, T2>(keyValuePair);
        }
    }
}
