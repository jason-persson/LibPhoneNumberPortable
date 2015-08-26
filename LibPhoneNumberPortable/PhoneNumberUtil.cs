using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using com.google.i18n.phonenumbers;
using _PhoneNumberFormat = com.google.i18n.phonenumbers.PhoneNumberUtil.PhoneNumberFormat;
using _PhoneNumberUtil = com.google.i18n.phonenumbers.PhoneNumberUtil;
using _NumberFormat = com.google.i18n.phonenumbers.Phonemetadata.NumberFormat;
using _PhoneNumberType = com.google.i18n.phonenumbers.PhoneNumberUtil.PhoneNumberType;
using _Leniency = com.google.i18n.phonenumbers.PhoneNumberUtil.Leniency;

namespace libphonenumber
{

    public class PhoneNumberUtil
    {
        /// <summary>
        /// Leniency when {@linkplain PhoneNumberUtil#findNumbers finding} potential phone numbers in text segments. The levels here are ordered in increasing strictness.
        /// </summary>
        public enum Leniency
        {
            POSSIBLE,
            VALID,
            STRICT_GROUPING,
            EXACT_GROUPING
        }

        /// <summary>
        /// Type of phone numbers.
        /// </summary>
        public enum PhoneNumberType
        {
            FIXED_LINE,
            MOBILE,
            /// <summary>
            /// In some regions (e.g. the USA), it is impossible to distinguish between fixed-line and
            /// mobile numbers by looking at the phone number itself.
            /// </summary>
            FIXED_LINE_OR_MOBILE,
            /// <summary>
            /// Freephone lines
            /// </summary>
            TOLL_FREE,
            PREMIUM_RATE,
            /// <summary>
            /// The cost of this call is shared between the caller and the recipient, and is hence typically
            /// less than PREMIUM_RATE calls. See // http://en.wikipedia.org/wiki/Shared_Cost_Service for
            /// more information.
            /// </summary>
            SHARED_COST,
            /// <summary>
            /// Voice over IP numbers. This includes TSoIP (Telephony Service over IP).
            /// </summary>
            VOIP,
            /// <summary>
            /// A personal number is associated with a particular person, and may be routed to either a
            /// MOBILE or FIXED_LINE number. Some more information can be found here:
            /// http://en.wikipedia.org/wiki/Personal_Numbers
            /// </summary>
            PERSONAL_NUMBER,
            PAGER,
            /// <summary>
            /// Used for "Universal Access Numbers" or "Company Numbers". They may be further routed to
            /// specific offices, but allow one number to be used for a company.
            /// </summary>
            UAN,
            /// <summary>
            /// Used for "Voice Mail Access Numbers".
            /// </summary>
            VOICEMAIL,
            /// <summary>
            /// A phone number is of type UNKNOWN when it does not fit any of the known patterns for a
            /// specific region.
            /// </summary>
            UNKNOWN
        }

        /// <summary>
        /// Possible outcomes when testing if a PhoneNumber is possible.
        /// </summary>
        public enum ValidationResult
        {
            IS_POSSIBLE,
            INVALID_COUNTRY_CODE,
            TOO_SHORT,
            TOO_LONG,
        }

        /// <summary>
        /// INTERNATIONAL and NATIONAL formats are consistent with the definition in ITU-T Recommendation
        /// E123. For example, the number of the Google Switzerland office will be written as
        /// "+41 44 668 1800" in INTERNATIONAL format, and as "044 668 1800" in NATIONAL format.
        /// E164 format is as per INTERNATIONAL format but with no formatting applied, e.g.
        /// "+41446681800". RFC3966 is as per INTERNATIONAL format, but with all spaces and other
        /// separating symbols replaced with a hyphen, and with any phone number extension appended with
        /// ";ext=". It also will have a prefix of "tel:" added, e.g. "tel:+41-44-668-1800".
        ///
        /// Note: If you are considering storing the number in a neutral format, you are highly advised to
        /// use the PhoneNumber class.
        /// </summary>
        public enum PhoneNumberFormat
        {
            E164,
            INTERNATIONAL,
            NATIONAL,
            RFC3966
        }

