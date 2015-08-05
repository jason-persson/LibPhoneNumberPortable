using java.util;
using System;
namespace java.lang {
public abstract class AbstractStringBuilder {
    internal char[] value;
    internal int count;
    internal AbstractStringBuilder() {
    }
    internal AbstractStringBuilder(int capacity) {
        value = new char[capacity];
    }
    public virtual int length()
    {
        return count;
    }
    public virtual int capacity() {
        return value.Length;
    }
    public virtual void ensureCapacity(int minimumCapacity) {
        if (minimumCapacity > 0)
            ensureCapacityInternal(minimumCapacity);
    }
    private void ensureCapacityInternal(int minimumCapacity) {
        // overflow-conscious code
        if (minimumCapacity - value.Length > 0)
            expandCapacity(minimumCapacity);
    }
    internal virtual void expandCapacity(int minimumCapacity)
    {
        int newCapacity = value.Length * 2 + 2;
        if (newCapacity - minimumCapacity < 0)
            newCapacity = minimumCapacity;
        if (newCapacity < 0) {
            if (minimumCapacity < 0) // overflow
                throw new OutOfMemoryException();
            newCapacity = Integer.MAX_VALUE;
        }
        value = Arrays.copyOf(value, newCapacity);
    }
    public virtual void trimToSize()
    {
        if (count < value.Length) {
            value = Arrays.copyOf(value, count);
        }
    }
    public virtual void setLength(int newLength)
    {
        if (newLength < 0)
            throw new IndexOutOfRangeException();
        ensureCapacityInternal(newLength);
        if (count < newLength) {
            for (; count < newLength; count++)
                value[count] = '\0';
        } else {
            count = newLength;
        }
    }
    public virtual char charAt(int index)
    {
        if ((index < 0) || (index >= count))
            throw new IndexOutOfRangeException();
        return value[index];
    }
    public virtual int codePointAt(int index)
    {
        if ((index < 0) || (index >= count)) {
            throw new IndexOutOfRangeException();
        }
        return Character.codePointAt(value, index);
    }
    public virtual int codePointBefore(int index)
    {
        int i = index - 1;
        if ((i < 0) || (i >= count)) {
            throw new IndexOutOfRangeException();
        }
        return Character.codePointBefore(value, index);
    }
    public virtual int codePointCount(int beginIndex, int endIndex)
    {
        if (beginIndex < 0 || endIndex > count || beginIndex > endIndex) {
            throw new IndexOutOfRangeException();
        }
        return Character.codePointCountImpl(value, beginIndex, endIndex-beginIndex);
    }
    public virtual int offsetByCodePoints(int index, int codePointOffset)
    {
        if (index < 0 || index > count) {
            throw new IndexOutOfRangeException();
        }
        return Character.offsetByCodePointsImpl(value, 0, count,
                                                index, codePointOffset);
    }
    public virtual void getChars(int srcBegin, int srcEnd, char[] dst, int dstBegin)
    {
        if (srcBegin < 0)
            throw new IndexOutOfRangeException();
        if ((srcEnd < 0) || (srcEnd > count))
            throw new IndexOutOfRangeException();
        if (srcBegin > srcEnd)
            throw new IndexOutOfRangeException("srcBegin > srcEnd");
        Array.Copy(value, srcBegin, dst, dstBegin, srcEnd - srcBegin);
    }
    public virtual void setCharAt(int index, char ch)
    {
        if ((index < 0) || (index >= count))
            throw new IndexOutOfRangeException();
        value[index] = ch;
    }
    public virtual AbstractStringBuilder append(Object obj)
    {
        return append(String.valueOf(obj));
    }
    public virtual AbstractStringBuilder append(String str)
    {
        if (str == null) str = new String("null");
        int len = str.length();
        ensureCapacityInternal(count + len);
        str.getChars(0, len, value, count);
        count += len;
        return this;
    }
    public virtual AbstractStringBuilder append(string str)
    {
        return append(new String(str));
    }
    // Documentation in subclasses because of synchro difference
    public virtual AbstractStringBuilder append(StringBuffer sb)
    {
        if (sb == null)
            return append(new String("null"));
        int len = sb.length();
        ensureCapacityInternal(count + len);
        sb.getChars(0, len, value, count);
        count += len;
        return this;
    }
    // Documentation in subclasses because of synchro difference
    public virtual AbstractStringBuilder append(CharSequence s)
    {
        if (s == null)
            s = new String("null");
/*        if (s is String)
            return this.append((String)s);
        if (s is StringBuffer)
            return this.append((StringBuffer)s);*/
        return this.append(s, 0, s.length());
    }
    public virtual AbstractStringBuilder append(CharSequence s, int start, int end)
    {
        if (s == null)
            s = new String("null");
        if ((start < 0) || (start > end) || (end > s.length()))
            throw new IndexOutOfRangeException(
                "start " + start + ", end " + end + ", s.length() "
                + s.length());
        int len = end - start;
        ensureCapacityInternal(count + len);
        for (int i = start, j = count; i < end; i++, j++)
            value[j] = s.charAt(i);
        count += len;
        return this;
    }
    public virtual AbstractStringBuilder append(char[] str)
    {
        int len = str.Length;
        ensureCapacityInternal(count + len);
        Array.Copy(str, 0, value, count, len);
        count += len;
        return this;
    }
    public virtual AbstractStringBuilder append(char[] str, int offset, int len)
    {
        if (len > 0)                // let arraycopy report AIOOBE for len < 0
            ensureCapacityInternal(count + len);
        Array.Copy(str, offset, value, count, len);
        count += len;
        return this;
    }
    public virtual AbstractStringBuilder append(boolean b)
    {
        if (b) {
            ensureCapacityInternal(count + 4);
            value[count++] = 't';
            value[count++] = 'r';
            value[count++] = 'u';
            value[count++] = 'e';
        } else {
            ensureCapacityInternal(count + 5);
            value[count++] = 'f';
            value[count++] = 'a';
            value[count++] = 'l';
            value[count++] = 's';
            value[count++] = 'e';
        }
        return this;
    }
    public virtual AbstractStringBuilder append(char c)
    {
        ensureCapacityInternal(count + 1);
        value[count++] = c;
        return this;
    }
    public virtual AbstractStringBuilder append(int i)
    {
        if (i == Integer.MIN_VALUE) {
            append(new String("-2147483648"));
            return this;
        }
        int appendedLength = (i < 0) ? Integer.stringSize(-i) + 1
                                     : Integer.stringSize(i);
        int spaceNeeded = count + appendedLength;
        ensureCapacityInternal(spaceNeeded);
        Integer.getChars(i, spaceNeeded, value);
        count = spaceNeeded;
        return this;
    }
    public virtual AbstractStringBuilder append(long l) {
        if (l == Long.MIN_VALUE) {
            append(new String("-9223372036854775808"));
            return this;
        }
        int appendedLength = (l < 0) ? Long.stringSize(-l) + 1
                                     : Long.stringSize(l);
        int spaceNeeded = count + appendedLength;
        ensureCapacityInternal(spaceNeeded);
        Long.getChars(l, spaceNeeded, value);
        count = spaceNeeded;
        return this;
    }
    public virtual AbstractStringBuilder append(float f)
    {
    	throw new NotImplementedException();
//        new FloatingDecimal(f).appendTo(this);
//        return this;
    }
    public virtual AbstractStringBuilder append(double d)
    {
    	throw new NotImplementedException();
//        new FloatingDecimal(d).appendTo(this);
//        return this;
    }
    public virtual AbstractStringBuilder delete(int start, int end)
    {
        if (start < 0)
            throw new IndexOutOfRangeException();
        if (end > count)
            end = count;
        if (start > end)
            throw new IndexOutOfRangeException();
        int len = end - start;
        if (len > 0) {
            Array.Copy(value, start+len, value, start, count-end);
            count -= len;
        }
        return this;
    }
    public virtual AbstractStringBuilder appendCodePoint(int codePoint)
    {
        int count = this.count;
        if (Character.isBmpCodePoint(codePoint)) {
            ensureCapacityInternal(count + 1);
            value[count] = (char) codePoint;
            this.count = count + 1;
        } else if (Character.isValidCodePoint(codePoint)) {
            ensureCapacityInternal(count + 2);
            Character.toSurrogates(codePoint, value, count);
            this.count = count + 2;
        } else {
            throw new IllegalArgumentException();
        }
        return this;
    }
    public virtual AbstractStringBuilder deleteCharAt(int index)
    {
        if ((index < 0) || (index >= count))
            throw new IndexOutOfRangeException();
        Array.Copy(value, index+1, value, index, count-index-1);
        count--;
        return this;
    }
    public virtual AbstractStringBuilder replace(int start, int end, String str)
    {
        if (start < 0)
            throw new IndexOutOfRangeException();
        if (start > count)
            throw new IndexOutOfRangeException("start > length()");
        if (start > end)
            throw new IndexOutOfRangeException("start > end");
        if (end > count)
            end = count;
        int len = str.length();
        int newCount = count + len - (end - start);
        ensureCapacityInternal(newCount);
        Array.Copy(value, end, value, start + len, count - end);
        str.getChars(value, start);
        count = newCount;
        return this;
    }
    public virtual String substring(int start)
    {
        return substring(start, count);
    }
    public virtual CharSequence subSequence(int start, int end)
    {
        return substring(start, end);
    }
    public virtual String substring(int start, int end)
    {
        if (start < 0)
            throw new IndexOutOfRangeException();
        if (end > count)
            throw new IndexOutOfRangeException();
        if (start > end)
            throw new IndexOutOfRangeException();
        return new String(value, start, end - start);
    }
    public virtual AbstractStringBuilder insert(int index, char[] str, int offset,
                                        int len)
    {
        if ((index < 0) || (index > length()))
            throw new IndexOutOfRangeException();
        if ((offset < 0) || (len < 0) || (offset > str.Length - len))
            throw new IndexOutOfRangeException(
                "offset " + offset + ", len " + len + ", str.length "
                + str.Length);
        ensureCapacityInternal(count + len);
        Array.Copy(value, index, value, index + len, count - index);
        Array.Copy(str, offset, value, index, len);
        count += len;
        return this;
    }
    public virtual AbstractStringBuilder insert(int offset, Object obj)
    {
        return insert(offset, String.valueOf(obj));
    }
    public virtual AbstractStringBuilder insert(int offset, String str)
    {
        if ((offset < 0) || (offset > length()))
            throw new IndexOutOfRangeException();
        if (str == null)
            str = new String("null");
        int len = str.length();
        ensureCapacityInternal(count + len);
        Array.Copy(value, offset, value, offset + len, count - offset);
        str.getChars(value, offset);
        count += len;
        return this;
    }
    public virtual AbstractStringBuilder insert(int offset, char[] str)
    {
        if ((offset < 0) || (offset > length()))
            throw new IndexOutOfRangeException();
        int len = str.Length;
        ensureCapacityInternal(count + len);
        Array.Copy(value, offset, value, offset + len, count - offset);
        Array.Copy(str, 0, value, offset, len);
        count += len;
        return this;
    }
    public virtual AbstractStringBuilder insert(int dstOffset, CharSequence s)
    {
        if (s == null)
            s = new String("null");
/*        if (s is String)
            return this.insert(dstOffset, (String)s);*/
        return this.insert(dstOffset, s, 0, s.length());
    }
    public virtual AbstractStringBuilder insert(int dstOffset, CharSequence s,
                                         int start, int end) {
        if (s == null)
            s = new String("null");
        if ((dstOffset < 0) || (dstOffset > this.length()))
            throw new IndexOutOfRangeException("dstOffset "+dstOffset);
        if ((start < 0) || (end < 0) || (start > end) || (end > s.length()))
            throw new IndexOutOfRangeException(
                "start " + start + ", end " + end + ", s.length() "
                + s.length());
        int len = end - start;
        ensureCapacityInternal(count + len);
        Array.Copy(value, dstOffset, value, dstOffset + len,
                         count - dstOffset);
        for (int i=start; i<end; i++)
            value[dstOffset++] = s.charAt(i);
        count += len;
        return this;
    }
    public virtual AbstractStringBuilder insert(int offset, boolean b)
    {
        return insert(offset, String.valueOf(b));
    }
    public virtual AbstractStringBuilder insert(int offset, char c)
    {
        ensureCapacityInternal(count + 1);
        Array.Copy(value, offset, value, offset + 1, count - offset);
        value[offset] = c;
        count += 1;
        return this;
    }
    public virtual AbstractStringBuilder insert(int offset, int i)
    {
        return insert(offset, String.valueOf(i));
    }
    public virtual AbstractStringBuilder insert(int offset, long l)
    {
        return insert(offset, String.valueOf(l));
    }
    public virtual AbstractStringBuilder insert(int offset, float f)
    {
        return insert(offset, String.valueOf(f));
    }
    public virtual AbstractStringBuilder insert(int offset, double d)
    {
        return insert(offset, String.valueOf(d));
    }
    public virtual int indexOf(String str)
    {
        return indexOf(str, 0);
    }
    public virtual int indexOf(String str, int fromIndex)
    {
        return String.indexOf(value, 0, count,
                              str.toCharArray(), 0, str.length(), fromIndex);
    }
    public virtual int lastIndexOf(String str)
    {
        return lastIndexOf(str, count);
    }
    public virtual int lastIndexOf(String str, int fromIndex)
    {
        return String.lastIndexOf(value, 0, count,
                              str.toCharArray(), 0, str.length(), fromIndex);
    }
    public virtual AbstractStringBuilder reverse()
    {
        boolean hasSurrogate = false;
        int n = count - 1;
        for (int j = (n-1) >> 1; j >= 0; --j) {
            char temp = value[j];
            char temp2 = value[n - j];
            if (!hasSurrogate) {
                hasSurrogate = (temp >= Character.MIN_SURROGATE && temp <= Character.MAX_SURROGATE)
                    || (temp2 >= Character.MIN_SURROGATE && temp2 <= Character.MAX_SURROGATE);
            }
            value[j] = temp2;
            value[n - j] = temp;
        }
        if (hasSurrogate) {
            // Reverse back all valid surrogate pairs
            for (int i = 0; i < count - 1; i++) {
                char c2 = value[i];
                if (Character.isLowSurrogate(c2)) {
                    char c1 = value[i + 1];
                    if (Character.isHighSurrogate(c1)) {
                        value[i++] = c1;
                        value[i] = c2;
                    }
                }
            }
        }
        return this;
    }

    internal char[] getValue() {
        return value;
    }

    public virtual String toString()
    {
        throw new NotImplementedException();
    }
}
}