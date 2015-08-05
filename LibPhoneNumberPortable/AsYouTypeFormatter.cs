using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using _AsYouTypeFormatter = com.google.i18n.phonenumbers.AsYouTypeFormatter;

namespace libphonenumber
{
    public class AsYouTypeFormatter
    {
        internal _AsYouTypeFormatter _inner { get; set; }

        public AsYouTypeFormatter(_AsYouTypeFormatter phoneNumber)
        {
            this._inner = phoneNumber;
        }

        /// <summary>
        /// Clears the internal state of the formatter, so it can be reused.
        /// </summary>
        public void Clear()
        {
            _inner.clear();
        }

        /// <summary>
        /// Formats a phone number on-the-fly as each digit is entered.
        /// </summary>
        /// <param name="nextChar">the most recently entered digit of a phone number. Formatting characters are
        ///     allowed, but as soon as they are encountered this method formats the number as entered and
        ///     not "as you type" anymore. Full width digits and Arabic-indic digits are allowed, and will
        ///     be shown as they are.</param>
        /// <returns>the partially formatted phone number.</returns>
        public string InputDigit(char nextChar)
        {
            return _inner.inputDigit(nextChar);
        }

        /// <summary>
        /// Same as {@link #inputDigit}, but remembers the position where <paramref name="nextChar"/> is inserted, so
        /// that it can be retrieved later by using {@link #getRememberedPosition}. The remembered
        /// position will be automatically adjusted if additional formatting characters are later
        /// inserted/removed in front of <paramref name="nextChar"/>.
        /// </summary>
        /// <param name="nextChar"></param>
        /// <returns></returns>
        public string InputDigitAndRememberPosition(char nextChar)
        {
            return _inner.inputDigitAndRememberPosition(nextChar);
        }

        /// <summary>
        /// Returns the current position in the partially formatted phone number of the character which was previously passed in as the parameter of {@link #inputDigitAndRememberPosition}.
        /// </summary>
        public int RememberedPosition
        {
            get
            {
                return _inner.getRememberedPosition();
            }
        }

        public static implicit operator _AsYouTypeFormatter(AsYouTypeFormatter other)
        {
            return other._inner;
        }

        public static implicit operator AsYouTypeFormatter(_AsYouTypeFormatter other)
        {
            return new AsYouTypeFormatter(other);
        }
    }
}