        /// <summary>
        /// Types of phone number matches. See detailed description beside the isNumberMatch() method.
        /// </summary>
        public enum MatchType
        {
            NOT_A_NUMBER,
            NO_MATCH,
            SHORT_NSN_MATCH,
            NSN_MATCH,
            EXACT_MATCH,
        }

        public const string REGION_CODE_FOR_NON_GEO_ENTITY = "001";

        static PhoneNumberUtil INSTANCE = new PhoneNumberUtil(_PhoneNumberUtil.getInstance());

        private _PhoneNumberUtil phoneNumberUtil;

        private PhoneNumberUtil(_PhoneNumberUtil phoneNumberUtil)
        {
            this.phoneNumberUtil = phoneNumberUtil;
        }

        /// <summary>
        /// Normalizes a string of characters representing a phone number. This converts wide-ascii and arabic-indic numerals to European numerals, and strips punctuation and alpha characters.
        /// </summary>
        /// <param name="number">a string of characters representing a phone number</param>
        /// <returns>the normalized string version of the phone number</returns>
        public static string NormalizeDigitsOnly(string number)
        {
            return _PhoneNumberUtil.normalizeDigitsOnly(number);
        }

        /// <summary>
        /// Converts all alpha characters in a number to their respective digits on a keypad, but retains existing formatting.
        /// </summary>
        /// <param name="number">a string of characters representing a phone number</param>
        /// <returns>the converted string version of the phone number</returns>
        public static string ConvertAlphaCharactersInNumber(string number)
        {
            return _PhoneNumberUtil.convertAlphaCharactersInNumber(number);
        }

        /// <summary>
        /// Convenience method to get a list of what regions the library has metadata for.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSupportedRegions()
        {
            foreach (var item in phoneNumberUtil.getSupportedRegions())
            {
                yield return (string)item;
            }
        }

        public IEnumerable<int> GetAllDiallingCodes()
        {
            foreach (var item in CountryCodeToRegionCodeMap.getCountryCodeToRegionCodeMap())
            {
                yield return item.Key;
            }
        }

        /// <summary>
        /// Convenience method to get a list of what global network calling codes the library has metadata for.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetSupportedGlobalNetworkCallingCodes()
        {
            foreach (var item in phoneNumberUtil.getSupportedGlobalNetworkCallingCodes())
            {
                yield return (int)item;
            }
        }

        /// <summary>
        /// Gets a {@link PhoneNumberUtil} instance to carry out international phone number formatting,
        /// parsing, or validation. The instance is loaded with phone number metadata for a number of most
        /// commonly used regions.
        /// 
        /// <p>The {@link PhoneNumberUtil} is implemented as a singleton. Therefore, calling getInstance
        /// multiple times will only result in one instance being created.
        /// </summary>
        public static PhoneNumberUtil Instance
        {
            get
            {
                return INSTANCE;
            }
        }

        /// <summary>
        /// Gets a valid number for the specified region.
        /// </summary>
        /// <param name="regionCode">the region for which an example number is needed</param>
        /// <returns>a valid fixed-line number for the specified region. Returns null when the metadata does not contain such information, or the region 001 is passed in. For 001 (representing non-geographical numbers), call {@link #getExampleNumberForNonGeoEntity} instead.</returns>
        public PhoneNumber GetExampleNumber(string regionCode)
        {
            var temp = phoneNumberUtil.getExampleNumber(regionCode);
            return temp == null ? null : new PhoneNumber(temp);
        }

