using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace java.lang
{

abstract class CharacterData {
    public abstract int getProperties(int ch);
    public abstract int getType(int ch);
    public abstract boolean isWhitespace(int ch);
    public abstract boolean isMirrored(int ch);
    public abstract boolean isJavaIdentifierStart(int ch);
    public abstract boolean isJavaIdentifierPart(int ch);
    public abstract boolean isUnicodeIdentifierStart(int ch);
    public abstract boolean isUnicodeIdentifierPart(int ch);
    public abstract boolean isIdentifierIgnorable(int ch);
    public abstract int toLowerCase(int ch);
    public abstract int toUpperCase(int ch);
    public abstract int toTitleCase(int ch);
    public abstract int digit(int ch, int radix);
    public abstract int getNumericValue(int ch);
    public abstract byte getDirectionality(int ch);

    //need to implement for JSR204
    internal virtual int toUpperCaseEx(int ch)
    {
        return toUpperCase(ch);
    }

    internal virtual char[] toUpperCaseCharArray(int ch)
    {
        return null;
    }

    internal virtual boolean isOtherLowercase(int ch)
    {
        return false;
    }

    internal virtual boolean isOtherUppercase(int ch)
    {
        return false;
    }

    internal virtual boolean isOtherAlphabetic(int ch)
    {
        return false;
    }

    internal virtual boolean isIdeographic(int ch)
    {
        return false;
    }

    // Character <= 0xff (basic latin) is handled by internal fast-path
    // to avoid initializing large tables.
    // Note: performance of this "fast-path" code may be sub-optimal
    // in negative cases for some accessors due to complicated ranges.
    // Should revisit after optimization of table initialization.

    internal static CharacterData of(int ch) {
        if ((int)(((uint)ch) >> 8) == 0) {     // fast-path
            return CharacterDataLatin1.instance;
        } else {
            switch((int)((uint)ch >> 16)) {  //plane 00-16
            case(0):
                return CharacterData00.instance;
            case (1):
                return CharacterData01.instance;
            case (2):
                throw new NotImplementedException();
                //return CharacterData02.instance;
            case(14):
                throw new NotImplementedException();
                //return CharacterData0E.instance;
            case(15):   // Private Use
            case(16):   // Private Use

                throw new NotImplementedException();
                //return CharacterDataPrivateUse.instance;
            default:
                throw new NotImplementedException();
                //return CharacterDataUndefined.instance;
            }
        }
    }
}
}
