using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libphonenumber
{
    public class NumberParseException : Exception
    {
        public NumberParseException(string message)
            : base(message)
        {
        }
    }
}
