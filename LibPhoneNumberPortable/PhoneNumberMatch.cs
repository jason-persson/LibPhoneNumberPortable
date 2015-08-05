using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using _PhoneNumberMatch = com.google.i18n.phonenumbers.PhoneNumberMatch;

namespace libphonenumber
{
    public class PhoneNumberMatch
    {
        internal _PhoneNumberMatch _inner { get; set; }

        public PhoneNumberMatch(_PhoneNumberMatch phoneNumber)
        {
            this._inner = phoneNumber;
        }

        /** Returns the phone number matched by the receiver. */
        public PhoneNumber Number
        {
            get
            {
                return _inner.number();
            }
        }

        /** Returns the start index of the matched phone number within the searched text. */
        public int Start
        {
            get
            {
                return _inner.start();
            }
        }

        /** Returns the exclusive end index of the matched phone number within the searched text. */
        public int End
        {
            get
            {
                return _inner.end();
            }
        }

        /** Returns the raw string matched as a phone number in the searched text. */
        public string RawString
        {
            get
            {
                return _inner.rawString();
            }
        }

        public override bool Equals(object obj)
        {
            return _inner.equals(obj);
        }

        public override int GetHashCode()
        {
            return _inner.hashCode();
        }

        public override string ToString()
        {
            return _inner.toString();
        }

        public static implicit operator _PhoneNumberMatch(PhoneNumberMatch other)
        {
            return other._inner;
        }

        public static implicit operator PhoneNumberMatch(_PhoneNumberMatch other)
        {
            return new PhoneNumberMatch(other);
        }
    }
}
