using System;
namespace java.lang {
public sealed class Integer : IComparable<Integer> {
    public static readonly int   MIN_VALUE = unchecked((int)0x80000000);
    public static readonly int   MAX_VALUE = 0x7fffffff;
    internal readonly static char[] digits = {
        '0' , '1' , '2' , '3' , '4' , '5' ,
        '6' , '7' , '8' , '9' , 'a' , 'b' ,
        'c' , 'd' , 'e' , 'f' , 'g' , 'h' ,
        'i' , 'j' , 'k' , 'l' , 'm' , 'n' ,
        'o' , 'p' , 'q' , 'r' , 's' , 't' ,
        'u' , 'v' , 'w' , 'x' , 'y' , 'z'
    };
    public static String toString(int i, int radix) {
        if (radix < Character.MIN_RADIX || radix > Character.MAX_RADIX)
            radix = 10;
        /* Use the faster version */
        if (radix == 10) {
            return toString(i);
        }
        char[] buf = new char[33];
        boolean negative = (i < 0);
        int charPos = 32;
        if (!negative) {
            i = -i;
        }
        while (i <= -radix) {
            buf[charPos--] = digits[-(i % radix)];
            i = i / radix;
        }
        buf[charPos] = digits[-i];
        if (negative) {
            buf[--charPos] = '-';
        }
        return new String(buf, charPos, (33 - charPos));
    }
    public static String toHexString(int i) {
        return toUnsignedString(i, 4);
    }
    public static String toOctalString(int i) {
        return toUnsignedString(i, 3);
    }
    public static String toBinaryString(int i) {
        return toUnsignedString(i, 1);
    }
    private static String toUnsignedString(int i, int shift) {
        char[] buf = new char[32];
        int charPos = 32;
        int radix = 1 << shift;
        int mask = radix - 1;
        do {
            buf[--charPos] = digits[i & mask];
            i = (int)((uint)i >> shift);
        } while (i != 0);
        return new String(buf, charPos, (32 - charPos));
    }
    internal readonly static char[] DigitTens = {
        '0', '0', '0', '0', '0', '0', '0', '0', '0', '0',
        '1', '1', '1', '1', '1', '1', '1', '1', '1', '1',
        '2', '2', '2', '2', '2', '2', '2', '2', '2', '2',
        '3', '3', '3', '3', '3', '3', '3', '3', '3', '3',
        '4', '4', '4', '4', '4', '4', '4', '4', '4', '4',
        '5', '5', '5', '5', '5', '5', '5', '5', '5', '5',
        '6', '6', '6', '6', '6', '6', '6', '6', '6', '6',
        '7', '7', '7', '7', '7', '7', '7', '7', '7', '7',
        '8', '8', '8', '8', '8', '8', '8', '8', '8', '8',
        '9', '9', '9', '9', '9', '9', '9', '9', '9', '9',
        } ;
    internal readonly static char [] DigitOnes = {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        } ;
        // I use the "invariant division by multiplication" trick to
        // accelerate Integer.toString.  In particular we want to
        // avoid division by 10.
        //
        // The "trick" has roughly the same performance characteristics
        // as the "classic" Integer.toString code on a non-JIT VM.
        // The trick avoids .rem and .div calls but has a longer code
        // path and is thus dominated by dispatch overhead.  In the
        // JIT case the dispatch overhead doesn't exist and the
        // "trick" is considerably faster than the classic code.
        //
        // TODO-FIXME: convert (x * 52429) into the equiv shift-add
        // sequence.
        //
        // RE:  Division by Invariant Integers using Multiplication
        //      T Gralund, P Montgomery
        //      ACM PLDI 1994
        //
    public static String toString(int i) {
        if (i == Integer.MIN_VALUE)
            return new String("-2147483648");
        int size = (i < 0) ? stringSize(-i) + 1 : stringSize(i);
        char[] buf = new char[size];
        getChars(i, size, buf);
        return new String(buf, true);
    }
    internal static void getChars(int i, int index, char[] buf) {
        int q, r;
        int charPos = index;
        char sign = (char)0;
        if (i < 0) {
            sign = '-';
            i = -i;
        }
        // Generate two digits per iteration
        while (i >= 65536) {
            q = i / 100;
        // really: r = i - (q * 100);
            r = i - ((q << 6) + (q << 5) + (q << 2));
            i = q;
            buf [--charPos] = DigitOnes[r];
            buf [--charPos] = DigitTens[r];
        }
        // Fall thru to fast mode for smaller numbers
        // assert(i <= 65536, i);
        for (;;) {
            q = (int)((uint)(i * 52429) >> (16+3));
            r = i - ((q << 3) + (q << 1));  // r = i-(q*10) ...
            buf [--charPos] = digits [r];
            i = q;
            if (i == 0) break;
        }
        if (sign != 0) {
            buf [--charPos] = sign;
        }
    }
    readonly static int [] sizeTable = { 9, 99, 999, 9999, 99999, 999999, 9999999,
                                      99999999, 999999999, Integer.MAX_VALUE };
    // Requires positive x
    internal static int stringSize(int x) {
        for (int i=0; ; i++)
            if (x <= sizeTable[i])
                return i+1;
    }
    public static int parseInt(String s, int radix)
    {
        /*
         * WARNING: This method may be invoked early during VM initialization
         * before IntegerCache is initialized. Care must be taken to not use
         * the valueOf method.
         */
        if (s == null) {
            throw new NumberFormatException("null");
        }
        if (radix < Character.MIN_RADIX) {
            throw new NumberFormatException("radix " + radix +
                                            " less than Character.MIN_RADIX");
        }
        if (radix > Character.MAX_RADIX) {
            throw new NumberFormatException("radix " + radix +
                                            " greater than Character.MAX_RADIX");
        }
        int result = 0;
        boolean negative = false;
        int i = 0, len = s.length();
        int limit = -Integer.MAX_VALUE;
        int multmin;
        int digit;
        if (len > 0) {
            char firstChar = s.charAt(0);
            if (firstChar < '0') { // Possible leading "+" or "-"
                if (firstChar == '-') {
                    negative = true;
                    limit = Integer.MIN_VALUE;
                } else if (firstChar != '+')
                    throw NumberFormatException.forInputString(s);
                if (len == 1) // Cannot have lone "+" or "-"
                    throw NumberFormatException.forInputString(s);
                i++;
            }
            multmin = limit / radix;
            while (i < len) {
                // Accumulating negatively avoids surprises near MAX_VALUE
                digit = Character.digit(s.charAt(i++),radix);
                if (digit < 0) {
                    throw NumberFormatException.forInputString(s);
                }
                if (result < multmin) {
                    throw NumberFormatException.forInputString(s);
                }
                result *= radix;
                if (result < limit + digit) {
                    throw NumberFormatException.forInputString(s);
                }
                result -= digit;
            }
        } else {
            throw NumberFormatException.forInputString(s);
        }
        return negative ? result : -result;
    }
    public static int parseInt(String s) {
        return parseInt(s,10);
    }
    public static Integer valueOf(String s, int radix) {
        return Integer.valueOf(parseInt(s,radix));
    }
    public static Integer valueOf(String s) {
        return Integer.valueOf(parseInt(s, 10));
    }
    private class IntegerCache {
        internal static readonly int low = -128;
        internal static readonly int high;
        internal static readonly Integer[] cache;
        static IntegerCache() {
            // high value may be configured by property
            int h = 127;
//            String integerCacheHighPropValue = null;
//            if (integerCacheHighPropValue != null) {
//                int i = parseInt(integerCacheHighPropValue);
//                i = Math.max(i, 127);
//                // Maximum array size is Integer.MAX_VALUE
//                h = Math.min(i, Integer.MAX_VALUE - (-low));
//            }
            high = h;
            cache = new Integer[(high - low) + 1];
            int j = low;
            for(int k = 0; k < cache.Length; k++)
                cache[k] = new Integer(j++);
        }
        private IntegerCache() {}
    }
    public static Integer valueOf(int i) {
        if (i >= IntegerCache.low && i <= IntegerCache.high)
            return IntegerCache.cache[i + (-IntegerCache.low)];
        return new Integer(i);
    }
    private readonly int value;
    public Integer(int value) {
        this.value = value;
    }
    public Integer(String s) {
        this.value = parseInt(s, 10);
    }
    public byte byteValue() {
        return (byte)value;
    }
    public short shortValue() {
        return (short)value;
    }
    public int intValue() {
        return value;
    }
    public long longValue() {
        return (long)value;
    }
    public float floatValue() {
        return (float)value;
    }
    public double doubleValue() {
        return (double)value;
    }
    public String toString() {
        return toString(value);
    }
    public int hashCode() {
        return value;
    }
    public boolean equals(Object obj) {
        if (obj is Integer) {
            return value == ((Integer)obj).intValue();
        }
        return false;
    }
    public static Integer getInteger(String nm) {
        return getInteger(nm, null);
    }
    public static Integer getInteger(String nm, int val) {
        Integer result = getInteger(nm, null);
        return (result == null) ? Integer.valueOf(val) : result;
    }
    public static Integer getInteger(String nm, Integer val) {
    	throw new NotImplementedException();
//        String v = null;
//        try {
//            v = System.getProperty(nm);
//        } catch (IllegalArgumentException e) {
//        } catch (NullPointerException e) {
//        }
//        if (v != null) {
//            try {
//                return Integer.decode(v);
//            } catch (NumberFormatException e) {
//            }
//        }
//        return val;
    }
    public static Integer decode(String nm) {
        int radix = 10;
        int index = 0;
        boolean negative = false;
        Integer result;
        if (nm.length() == 0)
            throw new NumberFormatException("Zero length string");
        char firstChar = nm.charAt(0);
        // Handle sign, if present
        if (firstChar == '-') {
            negative = true;
            index++;
        } else if (firstChar == '+')
            index++;
        // Handle radix specifier, if present
        if (nm.startsWith(new String("0x"), index) || nm.startsWith(new String("0X"), index)) {
            index += 2;
            radix = 16;
        }
        else if (nm.startsWith(new String("#"), index)) {
            index ++;
            radix = 16;
        }
        else if (nm.startsWith(new String("0"), index) && nm.length() > 1 + index) {
            index ++;
            radix = 8;
        }
        if (nm.startsWith(new String("-"), index) || nm.startsWith(new String("+"), index))
            throw new NumberFormatException("Sign character in wrong position");
        try {
            result = Integer.valueOf(nm.substring(index), radix);
            result = negative ? Integer.valueOf(-result.intValue()) : result;
        } catch (NumberFormatException) {
            // If number is Integer.MIN_VALUE, we'll end up here. The next line
            // handles this case, and causes any genuine format error to be
            // rethrown.
            String constant = negative ? new String("-" + nm.substring(index))
                                       : nm.substring(index);
            result = Integer.valueOf(constant, radix);
        }
        return result;
    }
    public int compareTo(Integer anotherInteger) {
        return compare(this.value, anotherInteger.value);
    }
    public static int compare(int x, int y) {
        return (x < y) ? -1 : ((x == y) ? 0 : 1);
    }
    // Bit twiddling
    public static readonly int SIZE = 32;
    public static int highestOneBit(int i) {
        // HD, Figure 3-1
        i |= (i >>  1);
        i |= (i >>  2);
        i |= (i >>  4);
        i |= (i >>  8);
        i |= (i >> 16);
        return i - (int)(((uint)i) >> 1);
    }
    public static int lowestOneBit(int i) {
        // HD, Section 2-1
        return i & -i;
    }
    public static int numberOfLeadingZeros(int i) {
        // HD, Figure 5-6
        if (i == 0)
            return 32;
        int n = 1;
        if ((int)(((uint)i) >> 16) == 0) { n += 16; i <<= 16; }
        if ((int)(((uint)i) >> 24) == 0) { n += 8; i <<= 8; }
        if ((int)(((uint)i) >> 28) == 0) { n += 4; i <<= 4; }
        if ((int)(((uint)i) >> 30) == 0) { n += 2; i <<= 2; }
        n -= (int)(((uint)i) >> 31);
        return n;
    }
    public static int numberOfTrailingZeros(int i) {
        // HD, Figure 5-14
        int y;
        if (i == 0) return 32;
        int n = 31;
        y = i <<16; if (y != 0) { n = n -16; i = y; }
        y = i << 8; if (y != 0) { n = n - 8; i = y; }
        y = i << 4; if (y != 0) { n = n - 4; i = y; }
        y = i << 2; if (y != 0) { n = n - 2; i = y; }
        return n - (int)(((uint)(i << 1)) >> 31);
    }
    public static int bitCount(int i) {
        // HD, Figure 5-2
        i = i - (((int)(((uint)i) >> 1)) & 0x55555555);
        i = (i & 0x33333333) + (((int)(((uint)i) >> 2)) & 0x33333333);
        i = (i + ((int)(((uint)i) >> 4))) & 0x0f0f0f0f;
        i = i + ((int)(((uint)i) >> 8));
        i = i + ((int)(((uint)i) >> 16));
        return i & 0x3f;
    }
    public static int rotateLeft(int i, int distance) {
        return (i << distance) | ((int)(((uint)i) >> -distance));
    }
    public static int rotateRight(int i, int distance) {
        return ((int)(((uint)i) >> distance)) | (i << -distance);
    }
    public static int reverse(int i) {
        // HD, Figure 7-1
        i = (i & 0x55555555) << 1 | ((int)(((uint)i) >> 1)) & 0x55555555;
        i = (i & 0x33333333) << 2 | ((int)(((uint)i) >> 2)) & 0x33333333;
        i = (i & 0x0f0f0f0f) << 4 | ((int)(((uint)i) >> 4)) & 0x0f0f0f0f;
        i = (i << 24) | ((i & 0xff00) << 8) |
            (((int)(((uint)i) >> 8)) & 0xff00) | ((int)(((uint)i) >> 24));
        return i;
    }
    public static int signum(int i) {
        // HD, Section 2-7
        return (i >> 31) | ((int)(((uint)-i) >> 31));
    }
    public static int reverseBytes(int i) {
        return (((int)(((uint)i) >> 24))) |
               ((i >>   8) &   0xFF00) |
               ((i <<   8) & 0xFF0000) |
               ((i << 24));
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    public static implicit operator int(Integer other)
    {
        return other.value;
    }

    public static implicit operator Integer(int other)
    {
        return new Integer(other);
    }

    public override string ToString()
    {
        return toString();
    }

    public override bool Equals(object obj)
    {
        if (obj is Integer)
        {
            return this.equals((Integer)obj);
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return ((int)this).GetHashCode();
    }

    public int CompareTo(Integer other)
    {
        return ((int)this).CompareTo(other);
    }
}
}