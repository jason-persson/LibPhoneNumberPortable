using java.util;
using java.util.regex;
using JavaPort.Collections;
using System;

using String = java.lang.String;
using StringBuilder = java.lang.StringBuilder;
using Math = java.lang.Math;
using IOException = java.io.IOException;
using NullPointerException = java.lang.NullPointerException;

namespace java.lang {
    public sealed class String : IComparable<String>/*, CharSequence*/ {
    private readonly char[] value;

    private int hash;

    public String() {
        this.value = new char[0];
    }

    public String(String original)
    {
        this.value = original.value;
        this.hash = original.hash;
    }

    public String(string value)
    {
        if (value == null)
            this.value = new char[0];
        else
            this.value = value.ToCharArray();
    }

    public String(char[] value) {
        this.value = Arrays.copyOf(value, value.Length);
    }

    public String(char[] value, int offset, int count) {
        if (offset < 0) {
            throw new IndexOutOfRangeException();
        }
        if (count < 0) {
            throw new IndexOutOfRangeException();
        }
        // Note: offset or count might be near -1>>>1.
        if (offset > value.Length - count) {
            throw new IndexOutOfRangeException();
        }
        this.value = Arrays.copyOfRange(value, offset, offset+count);
    }

    public String(int[] codePoints, int offset, int count) {
        if (offset < 0) {
            throw new IndexOutOfRangeException();
        }
        if (count < 0) {
            throw new IndexOutOfRangeException();
        }
        // Note: offset or count might be near -1>>>1.
        if (offset > codePoints.Length - count) {
            throw new IndexOutOfRangeException();
        }

        int end = offset + count;

        // Pass 1: Compute precise size of char[]
        int n = count;
        for (int i = offset; i < end; i++) {
            int c = codePoints[i];
            if (Character.isBmpCodePoint(c))
                continue;
            else if (Character.isValidCodePoint(c))
                n++;
            else throw new IllegalArgumentException();
        }

        // Pass 2: Allocate and fill in char[]
        char[] v = new char[n];

        for (int i = offset, j = 0; i < end; i++, j++) {
            int c = codePoints[i];
            if (Character.isBmpCodePoint(c))
                v[j] = (char)c;
            else
                Character.toSurrogates(c, v, j++);
        }

        this.value = v;
    }

    private static void checkBounds(byte[] bytes, int offset, int length) {
        if (length < 0)
            throw new IndexOutOfRangeException();
        if (offset < 0)
            throw new IndexOutOfRangeException();
        if (offset > bytes.Length - length)
            throw new IndexOutOfRangeException();
    }
    /*
    public String(byte[] bytes, int offset, int length, String charsetName) {
        if (charsetName == null)
            throw new ArgumentNullException("charsetName");
        checkBounds(bytes, offset, length);
        this.value = StringCoding.decode(charsetName, bytes, offset, length);
    }

    public String(byte[] bytes, int offset, int length, Charset charset) {
        if (charset == null)
            throw new ArgumentNullException("charset");
        checkBounds(bytes, offset, length);
        this.value =  StringCoding.decode(charset, bytes, offset, length);
    }

    public String(byte[] bytes, String charsetName) {
        this(bytes, 0, bytes.length, charsetName);
    }

    public String(byte[] bytes, Charset charset) {
        this(bytes, 0, bytes.length, charset);
    }

    public String(byte[] bytes, int offset, int length) {
        checkBounds(bytes, offset, length);
        this.value = StringCoding.decode(bytes, offset, length);
    }

    public String(byte[] bytes) {
        this(bytes, 0, bytes.Length);
    }
    */
    public String(StringBuffer buffer) {
        lock(buffer) {
            this.value = Arrays.copyOf(buffer.getValue(), buffer.length());
        }
    }

    public String(StringBuilder builder) {
        this.value = Arrays.copyOf(builder.getValue(), builder.length());
    }

    internal String(char[] value, boolean share) {
        this.value = value;
    }

