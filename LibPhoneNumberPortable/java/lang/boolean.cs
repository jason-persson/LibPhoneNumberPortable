using System;
namespace java.lang
{
public struct boolean// : Comparable<boolean>
{
    public static readonly boolean TRUE = new boolean(true);
    public static readonly boolean FALSE = new boolean(false);

    private readonly bool value;

    public boolean(bool value) {
        this.value = value;
    }

    public boolean(String s) {
        this.value = toBoolean(s).value;
    }

    public static boolean parseBoolean(String s) {
        return toBoolean(s);
    }

    public bool booleanValue() {
        return value;
    }

    public static boolean valueOf(boolean b) {
        return (b ? TRUE : FALSE);
    }

    public static boolean valueOf(String s) {
        return toBoolean(s) ? TRUE : FALSE;
    }

    public static String toString(boolean b) {
        return b ? new String("true") : new String("false");
    }

    public String toString() {
        return value ? new String("true") : new String("false");
    }

    public int hashCode() {
        return value ? 1231 : 1237;
    }

    public boolean equals(Object obj) {
        if (obj is boolean) {
            return value == ((boolean)obj).value;
        }
        return false;
    }

    public int compareTo(boolean b) {
        return compare(this.value, b.value);
    }

    public static int compare(boolean x, boolean y) {
        return (x == y) ? 0 : (x ? 1 : -1);
    }

    private static boolean toBoolean(String name) {
        return ((name != null) && name.equalsIgnoreCase(new String("true")));
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    public static implicit operator bool(boolean other)
    {
        return other.value;
    }

    public static implicit operator boolean(bool other)
    {
        return new boolean(other);
    }

    public override string ToString()
    {
        return toString();
    }
}
}