using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace java.util.logging
{
    public class Logger
    {
        string tag;

        Logger(string tag)
        {
            this.tag = tag;
        }

        internal static Logger getLogger(string tag)
        {
            return new Logger(tag);
        }

        internal void severe(string text)
        {
            Debug.WriteLine(text);
        }

        internal void log(string level, string text)
        {
            Debug.WriteLine(text, level);
        }

        internal void log(string p1, string p2, IOException e)
        {
            throw new NotImplementedException();
        }
    }
}
