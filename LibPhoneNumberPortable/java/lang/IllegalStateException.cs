using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.lang
{
    class IllegalStateException : Exception
    {
        private string p;

        public IllegalStateException(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }
    }
}
