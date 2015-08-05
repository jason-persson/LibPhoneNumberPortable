using java.util;
using JavaPort.Collections;
using System;
namespace java.lang
{
public sealed
class Character : IComparable<Character> {
    public static readonly int MIN_RADIX = 2;
    public static readonly int MAX_RADIX = 36;
    public static readonly char MIN_VALUE = '\u0000';
    public static readonly char MAX_VALUE = '\uFFFF';
    public static readonly byte UNASSIGNED = 0;
    public static readonly byte UPPERCASE_LETTER = 1;
    public static readonly byte LOWERCASE_LETTER = 2;
    public static readonly byte TITLECASE_LETTER = 3;
    public static readonly byte MODIFIER_LETTER = 4;
    public static readonly byte OTHER_LETTER = 5;
    public static readonly byte NON_SPACING_MARK = 6;
    public static readonly byte ENCLOSING_MARK = 7;
    public static readonly byte COMBINING_SPACING_MARK = 8;
    public static readonly byte DECIMAL_DIGIT_NUMBER        = 9;
    public static readonly byte LETTER_NUMBER = 10;
    public static readonly byte OTHER_NUMBER = 11;
    public static readonly byte SPACE_SEPARATOR = 12;
    public static readonly byte LINE_SEPARATOR = 13;
    public static readonly byte PARAGRAPH_SEPARATOR = 14;
    public static readonly byte CONTROL = 15;
    public static readonly byte FORMAT = 16;
    public static readonly byte PRIVATE_USE = 18;
    public static readonly byte SURROGATE = 19;
    public static readonly byte DASH_PUNCTUATION = 20;
    public static readonly byte START_PUNCTUATION = 21;
    public static readonly byte END_PUNCTUATION = 22;
    public static readonly byte CONNECTOR_PUNCTUATION = 23;
    public static readonly byte OTHER_PUNCTUATION = 24;
    public static readonly byte MATH_SYMBOL = 25;
    public static readonly byte CURRENCY_SYMBOL = 26;
    public static readonly byte MODIFIER_SYMBOL = 27;
    public static readonly byte OTHER_SYMBOL = 28;
    public static readonly byte INITIAL_QUOTE_PUNCTUATION = 29;
    public static readonly byte FINAL_QUOTE_PUNCTUATION = 30;
    internal static readonly int ERROR = -1;//0xFFFFFFFF;
    public static readonly byte DIRECTIONALITY_UNDEFINED = unchecked((byte)-1);
    public static readonly byte DIRECTIONALITY_LEFT_TO_RIGHT = 0;
    public static readonly byte DIRECTIONALITY_RIGHT_TO_LEFT = 1;
    public static readonly byte DIRECTIONALITY_RIGHT_TO_LEFT_ARABIC = 2;
    public static readonly byte DIRECTIONALITY_EUROPEAN_NUMBER = 3;
    public static readonly byte DIRECTIONALITY_EUROPEAN_NUMBER_SEPARATOR = 4;
    public static readonly byte DIRECTIONALITY_EUROPEAN_NUMBER_TERMINATOR = 5;
    public static readonly byte DIRECTIONALITY_ARABIC_NUMBER = 6;
    public static readonly byte DIRECTIONALITY_COMMON_NUMBER_SEPARATOR = 7;
    public static readonly byte DIRECTIONALITY_NONSPACING_MARK = 8;
    public static readonly byte DIRECTIONALITY_BOUNDARY_NEUTRAL = 9;
    public static readonly byte DIRECTIONALITY_PARAGRAPH_SEPARATOR = 10;
    public static readonly byte DIRECTIONALITY_SEGMENT_SEPARATOR = 11;
    public static readonly byte DIRECTIONALITY_WHITESPACE = 12;
    public static readonly byte DIRECTIONALITY_OTHER_NEUTRALS = 13;
    public static readonly byte DIRECTIONALITY_LEFT_TO_RIGHT_EMBEDDING = 14;
    public static readonly byte DIRECTIONALITY_LEFT_TO_RIGHT_OVERRIDE = 15;
    public static readonly byte DIRECTIONALITY_RIGHT_TO_LEFT_EMBEDDING = 16;
    public static readonly byte DIRECTIONALITY_RIGHT_TO_LEFT_OVERRIDE = 17;
    public static readonly byte DIRECTIONALITY_POP_DIRECTIONAL_FORMAT = 18;
    public static readonly char MIN_HIGH_SURROGATE = '\uD800';
    public static readonly char MAX_HIGH_SURROGATE = '\uDBFF';
    public static readonly char MIN_LOW_SURROGATE  = '\uDC00';
    public static readonly char MAX_LOW_SURROGATE  = '\uDFFF';
    public static readonly char MIN_SURROGATE = MIN_HIGH_SURROGATE;
    public static readonly char MAX_SURROGATE = MAX_LOW_SURROGATE;
    public static readonly int MIN_SUPPLEMENTARY_CODE_POINT = 0x010000;
    public static readonly int MIN_CODE_POINT = 0x000000;
    public static readonly int MAX_CODE_POINT = 0X10FFFF;

    public class Subset  {
        private String name;
        protected Subset(String name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            this.name = name;
        }
        public boolean equals(Object obj) {
            return (this == obj);
        }
        public int hashCode() {
            return name.hashCode();
        }
        public String toString() {
            return name;
        }
    }

    // See http://www.unicode.org/Public/UNIDATA/Blocks.txt
    // for the latest specification of Unicode Blocks.
    public sealed class UnicodeBlock : Subset {
        private static Map<String, UnicodeBlock> map = new HashMap<String, UnicodeBlock>(256);

        private UnicodeBlock(String idName) : base(idName) {
            map.put(idName, this);
        }

        private UnicodeBlock(String idName, String alias) : base(idName) {
            map.put(alias, this);
        }

        private UnicodeBlock(String idName, params String[] aliases) : base(idName) {
            foreach (String alias in aliases)
                map.put(alias, this);
        }

