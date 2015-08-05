using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using _NumberFormat = com.google.i18n.phonenumbers.Phonemetadata.NumberFormat;

namespace libphonenumber
{
    public class NumberFormat
    {
        internal _NumberFormat _inner { get; set; }

        public NumberFormat()
        {
            this._inner = new _NumberFormat();
        }

        internal NumberFormat(_NumberFormat numberFormat)
        {
            this._inner = numberFormat;
        }

        // required string pattern = 1;
        public string Pattern
        {
            get
            {
                if (_inner.hasPattern())
                    return _inner.getPattern();
                return null;
            }
            set
            {
                _inner.setPattern(value);
            }
        }

        // required string format = 2;
        public string Format
        {
            get
            {
                if (_inner.hasFormat())
                    return _inner.getFormat();
                return null;
            }
            set
            {
                _inner.setFormat(value);
            }
        }

        // repeated string leading_digits_pattern = 3;
        private List<string> _leadingDigitsPattern = new List<string>();
        public List<string> LeadingDigitsPattern { get { return _leadingDigitsPattern; } }

        // optional string national_prefix_formatting_rule = 4;
        public string NationalPrefixFormattingRule
        {
            get
            {
                if (_inner.hasNationalPrefixFormattingRule())
                    return _inner.getNationalPrefixFormattingRule();
                return null;
            }
            set
            {
                if (value == null)
                    _inner.clearNationalPrefixFormattingRule();
                else
                    _inner.setNationalPrefixFormattingRule(value);
            }
        }

        // optional bool national_prefix_optional_when_formatting = 6;
        public bool? NationalPrefixOptionalWhenFormatting
        {
            get
            {
                if (_inner.hasNationalPrefixOptionalWhenFormatting())
                    return _inner.isNationalPrefixOptionalWhenFormatting();
                return null;
            }
            set
            {
                _inner.setNationalPrefixOptionalWhenFormatting(value.Value);
            }
        }

        // optional string domestic_carrier_code_formatting_rule = 5;
        public string DomesticCarrierCodeFormattingRule
        {
            get
            {
                if (_inner.hasDomesticCarrierCodeFormattingRule())
                    return _inner.getDomesticCarrierCodeFormattingRule();
                return null;
            }
            set
            {
                _inner.setDomesticCarrierCodeFormattingRule(value);
            }
        }

        public NumberFormat MergeFrom(NumberFormat other)
        {
            _inner.mergeFrom(other._inner);
            return this;
        }

        public static implicit operator _NumberFormat(NumberFormat other)
        {
            return other._inner;
        }

        public static implicit operator NumberFormat(_NumberFormat other)
        {
            return new NumberFormat(other);
        }
    }
}
