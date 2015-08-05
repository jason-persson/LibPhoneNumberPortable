using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace java.io
{
public interface ObjectOutput : DataOutput/*, AutoCloseable*/ {
    //void writeObject(Object obj);

    void write(int b);

    void write(byte[] b);

    void write(byte[] b, int off, int len);

    void flush();

    void close();
}
}
