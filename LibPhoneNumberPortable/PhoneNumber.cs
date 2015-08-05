using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _PhoneNumber = com.google.i18n.phonenumbers.Phonenumber.PhoneNumber;
using _CountryCodeSource = com.google.i18n.phonenumbers.Phonenumber.PhoneNumber.CountryCodeSource;
using _PhoneNumberOfflineGeocoder = com.google.i18n.phonenumbers.geocoding.PhoneNumberOfflineGeocoder;

using _PhoneNumberFormat = com.google.i18n.phonenumbers.PhoneNumberUtil.PhoneNumberFormat;
using _PhoneNumberUtil = com.google.i18n.phonenumbers.PhoneNumberUtil;
using _NumberFormat = com.google.i18n.phonenumbers.Phonemetadata.NumberFormat;
using _PhoneNumberType = com.google.i18n.phonenumbers.PhoneNumberUtil.PhoneNumberType;
using _Leniency = com.google.i18n.phonenumbers.PhoneNumberUtil.Leniency;

namespace libphonenumber
{
    public class PhoneNumber
    {
        internal _PhoneNumber _inner { get; set; }

        internal PhoneNumber(_PhoneNumber phoneNumber)
        {
            this._inner = phoneNumber;
        }

        public PhoneNumber()
        {
            _inner = new _PhoneNumber();
        }

        // required int32 country_code = 1;
        public int? CountryCode
        {
            get
            {
                if(_inner.hasCountryCode())
                    return _inner.getCountryCode();
                return null;
            }
            set
            {
                if (value == null)
                    _inner.clearCountryCode();
                else
                    _inner.setCountryCode(value.Value);
            }
        }

        // required uint64 national_number = 2;
        public long? NationalNumber
        {
            get
            {
                if (_inner.hasNationalNumber())
                    return _inner.getNationalNumber();
                return null;
            }
            set
            {
                if (value == null)
                    _inner.clearNationalNumber();
                else
                    _inner.setNationalNumber(value.Value);
            }
        }

        // optional string extension = 3;
        public string Extension
        {
            get
            {
                if (_inner.hasExtension())
                    return _inner.getExtension();
                return null;
            }
            set
            {
                if (value == null)
                    _inner.clearExtension();
                else
                    _inner.setExtension(value);
            }
        }

        // optional bool italian_leading_zero = 4;
        public bool? ItalianLeadingZero
        {
            get
            {
                if (_inner.hasItalianLeadingZero())
                    return _inner.isItalianLeadingZero();
                return null;
            }
            set
            {
                if (value == null)
                    _inner.clearItalianLeadingZero();
                else
                    _inner.setItalianLeadingZero(value.Value);
            }
        }

        // optional string raw_input = 5;
        public string RawInput
        {
            get
            {
                if (_inner.hasRawInput())
                    return _inner.getRawInput();
                return null;
            }
            set
            {
                if (value == null)
                    _inner.clearRawInput();
                else
                    _inner.setRawInput(value);
            }
        }

        // optional CountryCodeSource country_code_source = 6;
        public CountryCodeSource? CountryCodeSource
        {
            get
            {
                if (_inner.hasCountryCodeSource())
                    return (CountryCodeSource)_inner.getCountryCodeSource();
                return null;
            }
            set
            {
                if (value == null)
                    _inner.clearCountryCodeSource();
                else
                    _inner.setCountryCodeSource((_CountryCodeSource)value);
            }
        }

        // optional string preferred_domestic_carrier_code = 7;
        public string PreferredDomesticCarrierCode
        {
            get
            {
                if (_inner.hasPreferredDomesticCarrierCode())
                    return _inner.getPreferredDomesticCarrierCode();
                return null;
            }
            set
            {
                if (value == null)
                    _inner.clearPreferredDomesticCarrierCode();
                else
                    _inner.setPreferredDomesticCarrierCode(value);
            }
        }

        public PhoneNumber Clear()
        {
            _inner.clear();
            return this;
        }

        public PhoneNumber MergeFrom(PhoneNumber other)
        {
            _inner.mergeFrom(other._inner);
            return this;
        }

