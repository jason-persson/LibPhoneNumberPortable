using java.lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using String = java.lang.String;

namespace java.io
{
public
interface DataInput {
    void readFully(byte[] b);
    void readFully(byte[] b, int off, int len);
    int skipBytes(int n);
    boolean readBoolean();
    byte readByte();
    int readUnsignedByte();
    short readShort();
    int readUnsignedShort();
    char readChar();
    int readInt();
    //long readLong();
    //float readFloat();
    //double readDouble();
    //String readLine();
    String readUTF();
}
}
