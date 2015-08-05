using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using String = java.lang.String;
using StringBuilder = java.lang.StringBuilder;
using Math = java.lang.Math;
using IOException = java.io.IOException;
using NullPointerException = java.lang.NullPointerException;

namespace java.io
{

public abstract class OutputStream/* : Closeable, Flushable*/ {

    public abstract void write(int b);

    public virtual void write(byte[] b) {
        write(b, 0, b.Length);
    }

    public virtual void write(byte[] b, int off, int len) {
        if (b == null) {
            throw new NullPointerException();
        } else if ((off < 0) || (off > b.Length) || (len < 0) ||
                   ((off + len) > b.Length) || ((off + len) < 0)) {
            throw new Exception();
        } else if (len == 0) {
            return;
        }
        for (int i = 0 ; i < len ; i++) {
            write(b[off + i]);
        }
    }

    public virtual void flush() {
    }

    public virtual void close() {
    }
}
}