        public static readonly UnicodeBlock  BASIC_LATIN =
            new UnicodeBlock("BASIC_LATIN", "BASIC LATIN", "BASICLATIN");
        public static readonly UnicodeBlock LATIN_1_SUPPLEMENT =
            new UnicodeBlock("LATIN_1_SUPPLEMENT", "LATIN-1 SUPPLEMENT", "LATIN-1SUPPLEMENT");
        public static readonly UnicodeBlock LATIN_EXTENDED_A =
            new UnicodeBlock("LATIN_EXTENDED_A", "LATIN EXTENDED-A", "LATINEXTENDED-A");
        public static readonly UnicodeBlock LATIN_EXTENDED_B =
            new UnicodeBlock("LATIN_EXTENDED_B", "LATIN EXTENDED-B", "LATINEXTENDED-B");
        public static readonly UnicodeBlock IPA_EXTENSIONS =
            new UnicodeBlock("IPA_EXTENSIONS", "IPA EXTENSIONS", "IPAEXTENSIONS");
        public static readonly UnicodeBlock SPACING_MODIFIER_LETTERS =
            new UnicodeBlock("SPACING_MODIFIER_LETTERS", "SPACING MODIFIER LETTERS", "SPACINGMODIFIERLETTERS");
        public static readonly UnicodeBlock COMBINING_DIACRITICAL_MARKS =
            new UnicodeBlock("COMBINING_DIACRITICAL_MARKS", "COMBINING DIACRITICAL MARKS", "COMBININGDIACRITICALMARKS");
        public static readonly UnicodeBlock GREEK =
            new UnicodeBlock("GREEK", "GREEK AND COPTIC", "GREEKANDCOPTIC");
        public static readonly UnicodeBlock CYRILLIC =
            new UnicodeBlock("CYRILLIC");
        public static readonly UnicodeBlock ARMENIAN =
            new UnicodeBlock("ARMENIAN");
        public static readonly UnicodeBlock HEBREW =
            new UnicodeBlock("HEBREW");
        public static readonly UnicodeBlock ARABIC =
            new UnicodeBlock("ARABIC");
        public static readonly UnicodeBlock DEVANAGARI =
            new UnicodeBlock("DEVANAGARI");
        public static readonly UnicodeBlock BENGALI =
            new UnicodeBlock("BENGALI");
        public static readonly UnicodeBlock GURMUKHI =
            new UnicodeBlock("GURMUKHI");
        public static readonly UnicodeBlock GUJARATI =
            new UnicodeBlock("GUJARATI");
        public static readonly UnicodeBlock ORIYA =
            new UnicodeBlock("ORIYA");
        public static readonly UnicodeBlock TAMIL =
            new UnicodeBlock("TAMIL");
        public static readonly UnicodeBlock TELUGU =
            new UnicodeBlock("TELUGU");
        public static readonly UnicodeBlock KANNADA =
            new UnicodeBlock("KANNADA");
        public static readonly UnicodeBlock MALAYALAM =
            new UnicodeBlock("MALAYALAM");
        public static readonly UnicodeBlock THAI =
            new UnicodeBlock("THAI");
        public static readonly UnicodeBlock LAO =
            new UnicodeBlock("LAO");
        public static readonly UnicodeBlock TIBETAN =
            new UnicodeBlock("TIBETAN");
        public static readonly UnicodeBlock GEORGIAN =
            new UnicodeBlock("GEORGIAN");
        public static readonly UnicodeBlock HANGUL_JAMO =
            new UnicodeBlock("HANGUL_JAMO", "HANGUL JAMO", "HANGULJAMO");
        public static readonly UnicodeBlock LATIN_EXTENDED_ADDITIONAL =
            new UnicodeBlock("LATIN_EXTENDED_ADDITIONAL", "LATIN EXTENDED ADDITIONAL", "LATINEXTENDEDADDITIONAL");
        public static readonly UnicodeBlock GREEK_EXTENDED =
            new UnicodeBlock("GREEK_EXTENDED", "GREEK EXTENDED", "GREEKEXTENDED");
        public static readonly UnicodeBlock GENERAL_PUNCTUATION =
            new UnicodeBlock("GENERAL_PUNCTUATION", "GENERAL PUNCTUATION", "GENERALPUNCTUATION");
        public static readonly UnicodeBlock SUPERSCRIPTS_AND_SUBSCRIPTS =
            new UnicodeBlock("SUPERSCRIPTS_AND_SUBSCRIPTS", "SUPERSCRIPTS AND SUBSCRIPTS", "SUPERSCRIPTSANDSUBSCRIPTS");
        public static readonly UnicodeBlock CURRENCY_SYMBOLS =
            new UnicodeBlock("CURRENCY_SYMBOLS", "CURRENCY SYMBOLS", "CURRENCYSYMBOLS");
        public static readonly UnicodeBlock COMBINING_MARKS_FOR_SYMBOLS =
            new UnicodeBlock("COMBINING_MARKS_FOR_SYMBOLS", "COMBINING DIACRITICAL MARKS FOR SYMBOLS", "COMBININGDIACRITICALMARKSFORSYMBOLS", "COMBINING MARKS FOR SYMBOLS", "COMBININGMARKSFORSYMBOLS");
        public static readonly UnicodeBlock LETTERLIKE_SYMBOLS =
            new UnicodeBlock("LETTERLIKE_SYMBOLS", "LETTERLIKE SYMBOLS", "LETTERLIKESYMBOLS");
        public static readonly UnicodeBlock NUMBER_FORMS =
            new UnicodeBlock("NUMBER_FORMS", "NUMBER FORMS", "NUMBERFORMS");
        public static readonly UnicodeBlock ARROWS =
            new UnicodeBlock("ARROWS");
        public static readonly UnicodeBlock MATHEMATICAL_OPERATORS =
            new UnicodeBlock("MATHEMATICAL_OPERATORS", "MATHEMATICAL OPERATORS", "MATHEMATICALOPERATORS");
        public static readonly UnicodeBlock MISCELLANEOUS_TECHNICAL =
            new UnicodeBlock("MISCELLANEOUS_TECHNICAL", "MISCELLANEOUS TECHNICAL", "MISCELLANEOUSTECHNICAL");
        public static readonly UnicodeBlock CONTROL_PICTURES =
            new UnicodeBlock("CONTROL_PICTURES", "CONTROL PICTURES", "CONTROLPICTURES");
        public static readonly UnicodeBlock OPTICAL_CHARACTER_RECOGNITION =
            new UnicodeBlock("OPTICAL_CHARACTER_RECOGNITION", "OPTICAL CHARACTER RECOGNITION", "OPTICALCHARACTERRECOGNITION");
        public static readonly UnicodeBlock ENCLOSED_ALPHANUMERICS =
            new UnicodeBlock("ENCLOSED_ALPHANUMERICS", "ENCLOSED ALPHANUMERICS", "ENCLOSEDALPHANUMERICS");
        public static readonly UnicodeBlock BOX_DRAWING =
            new UnicodeBlock("BOX_DRAWING", "BOX DRAWING", "BOXDRAWING");
        public static readonly UnicodeBlock BLOCK_ELEMENTS =
            new UnicodeBlock("BLOCK_ELEMENTS", "BLOCK ELEMENTS", "BLOCKELEMENTS");
        public static readonly UnicodeBlock GEOMETRIC_SHAPES =
            new UnicodeBlock("GEOMETRIC_SHAPES", "GEOMETRIC SHAPES", "GEOMETRICSHAPES");
        public static readonly UnicodeBlock MISCELLANEOUS_SYMBOLS =
            new UnicodeBlock("MISCELLANEOUS_SYMBOLS", "MISCELLANEOUS SYMBOLS", "MISCELLANEOUSSYMBOLS");
        public static readonly UnicodeBlock DINGBATS =
            new UnicodeBlock("DINGBATS");
        public static readonly UnicodeBlock CJK_SYMBOLS_AND_PUNCTUATION =
            new UnicodeBlock("CJK_SYMBOLS_AND_PUNCTUATION", "CJK SYMBOLS AND PUNCTUATION", "CJKSYMBOLSANDPUNCTUATION");
        public static readonly UnicodeBlock HIRAGANA =
            new UnicodeBlock("HIRAGANA");
        public static readonly UnicodeBlock KATAKANA =
            new UnicodeBlock("KATAKANA");
        public static readonly UnicodeBlock BOPOMOFO =
            new UnicodeBlock("BOPOMOFO");
        public static readonly UnicodeBlock HANGUL_COMPATIBILITY_JAMO =
            new UnicodeBlock("HANGUL_COMPATIBILITY_JAMO", "HANGUL COMPATIBILITY JAMO", "HANGULCOMPATIBILITYJAMO");
        public static readonly UnicodeBlock KANBUN =
            new UnicodeBlock("KANBUN");
        public static readonly UnicodeBlock ENCLOSED_CJK_LETTERS_AND_MONTHS =
            new UnicodeBlock("ENCLOSED_CJK_LETTERS_AND_MONTHS", "ENCLOSED CJK LETTERS AND MONTHS", "ENCLOSEDCJKLETTERSANDMONTHS");
        public static readonly UnicodeBlock CJK_COMPATIBILITY =
            new UnicodeBlock("CJK_COMPATIBILITY", "CJK COMPATIBILITY", "CJKCOMPATIBILITY");
        public static readonly UnicodeBlock CJK_UNIFIED_IDEOGRAPHS =
            new UnicodeBlock("CJK_UNIFIED_IDEOGRAPHS", "CJK UNIFIED IDEOGRAPHS", "CJKUNIFIEDIDEOGRAPHS");
        public static readonly UnicodeBlock HANGUL_SYLLABLES =
            new UnicodeBlock("HANGUL_SYLLABLES", "HANGUL SYLLABLES", "HANGULSYLLABLES");
        public static readonly UnicodeBlock PRIVATE_USE_AREA =
            new UnicodeBlock("PRIVATE_USE_AREA", "PRIVATE USE AREA", "PRIVATEUSEAREA");
        public static readonly UnicodeBlock CJK_COMPATIBILITY_IDEOGRAPHS =
            new UnicodeBlock("CJK_COMPATIBILITY_IDEOGRAPHS", "CJK COMPATIBILITY IDEOGRAPHS", "CJKCOMPATIBILITYIDEOGRAPHS");
        public static readonly UnicodeBlock ALPHABETIC_PRESENTATION_FORMS =
            new UnicodeBlock("ALPHABETIC_PRESENTATION_FORMS", "ALPHABETIC PRESENTATION FORMS", "ALPHABETICPRESENTATIONFORMS");
        public static readonly UnicodeBlock ARABIC_PRESENTATION_FORMS_A =
            new UnicodeBlock("ARABIC_PRESENTATION_FORMS_A", "ARABIC PRESENTATION FORMS-A", "ARABICPRESENTATIONFORMS-A");
        public static readonly UnicodeBlock COMBINING_HALF_MARKS =
            new UnicodeBlock("COMBINING_HALF_MARKS", "COMBINING HALF MARKS", "COMBININGHALFMARKS");
        public static readonly UnicodeBlock CJK_COMPATIBILITY_FORMS =
            new UnicodeBlock("CJK_COMPATIBILITY_FORMS", "CJK COMPATIBILITY FORMS", "CJKCOMPATIBILITYFORMS");
        public static readonly UnicodeBlock SMALL_FORM_VARIANTS =
            new UnicodeBlock("SMALL_FORM_VARIANTS", "SMALL FORM VARIANTS", "SMALLFORMVARIANTS");
        public static readonly UnicodeBlock ARABIC_PRESENTATION_FORMS_B =
            new UnicodeBlock("ARABIC_PRESENTATION_FORMS_B", "ARABIC PRESENTATION FORMS-B", "ARABICPRESENTATIONFORMS-B");
        public static readonly UnicodeBlock HALFWIDTH_AND_FULLWIDTH_FORMS =
            new UnicodeBlock("HALFWIDTH_AND_FULLWIDTH_FORMS", "HALFWIDTH AND FULLWIDTH FORMS", "HALFWIDTHANDFULLWIDTHFORMS");
        public static readonly UnicodeBlock SPECIALS =
            new UnicodeBlock("SPECIALS");
        public static readonly UnicodeBlock SYRIAC =
            new UnicodeBlock("SYRIAC");
        public static readonly UnicodeBlock THAANA =
            new UnicodeBlock("THAANA");
        public static readonly UnicodeBlock SINHALA =
            new UnicodeBlock("SINHALA");
        public static readonly UnicodeBlock MYANMAR =
            new UnicodeBlock("MYANMAR");
        public static readonly UnicodeBlock ETHIOPIC =
            new UnicodeBlock("ETHIOPIC");
        public static readonly UnicodeBlock CHEROKEE =
            new UnicodeBlock("CHEROKEE");
        public static readonly UnicodeBlock UNIFIED_CANADIAN_ABORIGINAL_SYLLABICS =
            new UnicodeBlock("UNIFIED_CANADIAN_ABORIGINAL_SYLLABICS", "UNIFIED CANADIAN ABORIGINAL SYLLABICS", "UNIFIEDCANADIANABORIGINALSYLLABICS");
        public static readonly UnicodeBlock OGHAM =
            new UnicodeBlock("OGHAM");
        public static readonly UnicodeBlock RUNIC =
            new UnicodeBlock("RUNIC");
        public static readonly UnicodeBlock KHMER =
            new UnicodeBlock("KHMER");
        public static readonly UnicodeBlock MONGOLIAN =
            new UnicodeBlock("MONGOLIAN");
        public static readonly UnicodeBlock BRAILLE_PATTERNS =
            new UnicodeBlock("BRAILLE_PATTERNS", "BRAILLE PATTERNS", "BRAILLEPATTERNS");
        public static readonly UnicodeBlock CJK_RADICALS_SUPPLEMENT =
            new UnicodeBlock("CJK_RADICALS_SUPPLEMENT", "CJK RADICALS SUPPLEMENT", "CJKRADICALSSUPPLEMENT");
        public static readonly UnicodeBlock KANGXI_RADICALS =
            new UnicodeBlock("KANGXI_RADICALS", "KANGXI RADICALS", "KANGXIRADICALS");
        public static readonly UnicodeBlock IDEOGRAPHIC_DESCRIPTION_CHARACTERS =
            new UnicodeBlock("IDEOGRAPHIC_DESCRIPTION_CHARACTERS", "IDEOGRAPHIC DESCRIPTION CHARACTERS", "IDEOGRAPHICDESCRIPTIONCHARACTERS");
        public static readonly UnicodeBlock BOPOMOFO_EXTENDED =
            new UnicodeBlock("BOPOMOFO_EXTENDED", "BOPOMOFO EXTENDED", "BOPOMOFOEXTENDED");
        public static readonly UnicodeBlock CJK_UNIFIED_IDEOGRAPHS_EXTENSION_A =
            new UnicodeBlock("CJK_UNIFIED_IDEOGRAPHS_EXTENSION_A", "CJK UNIFIED IDEOGRAPHS EXTENSION A", "CJKUNIFIEDIDEOGRAPHSEXTENSIONA");
        public static readonly UnicodeBlock YI_SYLLABLES =
            new UnicodeBlock("YI_SYLLABLES", "YI SYLLABLES", "YISYLLABLES");
        public static readonly UnicodeBlock YI_RADICALS =
            new UnicodeBlock("YI_RADICALS", "YI RADICALS", "YIRADICALS");
        public static readonly UnicodeBlock CYRILLIC_SUPPLEMENTARY =
            new UnicodeBlock("CYRILLIC_SUPPLEMENTARY", "CYRILLIC SUPPLEMENTARY", "CYRILLICSUPPLEMENTARY", "CYRILLIC SUPPLEMENT", "CYRILLICSUPPLEMENT");
        public static readonly UnicodeBlock TAGALOG =
            new UnicodeBlock("TAGALOG");
        public static readonly UnicodeBlock HANUNOO =
            new UnicodeBlock("HANUNOO");
        public static readonly UnicodeBlock BUHID =
            new UnicodeBlock("BUHID");
        public static readonly UnicodeBlock TAGBANWA =
            new UnicodeBlock("TAGBANWA");
        public static readonly UnicodeBlock LIMBU =
            new UnicodeBlock("LIMBU");
        public static readonly UnicodeBlock TAI_LE =
            new UnicodeBlock("TAI_LE", "TAI LE", "TAILE");
        public static readonly UnicodeBlock KHMER_SYMBOLS =
            new UnicodeBlock("KHMER_SYMBOLS", "KHMER SYMBOLS", "KHMERSYMBOLS");
        public static readonly UnicodeBlock PHONETIC_EXTENSIONS =
            new UnicodeBlock("PHONETIC_EXTENSIONS", "PHONETIC EXTENSIONS", "PHONETICEXTENSIONS");
        public static readonly UnicodeBlock MISCELLANEOUS_MATHEMATICAL_SYMBOLS_A =
            new UnicodeBlock("MISCELLANEOUS_MATHEMATICAL_SYMBOLS_A", "MISCELLANEOUS MATHEMATICAL SYMBOLS-A", "MISCELLANEOUSMATHEMATICALSYMBOLS-A");
        public static readonly UnicodeBlock SUPPLEMENTAL_ARROWS_A =
            new UnicodeBlock("SUPPLEMENTAL_ARROWS_A", "SUPPLEMENTAL ARROWS-A", "SUPPLEMENTALARROWS-A");
        public static readonly UnicodeBlock SUPPLEMENTAL_ARROWS_B =
            new UnicodeBlock("SUPPLEMENTAL_ARROWS_B", "SUPPLEMENTAL ARROWS-B", "SUPPLEMENTALARROWS-B");
        public static readonly UnicodeBlock MISCELLANEOUS_MATHEMATICAL_SYMBOLS_B =
            new UnicodeBlock("MISCELLANEOUS_MATHEMATICAL_SYMBOLS_B", "MISCELLANEOUS MATHEMATICAL SYMBOLS-B", "MISCELLANEOUSMATHEMATICALSYMBOLS-B");
        public static readonly UnicodeBlock SUPPLEMENTAL_MATHEMATICAL_OPERATORS =
            new UnicodeBlock("SUPPLEMENTAL_MATHEMATICAL_OPERATORS", "SUPPLEMENTAL MATHEMATICAL OPERATORS", "SUPPLEMENTALMATHEMATICALOPERATORS");
        public static readonly UnicodeBlock MISCELLANEOUS_SYMBOLS_AND_ARROWS =
            new UnicodeBlock("MISCELLANEOUS_SYMBOLS_AND_ARROWS", "MISCELLANEOUS SYMBOLS AND ARROWS", "MISCELLANEOUSSYMBOLSANDARROWS");
        public static readonly UnicodeBlock KATAKANA_PHONETIC_EXTENSIONS =
            new UnicodeBlock("KATAKANA_PHONETIC_EXTENSIONS", "KATAKANA PHONETIC EXTENSIONS", "KATAKANAPHONETICEXTENSIONS");
        public static readonly UnicodeBlock YIJING_HEXAGRAM_SYMBOLS =
            new UnicodeBlock("YIJING_HEXAGRAM_SYMBOLS", "YIJING HEXAGRAM SYMBOLS", "YIJINGHEXAGRAMSYMBOLS");
        public static readonly UnicodeBlock VARIATION_SELECTORS =
            new UnicodeBlock("VARIATION_SELECTORS", "VARIATION SELECTORS", "VARIATIONSELECTORS");
        public static readonly UnicodeBlock LINEAR_B_SYLLABARY =
            new UnicodeBlock("LINEAR_B_SYLLABARY", "LINEAR B SYLLABARY", "LINEARBSYLLABARY");
        public static readonly UnicodeBlock LINEAR_B_IDEOGRAMS =
            new UnicodeBlock("LINEAR_B_IDEOGRAMS", "LINEAR B IDEOGRAMS", "LINEARBIDEOGRAMS");
        public static readonly UnicodeBlock AEGEAN_NUMBERS =
            new UnicodeBlock("AEGEAN_NUMBERS", "AEGEAN NUMBERS", "AEGEANNUMBERS");
        public static readonly UnicodeBlock OLD_ITALIC =
            new UnicodeBlock("OLD_ITALIC", "OLD ITALIC", "OLDITALIC");
        public static readonly UnicodeBlock GOTHIC =
            new UnicodeBlock("GOTHIC");
        public static readonly UnicodeBlock UGARITIC =
            new UnicodeBlock("UGARITIC");
        public static readonly UnicodeBlock DESERET =
            new UnicodeBlock("DESERET");
        public static readonly UnicodeBlock SHAVIAN =
            new UnicodeBlock("SHAVIAN");
        public static readonly UnicodeBlock OSMANYA =
            new UnicodeBlock("OSMANYA");
        public static readonly UnicodeBlock CYPRIOT_SYLLABARY =
            new UnicodeBlock("CYPRIOT_SYLLABARY", "CYPRIOT SYLLABARY", "CYPRIOTSYLLABARY");
        public static readonly UnicodeBlock BYZANTINE_MUSICAL_SYMBOLS =
            new UnicodeBlock("BYZANTINE_MUSICAL_SYMBOLS", "BYZANTINE MUSICAL SYMBOLS", "BYZANTINEMUSICALSYMBOLS");
        public static readonly UnicodeBlock MUSICAL_SYMBOLS =
            new UnicodeBlock("MUSICAL_SYMBOLS", "MUSICAL SYMBOLS", "MUSICALSYMBOLS");
        public static readonly UnicodeBlock TAI_XUAN_JING_SYMBOLS =
            new UnicodeBlock("TAI_XUAN_JING_SYMBOLS", "TAI XUAN JING SYMBOLS", "TAIXUANJINGSYMBOLS");
        public static readonly UnicodeBlock MATHEMATICAL_ALPHANUMERIC_SYMBOLS =
            new UnicodeBlock("MATHEMATICAL_ALPHANUMERIC_SYMBOLS", "MATHEMATICAL ALPHANUMERIC SYMBOLS", "MATHEMATICALALPHANUMERICSYMBOLS");
        public static readonly UnicodeBlock CJK_UNIFIED_IDEOGRAPHS_EXTENSION_B =
            new UnicodeBlock("CJK_UNIFIED_IDEOGRAPHS_EXTENSION_B", "CJK UNIFIED IDEOGRAPHS EXTENSION B", "CJKUNIFIEDIDEOGRAPHSEXTENSIONB");
        public static readonly UnicodeBlock CJK_COMPATIBILITY_IDEOGRAPHS_SUPPLEMENT =
            new UnicodeBlock("CJK_COMPATIBILITY_IDEOGRAPHS_SUPPLEMENT", "CJK COMPATIBILITY IDEOGRAPHS SUPPLEMENT", "CJKCOMPATIBILITYIDEOGRAPHSSUPPLEMENT");
        public static readonly UnicodeBlock TAGS =
            new UnicodeBlock("TAGS");
        public static readonly UnicodeBlock VARIATION_SELECTORS_SUPPLEMENT =
            new UnicodeBlock("VARIATION_SELECTORS_SUPPLEMENT", "VARIATION SELECTORS SUPPLEMENT", "VARIATIONSELECTORSSUPPLEMENT");
        public static readonly UnicodeBlock SUPPLEMENTARY_PRIVATE_USE_AREA_A =
            new UnicodeBlock("SUPPLEMENTARY_PRIVATE_USE_AREA_A", "SUPPLEMENTARY PRIVATE USE AREA-A", "SUPPLEMENTARYPRIVATEUSEAREA-A");
        public static readonly UnicodeBlock SUPPLEMENTARY_PRIVATE_USE_AREA_B =
            new UnicodeBlock("SUPPLEMENTARY_PRIVATE_USE_AREA_B", "SUPPLEMENTARY PRIVATE USE AREA-B", "SUPPLEMENTARYPRIVATEUSEAREA-B");
        public static readonly UnicodeBlock HIGH_SURROGATES =
            new UnicodeBlock("HIGH_SURROGATES", "HIGH SURROGATES", "HIGHSURROGATES");
        public static readonly UnicodeBlock HIGH_PRIVATE_USE_SURROGATES =
            new UnicodeBlock("HIGH_PRIVATE_USE_SURROGATES", "HIGH PRIVATE USE SURROGATES", "HIGHPRIVATEUSESURROGATES");
        public static readonly UnicodeBlock LOW_SURROGATES =
            new UnicodeBlock("LOW_SURROGATES", "LOW SURROGATES", "LOWSURROGATES");
        public static readonly UnicodeBlock ARABIC_SUPPLEMENT =
            new UnicodeBlock("ARABIC_SUPPLEMENT", "ARABIC SUPPLEMENT", "ARABICSUPPLEMENT");
        public static readonly UnicodeBlock NKO =
            new UnicodeBlock("NKO");
        public static readonly UnicodeBlock SAMARITAN =
            new UnicodeBlock("SAMARITAN");
        public static readonly UnicodeBlock MANDAIC =
            new UnicodeBlock("MANDAIC");
        public static readonly UnicodeBlock ETHIOPIC_SUPPLEMENT =
            new UnicodeBlock("ETHIOPIC_SUPPLEMENT", "ETHIOPIC SUPPLEMENT", "ETHIOPICSUPPLEMENT");
        public static readonly UnicodeBlock UNIFIED_CANADIAN_ABORIGINAL_SYLLABICS_EXTENDED =
            new UnicodeBlock("UNIFIED_CANADIAN_ABORIGINAL_SYLLABICS_EXTENDED", "UNIFIED CANADIAN ABORIGINAL SYLLABICS EXTENDED", "UNIFIEDCANADIANABORIGINALSYLLABICSEXTENDED");
        public static readonly UnicodeBlock NEW_TAI_LUE =
            new UnicodeBlock("NEW_TAI_LUE", "NEW TAI LUE", "NEWTAILUE");
        public static readonly UnicodeBlock BUGINESE =
            new UnicodeBlock("BUGINESE");
        public static readonly UnicodeBlock TAI_THAM =
            new UnicodeBlock("TAI_THAM", "TAI THAM", "TAITHAM");
        public static readonly UnicodeBlock BALINESE =
            new UnicodeBlock("BALINESE");
        public static readonly UnicodeBlock SUNDANESE =
            new UnicodeBlock("SUNDANESE");
        public static readonly UnicodeBlock BATAK =
            new UnicodeBlock("BATAK");
        public static readonly UnicodeBlock LEPCHA =
            new UnicodeBlock("LEPCHA");
        public static readonly UnicodeBlock OL_CHIKI =
            new UnicodeBlock("OL_CHIKI", "OL CHIKI", "OLCHIKI");
        public static readonly UnicodeBlock VEDIC_EXTENSIONS =
            new UnicodeBlock("VEDIC_EXTENSIONS", "VEDIC EXTENSIONS", "VEDICEXTENSIONS");
        public static readonly UnicodeBlock PHONETIC_EXTENSIONS_SUPPLEMENT =
            new UnicodeBlock("PHONETIC_EXTENSIONS_SUPPLEMENT", "PHONETIC EXTENSIONS SUPPLEMENT", "PHONETICEXTENSIONSSUPPLEMENT");
        public static readonly UnicodeBlock COMBINING_DIACRITICAL_MARKS_SUPPLEMENT =
            new UnicodeBlock("COMBINING_DIACRITICAL_MARKS_SUPPLEMENT", "COMBINING DIACRITICAL MARKS SUPPLEMENT", "COMBININGDIACRITICALMARKSSUPPLEMENT");
        public static readonly UnicodeBlock GLAGOLITIC =
            new UnicodeBlock("GLAGOLITIC");
        public static readonly UnicodeBlock LATIN_EXTENDED_C =
            new UnicodeBlock("LATIN_EXTENDED_C", "LATIN EXTENDED-C", "LATINEXTENDED-C");
        public static readonly UnicodeBlock COPTIC =
            new UnicodeBlock("COPTIC");
        public static readonly UnicodeBlock GEORGIAN_SUPPLEMENT =
            new UnicodeBlock("GEORGIAN_SUPPLEMENT", "GEORGIAN SUPPLEMENT", "GEORGIANSUPPLEMENT");
        public static readonly UnicodeBlock TIFINAGH =
            new UnicodeBlock("TIFINAGH");
        public static readonly UnicodeBlock ETHIOPIC_EXTENDED =
            new UnicodeBlock("ETHIOPIC_EXTENDED", "ETHIOPIC EXTENDED", "ETHIOPICEXTENDED");
        public static readonly UnicodeBlock CYRILLIC_EXTENDED_A =
            new UnicodeBlock("CYRILLIC_EXTENDED_A", "CYRILLIC EXTENDED-A", "CYRILLICEXTENDED-A");
        public static readonly UnicodeBlock SUPPLEMENTAL_PUNCTUATION =
            new UnicodeBlock("SUPPLEMENTAL_PUNCTUATION", "SUPPLEMENTAL PUNCTUATION", "SUPPLEMENTALPUNCTUATION");
        public static readonly UnicodeBlock CJK_STROKES =
            new UnicodeBlock("CJK_STROKES", "CJK STROKES", "CJKSTROKES");
        public static readonly UnicodeBlock LISU =
            new UnicodeBlock("LISU");
        public static readonly UnicodeBlock VAI =
            new UnicodeBlock("VAI");
        public static readonly UnicodeBlock CYRILLIC_EXTENDED_B =
            new UnicodeBlock("CYRILLIC_EXTENDED_B", "CYRILLIC EXTENDED-B", "CYRILLICEXTENDED-B");
        public static readonly UnicodeBlock BAMUM =
            new UnicodeBlock("BAMUM");
        public static readonly UnicodeBlock MODIFIER_TONE_LETTERS =
            new UnicodeBlock("MODIFIER_TONE_LETTERS", "MODIFIER TONE LETTERS", "MODIFIERTONELETTERS");
        public static readonly UnicodeBlock LATIN_EXTENDED_D =
            new UnicodeBlock("LATIN_EXTENDED_D", "LATIN EXTENDED-D", "LATINEXTENDED-D");
        public static readonly UnicodeBlock SYLOTI_NAGRI =
            new UnicodeBlock("SYLOTI_NAGRI", "SYLOTI NAGRI", "SYLOTINAGRI");
        public static readonly UnicodeBlock COMMON_INDIC_NUMBER_FORMS =
            new UnicodeBlock("COMMON_INDIC_NUMBER_FORMS", "COMMON INDIC NUMBER FORMS", "COMMONINDICNUMBERFORMS");
        public static readonly UnicodeBlock PHAGS_PA =
            new UnicodeBlock("PHAGS_PA", "PHAGS-PA");
        public static readonly UnicodeBlock SAURASHTRA =
            new UnicodeBlock("SAURASHTRA");
        public static readonly UnicodeBlock DEVANAGARI_EXTENDED =
            new UnicodeBlock("DEVANAGARI_EXTENDED", "DEVANAGARI EXTENDED", "DEVANAGARIEXTENDED");
        public static readonly UnicodeBlock KAYAH_LI =
            new UnicodeBlock("KAYAH_LI", "KAYAH LI", "KAYAHLI");
        public static readonly UnicodeBlock REJANG =
            new UnicodeBlock("REJANG");
        public static readonly UnicodeBlock HANGUL_JAMO_EXTENDED_A =
            new UnicodeBlock("HANGUL_JAMO_EXTENDED_A", "HANGUL JAMO EXTENDED-A", "HANGULJAMOEXTENDED-A");
        public static readonly UnicodeBlock JAVANESE =
            new UnicodeBlock("JAVANESE");
        public static readonly UnicodeBlock CHAM =
            new UnicodeBlock("CHAM");
        public static readonly UnicodeBlock MYANMAR_EXTENDED_A =
            new UnicodeBlock("MYANMAR_EXTENDED_A", "MYANMAR EXTENDED-A", "MYANMAREXTENDED-A");
        public static readonly UnicodeBlock TAI_VIET =
            new UnicodeBlock("TAI_VIET", "TAI VIET", "TAIVIET");
        public static readonly UnicodeBlock ETHIOPIC_EXTENDED_A =
            new UnicodeBlock("ETHIOPIC_EXTENDED_A", "ETHIOPIC EXTENDED-A", "ETHIOPICEXTENDED-A");
        public static readonly UnicodeBlock MEETEI_MAYEK =
            new UnicodeBlock("MEETEI_MAYEK", "MEETEI MAYEK", "MEETEIMAYEK");
        public static readonly UnicodeBlock HANGUL_JAMO_EXTENDED_B =
            new UnicodeBlock("HANGUL_JAMO_EXTENDED_B", "HANGUL JAMO EXTENDED-B", "HANGULJAMOEXTENDED-B");
        public static readonly UnicodeBlock VERTICAL_FORMS =
            new UnicodeBlock("VERTICAL_FORMS", "VERTICAL FORMS", "VERTICALFORMS");
        public static readonly UnicodeBlock ANCIENT_GREEK_NUMBERS =
            new UnicodeBlock("ANCIENT_GREEK_NUMBERS", "ANCIENT GREEK NUMBERS", "ANCIENTGREEKNUMBERS");
        public static readonly UnicodeBlock ANCIENT_SYMBOLS =
            new UnicodeBlock("ANCIENT_SYMBOLS", "ANCIENT SYMBOLS", "ANCIENTSYMBOLS");
        public static readonly UnicodeBlock PHAISTOS_DISC =
            new UnicodeBlock("PHAISTOS_DISC", "PHAISTOS DISC", "PHAISTOSDISC");
        public static readonly UnicodeBlock LYCIAN =
            new UnicodeBlock("LYCIAN");
        public static readonly UnicodeBlock CARIAN =
            new UnicodeBlock("CARIAN");
        public static readonly UnicodeBlock OLD_PERSIAN =
            new UnicodeBlock("OLD_PERSIAN", "OLD PERSIAN", "OLDPERSIAN");
        public static readonly UnicodeBlock IMPERIAL_ARAMAIC =
            new UnicodeBlock("IMPERIAL_ARAMAIC", "IMPERIAL ARAMAIC", "IMPERIALARAMAIC");
        public static readonly UnicodeBlock PHOENICIAN =
            new UnicodeBlock("PHOENICIAN");
        public static readonly UnicodeBlock LYDIAN =
            new UnicodeBlock("LYDIAN");
        public static readonly UnicodeBlock KHAROSHTHI =
            new UnicodeBlock("KHAROSHTHI");
        public static readonly UnicodeBlock OLD_SOUTH_ARABIAN =
            new UnicodeBlock("OLD_SOUTH_ARABIAN", "OLD SOUTH ARABIAN", "OLDSOUTHARABIAN");
        public static readonly UnicodeBlock AVESTAN =
            new UnicodeBlock("AVESTAN");
        public static readonly UnicodeBlock INSCRIPTIONAL_PARTHIAN =
            new UnicodeBlock("INSCRIPTIONAL_PARTHIAN", "INSCRIPTIONAL PARTHIAN", "INSCRIPTIONALPARTHIAN");
        public static readonly UnicodeBlock INSCRIPTIONAL_PAHLAVI =
            new UnicodeBlock("INSCRIPTIONAL_PAHLAVI", "INSCRIPTIONAL PAHLAVI", "INSCRIPTIONALPAHLAVI");
        public static readonly UnicodeBlock OLD_TURKIC =
            new UnicodeBlock("OLD_TURKIC", "OLD TURKIC", "OLDTURKIC");
        public static readonly UnicodeBlock RUMI_NUMERAL_SYMBOLS =
            new UnicodeBlock("RUMI_NUMERAL_SYMBOLS", "RUMI NUMERAL SYMBOLS", "RUMINUMERALSYMBOLS");
        public static readonly UnicodeBlock BRAHMI =
            new UnicodeBlock("BRAHMI");
        public static readonly UnicodeBlock KAITHI =
            new UnicodeBlock("KAITHI");
        public static readonly UnicodeBlock CUNEIFORM =
            new UnicodeBlock("CUNEIFORM");
        public static readonly UnicodeBlock CUNEIFORM_NUMBERS_AND_PUNCTUATION =
            new UnicodeBlock("CUNEIFORM_NUMBERS_AND_PUNCTUATION", "CUNEIFORM NUMBERS AND PUNCTUATION", "CUNEIFORMNUMBERSANDPUNCTUATION");
        public static readonly UnicodeBlock EGYPTIAN_HIEROGLYPHS =
            new UnicodeBlock("EGYPTIAN_HIEROGLYPHS", "EGYPTIAN HIEROGLYPHS", "EGYPTIANHIEROGLYPHS");
        public static readonly UnicodeBlock BAMUM_SUPPLEMENT =
            new UnicodeBlock("BAMUM_SUPPLEMENT", "BAMUM SUPPLEMENT", "BAMUMSUPPLEMENT");
        public static readonly UnicodeBlock KANA_SUPPLEMENT =
            new UnicodeBlock("KANA_SUPPLEMENT", "KANA SUPPLEMENT", "KANASUPPLEMENT");
        public static readonly UnicodeBlock ANCIENT_GREEK_MUSICAL_NOTATION =
            new UnicodeBlock("ANCIENT_GREEK_MUSICAL_NOTATION", "ANCIENT GREEK MUSICAL NOTATION", "ANCIENTGREEKMUSICALNOTATION");
        public static readonly UnicodeBlock COUNTING_ROD_NUMERALS =
            new UnicodeBlock("COUNTING_ROD_NUMERALS", "COUNTING ROD NUMERALS", "COUNTINGRODNUMERALS");
        public static readonly UnicodeBlock MAHJONG_TILES =
            new UnicodeBlock("MAHJONG_TILES", "MAHJONG TILES", "MAHJONGTILES");
        public static readonly UnicodeBlock DOMINO_TILES =
            new UnicodeBlock("DOMINO_TILES", "DOMINO TILES", "DOMINOTILES");
        public static readonly UnicodeBlock PLAYING_CARDS =
            new UnicodeBlock("PLAYING_CARDS", "PLAYING CARDS", "PLAYINGCARDS");
        public static readonly UnicodeBlock ENCLOSED_ALPHANUMERIC_SUPPLEMENT =
            new UnicodeBlock("ENCLOSED_ALPHANUMERIC_SUPPLEMENT", "ENCLOSED ALPHANUMERIC SUPPLEMENT", "ENCLOSEDALPHANUMERICSUPPLEMENT");
        public static readonly UnicodeBlock ENCLOSED_IDEOGRAPHIC_SUPPLEMENT =
            new UnicodeBlock("ENCLOSED_IDEOGRAPHIC_SUPPLEMENT", "ENCLOSED IDEOGRAPHIC SUPPLEMENT", "ENCLOSEDIDEOGRAPHICSUPPLEMENT");
        public static readonly UnicodeBlock MISCELLANEOUS_SYMBOLS_AND_PICTOGRAPHS =
            new UnicodeBlock("MISCELLANEOUS_SYMBOLS_AND_PICTOGRAPHS", "MISCELLANEOUS SYMBOLS AND PICTOGRAPHS", "MISCELLANEOUSSYMBOLSANDPICTOGRAPHS");
        public static readonly UnicodeBlock EMOTICONS =
            new UnicodeBlock("EMOTICONS");
        public static readonly UnicodeBlock TRANSPORT_AND_MAP_SYMBOLS =
            new UnicodeBlock("TRANSPORT_AND_MAP_SYMBOLS", "TRANSPORT AND MAP SYMBOLS", "TRANSPORTANDMAPSYMBOLS");
        public static readonly UnicodeBlock ALCHEMICAL_SYMBOLS =
            new UnicodeBlock("ALCHEMICAL_SYMBOLS", "ALCHEMICAL SYMBOLS", "ALCHEMICALSYMBOLS");
        public static readonly UnicodeBlock CJK_UNIFIED_IDEOGRAPHS_EXTENSION_C =
            new UnicodeBlock("CJK_UNIFIED_IDEOGRAPHS_EXTENSION_C", "CJK UNIFIED IDEOGRAPHS EXTENSION C", "CJKUNIFIEDIDEOGRAPHSEXTENSIONC");
        public static readonly UnicodeBlock CJK_UNIFIED_IDEOGRAPHS_EXTENSION_D =
            new UnicodeBlock("CJK_UNIFIED_IDEOGRAPHS_EXTENSION_D", "CJK UNIFIED IDEOGRAPHS EXTENSION D", "CJKUNIFIEDIDEOGRAPHSEXTENSIOND");
        private static readonly int[] blockStarts = {
            0x0000,   // 0000..007F; Basic Latin
            0x0080,   // 0080..00FF; Latin-1 Supplement
            0x0100,   // 0100..017F; Latin Extended-A
            0x0180,   // 0180..024F; Latin Extended-B
            0x0250,   // 0250..02AF; IPA Extensions
            0x02B0,   // 02B0..02FF; Spacing Modifier Letters
            0x0300,   // 0300..036F; Combining Diacritical Marks
            0x0370,   // 0370..03FF; Greek and Coptic
            0x0400,   // 0400..04FF; Cyrillic
            0x0500,   // 0500..052F; Cyrillic Supplement
            0x0530,   // 0530..058F; Armenian
            0x0590,   // 0590..05FF; Hebrew
            0x0600,   // 0600..06FF; Arabic
            0x0700,   // 0700..074F; Syriac
            0x0750,   // 0750..077F; Arabic Supplement
            0x0780,   // 0780..07BF; Thaana
            0x07C0,   // 07C0..07FF; NKo
            0x0800,   // 0800..083F; Samaritan
            0x0840,   // 0840..085F; Mandaic
            0x0860,   //             unassigned
            0x0900,   // 0900..097F; Devanagari
            0x0980,   // 0980..09FF; Bengali
            0x0A00,   // 0A00..0A7F; Gurmukhi
            0x0A80,   // 0A80..0AFF; Gujarati
            0x0B00,   // 0B00..0B7F; Oriya
            0x0B80,   // 0B80..0BFF; Tamil
            0x0C00,   // 0C00..0C7F; Telugu
            0x0C80,   // 0C80..0CFF; Kannada
            0x0D00,   // 0D00..0D7F; Malayalam
            0x0D80,   // 0D80..0DFF; Sinhala
            0x0E00,   // 0E00..0E7F; Thai
            0x0E80,   // 0E80..0EFF; Lao
            0x0F00,   // 0F00..0FFF; Tibetan
            0x1000,   // 1000..109F; Myanmar
            0x10A0,   // 10A0..10FF; Georgian
            0x1100,   // 1100..11FF; Hangul Jamo
            0x1200,   // 1200..137F; Ethiopic
            0x1380,   // 1380..139F; Ethiopic Supplement
            0x13A0,   // 13A0..13FF; Cherokee
            0x1400,   // 1400..167F; Unified Canadian Aboriginal Syllabics
            0x1680,   // 1680..169F; Ogham
            0x16A0,   // 16A0..16FF; Runic
            0x1700,   // 1700..171F; Tagalog
            0x1720,   // 1720..173F; Hanunoo
            0x1740,   // 1740..175F; Buhid
            0x1760,   // 1760..177F; Tagbanwa
            0x1780,   // 1780..17FF; Khmer
            0x1800,   // 1800..18AF; Mongolian
            0x18B0,   // 18B0..18FF; Unified Canadian Aboriginal Syllabics Extended
            0x1900,   // 1900..194F; Limbu
            0x1950,   // 1950..197F; Tai Le
            0x1980,   // 1980..19DF; New Tai Lue
            0x19E0,   // 19E0..19FF; Khmer Symbols
            0x1A00,   // 1A00..1A1F; Buginese
            0x1A20,   // 1A20..1AAF; Tai Tham
            0x1AB0,   //             unassigned
            0x1B00,   // 1B00..1B7F; Balinese
            0x1B80,   // 1B80..1BBF; Sundanese
            0x1BC0,   // 1BC0..1BFF; Batak
            0x1C00,   // 1C00..1C4F; Lepcha
            0x1C50,   // 1C50..1C7F; Ol Chiki
            0x1C80,   //             unassigned
            0x1CD0,   // 1CD0..1CFF; Vedic Extensions
            0x1D00,   // 1D00..1D7F; Phonetic Extensions
            0x1D80,   // 1D80..1DBF; Phonetic Extensions Supplement
            0x1DC0,   // 1DC0..1DFF; Combining Diacritical Marks Supplement
            0x1E00,   // 1E00..1EFF; Latin Extended Additional
            0x1F00,   // 1F00..1FFF; Greek Extended
            0x2000,   // 2000..206F; General Punctuation
            0x2070,   // 2070..209F; Superscripts and Subscripts
            0x20A0,   // 20A0..20CF; Currency Symbols
            0x20D0,   // 20D0..20FF; Combining Diacritical Marks for Symbols
            0x2100,   // 2100..214F; Letterlike Symbols
            0x2150,   // 2150..218F; Number Forms
            0x2190,   // 2190..21FF; Arrows
            0x2200,   // 2200..22FF; Mathematical Operators
            0x2300,   // 2300..23FF; Miscellaneous Technical
            0x2400,   // 2400..243F; Control Pictures
            0x2440,   // 2440..245F; Optical Character Recognition
            0x2460,   // 2460..24FF; Enclosed Alphanumerics
            0x2500,   // 2500..257F; Box Drawing
            0x2580,   // 2580..259F; Block Elements
            0x25A0,   // 25A0..25FF; Geometric Shapes
            0x2600,   // 2600..26FF; Miscellaneous Symbols
            0x2700,   // 2700..27BF; Dingbats
            0x27C0,   // 27C0..27EF; Miscellaneous Mathematical Symbols-A
            0x27F0,   // 27F0..27FF; Supplemental Arrows-A
            0x2800,   // 2800..28FF; Braille Patterns
            0x2900,   // 2900..297F; Supplemental Arrows-B
            0x2980,   // 2980..29FF; Miscellaneous Mathematical Symbols-B
            0x2A00,   // 2A00..2AFF; Supplemental Mathematical Operators
            0x2B00,   // 2B00..2BFF; Miscellaneous Symbols and Arrows
            0x2C00,   // 2C00..2C5F; Glagolitic
            0x2C60,   // 2C60..2C7F; Latin Extended-C
            0x2C80,   // 2C80..2CFF; Coptic
            0x2D00,   // 2D00..2D2F; Georgian Supplement
            0x2D30,   // 2D30..2D7F; Tifinagh
            0x2D80,   // 2D80..2DDF; Ethiopic Extended
            0x2DE0,   // 2DE0..2DFF; Cyrillic Extended-A
            0x2E00,   // 2E00..2E7F; Supplemental Punctuation
            0x2E80,   // 2E80..2EFF; CJK Radicals Supplement
            0x2F00,   // 2F00..2FDF; Kangxi Radicals
            0x2FE0,   //             unassigned
            0x2FF0,   // 2FF0..2FFF; Ideographic Description Characters
            0x3000,   // 3000..303F; CJK Symbols and Punctuation
            0x3040,   // 3040..309F; Hiragana
            0x30A0,   // 30A0..30FF; Katakana
            0x3100,   // 3100..312F; Bopomofo
            0x3130,   // 3130..318F; Hangul Compatibility Jamo
            0x3190,   // 3190..319F; Kanbun
            0x31A0,   // 31A0..31BF; Bopomofo Extended
            0x31C0,   // 31C0..31EF; CJK Strokes
            0x31F0,   // 31F0..31FF; Katakana Phonetic Extensions
            0x3200,   // 3200..32FF; Enclosed CJK Letters and Months
            0x3300,   // 3300..33FF; CJK Compatibility
            0x3400,   // 3400..4DBF; CJK Unified Ideographs Extension A
            0x4DC0,   // 4DC0..4DFF; Yijing Hexagram Symbols
            0x4E00,   // 4E00..9FFF; CJK Unified Ideographs
            0xA000,   // A000..A48F; Yi Syllables
            0xA490,   // A490..A4CF; Yi Radicals
            0xA4D0,   // A4D0..A4FF; Lisu
            0xA500,   // A500..A63F; Vai
            0xA640,   // A640..A69F; Cyrillic Extended-B
            0xA6A0,   // A6A0..A6FF; Bamum
            0xA700,   // A700..A71F; Modifier Tone Letters
            0xA720,   // A720..A7FF; Latin Extended-D
            0xA800,   // A800..A82F; Syloti Nagri
            0xA830,   // A830..A83F; Common Indic Number Forms
            0xA840,   // A840..A87F; Phags-pa
            0xA880,   // A880..A8DF; Saurashtra
            0xA8E0,   // A8E0..A8FF; Devanagari Extended
            0xA900,   // A900..A92F; Kayah Li
            0xA930,   // A930..A95F; Rejang
            0xA960,   // A960..A97F; Hangul Jamo Extended-A
            0xA980,   // A980..A9DF; Javanese
            0xA9E0,   //             unassigned
            0xAA00,   // AA00..AA5F; Cham
            0xAA60,   // AA60..AA7F; Myanmar Extended-A
            0xAA80,   // AA80..AADF; Tai Viet
            0xAAE0,   //             unassigned
            0xAB00,   // AB00..AB2F; Ethiopic Extended-A
            0xAB30,   //             unassigned
            0xABC0,   // ABC0..ABFF; Meetei Mayek
            0xAC00,   // AC00..D7AF; Hangul Syllables
            0xD7B0,   // D7B0..D7FF; Hangul Jamo Extended-B
            0xD800,   // D800..DB7F; High Surrogates
            0xDB80,   // DB80..DBFF; High Private Use Surrogates
            0xDC00,   // DC00..DFFF; Low Surrogates
            0xE000,   // E000..F8FF; Private Use Area
            0xF900,   // F900..FAFF; CJK Compatibility Ideographs
            0xFB00,   // FB00..FB4F; Alphabetic Presentation Forms
            0xFB50,   // FB50..FDFF; Arabic Presentation Forms-A
            0xFE00,   // FE00..FE0F; Variation Selectors
            0xFE10,   // FE10..FE1F; Vertical Forms
            0xFE20,   // FE20..FE2F; Combining Half Marks
            0xFE30,   // FE30..FE4F; CJK Compatibility Forms
            0xFE50,   // FE50..FE6F; Small Form Variants
            0xFE70,   // FE70..FEFF; Arabic Presentation Forms-B
            0xFF00,   // FF00..FFEF; Halfwidth and Fullwidth Forms
            0xFFF0,   // FFF0..FFFF; Specials
            0x10000,  // 10000..1007F; Linear B Syllabary
            0x10080,  // 10080..100FF; Linear B Ideograms
            0x10100,  // 10100..1013F; Aegean Numbers
            0x10140,  // 10140..1018F; Ancient Greek Numbers
            0x10190,  // 10190..101CF; Ancient Symbols
            0x101D0,  // 101D0..101FF; Phaistos Disc
            0x10200,  //               unassigned
            0x10280,  // 10280..1029F; Lycian
            0x102A0,  // 102A0..102DF; Carian
            0x102E0,  //               unassigned
            0x10300,  // 10300..1032F; Old Italic
            0x10330,  // 10330..1034F; Gothic
            0x10350,  //               unassigned
            0x10380,  // 10380..1039F; Ugaritic
            0x103A0,  // 103A0..103DF; Old Persian
            0x103E0,  //               unassigned
            0x10400,  // 10400..1044F; Deseret
            0x10450,  // 10450..1047F; Shavian
            0x10480,  // 10480..104AF; Osmanya
            0x104B0,  //               unassigned
            0x10800,  // 10800..1083F; Cypriot Syllabary
            0x10840,  // 10840..1085F; Imperial Aramaic
            0x10860,  //               unassigned
            0x10900,  // 10900..1091F; Phoenician
            0x10920,  // 10920..1093F; Lydian
            0x10940,  //               unassigned
            0x10A00,  // 10A00..10A5F; Kharoshthi
            0x10A60,  // 10A60..10A7F; Old South Arabian
            0x10A80,  //               unassigned
            0x10B00,  // 10B00..10B3F; Avestan
            0x10B40,  // 10B40..10B5F; Inscriptional Parthian
            0x10B60,  // 10B60..10B7F; Inscriptional Pahlavi
            0x10B80,  //               unassigned
            0x10C00,  // 10C00..10C4F; Old Turkic
            0x10C50,  //               unassigned
            0x10E60,  // 10E60..10E7F; Rumi Numeral Symbols
            0x10E80,  //               unassigned
            0x11000,  // 11000..1107F; Brahmi
            0x11080,  // 11080..110CF; Kaithi
            0x110D0,  //               unassigned
            0x12000,  // 12000..123FF; Cuneiform
            0x12400,  // 12400..1247F; Cuneiform Numbers and Punctuation
            0x12480,  //               unassigned
            0x13000,  // 13000..1342F; Egyptian Hieroglyphs
            0x13430,  //               unassigned
            0x16800,  // 16800..16A3F; Bamum Supplement
            0x16A40,  //               unassigned
            0x1B000,  // 1B000..1B0FF; Kana Supplement
            0x1B100,  //               unassigned
            0x1D000,  // 1D000..1D0FF; Byzantine Musical Symbols
            0x1D100,  // 1D100..1D1FF; Musical Symbols
            0x1D200,  // 1D200..1D24F; Ancient Greek Musical Notation
            0x1D250,  //               unassigned
            0x1D300,  // 1D300..1D35F; Tai Xuan Jing Symbols
            0x1D360,  // 1D360..1D37F; Counting Rod Numerals
            0x1D380,  //               unassigned
            0x1D400,  // 1D400..1D7FF; Mathematical Alphanumeric Symbols
            0x1D800,  //               unassigned
            0x1F000,  // 1F000..1F02F; Mahjong Tiles
            0x1F030,  // 1F030..1F09F; Domino Tiles
            0x1F0A0,  // 1F0A0..1F0FF; Playing Cards
            0x1F100,  // 1F100..1F1FF; Enclosed Alphanumeric Supplement
            0x1F200,  // 1F200..1F2FF; Enclosed Ideographic Supplement
            0x1F300,  // 1F300..1F5FF; Miscellaneous Symbols And Pictographs
            0x1F600,  // 1F600..1F64F; Emoticons
            0x1F650,  //               unassigned
            0x1F680,  // 1F680..1F6FF; Transport And Map Symbols
            0x1F700,  // 1F700..1F77F; Alchemical Symbols
            0x1F780,  //               unassigned
            0x20000,  // 20000..2A6DF; CJK Unified Ideographs Extension B
            0x2A6E0,  //               unassigned
            0x2A700,  // 2A700..2B73F; CJK Unified Ideographs Extension C
            0x2B740,  // 2B740..2B81F; CJK Unified Ideographs Extension D
            0x2B820,  //               unassigned
            0x2F800,  // 2F800..2FA1F; CJK Compatibility Ideographs Supplement
            0x2FA20,  //               unassigned
            0xE0000,  // E0000..E007F; Tags
            0xE0080,  //               unassigned
            0xE0100,  // E0100..E01EF; Variation Selectors Supplement
            0xE01F0,  //               unassigned
            0xF0000,  // F0000..FFFFF; Supplementary Private Use Area-A
            0x100000  // 100000..10FFFF; Supplementary Private Use Area-B
        };
        private static readonly UnicodeBlock[] blocks = {
            BASIC_LATIN,
            LATIN_1_SUPPLEMENT,
            LATIN_EXTENDED_A,
            LATIN_EXTENDED_B,
            IPA_EXTENSIONS,
            SPACING_MODIFIER_LETTERS,
            COMBINING_DIACRITICAL_MARKS,
            GREEK,
            CYRILLIC,
            CYRILLIC_SUPPLEMENTARY,
            ARMENIAN,
            HEBREW,
            ARABIC,
            SYRIAC,
            ARABIC_SUPPLEMENT,
            THAANA,
            NKO,
            SAMARITAN,
            MANDAIC,
            null,
            DEVANAGARI,
            BENGALI,
            GURMUKHI,
            GUJARATI,
            ORIYA,
            TAMIL,
            TELUGU,
            KANNADA,
            MALAYALAM,
            SINHALA,
            THAI,
            LAO,
            TIBETAN,
            MYANMAR,
            GEORGIAN,
            HANGUL_JAMO,
            ETHIOPIC,
            ETHIOPIC_SUPPLEMENT,
            CHEROKEE,
            UNIFIED_CANADIAN_ABORIGINAL_SYLLABICS,
            OGHAM,
            RUNIC,
            TAGALOG,
            HANUNOO,
            BUHID,
            TAGBANWA,
            KHMER,
            MONGOLIAN,
            UNIFIED_CANADIAN_ABORIGINAL_SYLLABICS_EXTENDED,
            LIMBU,
            TAI_LE,
            NEW_TAI_LUE,
            KHMER_SYMBOLS,
            BUGINESE,
            TAI_THAM,
            null,
            BALINESE,
            SUNDANESE,
            BATAK,
            LEPCHA,
            OL_CHIKI,
            null,
            VEDIC_EXTENSIONS,
            PHONETIC_EXTENSIONS,
            PHONETIC_EXTENSIONS_SUPPLEMENT,
            COMBINING_DIACRITICAL_MARKS_SUPPLEMENT,
            LATIN_EXTENDED_ADDITIONAL,
            GREEK_EXTENDED,
            GENERAL_PUNCTUATION,
            SUPERSCRIPTS_AND_SUBSCRIPTS,
            CURRENCY_SYMBOLS,
            COMBINING_MARKS_FOR_SYMBOLS,
            LETTERLIKE_SYMBOLS,
            NUMBER_FORMS,
            ARROWS,
            MATHEMATICAL_OPERATORS,
            MISCELLANEOUS_TECHNICAL,
            CONTROL_PICTURES,
            OPTICAL_CHARACTER_RECOGNITION,
            ENCLOSED_ALPHANUMERICS,
            BOX_DRAWING,
            BLOCK_ELEMENTS,
            GEOMETRIC_SHAPES,
            MISCELLANEOUS_SYMBOLS,
            DINGBATS,
            MISCELLANEOUS_MATHEMATICAL_SYMBOLS_A,
            SUPPLEMENTAL_ARROWS_A,
            BRAILLE_PATTERNS,
            SUPPLEMENTAL_ARROWS_B,
            MISCELLANEOUS_MATHEMATICAL_SYMBOLS_B,
            SUPPLEMENTAL_MATHEMATICAL_OPERATORS,
            MISCELLANEOUS_SYMBOLS_AND_ARROWS,
            GLAGOLITIC,
            LATIN_EXTENDED_C,
            COPTIC,
            GEORGIAN_SUPPLEMENT,
            TIFINAGH,
            ETHIOPIC_EXTENDED,
            CYRILLIC_EXTENDED_A,
            SUPPLEMENTAL_PUNCTUATION,
            CJK_RADICALS_SUPPLEMENT,
            KANGXI_RADICALS,
            null,
            IDEOGRAPHIC_DESCRIPTION_CHARACTERS,
            CJK_SYMBOLS_AND_PUNCTUATION,
            HIRAGANA,
            KATAKANA,
            BOPOMOFO,
            HANGUL_COMPATIBILITY_JAMO,
            KANBUN,
            BOPOMOFO_EXTENDED,
            CJK_STROKES,
            KATAKANA_PHONETIC_EXTENSIONS,
            ENCLOSED_CJK_LETTERS_AND_MONTHS,
            CJK_COMPATIBILITY,
            CJK_UNIFIED_IDEOGRAPHS_EXTENSION_A,
            YIJING_HEXAGRAM_SYMBOLS,
            CJK_UNIFIED_IDEOGRAPHS,
            YI_SYLLABLES,
            YI_RADICALS,
            LISU,
            VAI,
            CYRILLIC_EXTENDED_B,
            BAMUM,
            MODIFIER_TONE_LETTERS,
            LATIN_EXTENDED_D,
            SYLOTI_NAGRI,
            COMMON_INDIC_NUMBER_FORMS,
            PHAGS_PA,
            SAURASHTRA,
            DEVANAGARI_EXTENDED,
            KAYAH_LI,
            REJANG,
            HANGUL_JAMO_EXTENDED_A,
            JAVANESE,
            null,
            CHAM,
            MYANMAR_EXTENDED_A,
            TAI_VIET,
            null,
            ETHIOPIC_EXTENDED_A,
            null,
            MEETEI_MAYEK,
            HANGUL_SYLLABLES,
            HANGUL_JAMO_EXTENDED_B,
            HIGH_SURROGATES,
            HIGH_PRIVATE_USE_SURROGATES,
            LOW_SURROGATES,
            PRIVATE_USE_AREA,
            CJK_COMPATIBILITY_IDEOGRAPHS,
            ALPHABETIC_PRESENTATION_FORMS,
            ARABIC_PRESENTATION_FORMS_A,
            VARIATION_SELECTORS,
            VERTICAL_FORMS,
            COMBINING_HALF_MARKS,
            CJK_COMPATIBILITY_FORMS,
            SMALL_FORM_VARIANTS,
            ARABIC_PRESENTATION_FORMS_B,
            HALFWIDTH_AND_FULLWIDTH_FORMS,
            SPECIALS,
            LINEAR_B_SYLLABARY,
            LINEAR_B_IDEOGRAMS,
            AEGEAN_NUMBERS,
            ANCIENT_GREEK_NUMBERS,
            ANCIENT_SYMBOLS,
            PHAISTOS_DISC,
            null,
            LYCIAN,
            CARIAN,
            null,
            OLD_ITALIC,
            GOTHIC,
            null,
            UGARITIC,
            OLD_PERSIAN,
            null,
            DESERET,
            SHAVIAN,
            OSMANYA,
            null,
            CYPRIOT_SYLLABARY,
            IMPERIAL_ARAMAIC,
            null,
            PHOENICIAN,
            LYDIAN,
            null,
            KHAROSHTHI,
            OLD_SOUTH_ARABIAN,
            null,
            AVESTAN,
            INSCRIPTIONAL_PARTHIAN,
            INSCRIPTIONAL_PAHLAVI,
            null,
            OLD_TURKIC,
            null,
            RUMI_NUMERAL_SYMBOLS,
            null,
            BRAHMI,
            KAITHI,
            null,
            CUNEIFORM,
            CUNEIFORM_NUMBERS_AND_PUNCTUATION,
            null,
            EGYPTIAN_HIEROGLYPHS,
            null,
            BAMUM_SUPPLEMENT,
            null,
            KANA_SUPPLEMENT,
            null,
            BYZANTINE_MUSICAL_SYMBOLS,
            MUSICAL_SYMBOLS,
            ANCIENT_GREEK_MUSICAL_NOTATION,
            null,
            TAI_XUAN_JING_SYMBOLS,
            COUNTING_ROD_NUMERALS,
            null,
            MATHEMATICAL_ALPHANUMERIC_SYMBOLS,
            null,
            MAHJONG_TILES,
            DOMINO_TILES,
            PLAYING_CARDS,
            ENCLOSED_ALPHANUMERIC_SUPPLEMENT,
            ENCLOSED_IDEOGRAPHIC_SUPPLEMENT,
            MISCELLANEOUS_SYMBOLS_AND_PICTOGRAPHS,
            EMOTICONS,
            null,
            TRANSPORT_AND_MAP_SYMBOLS,
            ALCHEMICAL_SYMBOLS,
            null,
            CJK_UNIFIED_IDEOGRAPHS_EXTENSION_B,
            null,
            CJK_UNIFIED_IDEOGRAPHS_EXTENSION_C,
            CJK_UNIFIED_IDEOGRAPHS_EXTENSION_D,
            null,
            CJK_COMPATIBILITY_IDEOGRAPHS_SUPPLEMENT,
            null,
            TAGS,
            null,
            VARIATION_SELECTORS_SUPPLEMENT,
            null,
            SUPPLEMENTARY_PRIVATE_USE_AREA_A,
            SUPPLEMENTARY_PRIVATE_USE_AREA_B
        };
        public static UnicodeBlock of(char c) {
            return of((int)c);
        }
        public static UnicodeBlock of(int codePoint) {
            if (!isValidCodePoint(codePoint)) {
                throw new IllegalArgumentException();
            }
            int top, bottom, current;
            bottom = 0;
            top = blockStarts.Length;
            current = top/2;
            // invariant: top > current >= bottom && codePoint >= unicodeBlockStarts[bottom]
            while (top - bottom > 1) {
                if (codePoint >= blockStarts[current]) {
                    bottom = current;
                } else {
                    top = current;
                }
                current = (top + bottom) / 2;
            }
            return blocks[current];
        }
        public static UnicodeBlock forName(String blockName) {
            UnicodeBlock block = map.get(blockName.toUpperCase());
            if (block == null) {
                throw new IllegalArgumentException();
            }
            return block;
        }
    }
    public class UnicodeScript {
        enum UnicodeScriptInternal
        {
            COMMON,
            LATIN,
            GREEK,
            CYRILLIC,
            ARMENIAN,
            HEBREW,
            ARABIC,
            SYRIAC,
            THAANA,
            DEVANAGARI,
            BENGALI,
            GURMUKHI,
            GUJARATI,
            ORIYA,
            TAMIL,
            TELUGU,
            KANNADA,
            MALAYALAM,
            SINHALA,
            THAI,
            LAO,
            TIBETAN,
            MYANMAR,
            GEORGIAN,
            HANGUL,
            ETHIOPIC,
            CHEROKEE,
            CANADIAN_ABORIGINAL,
            OGHAM,
            RUNIC,
            KHMER,
            MONGOLIAN,
            HIRAGANA,
            KATAKANA,
            BOPOMOFO,
            HAN,
            YI,
            OLD_ITALIC,
            GOTHIC,
            DESERET,
            INHERITED,
            TAGALOG,
            HANUNOO,
            BUHID,
            TAGBANWA,
            LIMBU,
            TAI_LE,
            LINEAR_B,
            UGARITIC,
            SHAVIAN,
            OSMANYA,
            CYPRIOT,
            BRAILLE,
            BUGINESE,
            COPTIC,
            NEW_TAI_LUE,
            GLAGOLITIC,
            TIFINAGH,
            SYLOTI_NAGRI,
            OLD_PERSIAN,
            KHAROSHTHI,
            BALINESE,
            CUNEIFORM,
            PHOENICIAN,
            PHAGS_PA,
            NKO,
            SUNDANESE,
            BATAK,
            LEPCHA,
            OL_CHIKI,
            VAI,
            SAURASHTRA,
            KAYAH_LI,
            REJANG,
            LYCIAN,
            CARIAN,
            LYDIAN,
            CHAM,
            TAI_THAM,
            TAI_VIET,
            AVESTAN,
            EGYPTIAN_HIEROGLYPHS,
            SAMARITAN,
            MANDAIC,
            LISU,
            BAMUM,
            JAVANESE,
            MEETEI_MAYEK,
            IMPERIAL_ARAMAIC,
            OLD_SOUTH_ARABIAN,
            INSCRIPTIONAL_PARTHIAN,
            INSCRIPTIONAL_PAHLAVI,
            OLD_TURKIC,
            BRAHMI,
            KAITHI,
            UNKNOWN
        }

