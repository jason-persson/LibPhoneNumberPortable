using System;
namespace java.lang {
public sealed class StringBuilder
    : AbstractStringBuilder
{
    public StringBuilder() : base(16) {
    }
    public StringBuilder(int capacity) : base(capacity) {
    }
    public StringBuilder(String str) : base(str.length() + 16) {
        append(str);
    }
    public StringBuilder(StringBuilder str) : base(str.length() + 16) {
        append(str.toString());
    }
    public StringBuilder(string str) : base(str.Length + 16) {
        append(str);
    }
    public StringBuilder(CharSequence seq) : base(seq.length() + 16) {
        append(seq);
    }
    public new StringBuilder append(Object obj)
    {
        return append(String.valueOf(obj));
    }
    public new StringBuilder append(String str)
    {
        base.append(str);
        return this;
    }
    public new StringBuilder append(string str)
    {
        base.append(str);
        return this;
    }
    // Appends the specified string builder to this sequence.
    internal StringBuilder append(StringBuilder sb) {
        if (sb == null)
            return append(new String("null"));
        int len = sb.length();
        int newcount = count + len;
        if (newcount > value.Length)
            expandCapacity(newcount);
        sb.getChars(0, len, value, count);
        count = newcount;
        return this;
    }
    public new StringBuilder append(StringBuffer sb)
    {
        base.append(sb);
        return this;
    }
    public new StringBuilder append(CharSequence s)
    {
        if (s == null)
            s = new String("null");
        //if (s is String)
        //    return this.append((String)s);
        //if (s is StringBuffer)
        //    return this.append((StringBuffer)s);
        //if (s is StringBuilder)
        //    return this.append((StringBuilder)s);
        return this.append(s, 0, s.length());
    }
    public new StringBuilder append(CharSequence s, int start, int end)
    {
        base.append(s, start, end);
        return this;
    }
    public new StringBuilder append(char[] str)
    {
        base.append(str);
        return this;
    }
    public new StringBuilder append(char[] str, int offset, int len)
    {
        base.append(new String(str), offset, len);
        return this;
    }
    public new StringBuilder append(boolean b)
    {
        base.append(b);
        return this;
    }
    public new StringBuilder append(char c)
    {
        base.append(c);
        return this;
    }
    public new StringBuilder append(int i)
    {
        base.append(i);
        return this;
    }
    public new StringBuilder append(long lng)
    {
        base.append(lng);
        return this;
    }
    public new StringBuilder append(float f)
    {
        base.append(f);
        return this;
    }
    public new StringBuilder append(double d)
    {
        base.append(d);
        return this;
    }
    public new StringBuilder appendCodePoint(int codePoint)
    {
        base.appendCodePoint(codePoint);
        return this;
    }
    public new StringBuilder delete(int start, int end)
    {
        base.delete(start, end);
        return this;
    }
    public new StringBuilder deleteCharAt(int index)
    {
        base.deleteCharAt(index);
        return this;
    }
    public new StringBuilder replace(int start, int end, String str)
    {
        base.replace(start, end, str);
        return this;
    }
    public new StringBuilder insert(int index, char[] str, int offset,
                                int len)
    {
        base.insert(index, str, offset, len);
        return this;
    }
    public new StringBuilder insert(int offset, Object obj)
    {
        return insert(offset, String.valueOf(obj));
    }
    public new StringBuilder insert(int offset, String str)
    {
        base.insert(offset, str);
        return this;
    }
    public StringBuilder insert(int offset, string str)
    {
        base.insert(offset, new String(str));
        return this;
    }
    public new StringBuilder insert(int offset, char[] str)
    {
        base.insert(offset, str);
        return this;
    }
    public new StringBuilder insert(int dstOffset, CharSequence s)
    {
        if (s == null)
            s = new String("null");
/*        if (s is String)
            return this.insert(dstOffset, (String)s);*/
        return this.insert(dstOffset, s, 0, s.length());
    }
    public new StringBuilder insert(int dstOffset, CharSequence s,
                                int start, int end)
    {
        base.insert(dstOffset, s, start, end);
        return this;
    }
    public new StringBuilder insert(int offset, boolean b)
    {
        base.insert(offset, b);
        return this;
    }
    public new StringBuilder insert(int offset, char c)
    {
        base.insert(offset, c);
        return this;
    }
    public new StringBuilder insert(int offset, int i)
    {
        return insert(offset, String.valueOf(i));
    }
    public new StringBuilder insert(int offset, long l)
    {
        return insert(offset, String.valueOf(l));
    }
    public new StringBuilder insert(int offset, float f)
    {
        return insert(offset, String.valueOf(f));
    }
    public new StringBuilder insert(int offset, double d)
    {
        return insert(offset, String.valueOf(d));
    }
    public new int indexOf(String str)
    {
        return indexOf(str, 0);
    }
    public new int indexOf(String str, int fromIndex)
    {
        return String.indexOf(value, 0, count,
                              str.toCharArray(), 0, str.length(), fromIndex);
    }
    public new int lastIndexOf(String str)
    {
        return lastIndexOf(str, count);
    }
    public new int lastIndexOf(String str, int fromIndex)
    {
        return String.lastIndexOf(value, 0, count,
                              str.toCharArray(), 0, str.length(), fromIndex);
    }
    public new StringBuilder reverse()
    {
        base.reverse();
        return this;
    }
    public override String toString() {
        // Create a copy, don't share the array
        return new String(value, 0, count);
    }
    /*
    private void writeObject(java.io.ObjectOutputStream s)
        throws java.io.IOException {
        s.defaultWriteObject();
        s.writeInt(count);
        s.writeObject(value);
    }
    private void readObject(java.io.ObjectInputStream s)
        throws java.io.IOException, ClassNotFoundException {
        s.defaultReadObject();
        count = s.readInt();
        value = (char[]) s.readObject();
    }
    */



    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    public static implicit operator String(StringBuilder other)
    {
        return new string(other.value);
    }

    public static implicit operator StringBuilder(String other)
    {
        return new StringBuilder(other);
    }

    public override string ToString()
    {
        return toString();
    }
}
}