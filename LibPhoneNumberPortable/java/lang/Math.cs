using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace java.lang
{
    class Math
    {
        internal static int min(int a, int b)
        {
            return System.Math.Min(a, b);
        }

        internal static int max(int a, int b)
        {
            return System.Math.Max(a, b);
        }

        internal static int log10(int d)
        {
            return (int)System.Math.Log10(d);
        }

        internal static int min(int a, long b)
        {
            return System.Math.Min(a, (int)b);
        }

        internal static int min(long a, int b)
        {
            return System.Math.Min((int)a, b);
        }
    }
}