        UnicodeScriptInternal value;

        UnicodeScript(UnicodeScriptInternal value) {
            this.value = value;
        }

        #region public static UnicodeScript XXXX = new UnicodeScript(UnicodeScriptInternal.XXXX);
        public static UnicodeScript COMMON = new UnicodeScript(UnicodeScriptInternal.COMMON);
        public static UnicodeScript LATIN = new UnicodeScript(UnicodeScriptInternal.LATIN );
        public static UnicodeScript GREEK = new UnicodeScript(UnicodeScriptInternal.GREEK );
        public static UnicodeScript CYRILLIC = new UnicodeScript(UnicodeScriptInternal.CYRILLIC );
        public static UnicodeScript ARMENIAN = new UnicodeScript(UnicodeScriptInternal.ARMENIAN );
        public static UnicodeScript HEBREW = new UnicodeScript(UnicodeScriptInternal.HEBREW );
        public static UnicodeScript ARABIC = new UnicodeScript(UnicodeScriptInternal.ARABIC );
        public static UnicodeScript SYRIAC = new UnicodeScript(UnicodeScriptInternal.SYRIAC );
        public static UnicodeScript THAANA = new UnicodeScript(UnicodeScriptInternal.THAANA );
        public static UnicodeScript DEVANAGARI = new UnicodeScript(UnicodeScriptInternal.DEVANAGARI );
        public static UnicodeScript BENGALI = new UnicodeScript(UnicodeScriptInternal.BENGALI );
        public static UnicodeScript GURMUKHI = new UnicodeScript(UnicodeScriptInternal.GURMUKHI );
        public static UnicodeScript GUJARATI = new UnicodeScript(UnicodeScriptInternal.GUJARATI );
        public static UnicodeScript ORIYA = new UnicodeScript(UnicodeScriptInternal.ORIYA );
        public static UnicodeScript TAMIL = new UnicodeScript(UnicodeScriptInternal.TAMIL );
        public static UnicodeScript TELUGU = new UnicodeScript(UnicodeScriptInternal.TELUGU );
        public static UnicodeScript KANNADA = new UnicodeScript(UnicodeScriptInternal.KANNADA );
        public static UnicodeScript MALAYALAM = new UnicodeScript(UnicodeScriptInternal.MALAYALAM );
        public static UnicodeScript SINHALA = new UnicodeScript(UnicodeScriptInternal.SINHALA );
        public static UnicodeScript THAI = new UnicodeScript(UnicodeScriptInternal.THAI );
        public static UnicodeScript LAO = new UnicodeScript(UnicodeScriptInternal.LAO );
        public static UnicodeScript TIBETAN = new UnicodeScript(UnicodeScriptInternal.TIBETAN );
        public static UnicodeScript MYANMAR = new UnicodeScript(UnicodeScriptInternal.MYANMAR );
        public static UnicodeScript GEORGIAN = new UnicodeScript(UnicodeScriptInternal.GEORGIAN );
        public static UnicodeScript HANGUL = new UnicodeScript(UnicodeScriptInternal.HANGUL );
        public static UnicodeScript ETHIOPIC = new UnicodeScript(UnicodeScriptInternal.ETHIOPIC );
        public static UnicodeScript CHEROKEE = new UnicodeScript(UnicodeScriptInternal.CHEROKEE );
        public static UnicodeScript CANADIAN_ABORIGINAL = new UnicodeScript(UnicodeScriptInternal.CANADIAN_ABORIGINAL );
        public static UnicodeScript OGHAM = new UnicodeScript(UnicodeScriptInternal.OGHAM );
        public static UnicodeScript RUNIC = new UnicodeScript(UnicodeScriptInternal.RUNIC );
        public static UnicodeScript KHMER = new UnicodeScript(UnicodeScriptInternal.KHMER );
        public static UnicodeScript MONGOLIAN = new UnicodeScript(UnicodeScriptInternal.MONGOLIAN );
        public static UnicodeScript HIRAGANA = new UnicodeScript(UnicodeScriptInternal.HIRAGANA );
        public static UnicodeScript KATAKANA = new UnicodeScript(UnicodeScriptInternal.KATAKANA );
        public static UnicodeScript BOPOMOFO = new UnicodeScript(UnicodeScriptInternal.BOPOMOFO );
        public static UnicodeScript HAN = new UnicodeScript(UnicodeScriptInternal.HAN );
        public static UnicodeScript YI = new UnicodeScript(UnicodeScriptInternal.YI );
        public static UnicodeScript OLD_ITALIC = new UnicodeScript(UnicodeScriptInternal.OLD_ITALIC );
        public static UnicodeScript GOTHIC = new UnicodeScript(UnicodeScriptInternal.GOTHIC );
        public static UnicodeScript DESERET = new UnicodeScript(UnicodeScriptInternal.DESERET );
        public static UnicodeScript INHERITED = new UnicodeScript(UnicodeScriptInternal.INHERITED );
        public static UnicodeScript TAGALOG = new UnicodeScript(UnicodeScriptInternal.TAGALOG );
        public static UnicodeScript HANUNOO = new UnicodeScript(UnicodeScriptInternal.HANUNOO );
        public static UnicodeScript BUHID = new UnicodeScript(UnicodeScriptInternal.BUHID );
        public static UnicodeScript TAGBANWA = new UnicodeScript(UnicodeScriptInternal.TAGBANWA );
        public static UnicodeScript LIMBU = new UnicodeScript(UnicodeScriptInternal.LIMBU );
        public static UnicodeScript TAI_LE = new UnicodeScript(UnicodeScriptInternal.TAI_LE );
        public static UnicodeScript LINEAR_B = new UnicodeScript(UnicodeScriptInternal.LINEAR_B );
        public static UnicodeScript UGARITIC = new UnicodeScript(UnicodeScriptInternal.UGARITIC );
        public static UnicodeScript SHAVIAN = new UnicodeScript(UnicodeScriptInternal.SHAVIAN );
        public static UnicodeScript OSMANYA = new UnicodeScript(UnicodeScriptInternal.OSMANYA );
        public static UnicodeScript CYPRIOT = new UnicodeScript(UnicodeScriptInternal.CYPRIOT );
        public static UnicodeScript BRAILLE = new UnicodeScript(UnicodeScriptInternal.BRAILLE );
        public static UnicodeScript BUGINESE = new UnicodeScript(UnicodeScriptInternal.BUGINESE );
        public static UnicodeScript COPTIC = new UnicodeScript(UnicodeScriptInternal.COPTIC );
        public static UnicodeScript NEW_TAI_LUE = new UnicodeScript(UnicodeScriptInternal.NEW_TAI_LUE );
        public static UnicodeScript GLAGOLITIC = new UnicodeScript(UnicodeScriptInternal.GLAGOLITIC );
        public static UnicodeScript TIFINAGH = new UnicodeScript(UnicodeScriptInternal.TIFINAGH );
        public static UnicodeScript SYLOTI_NAGRI = new UnicodeScript(UnicodeScriptInternal.SYLOTI_NAGRI );
        public static UnicodeScript OLD_PERSIAN = new UnicodeScript(UnicodeScriptInternal.OLD_PERSIAN );
        public static UnicodeScript KHAROSHTHI = new UnicodeScript(UnicodeScriptInternal.KHAROSHTHI );
        public static UnicodeScript BALINESE = new UnicodeScript(UnicodeScriptInternal.BALINESE );
        public static UnicodeScript CUNEIFORM = new UnicodeScript(UnicodeScriptInternal.CUNEIFORM );
        public static UnicodeScript PHOENICIAN = new UnicodeScript(UnicodeScriptInternal.PHOENICIAN );
        public static UnicodeScript PHAGS_PA = new UnicodeScript(UnicodeScriptInternal.PHAGS_PA );
        public static UnicodeScript NKO = new UnicodeScript(UnicodeScriptInternal.NKO );
        public static UnicodeScript SUNDANESE = new UnicodeScript(UnicodeScriptInternal.SUNDANESE );
        public static UnicodeScript BATAK = new UnicodeScript(UnicodeScriptInternal.BATAK );
        public static UnicodeScript LEPCHA = new UnicodeScript(UnicodeScriptInternal.LEPCHA );
        public static UnicodeScript OL_CHIKI = new UnicodeScript(UnicodeScriptInternal.OL_CHIKI );
        public static UnicodeScript VAI = new UnicodeScript(UnicodeScriptInternal.VAI );
        public static UnicodeScript SAURASHTRA = new UnicodeScript(UnicodeScriptInternal.SAURASHTRA );
        public static UnicodeScript KAYAH_LI = new UnicodeScript(UnicodeScriptInternal.KAYAH_LI );
        public static UnicodeScript REJANG = new UnicodeScript(UnicodeScriptInternal.REJANG );
        public static UnicodeScript LYCIAN = new UnicodeScript(UnicodeScriptInternal.LYCIAN );
        public static UnicodeScript CARIAN = new UnicodeScript(UnicodeScriptInternal.CARIAN );
        public static UnicodeScript LYDIAN = new UnicodeScript(UnicodeScriptInternal.LYDIAN );
        public static UnicodeScript CHAM = new UnicodeScript(UnicodeScriptInternal.CHAM );
        public static UnicodeScript TAI_THAM = new UnicodeScript(UnicodeScriptInternal.TAI_THAM );
        public static UnicodeScript TAI_VIET = new UnicodeScript(UnicodeScriptInternal.TAI_VIET );
        public static UnicodeScript AVESTAN = new UnicodeScript(UnicodeScriptInternal.AVESTAN );
        public static UnicodeScript EGYPTIAN_HIEROGLYPHS = new UnicodeScript(UnicodeScriptInternal.EGYPTIAN_HIEROGLYPHS );
        public static UnicodeScript SAMARITAN = new UnicodeScript(UnicodeScriptInternal.SAMARITAN );
        public static UnicodeScript MANDAIC = new UnicodeScript(UnicodeScriptInternal.MANDAIC );
        public static UnicodeScript LISU = new UnicodeScript(UnicodeScriptInternal.LISU );
        public static UnicodeScript BAMUM = new UnicodeScript(UnicodeScriptInternal.BAMUM );
        public static UnicodeScript JAVANESE = new UnicodeScript(UnicodeScriptInternal.JAVANESE );
        public static UnicodeScript MEETEI_MAYEK = new UnicodeScript(UnicodeScriptInternal.MEETEI_MAYEK );
        public static UnicodeScript IMPERIAL_ARAMAIC = new UnicodeScript(UnicodeScriptInternal.IMPERIAL_ARAMAIC );
        public static UnicodeScript OLD_SOUTH_ARABIAN = new UnicodeScript(UnicodeScriptInternal.OLD_SOUTH_ARABIAN );
        public static UnicodeScript INSCRIPTIONAL_PARTHIAN = new UnicodeScript(UnicodeScriptInternal.INSCRIPTIONAL_PARTHIAN );
        public static UnicodeScript INSCRIPTIONAL_PAHLAVI = new UnicodeScript(UnicodeScriptInternal.INSCRIPTIONAL_PAHLAVI );
        public static UnicodeScript OLD_TURKIC = new UnicodeScript(UnicodeScriptInternal.OLD_TURKIC );
        public static UnicodeScript BRAHMI = new UnicodeScript(UnicodeScriptInternal.BRAHMI );
        public static UnicodeScript KAITHI = new UnicodeScript(UnicodeScriptInternal.KAITHI );
        public static UnicodeScript UNKNOWN = new UnicodeScript(UnicodeScriptInternal.UNKNOWN );
        #endregion
        #region private static readonly int[] scriptStarts
        private static readonly int[] scriptStarts = {
            0x0000,   // 0000..0040; COMMON
            0x0041,   // 0041..005A; LATIN
            0x005B,   // 005B..0060; COMMON
            0x0061,   // 0061..007A; LATIN
            0x007B,   // 007B..00A9; COMMON
            0x00AA,   // 00AA..00AA; LATIN
            0x00AB,   // 00AB..00B9; COMMON
            0x00BA,   // 00BA..00BA; LATIN
            0x00BB,   // 00BB..00BF; COMMON
            0x00C0,   // 00C0..00D6; LATIN
            0x00D7,   // 00D7..00D7; COMMON
            0x00D8,   // 00D8..00F6; LATIN
            0x00F7,   // 00F7..00F7; COMMON
            0x00F8,   // 00F8..02B8; LATIN
            0x02B9,   // 02B9..02DF; COMMON
            0x02E0,   // 02E0..02E4; LATIN
            0x02E5,   // 02E5..02E9; COMMON
            0x02EA,   // 02EA..02EB; BOPOMOFO
            0x02EC,   // 02EC..02FF; COMMON
            0x0300,   // 0300..036F; INHERITED
            0x0370,   // 0370..0373; GREEK
            0x0374,   // 0374..0374; COMMON
            0x0375,   // 0375..037D; GREEK
            0x037E,   // 037E..0383; COMMON
            0x0384,   // 0384..0384; GREEK
            0x0385,   // 0385..0385; COMMON
            0x0386,   // 0386..0386; GREEK
            0x0387,   // 0387..0387; COMMON
            0x0388,   // 0388..03E1; GREEK
            0x03E2,   // 03E2..03EF; COPTIC
            0x03F0,   // 03F0..03FF; GREEK
            0x0400,   // 0400..0484; CYRILLIC
            0x0485,   // 0485..0486; INHERITED
            0x0487,   // 0487..0530; CYRILLIC
            0x0531,   // 0531..0588; ARMENIAN
            0x0589,   // 0589..0589; COMMON
            0x058A,   // 058A..0590; ARMENIAN
            0x0591,   // 0591..05FF; HEBREW
            0x0600,   // 0600..060B; ARABIC
            0x060C,   // 060C..060C; COMMON
            0x060D,   // 060D..061A; ARABIC
            0x061B,   // 061B..061D; COMMON
            0x061E,   // 061E..061E; ARABIC
            0x061F,   // 061F..061F; COMMON
            0x0620,   // 0620..063F; ARABIC
            0x0640,   // 0640..0640; COMMON
            0x0641,   // 0641..064A; ARABIC
            0x064B,   // 064B..0655; INHERITED
            0x0656,   // 0656..065E; ARABIC
            0x065F,   // 065F..065F; INHERITED
            0x0660,   // 0660..0669; COMMON
            0x066A,   // 066A..066F; ARABIC
            0x0670,   // 0670..0670; INHERITED
            0x0671,   // 0671..06DC; ARABIC
            0x06DD,   // 06DD..06DD; COMMON
            0x06DE,   // 06DE..06FF; ARABIC
            0x0700,   // 0700..074F; SYRIAC
            0x0750,   // 0750..077F; ARABIC
            0x0780,   // 0780..07BF; THAANA
            0x07C0,   // 07C0..07FF; NKO
            0x0800,   // 0800..083F; SAMARITAN
            0x0840,   // 0840..08FF; MANDAIC
            0x0900,   // 0900..0950; DEVANAGARI
            0x0951,   // 0951..0952; INHERITED
            0x0953,   // 0953..0963; DEVANAGARI
            0x0964,   // 0964..0965; COMMON
            0x0966,   // 0966..096F; DEVANAGARI
            0x0970,   // 0970..0970; COMMON
            0x0971,   // 0971..0980; DEVANAGARI
            0x0981,   // 0981..0A00; BENGALI
            0x0A01,   // 0A01..0A80; GURMUKHI
            0x0A81,   // 0A81..0B00; GUJARATI
            0x0B01,   // 0B01..0B81; ORIYA
            0x0B82,   // 0B82..0C00; TAMIL
            0x0C01,   // 0C01..0C81; TELUGU
            0x0C82,   // 0C82..0CF0; KANNADA
            0x0D02,   // 0D02..0D81; MALAYALAM
            0x0D82,   // 0D82..0E00; SINHALA
            0x0E01,   // 0E01..0E3E; THAI
            0x0E3F,   // 0E3F..0E3F; COMMON
            0x0E40,   // 0E40..0E80; THAI
            0x0E81,   // 0E81..0EFF; LAO
            0x0F00,   // 0F00..0FD4; TIBETAN
            0x0FD5,   // 0FD5..0FD8; COMMON
            0x0FD9,   // 0FD9..0FFF; TIBETAN
            0x1000,   // 1000..109F; MYANMAR
            0x10A0,   // 10A0..10FA; GEORGIAN
            0x10FB,   // 10FB..10FB; COMMON
            0x10FC,   // 10FC..10FF; GEORGIAN
            0x1100,   // 1100..11FF; HANGUL
            0x1200,   // 1200..139F; ETHIOPIC
            0x13A0,   // 13A0..13FF; CHEROKEE
            0x1400,   // 1400..167F; CANADIAN_ABORIGINAL
            0x1680,   // 1680..169F; OGHAM
            0x16A0,   // 16A0..16EA; RUNIC
            0x16EB,   // 16EB..16ED; COMMON
            0x16EE,   // 16EE..16FF; RUNIC
            0x1700,   // 1700..171F; TAGALOG
            0x1720,   // 1720..1734; HANUNOO
            0x1735,   // 1735..173F; COMMON
            0x1740,   // 1740..175F; BUHID
            0x1760,   // 1760..177F; TAGBANWA
            0x1780,   // 1780..17FF; KHMER
            0x1800,   // 1800..1801; MONGOLIAN
            0x1802,   // 1802..1803; COMMON
            0x1804,   // 1804..1804; MONGOLIAN
            0x1805,   // 1805..1805; COMMON
            0x1806,   // 1806..18AF; MONGOLIAN
            0x18B0,   // 18B0..18FF; CANADIAN_ABORIGINAL
            0x1900,   // 1900..194F; LIMBU
            0x1950,   // 1950..197F; TAI_LE
            0x1980,   // 1980..19DF; NEW_TAI_LUE
            0x19E0,   // 19E0..19FF; KHMER
            0x1A00,   // 1A00..1A1F; BUGINESE
            0x1A20,   // 1A20..1AFF; TAI_THAM
            0x1B00,   // 1B00..1B7F; BALINESE
            0x1B80,   // 1B80..1BBF; SUNDANESE
            0x1BC0,   // 1BC0..1BFF; BATAK
            0x1C00,   // 1C00..1C4F; LEPCHA
            0x1C50,   // 1C50..1CCF; OL_CHIKI
            0x1CD0,   // 1CD0..1CD2; INHERITED
            0x1CD3,   // 1CD3..1CD3; COMMON
            0x1CD4,   // 1CD4..1CE0; INHERITED
            0x1CE1,   // 1CE1..1CE1; COMMON
            0x1CE2,   // 1CE2..1CE8; INHERITED
            0x1CE9,   // 1CE9..1CEC; COMMON
            0x1CED,   // 1CED..1CED; INHERITED
            0x1CEE,   // 1CEE..1CFF; COMMON
            0x1D00,   // 1D00..1D25; LATIN
            0x1D26,   // 1D26..1D2A; GREEK
            0x1D2B,   // 1D2B..1D2B; CYRILLIC
            0x1D2C,   // 1D2C..1D5C; LATIN
            0x1D5D,   // 1D5D..1D61; GREEK
            0x1D62,   // 1D62..1D65; LATIN
            0x1D66,   // 1D66..1D6A; GREEK
            0x1D6B,   // 1D6B..1D77; LATIN
            0x1D78,   // 1D78..1D78; CYRILLIC
            0x1D79,   // 1D79..1DBE; LATIN
            0x1DBF,   // 1DBF..1DBF; GREEK
            0x1DC0,   // 1DC0..1DFF; INHERITED
            0x1E00,   // 1E00..1EFF; LATIN
            0x1F00,   // 1F00..1FFF; GREEK
            0x2000,   // 2000..200B; COMMON
            0x200C,   // 200C..200D; INHERITED
            0x200E,   // 200E..2070; COMMON
            0x2071,   // 2071..2073; LATIN
            0x2074,   // 2074..207E; COMMON
            0x207F,   // 207F..207F; LATIN
            0x2080,   // 2080..208F; COMMON
            0x2090,   // 2090..209F; LATIN
            0x20A0,   // 20A0..20CF; COMMON
            0x20D0,   // 20D0..20FF; INHERITED
            0x2100,   // 2100..2125; COMMON
            0x2126,   // 2126..2126; GREEK
            0x2127,   // 2127..2129; COMMON
            0x212A,   // 212A..212B; LATIN
            0x212C,   // 212C..2131; COMMON
            0x2132,   // 2132..2132; LATIN
            0x2133,   // 2133..214D; COMMON
            0x214E,   // 214E..214E; LATIN
            0x214F,   // 214F..215F; COMMON
            0x2160,   // 2160..2188; LATIN
            0x2189,   // 2189..27FF; COMMON
            0x2800,   // 2800..28FF; BRAILLE
            0x2900,   // 2900..2BFF; COMMON
            0x2C00,   // 2C00..2C5F; GLAGOLITIC
            0x2C60,   // 2C60..2C7F; LATIN
            0x2C80,   // 2C80..2CFF; COPTIC
            0x2D00,   // 2D00..2D2F; GEORGIAN
            0x2D30,   // 2D30..2D7F; TIFINAGH
            0x2D80,   // 2D80..2DDF; ETHIOPIC
            0x2DE0,   // 2DE0..2DFF; CYRILLIC
            0x2E00,   // 2E00..2E7F; COMMON
            0x2E80,   // 2E80..2FEF; HAN
            0x2FF0,   // 2FF0..3004; COMMON
            0x3005,   // 3005..3005; HAN
            0x3006,   // 3006..3006; COMMON
            0x3007,   // 3007..3007; HAN
            0x3008,   // 3008..3020; COMMON
            0x3021,   // 3021..3029; HAN
            0x302A,   // 302A..302D; INHERITED
            0x302E,   // 302E..302F; HANGUL
            0x3030,   // 3030..3037; COMMON
            0x3038,   // 3038..303B; HAN
            0x303C,   // 303C..3040; COMMON
            0x3041,   // 3041..3098; HIRAGANA
            0x3099,   // 3099..309A; INHERITED
            0x309B,   // 309B..309C; COMMON
            0x309D,   // 309D..309F; HIRAGANA
            0x30A0,   // 30A0..30A0; COMMON
            0x30A1,   // 30A1..30FA; KATAKANA
            0x30FB,   // 30FB..30FC; COMMON
            0x30FD,   // 30FD..3104; KATAKANA
            0x3105,   // 3105..3130; BOPOMOFO
            0x3131,   // 3131..318F; HANGUL
            0x3190,   // 3190..319F; COMMON
            0x31A0,   // 31A0..31BF; BOPOMOFO
            0x31C0,   // 31C0..31EF; COMMON
            0x31F0,   // 31F0..31FF; KATAKANA
            0x3200,   // 3200..321F; HANGUL
            0x3220,   // 3220..325F; COMMON
            0x3260,   // 3260..327E; HANGUL
            0x327F,   // 327F..32CF; COMMON
            0x32D0,   // 32D0..3357; KATAKANA
            0x3358,   // 3358..33FF; COMMON
            0x3400,   // 3400..4DBF; HAN
            0x4DC0,   // 4DC0..4DFF; COMMON
            0x4E00,   // 4E00..9FFF; HAN
            0xA000,   // A000..A4CF; YI
            0xA4D0,   // A4D0..A4FF; LISU
            0xA500,   // A500..A63F; VAI
            0xA640,   // A640..A69F; CYRILLIC
            0xA6A0,   // A6A0..A6FF; BAMUM
            0xA700,   // A700..A721; COMMON
            0xA722,   // A722..A787; LATIN
            0xA788,   // A788..A78A; COMMON
            0xA78B,   // A78B..A7FF; LATIN
            0xA800,   // A800..A82F; SYLOTI_NAGRI
            0xA830,   // A830..A83F; COMMON
            0xA840,   // A840..A87F; PHAGS_PA
            0xA880,   // A880..A8DF; SAURASHTRA
            0xA8E0,   // A8E0..A8FF; DEVANAGARI
            0xA900,   // A900..A92F; KAYAH_LI
            0xA930,   // A930..A95F; REJANG
            0xA960,   // A960..A97F; HANGUL
            0xA980,   // A980..A9FF; JAVANESE
            0xAA00,   // AA00..AA5F; CHAM
            0xAA60,   // AA60..AA7F; MYANMAR
            0xAA80,   // AA80..AB00; TAI_VIET
            0xAB01,   // AB01..ABBF; ETHIOPIC
            0xABC0,   // ABC0..ABFF; MEETEI_MAYEK
            0xAC00,   // AC00..D7FB; HANGUL
            0xD7FC,   // D7FC..F8FF; UNKNOWN
            0xF900,   // F900..FAFF; HAN
            0xFB00,   // FB00..FB12; LATIN
            0xFB13,   // FB13..FB1C; ARMENIAN
            0xFB1D,   // FB1D..FB4F; HEBREW
            0xFB50,   // FB50..FD3D; ARABIC
            0xFD3E,   // FD3E..FD4F; COMMON
            0xFD50,   // FD50..FDFC; ARABIC
            0xFDFD,   // FDFD..FDFF; COMMON
            0xFE00,   // FE00..FE0F; INHERITED
            0xFE10,   // FE10..FE1F; COMMON
            0xFE20,   // FE20..FE2F; INHERITED
            0xFE30,   // FE30..FE6F; COMMON
            0xFE70,   // FE70..FEFE; ARABIC
            0xFEFF,   // FEFF..FF20; COMMON
            0xFF21,   // FF21..FF3A; LATIN
            0xFF3B,   // FF3B..FF40; COMMON
            0xFF41,   // FF41..FF5A; LATIN
            0xFF5B,   // FF5B..FF65; COMMON
            0xFF66,   // FF66..FF6F; KATAKANA
            0xFF70,   // FF70..FF70; COMMON
            0xFF71,   // FF71..FF9D; KATAKANA
            0xFF9E,   // FF9E..FF9F; COMMON
            0xFFA0,   // FFA0..FFDF; HANGUL
            0xFFE0,   // FFE0..FFFF; COMMON
            0x10000,  // 10000..100FF; LINEAR_B
            0x10100,  // 10100..1013F; COMMON
            0x10140,  // 10140..1018F; GREEK
            0x10190,  // 10190..101FC; COMMON
            0x101FD,  // 101FD..1027F; INHERITED
            0x10280,  // 10280..1029F; LYCIAN
            0x102A0,  // 102A0..102FF; CARIAN
            0x10300,  // 10300..1032F; OLD_ITALIC
            0x10330,  // 10330..1037F; GOTHIC
            0x10380,  // 10380..1039F; UGARITIC
            0x103A0,  // 103A0..103FF; OLD_PERSIAN
            0x10400,  // 10400..1044F; DESERET
            0x10450,  // 10450..1047F; SHAVIAN
            0x10480,  // 10480..107FF; OSMANYA
            0x10800,  // 10800..1083F; CYPRIOT
            0x10840,  // 10840..108FF; IMPERIAL_ARAMAIC
            0x10900,  // 10900..1091F; PHOENICIAN
            0x10920,  // 10920..109FF; LYDIAN
            0x10A00,  // 10A00..10A5F; KHAROSHTHI
            0x10A60,  // 10A60..10AFF; OLD_SOUTH_ARABIAN
            0x10B00,  // 10B00..10B3F; AVESTAN
            0x10B40,  // 10B40..10B5F; INSCRIPTIONAL_PARTHIAN
            0x10B60,  // 10B60..10BFF; INSCRIPTIONAL_PAHLAVI
            0x10C00,  // 10C00..10E5F; OLD_TURKIC
            0x10E60,  // 10E60..10FFF; ARABIC
            0x11000,  // 11000..1107F; BRAHMI
            0x11080,  // 11080..11FFF; KAITHI
            0x12000,  // 12000..12FFF; CUNEIFORM
            0x13000,  // 13000..167FF; EGYPTIAN_HIEROGLYPHS
            0x16800,  // 16800..16A38; BAMUM
            0x1B000,  // 1B000..1B000; KATAKANA
            0x1B001,  // 1B001..1CFFF; HIRAGANA
            0x1D000,  // 1D000..1D166; COMMON
            0x1D167,  // 1D167..1D169; INHERITED
            0x1D16A,  // 1D16A..1D17A; COMMON
            0x1D17B,  // 1D17B..1D182; INHERITED
            0x1D183,  // 1D183..1D184; COMMON
            0x1D185,  // 1D185..1D18B; INHERITED
            0x1D18C,  // 1D18C..1D1A9; COMMON
            0x1D1AA,  // 1D1AA..1D1AD; INHERITED
            0x1D1AE,  // 1D1AE..1D1FF; COMMON
            0x1D200,  // 1D200..1D2FF; GREEK
            0x1D300,  // 1D300..1F1FF; COMMON
            0x1F200,  // 1F200..1F200; HIRAGANA
            0x1F201,  // 1F210..1FFFF; COMMON
            0x20000,  // 20000..E0000; HAN
            0xE0001,  // E0001..E00FF; COMMON
            0xE0100,  // E0100..E01EF; INHERITED
            0xE01F0   // E01F0..10FFFF; UNKNOWN
        };
        #endregion
        #region private static readonly UnicodeScript[] scripts
        private static readonly UnicodeScript[] scripts = {
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.BOPOMOFO,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.GREEK,
            UnicodeScript.COMMON,
            UnicodeScript.GREEK,
            UnicodeScript.COMMON,
            UnicodeScript.GREEK,
            UnicodeScript.COMMON,
            UnicodeScript.GREEK,
            UnicodeScript.COMMON,
            UnicodeScript.GREEK,
            UnicodeScript.COPTIC,
            UnicodeScript.GREEK,
            UnicodeScript.CYRILLIC,
            UnicodeScript.INHERITED,
            UnicodeScript.CYRILLIC,
            UnicodeScript.ARMENIAN,
            UnicodeScript.COMMON,
            UnicodeScript.ARMENIAN,
            UnicodeScript.HEBREW,
            UnicodeScript.ARABIC,
            UnicodeScript.COMMON,
            UnicodeScript.ARABIC,
            UnicodeScript.COMMON,
            UnicodeScript.ARABIC,
            UnicodeScript.COMMON,
            UnicodeScript.ARABIC,
            UnicodeScript.COMMON,
            UnicodeScript.ARABIC,
            UnicodeScript.INHERITED,
            UnicodeScript.ARABIC,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.ARABIC,
            UnicodeScript.INHERITED,
            UnicodeScript.ARABIC,
            UnicodeScript.COMMON,
            UnicodeScript.ARABIC,
            UnicodeScript.SYRIAC,
            UnicodeScript.ARABIC,
            UnicodeScript.THAANA,
            UnicodeScript.NKO,
            UnicodeScript.SAMARITAN,
            UnicodeScript.MANDAIC,
            UnicodeScript.DEVANAGARI,
            UnicodeScript.INHERITED,
            UnicodeScript.DEVANAGARI,
            UnicodeScript.COMMON,
            UnicodeScript.DEVANAGARI,
            UnicodeScript.COMMON,
            UnicodeScript.DEVANAGARI,
            UnicodeScript.BENGALI,
            UnicodeScript.GURMUKHI,
            UnicodeScript.GUJARATI,
            UnicodeScript.ORIYA,
            UnicodeScript.TAMIL,
            UnicodeScript.TELUGU,
            UnicodeScript.KANNADA,
            UnicodeScript.MALAYALAM,
            UnicodeScript.SINHALA,
            UnicodeScript.THAI,
            UnicodeScript.COMMON,
            UnicodeScript.THAI,
            UnicodeScript.LAO,
            UnicodeScript.TIBETAN,
            UnicodeScript.COMMON,
            UnicodeScript.TIBETAN,
            UnicodeScript.MYANMAR,
            UnicodeScript.GEORGIAN,
            UnicodeScript.COMMON,
            UnicodeScript.GEORGIAN,
            UnicodeScript.HANGUL,
            UnicodeScript.ETHIOPIC,
            UnicodeScript.CHEROKEE,
            UnicodeScript.CANADIAN_ABORIGINAL,
            UnicodeScript.OGHAM,
            UnicodeScript.RUNIC,
            UnicodeScript.COMMON,
            UnicodeScript.RUNIC,
            UnicodeScript.TAGALOG,
            UnicodeScript.HANUNOO,
            UnicodeScript.COMMON,
            UnicodeScript.BUHID,
            UnicodeScript.TAGBANWA,
            UnicodeScript.KHMER,
            UnicodeScript.MONGOLIAN,
            UnicodeScript.COMMON,
            UnicodeScript.MONGOLIAN,
            UnicodeScript.COMMON,
            UnicodeScript.MONGOLIAN,
            UnicodeScript.CANADIAN_ABORIGINAL,
            UnicodeScript.LIMBU,
            UnicodeScript.TAI_LE,
            UnicodeScript.NEW_TAI_LUE,
            UnicodeScript.KHMER,
            UnicodeScript.BUGINESE,
            UnicodeScript.TAI_THAM,
            UnicodeScript.BALINESE,
            UnicodeScript.SUNDANESE,
            UnicodeScript.BATAK,
            UnicodeScript.LEPCHA,
            UnicodeScript.OL_CHIKI,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.GREEK,
            UnicodeScript.CYRILLIC,
            UnicodeScript.LATIN,
            UnicodeScript.GREEK,
            UnicodeScript.LATIN,
            UnicodeScript.GREEK,
            UnicodeScript.LATIN,
            UnicodeScript.CYRILLIC,
            UnicodeScript.LATIN,
            UnicodeScript.GREEK,
            UnicodeScript.INHERITED,
            UnicodeScript.LATIN,
            UnicodeScript.GREEK,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.GREEK,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.BRAILLE,
            UnicodeScript.COMMON,
            UnicodeScript.GLAGOLITIC,
            UnicodeScript.LATIN,
            UnicodeScript.COPTIC,
            UnicodeScript.GEORGIAN,
            UnicodeScript.TIFINAGH,
            UnicodeScript.ETHIOPIC,
            UnicodeScript.CYRILLIC,
            UnicodeScript.COMMON,
            UnicodeScript.HAN,
            UnicodeScript.COMMON,
            UnicodeScript.HAN,
            UnicodeScript.COMMON,
            UnicodeScript.HAN,
            UnicodeScript.COMMON,
            UnicodeScript.HAN,
            UnicodeScript.INHERITED,
            UnicodeScript.HANGUL,
            UnicodeScript.COMMON,
            UnicodeScript.HAN,
            UnicodeScript.COMMON,
            UnicodeScript.HIRAGANA,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.HIRAGANA,
            UnicodeScript.COMMON,
            UnicodeScript.KATAKANA,
            UnicodeScript.COMMON,
            UnicodeScript.KATAKANA,
            UnicodeScript.BOPOMOFO,
            UnicodeScript.HANGUL,
            UnicodeScript.COMMON,
            UnicodeScript.BOPOMOFO,
            UnicodeScript.COMMON,
            UnicodeScript.KATAKANA,
            UnicodeScript.HANGUL,
            UnicodeScript.COMMON,
            UnicodeScript.HANGUL,
            UnicodeScript.COMMON,
            UnicodeScript.KATAKANA,
            UnicodeScript.COMMON,
            UnicodeScript.HAN,
            UnicodeScript.COMMON,
            UnicodeScript.HAN,
            UnicodeScript.YI,
            UnicodeScript.LISU,
            UnicodeScript.VAI,
            UnicodeScript.CYRILLIC,
            UnicodeScript.BAMUM,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.SYLOTI_NAGRI,
            UnicodeScript.COMMON,
            UnicodeScript.PHAGS_PA,
            UnicodeScript.SAURASHTRA,
            UnicodeScript.DEVANAGARI,
            UnicodeScript.KAYAH_LI,
            UnicodeScript.REJANG,
            UnicodeScript.HANGUL,
            UnicodeScript.JAVANESE,
            UnicodeScript.CHAM,
            UnicodeScript.MYANMAR,
            UnicodeScript.TAI_VIET,
            UnicodeScript.ETHIOPIC,
            UnicodeScript.MEETEI_MAYEK,
            UnicodeScript.HANGUL,
            UnicodeScript.UNKNOWN,
            UnicodeScript.HAN,
            UnicodeScript.LATIN,
            UnicodeScript.ARMENIAN,
            UnicodeScript.HEBREW,
            UnicodeScript.ARABIC,
            UnicodeScript.COMMON,
            UnicodeScript.ARABIC,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.ARABIC,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.LATIN,
            UnicodeScript.COMMON,
            UnicodeScript.KATAKANA,
            UnicodeScript.COMMON,
            UnicodeScript.KATAKANA,
            UnicodeScript.COMMON,
            UnicodeScript.HANGUL,
            UnicodeScript.COMMON,
            UnicodeScript.LINEAR_B,
            UnicodeScript.COMMON,
            UnicodeScript.GREEK,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.LYCIAN,
            UnicodeScript.CARIAN,
            UnicodeScript.OLD_ITALIC,
            UnicodeScript.GOTHIC,
            UnicodeScript.UGARITIC,
            UnicodeScript.OLD_PERSIAN,
            UnicodeScript.DESERET,
            UnicodeScript.SHAVIAN,
            UnicodeScript.OSMANYA,
            UnicodeScript.CYPRIOT,
            UnicodeScript.IMPERIAL_ARAMAIC,
            UnicodeScript.PHOENICIAN,
            UnicodeScript.LYDIAN,
            UnicodeScript.KHAROSHTHI,
            UnicodeScript.OLD_SOUTH_ARABIAN,
            UnicodeScript.AVESTAN,
            UnicodeScript.INSCRIPTIONAL_PARTHIAN,
            UnicodeScript.INSCRIPTIONAL_PAHLAVI,
            UnicodeScript.OLD_TURKIC,
            UnicodeScript.ARABIC,
            UnicodeScript.BRAHMI,
            UnicodeScript.KAITHI,
            UnicodeScript.CUNEIFORM,
            UnicodeScript.EGYPTIAN_HIEROGLYPHS,
            UnicodeScript.BAMUM,
            UnicodeScript.KATAKANA,
            UnicodeScript.HIRAGANA,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.COMMON,
            UnicodeScript.GREEK,
            UnicodeScript.COMMON,
            UnicodeScript.HIRAGANA,
            UnicodeScript.COMMON,
            UnicodeScript.HAN,
            UnicodeScript.COMMON,
            UnicodeScript.INHERITED,
            UnicodeScript.UNKNOWN
        };
        #endregion
        private static HashMap<String, UnicodeScript> aliases;
        static UnicodeScript()
        {
            aliases = new HashMap<String, UnicodeScript>(128);
            aliases.put("ARAB", UnicodeScript.ARABIC);
            aliases.put("ARMI", UnicodeScript.IMPERIAL_ARAMAIC);
            aliases.put("ARMN", UnicodeScript.ARMENIAN);
            aliases.put("AVST", UnicodeScript.AVESTAN);
            aliases.put("BALI", UnicodeScript.BALINESE);
            aliases.put("BAMU", UnicodeScript.BAMUM);
            aliases.put("BATK", UnicodeScript.BATAK);
            aliases.put("BENG", UnicodeScript.BENGALI);
            aliases.put("BOPO", UnicodeScript.BOPOMOFO);
            aliases.put("BRAI", UnicodeScript.BRAILLE);
            aliases.put("BRAH", UnicodeScript.BRAHMI);
            aliases.put("BUGI", UnicodeScript.BUGINESE);
            aliases.put("BUHD", UnicodeScript.BUHID);
            aliases.put("CANS", UnicodeScript.CANADIAN_ABORIGINAL);
            aliases.put("CARI", UnicodeScript.CARIAN);
            aliases.put("CHAM", UnicodeScript.CHAM);
            aliases.put("CHER", UnicodeScript.CHEROKEE);
            aliases.put("COPT", UnicodeScript.COPTIC);
            aliases.put("CPRT", UnicodeScript.CYPRIOT);
            aliases.put("CYRL", UnicodeScript.CYRILLIC);
            aliases.put("DEVA", UnicodeScript.DEVANAGARI);
            aliases.put("DSRT", UnicodeScript.DESERET);
            aliases.put("EGYP", UnicodeScript.EGYPTIAN_HIEROGLYPHS);
            aliases.put("ETHI", UnicodeScript.ETHIOPIC);
            aliases.put("GEOR", UnicodeScript.GEORGIAN);
            aliases.put("GLAG", UnicodeScript.GLAGOLITIC);
            aliases.put("GOTH", UnicodeScript.GOTHIC);
            aliases.put("GREK", UnicodeScript.GREEK);
            aliases.put("GUJR", UnicodeScript.GUJARATI);
            aliases.put("GURU", UnicodeScript.GURMUKHI);
            aliases.put("HANG", UnicodeScript.HANGUL);
            aliases.put("HANI", UnicodeScript.HAN);
            aliases.put("HANO", UnicodeScript.HANUNOO);
            aliases.put("HEBR", UnicodeScript.HEBREW);
            aliases.put("HIRA", UnicodeScript.HIRAGANA);
            // it appears we don't have the KATAKANA_OR_HIRAGANA
            //aliases.put("HRKT", UnicodeScript.KATAKANA_OR_HIRAGANA);
            aliases.put("ITAL", UnicodeScript.OLD_ITALIC);
            aliases.put("JAVA", UnicodeScript.JAVANESE);
            aliases.put("KALI", UnicodeScript.KAYAH_LI);
            aliases.put("KANA", UnicodeScript.KATAKANA);
            aliases.put("KHAR", UnicodeScript.KHAROSHTHI);
            aliases.put("KHMR", UnicodeScript.KHMER);
            aliases.put("KNDA", UnicodeScript.KANNADA);
            aliases.put("KTHI", UnicodeScript.KAITHI);
            aliases.put("LANA", UnicodeScript.TAI_THAM);
            aliases.put("LAOO", UnicodeScript.LAO);
            aliases.put("LATN", UnicodeScript.LATIN);
            aliases.put("LEPC", UnicodeScript.LEPCHA);
            aliases.put("LIMB", UnicodeScript.LIMBU);
            aliases.put("LINB", UnicodeScript.LINEAR_B);
            aliases.put("LISU", UnicodeScript.LISU);
            aliases.put("LYCI", UnicodeScript.LYCIAN);
            aliases.put("LYDI", UnicodeScript.LYDIAN);
            aliases.put("MAND", UnicodeScript.MANDAIC);
            aliases.put("MLYM", UnicodeScript.MALAYALAM);
            aliases.put("MONG", UnicodeScript.MONGOLIAN);
            aliases.put("MTEI", UnicodeScript.MEETEI_MAYEK);
            aliases.put("MYMR", UnicodeScript.MYANMAR);
            aliases.put("NKOO", UnicodeScript.NKO);
            aliases.put("OGAM", UnicodeScript.OGHAM);
            aliases.put("OLCK", UnicodeScript.OL_CHIKI);
            aliases.put("ORKH", UnicodeScript.OLD_TURKIC);
            aliases.put("ORYA", UnicodeScript.ORIYA);
            aliases.put("OSMA", UnicodeScript.OSMANYA);
            aliases.put("PHAG", UnicodeScript.PHAGS_PA);
            aliases.put("PHLI", UnicodeScript.INSCRIPTIONAL_PAHLAVI);
            aliases.put("PHNX", UnicodeScript.PHOENICIAN);
            aliases.put("PRTI", UnicodeScript.INSCRIPTIONAL_PARTHIAN);
            aliases.put("RJNG", UnicodeScript.REJANG);
            aliases.put("RUNR", UnicodeScript.RUNIC);
            aliases.put("SAMR", UnicodeScript.SAMARITAN);
            aliases.put("SARB", UnicodeScript.OLD_SOUTH_ARABIAN);
            aliases.put("SAUR", UnicodeScript.SAURASHTRA);
            aliases.put("SHAW", UnicodeScript.SHAVIAN);
            aliases.put("SINH", UnicodeScript.SINHALA);
            aliases.put("SUND", UnicodeScript.SUNDANESE);
            aliases.put("SYLO", UnicodeScript.SYLOTI_NAGRI);
            aliases.put("SYRC", UnicodeScript.SYRIAC);
            aliases.put("TAGB", UnicodeScript.TAGBANWA);
            aliases.put("TALE", UnicodeScript.TAI_LE);
            aliases.put("TALU", UnicodeScript.NEW_TAI_LUE);
            aliases.put("TAML", UnicodeScript.TAMIL);
            aliases.put("TAVT", UnicodeScript.TAI_VIET);
            aliases.put("TELU", UnicodeScript.TELUGU);
            aliases.put("TFNG", UnicodeScript.TIFINAGH);
            aliases.put("TGLG", UnicodeScript.TAGALOG);
            aliases.put("THAA", UnicodeScript.THAANA);
            aliases.put("THAI", UnicodeScript.THAI);
            aliases.put("TIBT", UnicodeScript.TIBETAN);
            aliases.put("UGAR", UnicodeScript.UGARITIC);
            aliases.put("VAII", UnicodeScript.VAI);
            aliases.put("XPEO", UnicodeScript.OLD_PERSIAN);
            aliases.put("XSUX", UnicodeScript.CUNEIFORM);
            aliases.put("YIII", UnicodeScript.YI);
            aliases.put("ZINH", UnicodeScript.INHERITED);
            aliases.put("ZYYY", UnicodeScript.COMMON);
            aliases.put("ZZZZ", UnicodeScript.UNKNOWN);
        }
        public static UnicodeScript of(int codePoint) {
            if (!isValidCodePoint(codePoint))
                throw new IllegalArgumentException();
            int type = getType(codePoint);
            // leave SURROGATE and PRIVATE_USE for table lookup
            if (type == UNASSIGNED)
                return UnicodeScript.UNKNOWN;
            int index = Array.BinarySearch(scriptStarts, codePoint);
            if (index < 0)
                index = -index - 2;
            return scripts[index];
        }
        public static UnicodeScript forName(String scriptName) {
            scriptName = scriptName.toUpperCase();
                                 //.replace(' ', '_'));
            UnicodeScript sc = aliases.get(scriptName);
            if (sc != null)
                return sc;
            return valueOf(scriptName);
        }

