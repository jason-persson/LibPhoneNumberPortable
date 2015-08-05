using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace java.io
{
    public abstract class Externalizable
    {
        public abstract void readExternal(ObjectInput objectInput);
        public abstract void writeExternal(ObjectOutput objectOutput);
    }
}
