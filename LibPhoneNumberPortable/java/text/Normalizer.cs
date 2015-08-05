using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using String = java.lang.String;
using StringBuilder = java.lang.StringBuilder;
using Math = java.lang.Math;
using IOException = java.io.IOException;
using NullPointerException = java.lang.NullPointerException;

namespace java.text
{
    class Normalizer
    {
        public class Form
        {
            public static int NFD = 1;
            public static int NFC = 2;
        }

        internal static String normalize(Func<String> pattern, object p)
        {
            throw new NotImplementedException();
        }

        internal static String normalize(String firstTwoCharacters, object p)
        {
            throw new NotImplementedException();
        }
    }
}