    public int length() {
        return value.Length;
    }

    public boolean isEmpty() {
        return value.Length == 0;
    }

    public char charAt(int index)
    {
        if ((index < 0) || (index >= value.Length)) {
            throw new IndexOutOfRangeException();
        }
        return value[index];
    }

    public int codePointAt(int index) {
        if ((index < 0) || (index >= value.Length)) {
            throw new IndexOutOfRangeException();
        }
        return Character.codePointAtImpl(value, index, value.Length);
    }

    public int codePointBefore(int index) {
        int i = index - 1;
        if ((i < 0) || (i >= value.Length)) {
            throw new IndexOutOfRangeException();
        }
        return Character.codePointBeforeImpl(value, index, 0);
    }

    public int codePointCount(int beginIndex, int endIndex) {
        if (beginIndex < 0 || endIndex > value.Length || beginIndex > endIndex) {
            throw new IndexOutOfRangeException();
        }
        return Character.codePointCountImpl(value, beginIndex, endIndex - beginIndex);
    }

    public int offsetByCodePoints(int index, int codePointOffset) {
        if (index < 0 || index > value.Length) {
            throw new IndexOutOfRangeException();
        }
        return Character.offsetByCodePointsImpl(value, 0, value.Length,
                index, codePointOffset);
    }

    internal void getChars(char[] dst, int dstBegin) {
        Array.Copy(value, 0, dst, dstBegin, value.Length);
    }

    public void getChars(int srcBegin, int srcEnd, char[] dst, int dstBegin) {
        if (srcBegin < 0) {
            throw new IndexOutOfRangeException();
        }
        if (srcEnd > value.Length) {
            throw new IndexOutOfRangeException();
        }
        if (srcBegin > srcEnd) {
            throw new IndexOutOfRangeException();
        }
        Array.Copy(value, srcBegin, dst, dstBegin, srcEnd - srcBegin);
    }
/*
    public byte[] getBytes(String charsetName) {
        if (charsetName == null) throw new ArgumentNullException();
        return StringCoding.encode(charsetName, value, 0, value.Length);
    }

    public byte[] getBytes(Charset charset) {
        if (charset == null) throw new ArgumentNullException();
        return StringCoding.encode(charset, value, 0, value.Length);
    }


    public byte[] getBytes() {
        return StringCoding.encode(value, 0, value.Length);
    }
*/
    public boolean equals(Object anObject) {
        if ((Object)this == anObject) {
            return true;
        }
        if (anObject is String) {
            String anotherString = (String) anObject;
            int n = value.Length;
            if (n == anotherString.value.Length) {
                char[] v1 = value;
                char[] v2 = anotherString.value;
                int i = 0;
                while (n-- != 0) {
                    if (v1[i] != v2[i])
                            return false;
                    i++;
                }
                return true;
            }
        }
        if (anObject is string)
        {
            string anotherString = (string)anObject;
            int n = value.Length;
            if (n == anotherString.Length)
            {
                char[] v1 = value;
                char[] v2 = anotherString.ToCharArray();
                int i = 0;
                while (n-- != 0)
                {
                    if (v1[i] != v2[i])
                        return false;
                    i++;
                }
                return true;
            }
        }
        return false;
    }

    public boolean contentEquals(StringBuffer sb) {
        lock (sb) {
            return contentEquals((CharSequence) sb);
        }
    }

    public boolean contentEquals(CharSequence cs) {
        if (value.Length != cs.length())
            return false;
        // Argument is a StringBuffer, StringBuilder
        //if (cs is StringBuilder) {
        //    char[] v1 = value;
        //    char[] v2 = ((AbstractStringBuilder) cs).getValue();
        //    int i = 0;
        //    int n = value.Length;
        //    while (n-- != 0) {
        //        if (v1[i] != v2[i])
        //            return false;
        //        i++;
        //    }
        //    return true;
        //}
        // Argument is a String
        if (cs.Equals(this))
            return true;
        // Argument is a generic CharSequence
        char[] _v1 = value;
        int _i = 0;
        int _n = value.Length;
        while (_n-- != 0) {
            if (_v1[_i] != cs.charAt(_i))
                return false;
            _i++;
        }
        return true;
    }

