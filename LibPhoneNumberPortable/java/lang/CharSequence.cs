using System;

using String = java.lang.String;
using StringBuilder = java.lang.StringBuilder;
using Math = java.lang.Math;
using IOException = java.io.IOException;
using NullPointerException = java.lang.NullPointerException;

namespace java.lang
{
public class CharSequence {

    private string value;

    CharSequence(string value) {
        this.value = value;
    }

    public int length() {
        return this.value.Length;
    }
    public char charAt(int index) {
        return this.value[index];
    }
    public CharSequence subSequence(int start, int end)
    {
        return this.value.Substring(start, end - start);
    }
    public String toString()
    {
        return this.value.ToString();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    // string <-> CharSequence
/*    public static implicit operator string(CharSequence other)
    {
        return other.value;
    }
    */
    public static implicit operator CharSequence(string other)
    {
        return new CharSequence(other);
    }
/*
    public static implicit operator String(CharSequence other)
    {
        return new String(other.value);
    }
*/
    public static implicit operator CharSequence(String other)
    {
        return new CharSequence(other);
    }

    public static implicit operator StringBuffer(CharSequence other)
    {
        return new StringBuffer((String)other.value);
    }

    public static implicit operator CharSequence(StringBuffer other)
    {
        return new CharSequence(other.toString());
    }
/*
    public static implicit operator StringBuilder(CharSequence other)
    {
        return new StringBuilder((String)other.value);
    }
*/
    public static implicit operator CharSequence(StringBuilder other)
    {
        return new CharSequence(other.toString());
    }

    public override string ToString()
    {
        return toString();
    }
}
}