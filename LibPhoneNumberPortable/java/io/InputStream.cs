using java.lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using String = java.lang.String;
using StringBuilder = java.lang.StringBuilder;
using Math = java.lang.Math;
using IOException = java.io.IOException;
using NullPointerException = java.lang.NullPointerException;

namespace java.io
{
public abstract class InputStream /*implements Closeable*/ {
    // MAX_SKIP_BUFFER_SIZE is used to determine the maximum buffer size to
    // use when skipping.
    private static readonly int MAX_SKIP_BUFFER_SIZE = 2048;
    public abstract int read();
    public int read(byte[] b) {
        return read(b, 0, b.Length);
    }
    public virtual int read(byte[] b, int off, int len) {
        if (b == null) {
            throw new NullPointerException();
        } else if (off < 0 || len < 0 || len > b.Length - off) {
            throw new Exception();
        } else if (len == 0) {
            return 0;
        }
        int c = read();
        if (c == -1) {
            return -1;
        }
        b[off] = (byte)c;
        int i = 1;
        try {
            for (; i < len ; i++) {
                c = read();
                if (c == -1) {
                    break;
                }
                b[off + i] = (byte)c;
            }
        } catch (IOException) {
        }
        return i;
    }
    public virtual long skip(long n) {
        long remaining = n;
        int nr;
        if (n <= 0) {
            return 0;
        }
        int size = (int)Math.min(MAX_SKIP_BUFFER_SIZE, (int)remaining);
        byte[] skipBuffer = new byte[size];
        while (remaining > 0) {
            nr = read(skipBuffer, 0, (int)Math.min(size, (int)remaining));
            if (nr < 0) {
                break;
            }
            remaining -= nr;
        }
        return n - remaining;
    }
    public virtual int available() {
        return 0;
    }
    public virtual void close() { }
    public virtual /*synchronized*/ void mark(int readlimit) { }
    public virtual /*synchronized*/ void reset() {
        throw new Exception("mark/reset not supported");
    }
    public virtual boolean markSupported() {
        return false;
    }
}
}