        /// <summary>
        /// Gets a valid number for the specified region and number type.
        /// </summary>
        /// <param name="regionCode">the region for which an example number is needed</param>
        /// <param name="type">the type of number that is needed</param>
        /// <returns>a valid number for the specified region and type. Returns null when the metadata does not contain such information or if an invalid region or region 001 was entered. For 001 (representing non-geographical numbers), call {@link #getExampleNumberForNonGeoEntity} instead.</returns>
        public PhoneNumber GetExampleNumberForType(string regionCode, PhoneNumberType type)
        {
            return phoneNumberUtil.getExampleNumberForType(regionCode, (_PhoneNumberType)type);
        }

        /// <summary>
        /// Gets a valid number for the specified country calling code for a non-geographical entity.
        /// </summary>
        /// <param name="countryCallingCode">the country calling code for a non-geographical entity</param>
        /// <returns>a valid number for the non-geographical entity. Returns null when the metadata does not contain such information, or the country calling code passed in does not belong to a non-geographical entity.</returns>
        public PhoneNumber GetExampleNumberForNonGeoEntity(int countryCallingCode)
        {
            return phoneNumberUtil.getExampleNumberForNonGeoEntity(countryCallingCode);
        }

        /// <summary>
        /// Returns the region code that matches the specific country calling code. In the case of no
        /// region code being found, ZZ will be returned. In the case of multiple regions, the one
        /// designated in the metadata as the "main" region for this calling code will be returned.
        /// </summary>
        /// <param name="countryCallingCode"></param>
        /// <returns></returns>
        public string GetRegionCodeForCountryCode(int countryCallingCode)
        {
            return phoneNumberUtil.getRegionCodeForCountryCode(countryCallingCode);
        }

        /// <summary>
        /// Returns a list with the region codes that match the specific country calling code. For
        /// non-geographical country calling codes, the region code 001 is returned. Also, in the case
        /// of no region code being found, an empty list is returned.
        /// </summary>
        /// <param name="countryCallingCode"></param>
        /// <returns></returns>
        public IEnumerable<string> GetRegionCodesForCountryCode(int countryCallingCode)
        {
            foreach (var item in phoneNumberUtil.getRegionCodesForCountryCode(countryCallingCode))
            {
                yield return (string)item;
            }
        }

        /// <summary>
        /// Returns the country calling code for a specific region. For example, this would be 1 for the
        /// United States, and 64 for New Zealand.
        /// </summary>
        /// <param name="regionCode">the region that we want to get the country calling code for</param>
        /// <returns>the country calling code for the region denoted by regionCode</returns>
        public int GetCountryCodeForRegion(string regionCode)
        {
            return phoneNumberUtil.getCountryCodeForRegion(regionCode);
        }

        /// <summary>
        /// Returns the national dialling prefix for a specific region. For example, this would be 1 for
        /// the United States, and 0 for New Zealand. Set stripNonDigits to true to strip symbols like "~"
        /// (which indicates a wait for a dialling tone) from the prefix returned. If no national prefix is
        /// present, we return null.
        ///
        /// <p>Warning: Do not use this method for do-your-own formatting - for some regions, the
        /// national dialling prefix is used only for certain types of numbers. Use the library's
        /// formatting functions to prefix the national prefix when required.
        /// </summary>
        /// <param name="regionCode">the region that we want to get the dialling prefix for</param>
        /// <param name="stripNonDigits">true to strip non-digits from the national dialling prefix</param>
        /// <returns>the dialling prefix for the region denoted by regionCode</returns>
        public string GetNddPrefixForRegion(string regionCode, bool stripNonDigits)
        {
            return phoneNumberUtil.getNddPrefixForRegion(regionCode, stripNonDigits);
        }

        /// <summary>
        /// Checks if this is a region under the North American Numbering Plan Administration (NANPA).
        /// </summary>
        /// <param name="regionCode"></param>
        /// <returns>true if regionCode is one of the regions under NANPA</returns>
        public bool IsNANPACountry(string regionCode)
        {
            return phoneNumberUtil.isNANPACountry(regionCode);
        }