    public boolean equalsIgnoreCase(String anotherString) {
        return (this == anotherString) ? true
                : (anotherString != null)
                && (anotherString.value.Length == value.Length)
                && regionMatches(true, 0, anotherString, 0, value.Length);
    }

    public int compareTo(String anotherString) {
        int len1 = value.Length;
        int len2 = anotherString.value.Length;
        int lim = Math.min(len1, len2);
        char[] v1 = value;
        char[] v2 = anotherString.value;

        int k = 0;
        while (k < lim) {
            char c1 = v1[k];
            char c2 = v2[k];
            if (c1 != c2) {
                return c1 - c2;
            }
            k++;
        }
        return len1 - len2;
    }

    public int compareToIgnoreCase(String str) {
        return this.toString().compareToIgnoreCase(str);
    }

    public boolean regionMatches(int toffset, String other, int ooffset,
            int len) {
        char[] ta = value;
        int to = toffset;
        char[] pa = other.value;
        int po = ooffset;
        // Note: toffset, ooffset, or len might be near -1>>>1.
        if ((ooffset < 0) || (toffset < 0)
                || (toffset > (long)value.Length - len)
                || (ooffset > (long)other.value.Length - len)) {
            return false;
        }
        while (len-- > 0) {
            if (ta[to++] != pa[po++]) {
                return false;
            }
        }
        return true;
    }

    public boolean regionMatches(boolean ignoreCase, int toffset,
            String other, int ooffset, int len) {
        char[] ta = value;
        int to = toffset;
        char[] pa = other.value;
        int po = ooffset;
        // Note: toffset, ooffset, or len might be near -1>>>1.
        if ((ooffset < 0) || (toffset < 0)
                || (toffset > (long)value.Length - len)
                || (ooffset > (long)other.value.Length - len)) {
            return false;
        }
        while (len-- > 0) {
            char c1 = ta[to++];
            char c2 = pa[po++];
            if (c1 == c2) {
                continue;
            }
            if (ignoreCase) {
                // If characters don't match but case may be ignored,
                // try converting both characters to uppercase.
                // If the results match, then the comparison scan should
                // continue.
                char u1 = Character.toUpperCase(c1);
                char u2 = Character.toUpperCase(c2);
                if (u1 == u2) {
                    continue;
                }
                // Unfortunately, conversion to uppercase does not work properly
                // for the Georgian alphabet, which has strange rules about case
                // conversion.  So we need to make one last check before
                // exiting.
                if (Character.toLowerCase(u1) == Character.toLowerCase(u2)) {
                    continue;
                }
            }
            return false;
        }
        return true;
    }

    public boolean startsWith(String prefix, int toffset) {
        char[] ta = value;
        int to = toffset;
        char[] pa = prefix.value;
        int po = 0;
        int pc = prefix.value.Length;
        // Note: toffset might be near -1>>>1.
        if ((toffset < 0) || (toffset > value.Length - pc)) {
            return false;
        }
        while (--pc >= 0) {
            if (ta[to++] != pa[po++]) {
                return false;
            }
        }
        return true;
    }

    public boolean startsWith(String prefix) {
        return startsWith(prefix, 0);
    }

    public boolean endsWith(String suffix) {
        return startsWith(suffix, value.Length - suffix.value.Length);
    }

    public int hashCode() {
        int h = hash;
        if (h == 0 && value.Length > 0) {
            char[] val = value;

            for (int i = 0; i < value.Length; i++) {
                h = 31 * h + val[i];
            }
            hash = h;
        }
        return h;
    }

    public int indexOf(int ch) {
        return indexOf(ch, 0);
    }

