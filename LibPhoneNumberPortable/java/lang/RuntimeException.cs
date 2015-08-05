using java.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.lang
{
    class RuntimeException : Exception
    {
        private IOException e;

        public RuntimeException(string message, IOException e) : base(message)
        {
            this.e = e;
        }

        internal String toString()
        {
            return ToString();
        }

        internal String getMessage()
        {
            return Message;
        }
    }
}
