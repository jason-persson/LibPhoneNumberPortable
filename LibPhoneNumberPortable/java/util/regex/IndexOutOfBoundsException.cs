using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util.regex
{
    class IndexOutOfBoundsException : Exception
    {
        private string p;

        public IndexOutOfBoundsException(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }
    }
}
