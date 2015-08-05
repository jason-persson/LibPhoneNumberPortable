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

namespace java.io
{
public
class ByteArrayInputStream : InputStream {
    protected byte[] buf;
    protected int pos;
    protected int _mark = 0;
    protected int count;
    public ByteArrayInputStream(byte[] buf) {
        this.buf = buf;
        this.pos = 0;
        this.count = buf.Length;
    }
    public ByteArrayInputStream(byte[] buf, int offset, int length) {
        this.buf = buf;
        this.pos = offset;
        this.count = Math.min(offset + length, buf.Length);
        this._mark = offset;
    }
    public override /*synchronized*/ int read() {
        return (pos < count) ? (buf[pos++] & 0xff) : -1;
    }
    public override /*synchronized*/ int read(byte[] b, int off, int len) {
        if (b == null) {
            throw new NullPointerException();
        } else if (off < 0 || len < 0 || len > b.Length - off) {
            throw new Exception();
        }
        if (pos >= count) {
            return -1;
        }
        int avail = count - pos;
        if (len > avail) {
            len = avail;
        }
        if (len <= 0) {
            return 0;
        }
        System.Array.Copy(buf, pos, b, off, len);
        pos += len;
        return len;
    }
    public override /*synchronized*/ long skip(long n) {
        long k = count - pos;
        if (n < k) {
            k = n < 0 ? 0 : n;
        }
        pos += (int)k;
        return k;
    }
    public override /*synchronized*/ int available() {
        return count - pos;
    }
    public override boolean markSupported() {
        return true;
    }
    public override void mark(int readAheadLimit) {
        _mark = pos;
    }
    public override /*synchronized*/ void reset() {
        pos = _mark;
    }
    public override void close() {
    }
}
}