        private static UnicodeScript valueOf(String scriptName) {
            throw new NotImplementedException();
        }
    }
    private readonly char value;

    public Character(char value) {
        this.value = value;
    }

    private class CharacterCache {
        private CharacterCache(){}
        internal static readonly Character[] cache = new Character[127 + 1];
        static CharacterCache() {
            for (int i = 0; i < cache.Length; i++)
                cache[i] = new Character((char)i);
        }
    }

    public static Character valueOf(char c) {
        if (c <= 127) { // must cache
            return CharacterCache.cache[(int)c];
        }
        return new Character(c);
    }
    public char charValue() {
        return value;
    }
    public int hashCode() {
        return (int)value;
    }
    public boolean equals(Object obj) {
        if (obj is Character) {
            return value == ((Character)obj).charValue();
        }
        return false;
    }
    public String toString() {
        char[] buf = {value};
        return String.valueOf(buf);
    }
    public static String toString(char c) {
        return String.valueOf(c);
    }
    public static boolean isValidCodePoint(int codePoint) {
        // Optimized form of:
        //     codePoint >= MIN_CODE_POINT && codePoint <= MAX_CODE_POINT
        int plane = (int)((uint)codePoint >> 16);
        return plane < (((uint)(MAX_CODE_POINT + 1) >> 16));
    }
    public static boolean isBmpCodePoint(int codePoint) {
        return (int)((uint)codePoint >> 16) == 0;
        // Optimized form of:
        //     codePoint >= MIN_VALUE && codePoint <= MAX_VALUE
        // We consistently use logical shift (>>>) to facilitate
        // additional runtime optimizations.
    }
    public static boolean isSupplementaryCodePoint(int codePoint) {
        return codePoint >= MIN_SUPPLEMENTARY_CODE_POINT
            && codePoint <  MAX_CODE_POINT + 1;
    }
    public static boolean isHighSurrogate(char ch) {
        // Help VM constant-fold; MAX_HIGH_SURROGATE + 1 == MIN_LOW_SURROGATE
        return ch >= MIN_HIGH_SURROGATE && ch < (MAX_HIGH_SURROGATE + 1);
    }
    public static boolean isLowSurrogate(char ch) {
        return ch >= MIN_LOW_SURROGATE && ch < (MAX_LOW_SURROGATE + 1);
    }
    public static boolean isSurrogate(char ch) {
        return ch >= MIN_SURROGATE && ch < (MAX_SURROGATE + 1);
    }
    public static boolean isSurrogatePair(char high, char low) {
        return isHighSurrogate(high) && isLowSurrogate(low);
    }
    public static int charCount(int codePoint) {
        return codePoint >= MIN_SUPPLEMENTARY_CODE_POINT ? 2 : 1;
    }
    public static int toCodePoint(char high, char low) {
        // Optimized form of:
        // return ((high - MIN_HIGH_SURROGATE) << 10)
        //         + (low - MIN_LOW_SURROGATE)
        //         + MIN_SUPPLEMENTARY_CODE_POINT;
        return ((high << 10) + low) + (MIN_SUPPLEMENTARY_CODE_POINT
                                       - (MIN_HIGH_SURROGATE << 10)
                                       - MIN_LOW_SURROGATE);
    }
    public static int codePointAt(CharSequence seq, int index) {
        char c1 = seq.charAt(index++);
        if (isHighSurrogate(c1)) {
            if (index < seq.length()) {
                char c2 = seq.charAt(index);
                if (isLowSurrogate(c2)) {
                    return toCodePoint(c1, c2);
                }
            }
        }
        return c1;
    }
    public static int codePointAt(char[] a, int index) {
        return codePointAtImpl(a, index, a.Length);
    }
    public static int codePointAt(char[] a, int index, int limit) {
        if (index >= limit || limit < 0 || limit > a.Length) {
            throw new IndexOutOfRangeException();
        }
        return codePointAtImpl(a, index, limit);
    }

