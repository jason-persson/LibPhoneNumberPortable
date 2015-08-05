using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _Locale = java.util.Locale;

namespace libphonenumber
{
    public class Locale
    {
        public static readonly Locale ENGLISH = new Locale("en", "");
        public static readonly Locale FRENCH = new Locale("fr", "");
        public static readonly Locale GERMAN = new Locale("de", "");
        public static readonly Locale ITALIAN = new Locale("it", "");
        public static readonly Locale JAPANESE = new Locale("ja", "");
        public static readonly Locale KOREAN = new Locale("ko", "");
        public static readonly Locale CHINESE = new Locale("zh", "");
        public static readonly Locale SIMPLIFIED_CHINESE = new Locale("zh", "CN");
        public static readonly Locale TRADITIONAL_CHINESE = new Locale("zh", "TW");
        public static readonly Locale FRANCE = new Locale("fr", "FR");
        public static readonly Locale GERMANY = new Locale("de", "DE");
        public static readonly Locale ITALY = new Locale("it", "IT");
        public static readonly Locale JAPAN = new Locale("ja", "JP");
        public static readonly Locale KOREA = new Locale("ko", "KR");
        public static readonly Locale CHINA = SIMPLIFIED_CHINESE;
        public static readonly Locale PRC = SIMPLIFIED_CHINESE;
        public static readonly Locale TAIWAN = TRADITIONAL_CHINESE;
        public static readonly Locale UK = new Locale("en", "GB");
        public static readonly Locale US = new Locale("en", "US");
        public static readonly Locale CANADA = new Locale("en", "CA");
        public static readonly Locale CANADA_FRENCH = new Locale("fr", "CA");

        internal _Locale _inner { get; set; }

        public Locale(string language, string country)
        {
            this._inner = new _Locale(language, country);
        }

        public string GetDisplayCountry(Locale displayLanguage)
        {
            return _inner.getDisplayCountry(displayLanguage._inner);
        }
    }
}