    public int indexOf(int ch, int fromIndex) {
        int max = value.Length;
        if (fromIndex < 0) {
            fromIndex = 0;
        } else if (fromIndex >= max) {
            // Note: fromIndex might be near -1>>>1.
            return -1;
        }

        if (ch < Character.MIN_SUPPLEMENTARY_CODE_POINT) {
            // handle most cases here (ch is a BMP code point or a
            // negative value (invalid code point))
            char[] _value = this.value;
            for (int i = fromIndex; i < max; i++) {
                if (_value[i] == ch) {
                    return i;
                }
            }
            return -1;
        } else {
            return indexOfSupplementary(ch, fromIndex);
        }
    }

    private int indexOfSupplementary(int ch, int fromIndex) {
        if (Character.isValidCodePoint(ch)) {
            char[] value = this.value;
            char hi = Character.highSurrogate(ch);
            char lo = Character.lowSurrogate(ch);
            int max = value.Length - 1;
            for (int i = fromIndex; i < max; i++) {
                if (value[i] == hi && value[i + 1] == lo) {
                    return i;
                }
            }
        }
        return -1;
    }

    public int lastIndexOf(int ch) {
        return lastIndexOf(ch, value.Length - 1);
    }

    public int lastIndexOf(int ch, int fromIndex) {
        if (ch < Character.MIN_SUPPLEMENTARY_CODE_POINT) {
            // handle most cases here (ch is a BMP code point or a
            // negative value (invalid code point))
            char[] value = this.value;
            int i = Math.min(fromIndex, value.Length - 1);
            for (; i >= 0; i--) {
                if (value[i] == ch) {
                    return i;
                }
            }
            return -1;
        } else {
            return lastIndexOfSupplementary(ch, fromIndex);
        }
    }

    private int lastIndexOfSupplementary(int ch, int fromIndex) {
        if (Character.isValidCodePoint(ch)) {
            char[] value = this.value;
            char hi = Character.highSurrogate(ch);
            char lo = Character.lowSurrogate(ch);
            int i = Math.min(fromIndex, value.Length - 2);
            for (; i >= 0; i--) {
                if (value[i] == hi && value[i + 1] == lo) {
                    return i;
                }
            }
        }
        return -1;
    }

    public int indexOf(String str) {
        return indexOf(str, 0);
    }

    public int indexOf(String str, int fromIndex) {
        return indexOf(value, 0, value.Length,
                str.value, 0, str.value.Length, fromIndex);
    }

    internal static int indexOf(char[] source, int sourceOffset, int sourceCount,
            char[] target, int targetOffset, int targetCount,
            int fromIndex) {
        if (fromIndex >= sourceCount) {
            return (targetCount == 0 ? sourceCount : -1);
        }
        if (fromIndex < 0) {
            fromIndex = 0;
        }
        if (targetCount == 0) {
            return fromIndex;
        }

        char first = target[targetOffset];
        int max = sourceOffset + (sourceCount - targetCount);

        for (int i = sourceOffset + fromIndex; i <= max; i++) {
            /* Look for first character. */
            if (source[i] != first) {
                while (++i <= max && source[i] != first);
            }

            /* Found first character, now look at the rest of v2 */
            if (i <= max) {
                int j = i + 1;
                int end = j + targetCount - 1;
                for (int k = targetOffset + 1; j < end && source[j]
                        == target[k]; j++, k++);

                if (j == end) {
                    /* Found whole string. */
                    return i - sourceOffset;
                }
            }
        }
        return -1;
    }

    public int lastIndexOf(String str) {
        return lastIndexOf(str, value.Length);
    }


    public int lastIndexOf(String str, int fromIndex) {
        return lastIndexOf(value, 0, value.Length,
                str.value, 0, str.value.Length, fromIndex);
    }


