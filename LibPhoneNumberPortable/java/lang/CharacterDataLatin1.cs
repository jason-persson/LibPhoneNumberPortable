using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace java.lang
{
class CharacterDataLatin1 : CharacterData {

    public override int getProperties(int ch)
    {
		char offset = (char)ch;
        int props = A[offset];
        return props;
    }

    public override int getType(int ch)
    {
        int props = getProperties(ch);
        return (props & 0x1F);
    }

    public override boolean isJavaIdentifierStart(int ch)
    {
        int props = getProperties(ch);
        return ((props & 0x00007000) >= 0x00005000);
    }

    public override boolean isJavaIdentifierPart(int ch)
    {
        int props = getProperties(ch);
        return ((props & 0x00003000) != 0);
    }

    public override boolean isUnicodeIdentifierStart(int ch)
    {
        int props = getProperties(ch);
        return ((props & 0x00007000) == 0x00007000);
    }

    public override boolean isUnicodeIdentifierPart(int ch)
    {
        int props = getProperties(ch);
        return ((props & 0x00001000) != 0);
    }

    public override boolean isIdentifierIgnorable(int ch)
    {
        int props = getProperties(ch);
        return ((props & 0x00007000) == 0x00001000);
    }

    public override int toLowerCase(int ch)
    {
        int mapChar = ch;
        int val = getProperties(ch);

        if (((val & 0x00020000) != 0) && 
                ((val & 0x07FC0000) != 0x07FC0000)) { 
            int offset = val << 5 >> (5+18);
            mapChar = ch + offset;
        }
        return mapChar;
    }

    public override int toUpperCase(int ch)
    {
        int mapChar = ch;
        int val = getProperties(ch);

        if ((val & 0x00010000) != 0) {
            if ((val & 0x07FC0000) != 0x07FC0000) {
                int offset = val  << 5 >> (5+18);
                mapChar =  ch - offset;
            } else if (ch == 0x00B5) {
                mapChar = 0x039C;
            }
        }
        return mapChar;
    }

    public override int toTitleCase(int ch)
    {
        return toUpperCase(ch);
    }

    public override int digit(int ch, int radix)
    {
        int value = -1;
        if (radix >= Character.MIN_RADIX && radix <= Character.MAX_RADIX) {
            int val = getProperties(ch);
            int kind = val & 0x1F;
            if (kind == Character.DECIMAL_DIGIT_NUMBER) {
                value = ch + ((val & 0x3E0) >> 5) & 0x1F;
            }
            else if ((val & 0xC00) == 0x00000C00) {
                // Java supradecimal digit
                value = (ch + ((val & 0x3E0) >> 5) & 0x1F) + 10;
            }
        }
        return (value < radix) ? value : -1;
    }

    public override int getNumericValue(int ch)
    {
        int val = getProperties(ch);
        int retval = -1;

        switch (val & 0xC00) {
            default: // cannot occur
            case (0x00000000):         // not numeric
                retval = -1;
                break;
            case (0x00000400):              // simple numeric
                retval = ch + ((val & 0x3E0) >> 5) & 0x1F;
                break;
            case (0x00000800)      :       // "strange" numeric
                 retval = -2; 
                 break;
            case (0x00000C00):           // Java supradecimal
                retval = (ch + ((val & 0x3E0) >> 5) & 0x1F) + 10;
                break;
        }
        return retval;
    }

    public override boolean isWhitespace(int ch)
    {
        int props = getProperties(ch);
        return ((props & 0x00007000) == 0x00004000);
    }

    public override byte getDirectionality(int ch)
    {
        int val = getProperties(ch);
        byte directionality = (byte)((val & 0x78000000) >> 27);

        if (directionality == 0xF ) {
            directionality = unchecked((byte)-1);
        }
        return directionality;
    }

    public override boolean isMirrored(int ch)
    {
        int props = getProperties(ch);
        return ((props & 0x80000000) != 0);
    }

    internal override int toUpperCaseEx(int ch)
    {
        int mapChar = ch;
        int val = getProperties(ch);

        if ((val & 0x00010000) != 0) {
            if ((val & 0x07FC0000) != 0x07FC0000) {
                int offset = val  << 5 >> (5+18);
                mapChar =  ch - offset;
            }
            else {
                switch(ch) {
                    // map overflow characters
                    case 0x00B5 : mapChar = 0x039C; break;
                    default       : mapChar = Character.ERROR; break;
                }
            }
        }
        return mapChar;
    }

    static char[] sharpsMap = new char[] {'S', 'S'};

    internal override char[] toUpperCaseCharArray(int ch)
    {
        char[] upperMap = {(char)ch};
        if (ch == 0x00DF) {
            upperMap = sharpsMap;
        }
        return upperMap;
    }

    internal static readonly CharacterDataLatin1 instance = new CharacterDataLatin1();
    private CharacterDataLatin1() {}

    // The following tables and code generated using:
  // java GenerateCharacter -template ../../tools/GenerateCharacter/CharacterDataLatin1.java.template -spec ../../tools/UnicodeData/UnicodeData.txt -specialcasing ../../tools/UnicodeData/SpecialCasing.txt -o /build/buildd/openjdk-6-6b14-1.4.1/build/openjdk/control/build/linux-i586/gensrc/java/lang/CharacterDataLatin1.java -string -usecharforbyte -latin1 8
  // The A table has 256 entries for a total of 1024 bytes.

  static readonly int[] A = new int[256];
  static readonly String A_DATA = "\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u5800\u400F\u5000\u400F\u5800\u400F\u6000\u400F\u5000\u400F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u5000\u400F\u5000\u400F\u5000\u400F\u5800\u400F\u6000\u400C\u6800\u0018\u6800\u0018\u2800\u0018\u2800\u601A\u2800\u0018\u6800\u0018\u6800\u0018\uE800\u0015\uE800\u0016\u6800\u0018\u2800\u0019\u3800\u0018\u2800\u0014\u3800\u0018\u2000\u0018\u1800\u3609\u1800\u3609\u1800\u3609\u1800\u3609\u1800\u3609\u1800\u3609\u1800\u3609\u1800\u3609\u1800\u3609\u1800\u3609\u3800\u0018\u6800\u0018\uE800\u0019\u6800\u0019\uE800\u0019\u6800\u0018\u6800\u0018\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\u0082\u7FE1\uE800\u0015\u6800\u0018\uE800\u0016\u6800\u001B\u6800\u5017\u6800\u001B\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\u0081\u7FE2\uE800\u0015\u6800\u0019\uE800\u0016\u6800\u0019\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u5000\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u4800\u100F\u3800\u000C\u6800\u0018\u2800\u601A\u2800\u601A\u2800\u601A\u2800\u601A\u6800\u001C\u6800\u001C\u6800\u001B\u6800\u001C\u0000\u7002\uE800\u001D\u6800\u0019\u6800\u1010\u6800\u001C\u6800\u001B\u2800\u001C\u2800\u0019\u1800\u060B\u1800\u060B\u6800\u001B\u07FD\u7002\u6800\u001C\u6800\u0018\u6800\u001B\u1800\u050B\u0000\u7002\uE800\u001E\u6800\u080B\u6800\u080B\u6800\u080B\u6800\u0018\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u6800\u0019\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u0082\u7001\u07FD\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u6800\u0019\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u0081\u7002\u061D\u7002";

  // In all, the character property tables require 1024 bytes.

    static CharacterDataLatin1() {
                { // THIS CODE WAS AUTOMATICALLY CREATED BY GenerateCharacter:
            char[] data = A_DATA.toCharArray();
            //assert (data.length == (256 * 2));
            int i = 0, j = 0;
            while (i < (256 * 2)) {
                int entry = data[i++] << 16;
                A[j++] = entry | data[i++];
            }
        }

    }
}
}
