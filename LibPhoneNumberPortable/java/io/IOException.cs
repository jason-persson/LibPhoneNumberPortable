using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _IOException = System.IO.IOException;

namespace java.io
{
    class IOException : _IOException
    {
        internal string getMessage()
        {
            return this.Message;
        }

        internal string toString()
        {
            return this.ToString();
        }
    }
}
