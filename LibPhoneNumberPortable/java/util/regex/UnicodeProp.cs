using java.lang;
using JavaPort.Collections;
using System;
using String = java.lang.String;

namespace java.util.regex {
class UnicodeProp {
    public static readonly UnicodeProp ALPHABETIC = new UnicodeProp(1);
    public static readonly UnicodeProp LETTER = new UnicodeProp(2);
    public static readonly UnicodeProp IDEOGRAPHIC = new UnicodeProp(3);
    public static readonly UnicodeProp LOWERCASE = new UnicodeProp(4);
    public static readonly UnicodeProp UPPERCASE = new UnicodeProp(5);
    public static readonly UnicodeProp TITLECASE = new UnicodeProp(6);
    public static readonly UnicodeProp WHITE_SPACE = new UnicodeProp(7);
    public static readonly UnicodeProp CONTROL = new UnicodeProp(8);
    public static readonly UnicodeProp PUNCTUATION = new UnicodeProp(9);
    public static readonly UnicodeProp HEX_DIGIT = new UnicodeProp(10);
    public static readonly UnicodeProp ASSIGNED = new UnicodeProp(11);
    public static readonly UnicodeProp NONCHARACTER_CODE_POINT = new UnicodeProp(12);
    public static readonly UnicodeProp DIGIT = new UnicodeProp(13);
    public static readonly UnicodeProp ALNUM = new UnicodeProp(14);
    public static readonly UnicodeProp BLANK = new UnicodeProp(15);
    public static readonly UnicodeProp GRAPH = new UnicodeProp(16);
    public static readonly UnicodeProp PRINT = new UnicodeProp(17);
    public static readonly UnicodeProp WORD = new UnicodeProp(18);

    private int value = 0;

    UnicodeProp(int value) {
        this.value = value;
    }

    private readonly static HashMap<String, String> posix = new HashMap<String, String>();
    private readonly static HashMap<String, String> aliases = new HashMap<String, String>();
    static UnicodeProp() {
        posix.put(new String("ALPHA"), new String("ALPHABETIC"));
        posix.put(new String("LOWER"), new String("LOWERCASE"));
        posix.put(new String("UPPER"), new String("UPPERCASE"));
        posix.put(new String("SPACE"), new String("WHITE_SPACE"));
        posix.put(new String("PUNCT"), new String("PUNCTUATION"));
        posix.put(new String("XDIGIT"),new String("HEX_DIGIT"));
        posix.put(new String("ALNUM"), new String("ALNUM"));
        posix.put(new String("CNTRL"), new String("CONTROL"));
        posix.put(new String("DIGIT"), new String("DIGIT"));
        posix.put(new String("BLANK"), new String("BLANK"));
        posix.put(new String("GRAPH"), new String("GRAPH"));
        posix.put(new String("PRINT"), new String("PRINT"));
        aliases.put(new String("WHITESPACE"), new String("WHITE_SPACE"));
        aliases.put(new String("HEXDIGIT"),new String("HEX_DIGIT"));
        aliases.put(new String("NONCHARACTERCODEPOINT"), new String("NONCHARACTER_CODE_POINT"));
    }
    public static UnicodeProp forName(String propName) {
        propName = propName.toUpperCase();
        String alias = aliases.get(propName);
        if (alias != null)
            propName = alias;
        try {
            return valueOf (propName);
        } catch (IllegalArgumentException) {}
        return null;
    }

    private static UnicodeProp valueOf(String propName)
    {
        switch(propName) {
            case "ALPHABETIC":
                return ALPHABETIC;
            case "LETTER":
                return LETTER;
            case "IDEOGRAPHIC":
                return IDEOGRAPHIC;
            case "LOWERCASE":
                return LOWERCASE;
            case "UPPERCASE":
                return UPPERCASE;
            case "TITLECASE":
                return TITLECASE;
            case "WHITE_SPACE":
                return WHITE_SPACE;
            case "CONTROL":
                return CONTROL;
            case "PUNCTUATION":
                return PUNCTUATION;
            case "HEX_DIGIT":
                return HEX_DIGIT;
            case "ASSIGNED":
                return ASSIGNED;
            case "NONCHARACTER_CODE_POINT":
                return NONCHARACTER_CODE_POINT;
            case "DIGIT":
                return DIGIT;
            case "ALNUM":
                return ALNUM;
            case "BLANK":
                return BLANK;
            case "GRAPH":
                return GRAPH;
            case "PRINT":
                return PRINT;
            case "WORD":
                return WORD;
            default:
                return null;
        }
    }
    