    internal static int codePointAtImpl(char[] a, int index, int limit) {
        char c1 = a[index++];
        if (isHighSurrogate(c1)) {
            if (index < limit) {
                char c2 = a[index];
                if (isLowSurrogate(c2)) {
                    return toCodePoint(c1, c2);
                }
            }
        }
        return c1;
    }
    public static int codePointBefore(CharSequence seq, int index) {
        char c2 = seq.charAt(--index);
        if (isLowSurrogate(c2)) {
            if (index > 0) {
                char c1 = seq.charAt(--index);
                if (isHighSurrogate(c1)) {
                    return toCodePoint(c1, c2);
                }
            }
        }
        return c2;
    }
    public static int codePointBefore(char[] a, int index) {
        return codePointBeforeImpl(a, index, 0);
    }
    public static int codePointBefore(char[] a, int index, int start) {
        if (index <= start || start < 0 || start >= a.Length) {
            throw new IndexOutOfRangeException();
        }
        return codePointBeforeImpl(a, index, start);
    }

    internal static int codePointBeforeImpl(char[] a, int index, int start) {
        char c2 = a[--index];
        if (isLowSurrogate(c2)) {
            if (index > start) {
                char c1 = a[--index];
                if (isHighSurrogate(c1)) {
                    return toCodePoint(c1, c2);
                }
            }
        }
        return c2;
    }
    public static char highSurrogate(int codePoint) {
        return (char) ((int)((uint)codePoint >> 10)
            + (MIN_HIGH_SURROGATE - (int)((uint)MIN_SUPPLEMENTARY_CODE_POINT >> 10)));
    }
    public static char lowSurrogate(int codePoint) {
        return (char) ((codePoint & 0x3ff) + MIN_LOW_SURROGATE);
    }
    public static int toChars(int codePoint, char[] dst, int dstIndex) {
        if (isBmpCodePoint(codePoint)) {
            dst[dstIndex] = (char) codePoint;
            return 1;
        } else if (isValidCodePoint(codePoint)) {
            toSurrogates(codePoint, dst, dstIndex);
            return 2;
        } else {
            throw new IllegalArgumentException();
        }
    }
    public static char[] toChars(int codePoint) {
        if (isBmpCodePoint(codePoint)) {
            return new char[] { (char) codePoint };
        } else if (isValidCodePoint(codePoint)) {
            char[] result = new char[2];
            toSurrogates(codePoint, result, 0);
            return result;
        } else {
            throw new IllegalArgumentException();
        }
    }
    internal static void toSurrogates(int codePoint, char[] dst, int index) {
        // We write elements "backwards" to guarantee all-or-nothing
        dst[index+1] = lowSurrogate(codePoint);
        dst[index] = highSurrogate(codePoint);
    }
    public static int codePointCount(CharSequence seq, int beginIndex, int endIndex) {
        int length = seq.length();
        if (beginIndex < 0 || endIndex > length || beginIndex > endIndex) {
            throw new IndexOutOfRangeException();
        }
        int n = endIndex - beginIndex;
        for (int i = beginIndex; i < endIndex; ) {
            if (isHighSurrogate(seq.charAt(i++)) && i < endIndex &&
                isLowSurrogate(seq.charAt(i))) {
                n--;
                i++;
            }
        }
        return n;
    }
    public static int codePointCount(char[] a, int offset, int count) {
        if (count > a.Length - offset || offset < 0 || count < 0) {
            throw new IndexOutOfRangeException();
        }
        return codePointCountImpl(a, offset, count);
    }
    internal static int codePointCountImpl(char[] a, int offset, int count) {
        int endIndex = offset + count;
        int n = count;
        for (int i = offset; i < endIndex; ) {
            if (isHighSurrogate(a[i++]) && i < endIndex &&
                isLowSurrogate(a[i])) {
                n--;
                i++;
            }
        }
        return n;
    }
    public static int offsetByCodePoints(CharSequence seq, int index,
                                         int codePointOffset) {
        int length = seq.length();
        if (index < 0 || index > length) {
            throw new IndexOutOfRangeException();
        }
        int x = index;
        if (codePointOffset >= 0) {
            int i;
            for (i = 0; x < length && i < codePointOffset; i++) {
                if (isHighSurrogate(seq.charAt(x++)) && x < length &&
                    isLowSurrogate(seq.charAt(x))) {
                    x++;
                }
            }
            if (i < codePointOffset) {
                throw new IndexOutOfRangeException();
            }
        } else {
            int i;
            for (i = codePointOffset; x > 0 && i < 0; i++) {
                if (isLowSurrogate(seq.charAt(--x)) && x > 0 &&
                    isHighSurrogate(seq.charAt(x-1))) {
                    x--;
                }
            }
            if (i < 0) {
                throw new IndexOutOfRangeException();
            }
        }
        return x;
    }
    public static int offsetByCodePoints(char[] a, int start, int count,
                                         int index, int codePointOffset) {
        if (count > a.Length-start || start < 0 || count < 0
            || index < start || index > start+count) {
            throw new IndexOutOfRangeException();
        }
        return offsetByCodePointsImpl(a, start, count, index, codePointOffset);
    }
    internal static int offsetByCodePointsImpl(char[]a, int start, int count,
                                      int index, int codePointOffset) {
        int x = index;
        if (codePointOffset >= 0) {
            int limit = start + count;
            int i;
            for (i = 0; x < limit && i < codePointOffset; i++) {
                if (isHighSurrogate(a[x++]) && x < limit &&
                    isLowSurrogate(a[x])) {
                    x++;
                }
            }
            if (i < codePointOffset) {
                throw new IndexOutOfRangeException();
            }
        } else {
            int i;
            for (i = codePointOffset; x > start && i < 0; i++) {
                if (isLowSurrogate(a[--x]) && x > start &&
                    isHighSurrogate(a[x-1])) {
                    x--;
                }
            }
            if (i < 0) {
                throw new IndexOutOfRangeException();
            }
        }
        return x;
    }
    public static boolean isLowerCase(char ch) {
        return isLowerCase((int)ch);
    }
    public static boolean isLowerCase(int codePoint) {
        return getType(codePoint) == Character.LOWERCASE_LETTER ||
               CharacterData.of(codePoint).isOtherLowercase(codePoint);
    }
    public static boolean isUpperCase(char ch) {
        return isUpperCase((int)ch);
    }
    public static boolean isUpperCase(int codePoint) {
        return getType(codePoint) == Character.UPPERCASE_LETTER ||
               CharacterData.of(codePoint).isOtherUppercase(codePoint);
    }
    public static boolean isTitleCase(char ch) {
        return isTitleCase((int)ch);
    }
    public static boolean isTitleCase(int codePoint) {
        return getType(codePoint) == Character.TITLECASE_LETTER;
    }
    public static boolean isDigit(char ch) {
        return isDigit((int)ch);
    }
    public static boolean isDigit(int codePoint) {
        return getType(codePoint) == Character.DECIMAL_DIGIT_NUMBER;
    }
    public static boolean isDefined(char ch) {
        return isDefined((int)ch);
    }
    public static boolean isDefined(int codePoint) {
        return getType(codePoint) != Character.UNASSIGNED;
    }
    public static boolean isLetter(char ch) {
        return isLetter((int)ch);
    }
    public static boolean isLetter(int codePoint) {
        return ((((1 << Character.UPPERCASE_LETTER) |
            (1 << Character.LOWERCASE_LETTER) |
            (1 << Character.TITLECASE_LETTER) |
            (1 << Character.MODIFIER_LETTER) |
            (1 << Character.OTHER_LETTER)) >> getType(codePoint)) & 1)
            != 0;
    }
    public static boolean isLetterOrDigit(char ch) {
        return isLetterOrDigit((int)ch);
    }
    public static boolean isLetterOrDigit(int codePoint) {
        return ((((1 << Character.UPPERCASE_LETTER) |
            (1 << Character.LOWERCASE_LETTER) |
            (1 << Character.TITLECASE_LETTER) |
            (1 << Character.MODIFIER_LETTER) |
            (1 << Character.OTHER_LETTER) |
            (1 << Character.DECIMAL_DIGIT_NUMBER)) >> getType(codePoint)) & 1)
            != 0;
    }
    public static boolean isAlphabetic(int codePoint) {
        return (((((1 << Character.UPPERCASE_LETTER) |
            (1 << Character.LOWERCASE_LETTER) |
            (1 << Character.TITLECASE_LETTER) |
            (1 << Character.MODIFIER_LETTER) |
            (1 << Character.OTHER_LETTER) |
            (1 << Character.LETTER_NUMBER)) >> getType(codePoint)) & 1) != 0) ||
            CharacterData.of(codePoint).isOtherAlphabetic(codePoint);
    }
    public static boolean isIdeographic(int codePoint) {
        return CharacterData.of(codePoint).isIdeographic(codePoint);
    }
    public static boolean isJavaIdentifierStart(char ch) {
        return isJavaIdentifierStart((int)ch);
    }
    public static boolean isJavaIdentifierStart(int codePoint) {
        return CharacterData.of(codePoint).isJavaIdentifierStart(codePoint);
    }
    public static boolean isJavaIdentifierPart(char ch) {
        return isJavaIdentifierPart((int)ch);
    }
    public static boolean isJavaIdentifierPart(int codePoint) {
        return CharacterData.of(codePoint).isJavaIdentifierPart(codePoint);
    }
    public static boolean isUnicodeIdentifierStart(char ch) {
        return isUnicodeIdentifierStart((int)ch);
    }
    public static boolean isUnicodeIdentifierStart(int codePoint) {
        return CharacterData.of(codePoint).isUnicodeIdentifierStart(codePoint);
    }
    public static boolean isUnicodeIdentifierPart(char ch) {
        return isUnicodeIdentifierPart((int)ch);
    }
    public static boolean isUnicodeIdentifierPart(int codePoint) {
        return CharacterData.of(codePoint).isUnicodeIdentifierPart(codePoint);
    }
    public static boolean isIdentifierIgnorable(char ch) {
        return isIdentifierIgnorable((int)ch);
    }
    public static boolean isIdentifierIgnorable(int codePoint) {
        return CharacterData.of(codePoint).isIdentifierIgnorable(codePoint);
    }
    public static char toLowerCase(char ch) {
        return (char)toLowerCase((int)ch);
    }
    public static int toLowerCase(int codePoint) {
        return CharacterData.of(codePoint).toLowerCase(codePoint);
    }
    public static char toUpperCase(char ch) {
        return (char)toUpperCase((int)ch);
    }
    public static int toUpperCase(int codePoint) {
        return CharacterData.of(codePoint).toUpperCase(codePoint);
    }
    public static char toTitleCase(char ch) {
        return (char)toTitleCase((int)ch);
    }
    public static int toTitleCase(int codePoint) {
        return CharacterData.of(codePoint).toTitleCase(codePoint);
    }
    public static int digit(char ch, int radix) {
        return digit((int)ch, radix);
    }
    public static int digit(int codePoint, int radix) {
        return CharacterData.of(codePoint).digit(codePoint, radix);
    }
    public static int getNumericValue(char ch) {
        return getNumericValue((int)ch);
    }
    public static int getNumericValue(int codePoint) {
        return CharacterData.of(codePoint).getNumericValue(codePoint);
    }
    public static boolean isSpaceChar(char ch) {
        return isSpaceChar((int)ch);
    }
    public static boolean isSpaceChar(int codePoint) {
        return ((((1 << Character.SPACE_SEPARATOR) |
                  (1 << Character.LINE_SEPARATOR) |
                  (1 << Character.PARAGRAPH_SEPARATOR)) >> getType(codePoint)) & 1)
            != 0;
    }
    public static boolean isWhitespace(char ch) {
        return isWhitespace((int)ch);
    }
    public static boolean isWhitespace(int codePoint) {
        return CharacterData.of(codePoint).isWhitespace(codePoint);
    }
    public static boolean isISOControl(char ch) {
        return isISOControl((int)ch);
    }
    public static boolean isISOControl(int codePoint) {
        // Optimized form of:
        //     (codePoint >= 0x00 && codePoint <= 0x1F) ||
        //     (codePoint >= 0x7F && codePoint <= 0x9F);
        return codePoint <= 0x9F &&
            (codePoint >= 0x7F || ((int)((uint)codePoint >> 5) == 0));
    }
    public static int getType(char ch) {
        return getType((int)ch);
    }
    public static int getType(int codePoint) {
        return CharacterData.of(codePoint).getType(codePoint);
    }
    public static char forDigit(int digit, int radix) {
        if ((digit >= radix) || (digit < 0)) {
            return '\0';
        }
        if ((radix < Character.MIN_RADIX) || (radix > Character.MAX_RADIX)) {
            return '\0';
        }
        if (digit < 10) {
            return (char)('0' + digit);
        }
        return (char)('a' - 10 + digit);
    }
    public static byte getDirectionality(char ch) {
        return getDirectionality((int)ch);
    }
    public static byte getDirectionality(int codePoint) {
        return CharacterData.of(codePoint).getDirectionality(codePoint);
    }
    public static boolean isMirrored(char ch) {
        return isMirrored((int)ch);
    }
    public static boolean isMirrored(int codePoint) {
        return CharacterData.of(codePoint).isMirrored(codePoint);
    }
    public int compareTo(Character anotherCharacter) {
        return compare(this.value, anotherCharacter.value);
    }
    public static int compare(char x, char y) {
        return x - y;
    }
    static int toUpperCaseEx(int codePoint) {
        return CharacterData.of(codePoint).toUpperCaseEx(codePoint);
    }
    static char[] toUpperCaseCharArray(int codePoint) {
        // As of Unicode 6.0, 1:M uppercasings only happen in the BMP.
        return CharacterData.of(codePoint).toUpperCaseCharArray(codePoint);
    }
    public static readonly int SIZE = 16;
    public static char reverseBytes(char ch) {
        return (char) (((ch & 0xFF00) >> 8) | (ch << 8));
    }/*
    public static String getName(int codePoint) {
        if (!isValidCodePoint(codePoint)) {
            throw new IllegalArgumentException();
        }
        String name = CharacterName.get(codePoint);
        if (name != null)
            return name;
        if (getType(codePoint) == UNASSIGNED)
            return null;
        UnicodeBlock block = UnicodeBlock.of(codePoint);
        if (block != null)
            return block.toString().replace('_', ' ') + " "
                   + Integer.toHexString(codePoint).toUpperCase();
        // should never come here
        return Integer.toHexString(codePoint).toUpperCase();
    }
    */
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    public static implicit operator char(Character other)
    {
        return other.value;
    }

    public static implicit operator Character(char other)
    {
        return new Character(other);
    }

    public override string ToString()
    {
        return toString();
    }

    public override bool Equals(object obj)
    {
        if (obj is Character)
        {
            return this.equals((Character)obj);
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return hashCode();
    }

    public int CompareTo(Character other)
    {
        return this.compareTo(other);
    }
}
}