    internal static int lastIndexOf(char[] source, int sourceOffset, int sourceCount,
            char[] target, int targetOffset, int targetCount,
            int fromIndex) {
        /*
         * Check arguments; return immediately where possible. For
         * consistency, don't check for null str.
         */
        int rightIndex = sourceCount - targetCount;
        if (fromIndex < 0) {
            return -1;
        }
        if (fromIndex > rightIndex) {
            fromIndex = rightIndex;
        }
        /* Empty string always matches. */
        if (targetCount == 0) {
            return fromIndex;
        }

        int strLastIndex = targetOffset + targetCount - 1;
        char strLastChar = target[strLastIndex];
        int min = sourceOffset + targetCount - 1;
        int i = min + fromIndex;

        startSearchForLastChar:
        while (true) {
            while (i >= min && source[i] != strLastChar) {
                i--;
            }
            if (i < min) {
                return -1;
            }
            int j = i - 1;
            int start = j - (targetCount - 1);
            int k = strLastIndex - 1;

            while (j > start) {
                if (source[j--] != target[k--]) {
                    i--;
                    goto startSearchForLastChar;
                }
            }
            return start - sourceOffset + 1;
        }
    }


    public String substring(int beginIndex) {
        if (beginIndex < 0) {
            throw new IndexOutOfRangeException();
        }
        int subLen = value.Length - beginIndex;
        if (subLen < 0) {
            throw new IndexOutOfRangeException();
        }
        return (beginIndex == 0) ? this : new String(value, beginIndex, subLen);
    }


    public String substring(int beginIndex, int endIndex) {
        if (beginIndex < 0) {
            throw new IndexOutOfRangeException();
        }
        if (endIndex > value.Length) {
            throw new IndexOutOfRangeException();
        }
        int subLen = endIndex - beginIndex;
        if (subLen < 0) {
            throw new IndexOutOfRangeException();
        }
        return ((beginIndex == 0) && (endIndex == value.Length)) ? this
                : new String(value, beginIndex, subLen);
    }


    public CharSequence subSequence(int beginIndex, int endIndex)
    {
        return this.substring(beginIndex, endIndex);
    }


    public String concat(String str) {
        int otherLen = str.length();
        if (otherLen == 0) {
            return this;
        }
        int len = value.Length;
        char[] buf = Arrays.copyOf(value, len + otherLen);
        str.getChars(buf, len);
        return new String(buf, true);
    }


    public String replace(char oldChar, char newChar) {
        if (oldChar != newChar) {
            int len = value.Length;
            int i = -1;
            char[] val = value; /* avoid getfield opcode */

            while (++i < len) {
                if (val[i] == oldChar) {
                    break;
                }
            }
            if (i < len) {
                char[] buf = new char[len];
                for (int j = 0; j < i; j++) {
                    buf[j] = val[j];
                }
                while (i < len) {
                    char c = val[i];
                    buf[i] = (c == oldChar) ? newChar : c;
                    i++;
                }
                return new String(buf, true);
            }
        }
        return this;
    }


    public boolean matches(String regex) {
        return Pattern.matches(regex, this);
    }


    public boolean contains(CharSequence s) {
        return indexOf(s.toString()) > -1;
    }


    public String replaceFirst(String regex, String replacement) {
        return Pattern.compile(regex).matcher(this).replaceFirst(replacement);
    }


    public String replaceAll(String regex, String replacement) {
        return Pattern.compile(regex).matcher(this).replaceAll(replacement);
    }


    public String replace(CharSequence target, CharSequence replacement) {
        return Pattern.compile(target.toString(), Pattern.LITERAL).matcher(
                this).replaceAll(Matcher.quoteReplacement(replacement.toString()));
    }