    public boolean @is(int ch) {
        switch (value) {
            case 1:
                return ALPHABETIC_is(ch);
            case 2:
                return LETTER_is(ch);
            case 3:
                return IDEOGRAPHIC_is(ch);
            case 4:
                return LOWERCASE_is(ch);
            case 5:
                return UPPERCASE_is(ch);
            case 6:
                return TITLECASE_is(ch);
            case 7:
                return WHITE_SPACE_is(ch);
            case 8:
                return CONTROL_is(ch);
            case 9:
                return PUNCTUATION_is(ch);
            case 10:
                return HEX_DIGIT_is(ch);
            case 11:
                return ASSIGNED_is(ch);
            case 12:
                return NONCHARACTER_CODE_POINT_is(ch);
            case 13:
                return DIGIT_is(ch);
            case 14:
                return ALNUM_is(ch);
            case 15:
                return BLANK_is(ch);
            case 16:
                return GRAPH_is(ch);
            case 17:
                return PRINT_is(ch);
            case 18:
                return WORD_is(ch);
        }
        throw new Exception();
    }

    public static UnicodeProp forPOSIXName(String propName) {
        propName = posix.get(propName.toUpperCase());
        if (propName == null)
            return null;
        return valueOf (propName);
    }


    public static boolean ALPHABETIC_is(int ch) {
            return Character.isAlphabetic(ch);
    }

    public static boolean LETTER_is(int ch) {
            return Character.isLetter(ch);
        }

    public static boolean IDEOGRAPHIC_is(int ch) {
            return Character.isIdeographic(ch);
        }

    public static boolean LOWERCASE_is(int ch) {
            return Character.isLowerCase(ch);
        }

    public static boolean UPPERCASE_is(int ch) {
            return Character.isUpperCase(ch);
        }

    public static boolean TITLECASE_is(int ch) {
            return Character.isTitleCase(ch);
        }

    public static boolean WHITE_SPACE_is(int ch) {
            return ((((1 << Character.SPACE_SEPARATOR) |
                      (1 << Character.LINE_SEPARATOR) |
                      (1 << Character.PARAGRAPH_SEPARATOR)) >> Character.getType(ch)) & 1)
                   != 0 || (ch >= 0x9 && ch <= 0xd) || (ch == 0x85);
        }

    public static boolean CONTROL_is(int ch) {
            return Character.getType(ch) == Character.CONTROL;
        }

    public static boolean PUNCTUATION_is(int ch) {
            return ((((1 << Character.CONNECTOR_PUNCTUATION) |
                      (1 << Character.DASH_PUNCTUATION) |
                      (1 << Character.START_PUNCTUATION) |
                      (1 << Character.END_PUNCTUATION) |
                      (1 << Character.OTHER_PUNCTUATION) |
                      (1 << Character.INITIAL_QUOTE_PUNCTUATION) |
                      (1 << Character.FINAL_QUOTE_PUNCTUATION)) >> Character.getType(ch)) & 1)
                   != 0;
        }

    public static boolean HEX_DIGIT_is(int ch) {
            return DIGIT_is(ch) ||
                   (ch >= 0x0030 && ch <= 0x0039) ||
                   (ch >= 0x0041 && ch <= 0x0046) ||
                   (ch >= 0x0061 && ch <= 0x0066) ||
                   (ch >= 0xFF10 && ch <= 0xFF19) ||
                   (ch >= 0xFF21 && ch <= 0xFF26) ||
                   (ch >= 0xFF41 && ch <= 0xFF46);
        }

    public static boolean ASSIGNED_is(int ch) {
            return Character.getType(ch) != Character.UNASSIGNED;
        }

    public static boolean NONCHARACTER_CODE_POINT_is(int ch) {
            return (ch & 0xfffe) == 0xfffe || (ch >= 0xfdd0 && ch <= 0xfdef);
        }

    public static boolean DIGIT_is(int ch) {
            return Character.isDigit(ch);
        }

    public static boolean ALNUM_is(int ch) {
            return ALPHABETIC_is(ch) || DIGIT_is(ch);
        }

    public static boolean BLANK_is(int ch) {
            return Character.getType(ch) == Character.SPACE_SEPARATOR ||
                   ch == 0x9; // \N{HT}
        }

    public static boolean GRAPH_is(int ch) {
            return ((((1 << Character.SPACE_SEPARATOR) |
                      (1 << Character.LINE_SEPARATOR) |
                      (1 << Character.PARAGRAPH_SEPARATOR) |
                      (1 << Character.CONTROL) |
                      (1 << Character.SURROGATE) |
                      (1 << Character.UNASSIGNED)) >> Character.getType(ch)) & 1)
                   == 0;
        }

    public static boolean PRINT_is(int ch) {
            return (GRAPH_is(ch) || BLANK_is(ch)) && !CONTROL_is(ch);
        }


    public static boolean WORD_is(int ch) {
            return ALPHABETIC_is(ch) ||
                   ((((1 << Character.NON_SPACING_MARK) |
                      (1 << Character.ENCLOSING_MARK) |
                      (1 << Character.COMBINING_SPACING_MARK) |
                      (1 << Character.DECIMAL_DIGIT_NUMBER) |
                      (1 << Character.CONNECTOR_PUNCTUATION)) >> Character.getType(ch)) & 1)
                   != 0;
        }
}
}