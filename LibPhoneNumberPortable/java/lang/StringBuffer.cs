using System;
namespace java.lang {
 public sealed class StringBuffer
    : AbstractStringBuilder
{
    public StringBuffer() : base(16) {
    }
    public StringBuffer(int capacity) : base(capacity) {
    }
    public StringBuffer(String str) : base(str.length() + 16) {
        append(str);
    }
    public StringBuffer(CharSequence seq) : base(seq.length() + 16) {
        append(seq);
    }
    public override /*synchronized*/ int length()
    {
        return count;
    }
    public override /*synchronized*/ int capacity()
    {
        return value.Length;
    }
    public override /*synchronized*/ void ensureCapacity(int minimumCapacity)
    {
        if (minimumCapacity > value.Length) {
            expandCapacity(minimumCapacity);
        }
    }
    public override /*synchronized*/ void trimToSize()
    {
        base.trimToSize();
    }
    public override /*synchronized*/ void setLength(int newLength)
    {
        base.setLength(newLength);
    }
    public override /*synchronized*/ char charAt(int index)
    {
        if ((index < 0) || (index >= count))
            throw new IndexOutOfRangeException();
        return value[index];
    }
    public override /*synchronized*/ int codePointAt(int index)
    {
        return base.codePointAt(index);
    }
    public override /*synchronized*/ int codePointBefore(int index)
    {
        return base.codePointBefore(index);
    }
    public override /*synchronized*/ int codePointCount(int beginIndex, int endIndex)
    {
        return base.codePointCount(beginIndex, endIndex);
    }
    public override /*synchronized*/ int offsetByCodePoints(int index, int codePointOffset)
    {
        return base.offsetByCodePoints(index, codePointOffset);
    }
    public override /*synchronized*/ void getChars(int srcBegin, int srcEnd, char[] dst,
                                      int dstBegin)
    {
        base.getChars(srcBegin, srcEnd, dst, dstBegin);
    }
    public override /*synchronized*/ void setCharAt(int index, char ch)
    {
        if ((index < 0) || (index >= count))
            throw new IndexOutOfRangeException();
        value[index] = ch;
    }
    public new /*synchronized*/ StringBuffer append(Object obj)
    {
        base.append(String.valueOf(obj));
        return this;
    }
    public new /*synchronized*/ StringBuffer append(String str)
    {
        base.append(str);
        return this;
    }
    public new /*synchronized*/ StringBuffer append(StringBuffer sb)
    {
        base.append(sb);
        return this;
    }
    public /*synchronized*/ StringBuffer append(StringBuilder sb)
    {
        base.append(sb.toString());
        return this;
    }
    public new StringBuffer append(CharSequence s)
    {
        // Note, synchronization achieved via other invocations
        if (s == null)
            s = new String("null");
/*        if (s is String)
            return this.append((String)s);
        if (s is StringBuffer)
            return this.append((StringBuffer)s);*/
        return this.append(s, 0, s.length());
    }
    public new /*synchronized*/ StringBuffer append(CharSequence s, int start, int end)
    {
        base.append(s, start, end);
        return this;
    }
    public new /*synchronized*/ StringBuffer append(char[] str)
    {
        base.append(str);
        return this;
    }
    public new /*synchronized*/ StringBuffer append(char[] str, int offset, int len)
    {
        base.append(str, offset, len);
        return this;
    }
    public new /*synchronized*/ StringBuffer append(boolean b)
    {
        base.append(b);
        return this;
    }
    public new /*synchronized*/ StringBuffer append(char c)
    {
        base.append(c);
        return this;
    }
    public new /*synchronized*/ StringBuffer append(int i)
    {
        base.append(i);
        return this;
    }
    public new /*synchronized*/ StringBuffer appendCodePoint(int codePoint)
    {
        base.appendCodePoint(codePoint);
        return this;
    }
    public new /*synchronized*/ StringBuffer append(long lng)
    {
        base.append(lng);
        return this;
    }
    public new /*synchronized*/ StringBuffer append(float f)
    {
        base.append(f);
        return this;
    }
    public new /*synchronized*/ StringBuffer append(double d)
    {
        base.append(d);
        return this;
    }
    public new /*synchronized*/ StringBuffer delete(int start, int end)
    {
        base.delete(start, end);
        return this;
    }
    public new /*synchronized*/ StringBuffer deleteCharAt(int index)
    {
        base.deleteCharAt(index);
        return this;
    }
    public new /*synchronized*/ StringBuffer replace(int start, int end, String str)
    {
        base.replace(start, end, str);
        return this;
    }
    public new /*synchronized*/ String substring(int start)
    {
        return substring(start, count);
    }
    public new /*synchronized*/ CharSequence subSequence(int start, int end)
    {
        return base.substring(start, end);
    }
    public new /*synchronized*/ String substring(int start, int end)
    {
        return base.substring(start, end);
    }
    public new /*synchronized*/ StringBuffer insert(int index, char[] str, int offset,
                                            int len)
    {
        base.insert(index, str, offset, len);
        return this;
    }
    public new /*synchronized*/ StringBuffer insert(int offset, Object obj)
    {
        base.insert(offset, String.valueOf(obj));
        return this;
    }
    public new /*synchronized*/ StringBuffer insert(int offset, String str)
    {
        base.insert(offset, str);
        return this;
    }
    public new /*synchronized*/ StringBuffer insert(int offset, char[] str)
    {
        base.insert(offset, str);
        return this;
    }
    public new StringBuffer insert(int dstOffset, CharSequence s)
    {
        // Note, synchronization achieved via other invocations
        if (s == null)
            s = new String("null");
/*        if (s is String)
            return this.insert(dstOffset, (String)s);*/
        return this.insert(dstOffset, s, 0, s.length());
    }
    public new /*synchronized*/ StringBuffer insert(int dstOffset, CharSequence s,
                                            int start, int end)
    {
        base.insert(dstOffset, s, start, end);
        return this;
    }
    public new StringBuffer insert(int offset, boolean b)
    {
        return insert(offset, String.valueOf(b));
    }
    public new /*synchronized*/ StringBuffer insert(int offset, char c)
    {
        base.insert(offset, c);
        return this;
    }
    public new StringBuffer insert(int offset, int i)
    {
        return insert(offset, String.valueOf(i));
    }
    public new StringBuffer insert(int offset, long l)
    {
        return insert(offset, String.valueOf(l));
    }
    public new StringBuffer insert(int offset, float f)
    {
        return insert(offset, String.valueOf(f));
    }
    public new StringBuffer insert(int offset, double d)
    {
        return insert(offset, String.valueOf(d));
    }
    public new int indexOf(String str)
    {
        return indexOf(str, 0);
    }
    public new /*synchronized*/ int indexOf(String str, int fromIndex)
    {
        return String.indexOf(value, 0, count,
                              str.toCharArray(), 0, str.length(), fromIndex);
    }
    public new int lastIndexOf(String str)
    {
        // Note, synchronization achieved via other invocations
        return lastIndexOf(str, count);
    }
    public new /*synchronized*/ int lastIndexOf(String str, int fromIndex)
    {
        return String.lastIndexOf(value, 0, count,
                              str.toCharArray(), 0, str.length(), fromIndex);
    }
    public new /*synchronized*/ StringBuffer reverse()
    {
        base.reverse();
        return this;
    }
    public new /*synchronized*/ String toString()
    {
        return new String(value, 0, count);
    }

//    private static final java.io.ObjectStreamField[] serialPersistentFields =
//    {
//        new java.io.ObjectStreamField("value", char[].class),
//        new java.io.ObjectStreamField("count", Integer.TYPE),
//        new java.io.ObjectStreamField("shared", Boolean.TYPE),
//    };
/*
    private synchronized void writeObject(java.io.ObjectOutputStream s)
        throws java.io.IOException {
        java.io.ObjectOutputStream.PutField fields = s.putFields();
        fields.put("value", value);
        fields.put("count", count);
        fields.put("shared", false);
        s.writeFields();
    }
    private void readObject(java.io.ObjectInputStream s)
        throws java.io.IOException, ClassNotFoundException {
        java.io.ObjectInputStream.GetField fields = s.readFields();
        value = (char[])fields.get("value", null);
        count = fields.get("count", 0);
    }
*/
}
}