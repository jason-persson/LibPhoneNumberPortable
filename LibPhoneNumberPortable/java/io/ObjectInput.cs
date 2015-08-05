using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace java.io
{
public interface ObjectInput : DataInput/*, AutoCloseable*/ {
    //Object readObject();
    int read();
    int read(byte[] b);
    int read(byte[] b, int off, int len);
    long skip(long n);
    int available();
    void close();
}

}