    public String[] split(String regex, int limit) {
        /* fastpath if the regex is a
         (1)one-char String and this character is not one of the
            RegEx's meta characters ".$|()[{^?*+\\", or
         (2)two-char String and the first char is the backslash and
            the second is not the ascii digit or ascii letter.
         */
        char ch = (char)0;
        if (((regex.value.Length == 1 &&
             new String(".$|()[{^?*+\\").indexOf(ch = regex.charAt(0)) == -1) ||
             (regex.length() == 2 &&
              regex.charAt(0) == '\\' &&
              (((ch = regex.charAt(1))-'0')|('9'-ch)) < 0 &&
              ((ch-'a')|('z'-ch)) < 0 &&
              ((ch-'A')|('Z'-ch)) < 0)) &&
            (ch < Character.MIN_HIGH_SURROGATE ||
             ch > Character.MAX_LOW_SURROGATE))
        {
            int off = 0;
            int next = 0;
            boolean limited = limit > 0;
            ArrayList<String> list = new ArrayList<String>();
            while ((next = indexOf(ch, off)) != -1) {
                if (!limited || list.size() < limit - 1) {
                    list.add(substring(off, next));
                    off = next + 1;
                } else {    // last one
                    //assert (list.size() == limit - 1);
                    list.add(substring(off, value.Length));
                    off = value.Length;
                    break;
                }
            }
            // If no match was found, return this
            if (off == 0)
                return new String[]{this};

            // Add remaining segment
            if (!limited || list.size() < limit)
                list.add(substring(off, value.Length));

            // Construct result
            int resultSize = list.size();
            if (limit == 0)
                while (resultSize > 0 && list.get(resultSize - 1).length() == 0)
                    resultSize--;
            String[] result = new String[resultSize];
            return list.subList(0, resultSize).toArray(result);
        }
        return Pattern.compile(regex).split(this, limit);
    }


    public String[] split(String regex) {
        return split(regex, 0);
    }

/*
    public String toLowerCase(Locale locale) {
    	throw new NotImplementedException();
//        if (locale == null) {
//            throw new ArgumentNullException();
//        }
//
//        int firstUpper;
//        readonly int len = value.Length;
//
//        /* Now check if there are any characters that need to be changed. * /
//        scan: {
//            for (firstUpper = 0 ; firstUpper < len; ) {
//                char c = value[firstUpper];
//                if ((c >= Character.MIN_HIGH_SURROGATE)
//                        && (c <= Character.MAX_HIGH_SURROGATE)) {
//                    int supplChar = codePointAt(firstUpper);
//                    if (supplChar != Character.toLowerCase(supplChar)) {
//                        break scan;
//                    }
//                    firstUpper += Character.charCount(supplChar);
//                } else {
//                    if (c != Character.toLowerCase(c)) {
//                        break scan;
//                    }
//                    firstUpper++;
//                }
//            }
//            return this;
//        }
//
//        char[] result = new char[len];
//        int resultOffset = 0;  /* result may grow, so i+resultOffset
//                                * is the write location in result * /
//
//        /* Just copy the first few lowerCase characters. * /
//        System.arraycopy(value, 0, result, 0, firstUpper);
//
//        String lang = locale.getLanguage();
//        boolean localeDependent =
//                (lang == new String("tr") || lang == new String("az") || lang == new String("lt"));
//        char[] lowerCharArray;
//        int lowerChar;
//        int srcChar;
//        int srcCount;
//        for (int i = firstUpper; i < len; i += srcCount) {
//            srcChar = (int)value[i];
//            if ((char)srcChar >= Character.MIN_HIGH_SURROGATE
//                    && (char)srcChar <= Character.MAX_HIGH_SURROGATE) {
//                srcChar = codePointAt(i);
//                srcCount = Character.charCount(srcChar);
//            } else {
//                srcCount = 1;
//            }
//            if (localeDependent || srcChar == '\u03A3') { // GREEK CAPITAL LETTER SIGMA
//                lowerChar = ConditionalSpecialCasing.toLowerCaseEx(this, i, locale);
//            } else if (srcChar == '\u0130') { // LATIN CAPITAL LETTER I DOT
//                lowerChar = Character.ERROR;
//            } else {
//                lowerChar = Character.toLowerCase(srcChar);
//            }
//            if ((lowerChar == Character.ERROR)
//                    || (lowerChar >= Character.MIN_SUPPLEMENTARY_CODE_POINT)) {
//                if (lowerChar == Character.ERROR) {
//                    if (!localeDependent && srcChar == '\u0130') {
//                        lowerCharArray =
//                                ConditionalSpecialCasing.toLowerCaseCharArray(this, i, Locale.ENGLISH);
//                    } else {
//                        lowerCharArray =
//                                ConditionalSpecialCasing.toLowerCaseCharArray(this, i, locale);
//                    }
//                } else if (srcCount == 2) {
//                    resultOffset += Character.toChars(lowerChar, result, i + resultOffset) - srcCount;
//                    continue;
//                } else {
//                    lowerCharArray = Character.toChars(lowerChar);
//                }
//
//                /* Grow result if needed * /
//                int mapLen = lowerCharArray.length;
//                if (mapLen > srcCount) {
//                    char[] result2 = new char[result.length + mapLen - srcCount];
//                    System.arraycopy(result, 0, result2, 0, i + resultOffset);
//                    result = result2;
//                }
//                for (int x = 0; x < mapLen; ++x) {
//                    result[i + resultOffset + x] = lowerCharArray[x];
//                }
//                resultOffset += (mapLen - srcCount);
//            } else {
//                result[i + resultOffset] = (char)lowerChar;
//            }
//        }
//        return new String(result, 0, len + resultOffset);
    }
    */