        public bool ExactlySameAs(PhoneNumber other)
        {
            return _inner.exactlySameAs(other._inner);
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the length of the geographical area code from the {@code nationalNumber_} field of the
        /// PhoneNumber object passed in, so that clients could use it to split a national significant
        /// number into geographical area code and subscriber number. It works in such a way that the
        /// resultant subscriber number should be diallable, at least on some devices. 
        /// N.B.: area code is a very ambiguous concept, so the I18N team generally recommends against
        /// using it for most purposes, but recommends using the more general {@code national_number}
        /// instead. Read the following carefully before deciding to use this method:
        /// <list type="bullet">
        ///     <item><description>geographical area codes change over time, and this method honors those changes; therefore, it doesn't guarantee the stability of the result it produces.</description></item>
        ///     <item><description>subscriber numbers may not be diallable from all devices (notably mobile devices, which typically requires the full national_number to be dialled in most regions).</description></item>
        ///     <item><description>most non-geographical numbers have no area codes, including numbers from non-geographical entities.</description></item> 
        ///     <item><description>some geographical numbers have no area codes.</description></item> 
        /// </list>
        /// </summary>
        /// <example>
        /// An example of how this could be used:
        /// <code>
        /// PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance();
        /// PhoneNumber number = phoneUtil.parse("16502530000", "US");
        /// string nationalSignificantNumber = phoneUtil.getNationalSignificantNumber(number);
        /// string areaCode;
        /// string subscriberNumber;
        /// int areaCodeLength = phoneUtil.getLengthOfGeographicalAreaCode(number);
        /// if (areaCodeLength > 0) {
        ///   areaCode = nationalSignificantNumber.substring(0, areaCodeLength);
        ///   subscriberNumber = nationalSignificantNumber.substring(areaCodeLength);
        /// } else {
        ///   areaCode = "";
        ///   subscriberNumber = nationalSignificantNumber;
        /// }
        /// </code>
        /// </example>
        /// <returns>the length of area code of the PhoneNumber object passed in.</returns>
        public int LengthOfGeographicalAreaCode
        {
            get
            {
                return _PhoneNumberUtil.getInstance().getLengthOfGeographicalAreaCode(_inner);
            }
        }

        /// <summary>
        /// Gets the length of the national destination code (NDC) from the PhoneNumber object passed in,
        /// so that clients could use it to split a national significant number into NDC and subscriber
        /// number. The NDC of a phone number is normally the first group of digit(s) right after the
        /// country calling code when the number is formatted in the international format, if there is a
        /// subscriber number part that follows. An example of how this could be used:
        ///
        /// <pre>
        /// PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance();
        /// PhoneNumber number = phoneUtil.parse("18002530000", "US");
        /// string nationalSignificantNumber = phoneUtil.getNationalSignificantNumber(number);
        /// string nationalDestinationCode;
        /// string subscriberNumber;
        ///
        /// int nationalDestinationCodeLength = phoneUtil.getLengthOfNationalDestinationCode(number);
        /// if (nationalDestinationCodeLength > 0) {
        ///   nationalDestinationCode = nationalSignificantNumber.substring(0,
        ///       nationalDestinationCodeLength);
        ///   subscriberNumber = nationalSignificantNumber.substring(nationalDestinationCodeLength);
        /// } else {
        ///   nationalDestinationCode = "";
        ///   subscriberNumber = nationalSignificantNumber;
        /// }
        /// </pre>
        ///
        /// Refer to the unittests to see the difference between this function and
        /// {@link #getLengthOfGeographicalAreaCode}.
        /// </summary>
        /// <param name="number">the PhoneNumber object for which clients want to know the length of the NDC.</param>
        /// <returns>the length of NDC of the PhoneNumber object passed in.</returns>
        public int LengthOfNationalDestinationCode
        {
            get
            {
                return _PhoneNumberUtil.getInstance().getLengthOfNationalDestinationCode(_inner);
            }
        }

        /// <summary>
        /// Formats a phone number in the specified format using default rules. Note that this does not
        /// promise to produce a phone number that the user can dial from where they are - although we do
        /// format in either 'national' or 'international' format depending on what the client asks for, we
        /// do not currently support a more abbreviated format, such as for users in the same "area" who
        /// could potentially dial the number without area code. Note that if the phone number has a
        /// country calling code of 0 or an otherwise invalid country calling code, we cannot work out
        /// which formatting rules to apply so we return the national significant number with no formatting
        /// applied.
        /// </summary>
        /// <param name="numberFormat">the format the phone number should be formatted into</param>
        /// <returns>the formatted phone number</returns>
        public string Format(libphonenumber.PhoneNumberUtil.PhoneNumberFormat numberFormat)
        {
            return _PhoneNumberUtil.getInstance().format(_inner, (_PhoneNumberFormat)numberFormat);
        }

        /// <summary>
        /// Formats a phone number for out-of-country dialing purposes. If no regionCallingFrom is
        /// supplied, we format the number in its INTERNATIONAL format. If the country calling code is the
        /// same as that of the region where the number is from, then NATIONAL formatting will be applied.
        ///
        /// <p>If the number itself has a country calling code of zero or an otherwise invalid country
        /// calling code, then we return the number with no formatting applied.
        ///
        /// <p>Note this function takes care of the case for calling inside of NANPA and between Russia and
        /// Kazakhstan (who share the same country calling code). In those cases, no international prefix
        /// is used. For regions which have multiple international prefixes, the number in its
        /// INTERNATIONAL format will be returned instead.
        /// </summary>
        /// <param name="regionCallingFrom">the region where the call is being placed</param>
        /// <returns>the formatted phone number</returns>
        public string FormatOutOfCountryCallingNumber(string regionCallingFrom)
        {
            return _PhoneNumberUtil.getInstance().formatOutOfCountryCallingNumber(_inner, regionCallingFrom);
        }

        /// <summary>
        /// Formats a phone number in the specified format using client-defined formatting rules. Note that
        /// if the phone number has a country calling code of zero or an otherwise invalid country calling
        /// code, we cannot work out things like whether there should be a national prefix applied, or how
        /// to format extensions, so we return the national significant number with no formatting applied.
        /// </summary>
        /// <param name="numberFormat">the format the phone number should be formatted into</param>
        /// <param name="userDefinedFormats">formatting rules specified by clients</param>
        /// <returns>the formatted phone number</returns>
        public string FormatByPattern(libphonenumber.PhoneNumberUtil.PhoneNumberFormat numberFormat, IEnumerable<NumberFormat> userDefinedFormats)
        {
            java.util.List<_NumberFormat> tempUserDefinedFormats = new java.util.List<_NumberFormat>();
            foreach (var item in userDefinedFormats)
                tempUserDefinedFormats.add(item);
            return _PhoneNumberUtil.getInstance().formatByPattern(_inner, (_PhoneNumberFormat)numberFormat, tempUserDefinedFormats);
        }


        /// <summary>
        /// Formats a phone number in national format for dialing using the carrier as specified in the
        /// {@code carrierCode}. The {@code carrierCode} will always be used regardless of whether the
        /// phone number already has a preferred domestic carrier code stored. If {@code carrierCode}
        /// contains an empty string, returns the number in national format without any carrier code.
        /// </summary>
        /// <param name="number">the phone number to be formatted</param>
        /// <param name="carrierCode">the carrier selection code to be used</param>
        /// <returns>the formatted phone number in national format for dialing using the carrier as specified in the {@code carrierCode}</returns>
        public string FormatNationalNumberWithCarrierCode(string carrierCode)
        {
            return _PhoneNumberUtil.getInstance().formatNationalNumberWithCarrierCode(_inner, carrierCode);
        }

        /// <summary>
        /// Formats a phone number in national format for dialing using the carrier as specified in the
        /// preferredDomesticCarrierCode field of the PhoneNumber object passed in. If that is missing,
        /// use the {@code fallbackCarrierCode} passed in instead. If there is no
        /// {@code preferredDomesticCarrierCode}, and the {@code fallbackCarrierCode} contains an empty
        /// string, return the number in national format without any carrier code.
        ///
        /// <p>Use {@link #formatNationalNumberWithCarrierCode} instead if the carrier code passed in
        /// should take precedence over the number's {@code preferredDomesticCarrierCode} when formatting.
        /// </summary>
        /// <param name="number">the phone number to be formatted</param>
        /// <param name="fallbackCarrierCode">the carrier selection code to be used, if none is found in the phone number itself</param>
        /// <returns>the formatted phone number in national format for dialing using the number's {@code preferredDomesticCarrierCode}, or the {@code fallbackCarrierCode} passed in if none is found</returns>
        public string FormatNationalNumberWithPreferredCarrierCode(string fallbackCarrierCode)
        {
            return _PhoneNumberUtil.getInstance().formatNationalNumberWithPreferredCarrierCode(_inner, fallbackCarrierCode);
        }

        /// <summary>
        /// Returns a number formatted in such a way that it can be dialed from a mobile phone in a
        /// specific region. If the number cannot be reached from the region (e.g. some countries block
        /// toll-free numbers from being called outside of the country), the method returns an empty
        /// string.
        /// </summary>
        /// <param name="number">the phone number to be formatted</param>
        /// <param name="regionCallingFrom">the region where the call is being placed</param>
        /// <param name="withFormatting">whether the number should be returned with formatting symbols, such as spaces and dashes.</param>
        /// <returns>the formatted phone number</returns>
        public string FormatNumberForMobileDialing(string regionCallingFrom, bool withFormatting)
        {
            return _PhoneNumberUtil.getInstance().formatNumberForMobileDialing(_inner, regionCallingFrom, withFormatting);
        }

        /// <summary>
        /// Formats a phone number using the original phone number format that the number is parsed from.
        /// The original format is embedded in the country_code_source field of the PhoneNumber object
        /// passed in. If such information is missing, the number will be formatted into the NATIONAL
        /// format by default. When the number contains a leading zero and this is unexpected for this
        /// country, or we don't have a formatting pattern for the number, the method returns the raw input
        /// when it is available.
        ///
        /// Note this method guarantees no digit will be inserted, removed or modified as a result of
        /// formatting.
        /// </summary>
        /// <param name="number">the phone number that needs to be formatted in its original number format</param>
        /// <param name="regionCallingFrom">the region whose IDD needs to be prefixed if the original number has one</param>
        /// <returns>the formatted phone number in its original number format</returns>
        public string FormatInOriginalFormat(string regionCallingFrom)
        {
            return _PhoneNumberUtil.getInstance().formatInOriginalFormat(_inner, regionCallingFrom);
        }

        /// <summary>
        /// Formats a phone number for out-of-country dialing purposes.
        /// 
        /// Note that in this version, if the number was entered originally using alpha characters and
        /// this version of the number is stored in raw_input, this representation of the number will be
        /// used rather than the digit representation. Grouping information, as specified by characters
        /// such as "-" and " ", will be retained.
        /// 
        /// <p><b>Caveats:</b></p>
        /// <ul>
        ///  <li> This will not produce good results if the country calling code is both present in the raw
        ///       input _and_ is the start of the national number. This is not a problem in the regions
        ///       which typically use alpha numbers.
        ///  <li> This will also not produce good results if the raw input has any grouping information
        ///       within the first three digits of the national number, and if the function needs to strip
        ///       preceding digits/words in the raw input before these digits. Normally people group the
        ///       first three digits together so this is not a huge problem - and will be fixed if it
        ///       proves to be so.
        /// </ul>
        /// </summary>
        /// <param name="number">the phone number that needs to be formatted</param>
        /// <param name="regionCallingFrom">the region where the call is being placed</param>
        /// <returns>the formatted phone number</returns>
        public string FormatOutOfCountryKeepingAlphaChars(string regionCallingFrom)
        {
            return _PhoneNumberUtil.getInstance().formatOutOfCountryKeepingAlphaChars(_inner, regionCallingFrom);
        }

        /// <summary>
        /// Gets the national significant number of the a phone number. Note a national significant number
        /// doesn't contain a national prefix or any formatting.
        /// </summary>
        /// <param name="number">the phone number for which the national significant number is needed</param>
        /// <returns>the national significant number of the PhoneNumber object passed in</returns>
        public string NationalSignificantNumber
        {
            get
            {
                return _PhoneNumberUtil.getInstance().getNationalSignificantNumber(_inner);
            }
        }


        /// <summary>
        /// Gets the type of a phone number.
        /// </summary>
        /// <param name="number">the phone number that we want to know the type</param>
        /// <returns>the type of the phone number</returns>
        public libphonenumber.PhoneNumberUtil.PhoneNumberType NumberType
        {
            get
            {
                return (libphonenumber.PhoneNumberUtil.PhoneNumberType)_PhoneNumberUtil.getInstance().getNumberType(_inner);
            }
        }

        /// <summary>
        /// Tests whether a phone number matches a valid pattern. Note this doesn't verify the number
        /// is actually in use, which is impossible to tell by just looking at a number itself.
        /// </summary>
        /// <returns>a bool that indicates whether the number is of a valid pattern</returns>
        public bool IsValidNumber
        {
            get
            {
                return _PhoneNumberUtil.getInstance().isValidNumber(_inner);
            }
        }

        /// <summary>
        /// Tests whether a phone number is valid for a certain region. Note this doesn't verify the number
        /// is actually in use, which is impossible to tell by just looking at a number itself. If the
        /// country calling code is not the same as the country calling code for the region, this
        /// immediately exits with false. After this, the specific number pattern rules for the region are
        /// examined. This is useful for determining for example whether a particular number is valid for
        /// Canada, rather than just a valid NANPA number.
        /// Warning: In most cases, you want to use {@link #isValidNumber} instead. For example, this
        /// method will mark numbers from British Crown dependencies such as the Isle of Man as invalid for
        /// the region "GB" (United Kingdom), since it has its own region code, "IM", which may be
        /// undesirable.
        /// </summary>
        /// <param name="regionCode">the region that we want to validate the phone number for</param>
        /// <returns>a bool that indicates whether the number is of a valid pattern</returns>
        public bool IsValidNumberForRegion(string regionCode)
        {
            return _PhoneNumberUtil.getInstance().isValidNumberForRegion(_inner, regionCode);
        }

        /// <summary>
        /// Returns the region where a phone number is from. This could be used for geocoding at the region
        /// level.
        /// </summary>
        /// <returns>the region where the phone number is from, or null if no region matches this calling code</returns>
        public string RegionCodeForNumber
        {
            get
            {
                return _PhoneNumberUtil.getInstance().getRegionCodeForNumber(_inner);
            }
        }

        /// <summary>
        /// Convenience wrapper around {@link #isPossibleNumberWithReason}. Instead of returning the reason
        /// for failure, this method returns a bool value.
        /// </summary>
        /// <returns>true if the number is possible</returns>
        public bool IsPossibleNumber
        {
            get
            {
                return _PhoneNumberUtil.getInstance().isPossibleNumber(_inner);
            }
        }

        /// <summary>
        /// Check whether a phone number is a possible number. It provides a more lenient check than
        /// {@link #isValidNumber} in the following sense:
        ///<ol>
        /// <li> It only checks the length of phone numbers. In particular, it doesn't check starting
        ///      digits of the number.
        /// <li> It doesn't attempt to figure out the type of the number, but uses general rules which
        ///      applies to all types of phone numbers in a region. Therefore, it is much faster than
        ///      isValidNumber.
        /// <li> For fixed line numbers, many regions have the concept of area code, which together with
        ///      subscriber number constitute the national significant number. It is sometimes okay to dial
        ///      the subscriber number only when dialing in the same area. This function will return
        ///      true if the subscriber-number-only version is passed in. On the other hand, because
        ///      isValidNumber validates using information on both starting digits (for fixed line
        ///      numbers, that would most likely be area codes) and length (obviously includes the
        ///      length of area codes for fixed line numbers), it will return false for the
        ///      subscriber-number-only version.
        /// </ol>
        /// </summary>
        /// <returns>a ValidationResult object which indicates whether the number is possible</returns>
        public libphonenumber.PhoneNumberUtil.ValidationResult IsPossibleNumberWithReason
        {
            get
            {
                return (libphonenumber.PhoneNumberUtil.ValidationResult)_PhoneNumberUtil.getInstance().isPossibleNumberWithReason(_inner);
            }
        }

        /// <summary>
        /// Attempts to extract a valid number from a phone number that is too long to be valid, and resets
        /// the PhoneNumber object passed in to that valid version. If no valid number could be extracted,
        /// the PhoneNumber object passed in will not be modified.
        /// </summary>
        /// <returns>true if a valid phone number can be successfully extracted.</returns>
        public bool TruncateTooLongNumber()
        {
            return _PhoneNumberUtil.getInstance().truncateTooLongNumber(_inner);
        }


        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Returns a text description for the given phone number, in the language provided. The
        /// description might consist of the name of the country where the phone number is from, or the
        /// name of the geographical area the phone number is from if more detailed information is
        /// available.
        ///
        /// <p>This method assumes the validity of the number passed in has already been checked, and that
        /// the number is suitable for geocoding. We consider fixed-line and mobile numbers possible
        /// candidates for geocoding.
        /// </summary>
        /// <param name="number">a valid phone number for which we want to get a text description</param>
        /// <param name="languageCode">the language code for which the description should be written</param>
        /// <returns>a text description for the given language code for the given phone number</returns>
        public string GetDescriptionForValidNumber(Locale languageCode)
        {
            return _PhoneNumberOfflineGeocoder.getInstance().getDescriptionForValidNumber(_inner, languageCode._inner);
        }

        /// <summary>
        /// As per {@link #getDescriptionForValidNumber(PhoneNumber, Locale)} but also considers the
        /// region of the user. If the phone number is from the same region as the user, only a lower-level
        /// description will be returned, if one exists. Otherwise, the phone number's region will be
        /// returned, with optionally some more detailed information.
        ///
        /// <p>For example, for a user from the region "US" (United States), we would show "Mountain View,
        /// CA" for a particular number, omitting the United States from the description. For a user from
        /// the United Kingdom (region "GB"), for the same number we may show "Mountain View, CA, United
        /// States" or even just "United States".
        ///
        /// <p>This method assumes the validity of the number passed in has already been checked.
        /// </summary>
        /// <param name="number">the phone number for which we want to get a text description</param>
        /// <param name="languageCode">the language code for which the description should be written</param>
        /// <param name="userRegion">the region code for a given user. This region will be omitted from the
        ///     description if the phone number comes from this region. It is a two-letter uppercase ISO
        ///     country code as defined by ISO 3166-1.</param>
        /// <returns>a text description for the given language code for the given phone number, or empty
        ///     string if the number passed in is invalid</returns>
        public string GetDescriptionForValidNumber(Locale languageCode, string userRegion)
        {
            return _PhoneNumberOfflineGeocoder.getInstance().getDescriptionForValidNumber(_inner, languageCode._inner, userRegion);
        }

        /// <summary>
        /// As per {@link #getDescriptionForValidNumber(PhoneNumber, Locale)} but explicitly checks
        /// the validity of the number passed in.
        /// </summary>
        /// <param name="number">the phone number for which we want to get a text description</param>
        /// <param name="languageCode">the language code for which the description should be written</param>
        /// <returns>a text description for the given language code for the given phone number, or empty
        ///     string if the number passed in is invalid</returns>
        public string GetDescriptionForNumber(Locale languageCode)
        {
            return _PhoneNumberOfflineGeocoder.getInstance().getDescriptionForNumber(_inner, languageCode._inner);
        }

        /// <summary>
        /// As per {@link #getDescriptionForValidNumber(PhoneNumber, Locale, String)} but
        /// explicitly checks the validity of the number passed in.
        /// </summary>
        /// <param name="number">the phone number for which we want to get a text description</param>
        /// <param name="languageCode">the language code for which the description should be written</param>
        /// <param name="userRegion">the region code for a given user. This region will be omitted from the
        ///     description if the phone number comes from this region. It is a two-letter uppercase ISO
        ///     country code as defined by ISO 3166-1.</param>
        /// <returns>a text description for the given language code for the given phone number, or empty
        ///     string if the number passed in is invalid</returns>
        public string GetDescriptionForNumber(Locale languageCode, string userRegion)
        {
            return _PhoneNumberOfflineGeocoder.getInstance().getDescriptionForNumber(_inner, languageCode._inner, userRegion);
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

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

        public static implicit operator _PhoneNumber(PhoneNumber other)
        {
            return other._inner;
        }

        public static implicit operator PhoneNumber(_PhoneNumber other)
        {
            return new PhoneNumber(other);
        }
    }
}