        /// <summary>
        /// Checks if the number is a valid vanity (alpha) number such as 800 MICROSOFT. A valid vanity
        /// number will start with at least 3 digits and will have three or more alpha characters. This
        /// does not do region-specific checks - to work out if this number is actually valid for a region,
        /// it should be parsed and methods such as {@link #isPossibleNumberWithReason} and
        /// {@link #isValidNumber} should be used.
        /// </summary>
        /// <param name="number">the number that needs to be checked</param>
        /// <returns>true if the number is a valid vanity number</returns>
        public bool IsAlphaNumber(string number)
        {
            return phoneNumberUtil.isAlphaNumber(number);
        }

        /// <summary>
        /// Check whether a phone number is a possible number given a number in the form of a string, and
        /// the region where the number could be dialed from. It provides a more lenient check than
        /// {@link #isValidNumber}. See {@link #isPossibleNumber(PhoneNumber)} for details.
        ///
        /// <p>This method first parses the number, then invokes {@link #isPossibleNumber(PhoneNumber)}
        /// with the resultant PhoneNumber object.
        /// </summary>
        /// <param name="number">the number that needs to be checked, in the form of a string</param>
        /// <param name="regionDialingFrom">the region that we are expecting the number to be dialed from.
        ///     Note this is different from the region where the number belongs.  For example, the number
        ///     +1 650 253 0000 is a number that belongs to US. When written in this form, it can be
        ///     dialed from any region. When it is written as 00 1 650 253 0000, it can be dialed from any
        ///     region which uses an international dialling prefix of 00. When it is written as
        ///     650 253 0000, it can only be dialed from within the US, and when written as 253 0000, it
        ///     can only be dialed from within a smaller area in the US (Mountain View, CA, to be more
        ///     specific).</param>
        /// <returns>true if the number is possible</returns>
        public bool IsPossibleNumber(string number, string regionDialingFrom)
        {
            return phoneNumberUtil.isPossibleNumber(number, regionDialingFrom);
        }

        /// <summary>
        /// Gets an {@link com.google.i18n.phonenumbers.AsYouTypeFormatter} for the specific region.
        /// </summary>
        /// <param name="regionCode">the region where the phone number is being entered</param>
        /// <returns>an {@link com.google.i18n.phonenumbers.AsYouTypeFormatter} object, which can be used
        /// to format phone numbers in the specific region "as you type"</returns>
        public AsYouTypeFormatter GetAsYouTypeFormatter(string regionCode)
        {
            return phoneNumberUtil.getAsYouTypeFormatter(regionCode);
        }

        /// <summary>
        /// Parses a string and returns it in proto buffer format. This method will throw a
        /// {@link com.google.i18n.phonenumbers.NumberParseException} if the number is not considered to be
        /// a possible number. Note that validation of whether the number is actually a valid number for a
        /// particular region is not performed. This can be done separately with {@link #isValidNumber}.
        /// </summary>
        /// <param name="numberToParse">number that we are attempting to parse. This can contain formatting
        ///                          such as +, ( and -, as well as a phone number extension. It can also
        ///                          be provided in RFC3966 format.</param>
        /// <param name="defaultRegion">region that we are expecting the number to be from. This is only used
        ///                          if the number being parsed is not written in international format.
        ///                          The country_code for the number in this case would be stored as that
        ///                          of the default region supplied. If the number is guaranteed to
        ///                          start with a '+' followed by the country calling code, then
        ///                          "ZZ" or null can be supplied.</param>
        /// <returns>a phone number proto buffer filled with the parsed number</returns>
        public PhoneNumber Parse(string numberToParse, string defaultRegion)
        {
            return phoneNumberUtil.parse(numberToParse, defaultRegion);
        }

