using java.lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using String = java.lang.String;

namespace java.io
{
public
interface DataOutput {
//    void write(int b);
//    void write(byte[] b);
//    void write(byte[] b, int off, int len);
    void writeBoolean(boolean v);
    void writeByte(int v);
    void writeShort(int v);
    void writeChar(int v);
    void writeInt(int v);
//    void writeLong(long v);
//    void writeFloat(float v);
//    void writeDouble(double v);
    void writeBytes(String s);
    void writeChars(String s);
    void writeUTF(String s);
}

}
