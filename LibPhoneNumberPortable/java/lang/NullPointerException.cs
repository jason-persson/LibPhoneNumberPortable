using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.lang
{
    class NullPointerException : Exception
    {
        private string p;

        public NullPointerException()
        {
        }

        public NullPointerException(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }
    }
}
