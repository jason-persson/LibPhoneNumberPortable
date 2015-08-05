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
class DataOutputStream /*: FilterOutputStream, DataOutput*/ {
    protected int written;
    private byte[] bytearr = null;
    OutputStream @out;
    public DataOutputStream(OutputStream @out) {
        //super(@out);
        this.@out = @out;
    }
    private void incCount(int value) {
        int temp = written + value;
        if (temp < 0) {
            temp = Integer.MAX_VALUE;
        }
        written = temp;
    }
    public /*synchronized*/ void write(int b) {
        @out.write(b);
        incCount(1);
    }
    public /*synchronized*/ void write(byte[] b, int off, int len)
    {
        @out.write(b, off, len);
        incCount(len);
    }
    public void flush() {
        @out.flush();
    }
    public void writeBoolean(boolean v) {
        @out.write(v ? 1 : 0);
        incCount(1);
    }
    public void writeByte(int v) {
        @out.write(v);
        incCount(1);
    }
    public void writeShort(int v) {
        @out.write((int)(((uint)v) >> 8) & 0xFF);
        @out.write((int)(((uint)v) >> 0) & 0xFF);
        incCount(2);
    }
    public void writeChar(int v) {
        @out.write((int)(((uint)v) >> 8) & 0xFF);
        @out.write((int)(((uint)v) >> 0) & 0xFF);
        incCount(2);
    }
    public void writeInt(int v) {
        @out.write((int)(((uint)v) >> 24) & 0xFF);
        @out.write((int)(((uint)v) >> 16) & 0xFF);
        @out.write((int)(((uint)v) >> 8) & 0xFF);
        @out.write((int)(((uint)v) >> 0) & 0xFF);
        incCount(4);
    }
    private byte[] writeBuffer = new byte[8];
    public void writeLong(long v) {
        writeBuffer[0] = (byte)(int)(((uint)v) >> 56);
        writeBuffer[1] = (byte)(int)(((uint)v) >> 48);
        writeBuffer[2] = (byte)(int)(((uint)v) >> 40);
        writeBuffer[3] = (byte)(int)(((uint)v) >> 32);
        writeBuffer[4] = (byte)(int)(((uint)v) >> 24);
        writeBuffer[5] = (byte)(int)(((uint)v) >> 16);
        writeBuffer[6] = (byte)(int)(((uint)v) >> 8);
        writeBuffer[7] = (byte)(int)(((uint)v) >> 0);
        @out.write(writeBuffer, 0, 8);
        incCount(8);
    }
    /*
    public void writeFloat(float v) {
        writeInt(Float.floatToIntBits(v));
    }
    public void writeDouble(double v) {
        writeLong(Double.doubleToLongBits(v));
    }
    */
    public void writeBytes(String s) {
        int len = s.length();
        for (int i = 0 ; i < len ; i++) {
            @out.write((byte)s.charAt(i));
        }
        incCount(len);
    }
    public void writeChars(String s) {
        int len = s.length();
        for (int i = 0 ; i < len ; i++) {
            int v = s.charAt(i);
            @out.write((int)(((uint)v) >> 8) & 0xFF);
            @out.write((int)(((uint)v) >> 0) & 0xFF);
        }
        incCount(len * 2);
    }
    public void writeUTF(String str) {
        writeUTF(str, this);
    }
    static int writeUTF(String str, DataOutputStream @out) {
        int strlen = str.length();
        int utflen = 0;
        int c, count = 0;
        /* use charAt instead of copying String to char array */
        int i;
        for (i = 0; i < strlen; i++) {
            c = str.charAt(i);
            if ((c >= 0x0001) && (c <= 0x007F)) {
                utflen++;
            } else if (c > 0x07FF) {
                utflen += 3;
            } else {
                utflen += 2;
            }
        }
        if (utflen > 65535)
            throw new Exception(
                "encoded string too long: " + utflen + " bytes");
        byte[] bytearr = null;
        if (@out is DataOutputStream) {
            DataOutputStream dos = (DataOutputStream)@out;
            if(dos.bytearr == null || (dos.bytearr.Length < (utflen+2)))
                dos.bytearr = new byte[(utflen*2) + 2];
            bytearr = dos.bytearr;
        } else {
            bytearr = new byte[utflen+2];
        }
        bytearr[count++] = (byte) ((int)(((uint)utflen) >> 8) & 0xFF);
        bytearr[count++] = (byte) ((int)(((uint)utflen) >> 0) & 0xFF);
        i=0;
        for (i=0; i<strlen; i++) {
           c = str.charAt(i);
           if (!((c >= 0x0001) && (c <= 0x007F))) break;
           bytearr[count++] = (byte) c;
        }
        for (;i < strlen; i++){
            c = str.charAt(i);
            if ((c >= 0x0001) && (c <= 0x007F)) {
                bytearr[count++] = (byte) c;
            } else if (c > 0x07FF) {
                bytearr[count++] = (byte) (0xE0 | ((c >> 12) & 0x0F));
                bytearr[count++] = (byte) (0x80 | ((c >>  6) & 0x3F));
                bytearr[count++] = (byte) (0x80 | ((c >>  0) & 0x3F));
            } else {
                bytearr[count++] = (byte) (0xC0 | ((c >>  6) & 0x1F));
                bytearr[count++] = (byte) (0x80 | ((c >>  0) & 0x3F));
            }
        }
        @out.write(bytearr, 0, utflen+2);
        return utflen + 2;
    }
    public int size() {
        return written;
    }
}
}