        /// <summary>
        /// Same as {@link #parse(string, string)}, but accepts mutable PhoneNumber as a parameter to
        /// decrease object creation when invoked many times.
        /// </summary>
        /// <param name="numberToParse"></param>
        /// <param name="defaultRegion"></param>
        /// <param name="phoneNumber"></param>
        public void Parse(string numberToParse, string defaultRegion, PhoneNumber phoneNumber)
        {
            phoneNumberUtil.parse(numberToParse, defaultRegion, phoneNumber);
        }

        /// <summary>
        /// Parses a string and returns it in proto buffer format. This method differs from {@link #parse}
        /// in that it always populates the raw_input field of the protocol buffer with numberToParse as
        /// well as the country_code_source field.
        /// </summary>
        /// <param name="numberToParse">number that we are attempting to parse. This can contain formatting
        ///                          such as +, ( and -, as well as a phone number extension.</param>
        /// <param name="defaultRegion">region that we are expecting the number to be from. This is only used
        ///                          if the number being parsed is not written in international format.
        ///                          The country calling code for the number in this case would be stored
        ///                          as that of the default region supplied.</param>
        /// <returns>a phone number proto buffer filled with the parsed number</returns>
        public PhoneNumber ParseAndKeepRawInput(string numberToParse, string defaultRegion)
        {
            return phoneNumberUtil.parseAndKeepRawInput(numberToParse, defaultRegion);
        }

        /// <summary>
        /// Same as{@link #parseAndKeepRawInput(string, string)}, but accepts a mutable PhoneNumber as
        /// a parameter to decrease object creation when invoked many times.
        /// </summary>
        /// <param name="numberToParse"></param>
        /// <param name="defaultRegion"></param>
        /// <param name="phoneNumber"></param>
        public void ParseAndKeepRawInput(string numberToParse, string defaultRegion, PhoneNumber phoneNumber)
        {
            phoneNumberUtil.parseAndKeepRawInput(numberToParse, defaultRegion, phoneNumber);
        }