    public String toLowerCase() {
        return ((string)this).ToLower();// toLowerCase(Locale.getDefault());
    }

/*
    public String toUpperCase(Locale locale) {
    	throw new NotImplementedException();
//        if (locale == null) {
//            throw new ArgumentNullException();
//        }
//
//        int firstLower;
//        readonly int len = value.Length;
//
//        /* Now check if there are any characters that need to be changed. * /
//        scan: {
//           for (firstLower = 0 ; firstLower < len; ) {
//                int c = (int)value[firstLower];
//                int srcCount;
//                if ((c >= Character.MIN_HIGH_SURROGATE)
//                        && (c <= Character.MAX_HIGH_SURROGATE)) {
//                    c = codePointAt(firstLower);
//                    srcCount = Character.charCount(c);
//                } else {
//                    srcCount = 1;
//                }
//                int upperCaseChar = Character.toUpperCaseEx(c);
//                if ((upperCaseChar == Character.ERROR)
//                        || (c != upperCaseChar)) {
//                    break scan;
//                }
//                firstLower += srcCount;
//            }
//            return this;
//        }
//
//        char[] result = new char[len]; /* may grow * /
//        int resultOffset = 0;  /* result may grow, so i+resultOffset
//         * is the write location in result * /
//
//        /* Just copy the first few upperCase characters. * /
//        System.arraycopy(value, 0, result, 0, firstLower);
//
//        String lang = locale.getLanguage();
//        boolean localeDependent =
//                (lang == new String("tr") || lang == new String("az") || lang == new String("lt"));
//        char[] upperCharArray;
//        int upperChar;
//        int srcChar;
//        int srcCount;
//        for (int i = firstLower; i < len; i += srcCount) {
//            srcChar = (int)value[i];
//            if ((char)srcChar >= Character.MIN_HIGH_SURROGATE &&
//                (char)srcChar <= Character.MAX_HIGH_SURROGATE) {
//                srcChar = codePointAt(i);
//                srcCount = Character.charCount(srcChar);
//            } else {
//                srcCount = 1;
//            }
//            if (localeDependent) {
//                upperChar = ConditionalSpecialCasing.toUpperCaseEx(this, i, locale);
//            } else {
//                upperChar = Character.toUpperCaseEx(srcChar);
//            }
//            if ((upperChar == Character.ERROR)
//                    || (upperChar >= Character.MIN_SUPPLEMENTARY_CODE_POINT)) {
//                if (upperChar == Character.ERROR) {
//                    if (localeDependent) {
//                        upperCharArray =
//                                ConditionalSpecialCasing.toUpperCaseCharArray(this, i, locale);
//                    } else {
//                        upperCharArray = Character.toUpperCaseCharArray(srcChar);
//                    }
//                } else if (srcCount == 2) {
//                    resultOffset += Character.toChars(upperChar, result, i + resultOffset) - srcCount;
//                    continue;
//                } else {
//                    upperCharArray = Character.toChars(upperChar);
//                }
//
//                /* Grow result if needed * /
//                int mapLen = upperCharArray.length;
//                if (mapLen > srcCount) {
//                    char[] result2 = new char[result.length + mapLen - srcCount];
//                    System.arraycopy(result, 0, result2, 0, i + resultOffset);
//                    result = result2;
//                }
//                for (int x = 0; x < mapLen; ++x) {
//                    result[i + resultOffset + x] = upperCharArray[x];
//                }
//                resultOffset += (mapLen - srcCount);
//            } else {
//                result[i + resultOffset] = (char)upperChar;
//            }
//        }
//        return new String(result, 0, len + resultOffset);
    }
*/

