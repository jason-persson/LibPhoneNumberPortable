using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace java.io
{
    class ByteBuffer : MemoryStream
    {
        internal static ByteBuffer allocate(int capacity)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.Capacity = capacity;
            return buffer;
        }

        internal int capacity()
        {
            return Capacity;
        }

        internal void putShort(int offset, short value)
        {
            this.Seek(offset, SeekOrigin.Begin);
            Write(BitConverter.GetBytes(value), 0, sizeof(short));
        }

        internal void putInt(int offset, int value)
        {
            this.Seek(offset, SeekOrigin.Begin);
            Write(BitConverter.GetBytes(value), 0, sizeof(int));
        }

        internal short getShort(int offset)
        {
            this.Seek(offset, SeekOrigin.Begin);
            byte[] temp = new byte[sizeof(short)];
            Read(temp, 0, sizeof(short));
            return BitConverter.ToInt16(temp, 0);
        }

        internal int getInt(int offset)
        {
            this.Seek(offset, SeekOrigin.Begin);
            byte[] temp = new byte[sizeof(int)];
            Read(temp, 0, sizeof(int));
            return BitConverter.ToInt32(temp, 0);
        }
    }
}