        /// <summary>
        /// Returns an iterable over all {@link PhoneNumberMatch PhoneNumberMatches} in {@code text}. This
        /// is a shortcut for {@link #findNumbers(CharSequence, string, Leniency, long)
        /// getMatcher(text, defaultRegion, Leniency.VALID, Long.MAX_VALUE)}.
        /// </summary>
        /// <param name="text">the text to search for phone numbers, null for no text</param>
        /// <param name="defaultRegion">region that we are expecting the number to be from. This is only used
        ///                          if the number being parsed is not written in international format. The
        ///                          country_code for the number in this case would be stored as that of
        ///                          the default region supplied. May be null if only international
        ///                          numbers are expected.</param>
        /// <returns></returns>
        public IEnumerable<PhoneNumberMatch> FindNumbers(string text, string defaultRegion)
        {
            foreach (var item in phoneNumberUtil.findNumbers(text, defaultRegion))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Returns an iterable over all {@link PhoneNumberMatch PhoneNumberMatches} in {@code text}.
        /// </summary>
        /// <param name="text">the text to search for phone numbers, null for no text</param>
        /// <param name="defaultRegion">region that we are expecting the number to be from. This is only used
        ///                          if the number being parsed is not written in international format. The
        ///                          country_code for the number in this case would be stored as that of
        ///                          the default region supplied. May be null if only international
        ///                          numbers are expected.</param>
        /// <param name="leniency">the leniency to use when evaluating candidate phone numbers</param>
        /// <param name="maxTries">the maximum number of invalid numbers to try before giving up on the
        ///                          text. This is to cover degenerate cases where the text has a lot of
        ///                          false positives in it. Must be {@code >= 0}.</param>
        /// <returns></returns>
        public IEnumerable<PhoneNumberMatch> FindNumbers(string text, string defaultRegion, Leniency leniency, long maxTries)
        {
            _PhoneNumberUtil.Leniency temp;
            switch (leniency)
            {
                case Leniency.POSSIBLE:
                    temp = _Leniency.POSSIBLE;
                    break;
                case Leniency.VALID:
                    temp = _Leniency.VALID;
                    break;
                case Leniency.STRICT_GROUPING:
                    temp = _Leniency.STRICT_GROUPING;
                    break;
                case Leniency.EXACT_GROUPING:
                    temp = _Leniency.EXACT_GROUPING;
                    break;
                default:
                    throw new NotSupportedException();
            }

            foreach (var item in phoneNumberUtil.findNumbers(text, defaultRegion, temp, maxTries))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Takes two phone numbers and compares them for equality.
        ///
        /// <p>Returns EXACT_MATCH if the country_code, NSN, presence of a leading zero for Italian numbers
        /// and any extension present are the same.
        /// Returns NSN_MATCH if either or both has no region specified, and the NSNs and extensions are
        /// the same.
        /// Returns SHORT_NSN_MATCH if either or both has no region specified, or the region specified is
        /// the same, and one NSN could be a shorter version of the other number. This includes the case
        /// where one has an extension specified, and the other does not.
        /// Returns NO_MATCH otherwise.
        /// For example, the numbers +1 345 657 1234 and 657 1234 are a SHORT_NSN_MATCH.
        /// The numbers +1 345 657 1234 and 345 657 are a NO_MATCH.
        /// </summary>
        /// <param name="firstNumberIn">first number to compare</param>
        /// <param name="secondNumberIn">second number to compare</param>
        /// <returns>NO_MATCH, SHORT_NSN_MATCH, NSN_MATCH or EXACT_MATCH depending on the level of equality of the two numbers, described in the method definition.</returns>
        public MatchType IsNumberMatch(PhoneNumber firstNumberIn, PhoneNumber secondNumberIn)
        {
            return (MatchType)phoneNumberUtil.isNumberMatch(firstNumberIn, secondNumberIn);
        }

        /// <summary>
        /// Takes two phone numbers as strings and compares them for equality. This is a convenience
        /// Takes two phone numbers as strings and compares them for equality. This is a convenience wrapper for {@link #isNumberMatch(PhoneNumber, PhoneNumber)}. No default region is known.
        /// </summary>
        /// <param name="firstNumber">first number to compare. Can contain formatting, and can have country
        ///     calling code specified with + at the start.</param>
        /// <param name="secondNumber">second number to compare. Can contain formatting, and can have country
        ///     calling code specified with + at the start.</param>
        /// <returns>NOT_A_NUMBER, NO_MATCH, SHORT_NSN_MATCH, NSN_MATCH, EXACT_MATCH. See
        ///     {@link #isNumberMatch(PhoneNumber, PhoneNumber)} for more details.</returns>
        public MatchType IsNumberMatch(string firstNumber, string secondNumber)
        {
            return (MatchType)phoneNumberUtil.isNumberMatch(firstNumber, secondNumber);
        }

        /// <summary>
        /// Takes two phone numbers and compares them for equality. This is a convenience wrapper for
        /// {@link #isNumberMatch(PhoneNumber, PhoneNumber)}. No default region is known.
        /// </summary>
        /// <param name="firstNumber">first number to compare in proto buffer format.</param>
        /// <param name="secondNumber">second number to compare. Can contain formatting, and can have country
        ///     calling code specified with + at the start.</param>
        /// <returns>NOT_A_NUMBER, NO_MATCH, SHORT_NSN_MATCH, NSN_MATCH, EXACT_MATCH. See
        ///     {@link #isNumberMatch(PhoneNumber, PhoneNumber)} for more details.</returns>
        public MatchType IsNumberMatch(PhoneNumber firstNumber, string secondNumber)
        {
            return (MatchType)phoneNumberUtil.isNumberMatch(firstNumber, secondNumber);
        }

        public string GetRegionCodeForNumber(string number)
        {
            return phoneNumberUtil.GetRegionCodeForNumber(number);
        }
    }
}