    public String toUpperCase() {
        return ((string)this).ToUpper();// toUpperCase(Locale.getDefault());
    }


    public String trim() {
        int len = value.Length;
        int st = 0;
        char[] val = value;    /* avoid getfield opcode */

        while ((st < len) && (val[st] <= ' ')) {
            st++;
        }
        while ((st < len) && (val[len - 1] <= ' ')) {
            len--;
        }
        return ((st > 0) || (len < value.Length)) ? substring(st, len) : this;
    }


    public String toString()
    {
        return this;
    }


    public char[] toCharArray() {
        // Cannot use Arrays.copyOf because of class initialization order issues
        char[] result = new char[value.Length];
        Array.Copy(value, 0, result, 0, value.Length);
        return result;
    }

    /*
    public static String format(String format, Object... args) {
        return new Formatter().format(format, args).toString();
    }


    public static String format(Locale l, String format, Object... args) {
        return new Formatter(l).format(format, args).toString();
    }
    */

    public static String valueOf(Object obj) {
        return (obj == null) ? new String("null") : new String(obj.ToString());
    }


    public static String valueOf(char[] data) {
        return new String(data);
    }


    public static String valueOf(char[] data, int offset, int count) {
        return new String(data, offset, count);
    }


    public static String copyValueOf(char[] data, int offset, int count) {
        // All public String constructors now copy the data.
        return new String(data, offset, count);
    }

    
    /*
    public static String copyValueOf(char data) {
        return new String(data);
    }


    public static String valueOf(boolean b) {
        return b ? new String("true") : new String("false");
    }


    public static String valueOf(char c) {
        char[] data = {c};
        return new String(data, true);
    }


    public static String valueOf(int i) {
        return Integer.toString(i);
    }


    public static String valueOf(long l) {
        return Long.toString(l);
    }

    public static String valueOf(float f) {
        return new String(Float.toString(f));
    }


    public static String valueOf(double d) {
        return new String(Double.toString(d));
    }
    */

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    // String <-> string
    public static implicit operator string(String other)
    {
        if (other == null)
            return null;
        return new string(other.value);
    }
    public static implicit operator String(string other)
    {
        if (other == null)
            return null;
        return new String(other);
    }

    // CharSequence -> String
    public static implicit operator String(CharSequence other)
    {
        if (other == null)
            return null;
        return new String(other.toString());
    }

    // CharSequence -> String
    public static bool operator ==(String me, String other)
    {
        if ((Object)other == null)
        {
            return (Object)me == null;
        }
        return me.equals(other);
    }

    // CharSequence -> String
    public static bool operator !=(String me, String other)
    {
        return !(me == other);
    }

    // Overrides
    public override string ToString()
    {
        return toString();
    }

    public override bool Equals(object obj)
    {
        if (obj is String) {
            return this.equals((String)obj);
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return ((string)this).GetHashCode();
    }

    public int CompareTo(String other)
    {
        return ((string)this).CompareTo(other);
    }
}

public static class StringExtensions
{
    internal static int length(this String[] t)
    {
        return t.Length;
    }
}
}