
using java.lang;
using System;
using System.Collections;
using System.Collections.Generic;
using String = java.lang.String;
namespace java.util
{
    class ArrayList<T> : List<T> where T : class
    {
        public ArrayList()
        {
        }

        public ArrayList(int capacity)
            : base(capacity)
        {
        }

        public ArrayList(IEnumerable<T> other)
            : base(other)
        {
        }

        internal ArrayList<T> subList(int fromIndex, int toIndex)
        {
            return new ArrayList<T>(this.GetRange(fromIndex, toIndex - fromIndex));
        }

        internal T[] toArray(T[] result)
        {
            Array.Copy(this.ToArray(), result, this.Count);
            return result;
        }
        public static String toString(Character[] items)
        {
            bool first = true;
            string result = "[";
            foreach (char c in items)
            {
                if (first)
                {
                    result += c;
                    first = false;
                }
                else
                {
                    result += ", " + c;
                }
            }
            return result + "]"; ;
        }

        internal static string toString(char[] items)
        {
            bool first = true;
            string result = "[";
            foreach (char c in items)
            {
                if (first)
                {
                    result += c;
                    first = false;
                }
                else
                {
                    result += ", " + c;
                }
            }
            return result + "]"; ;
        }

        internal static int hashCode(object[] objects)
        {
            int hash = 0;
            foreach (object o in objects)
                hash += o.GetHashCode();
            return hash;
        }
    }
}
