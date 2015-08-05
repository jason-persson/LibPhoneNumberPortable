using java.lang;
using java.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace java.io
{
public class ByteArrayOutputStream : OutputStream {
    protected byte[] buf;
    protected int count;
    public ByteArrayOutputStream() {
        int size = 32;
        if (size < 0) {
            throw new IllegalArgumentException("Negative initial size: "
                                               + size);
        }
        buf = new byte[size];
    }
    public ByteArrayOutputStream(int size) {
        if (size < 0) {
            throw new IllegalArgumentException("Negative initial size: "
                                               + size);
        }
        buf = new byte[size];
    }
    private void ensureCapacity(int minCapacity) {
        // overflow-conscious code
        if (minCapacity - buf.Length > 0)
            grow(minCapacity);
    }
    private void grow(int minCapacity) {
        // overflow-conscious code
        int oldCapacity = buf.Length;
        int newCapacity = oldCapacity << 1;
        if (newCapacity - minCapacity < 0)
            newCapacity = minCapacity;
        if (newCapacity < 0) {
            if (minCapacity < 0) // overflow
                throw new Exception();
            newCapacity = Integer.MAX_VALUE;
        }
        buf = Arrays.copyOf(buf, newCapacity);
    }
    public override /*synchronized*/ void write(int b) {
        ensureCapacity(count + 1);
        buf[count] = (byte) b;
        count += 1;
    }
    public override /*synchronized*/ void write(byte[] b, int off, int len) {
        if ((off < 0) || (off > b.Length) || (len < 0) ||
            ((off + len) - b.Length > 0)) {
            throw new Exception();
        }
        ensureCapacity(count + len);
        System.Array.Copy(b, off, buf, count, len);
        count += len;
    }
    public /*synchronized*/ void writeTo(OutputStream @out) {
        @out.write(buf, 0, count);
    }
    public /*synchronized*/ void reset() {
        count = 0;
    }
    public /*synchronized*/ byte[] toByteArray() {
        return Arrays.copyOf(buf, count);
    }
    public /*synchronized*/ int size() {
        return count;
    }

    //public /*synchronized*/ String toString() {
    //    return new String(buf, 0, count);
    //}
    //public /*synchronized*/ String toString(String charsetName)
    //{
    //    return new String(buf, 0, count, charsetName);
    //}
    //@Deprecated
    //public /*synchronized*/ String toString(int hibyte) {
    //    return new String(buf, hibyte, 0, count);
    //}

    public override void close() {
    }
}
}
