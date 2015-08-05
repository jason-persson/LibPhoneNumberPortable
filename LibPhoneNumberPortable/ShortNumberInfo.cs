using _ShortNumberInfo = com.google.i18n.phonenumbers.ShortNumberInfo;

namespace libphonenumber
{

    /// <summary>
    /// Methods for getting information about short phone numbers, such as short codes and emergency
    /// numbers. Note that most commercial short numbers are not handled here, but by the
    /// {@link PhoneNumberUtil}.
    /// </summary>
    public class ShortNumberInfo
    {
        /// <summary>
        /// Cost categories of short numbers.
        /// </summary>
        public enum ShortNumberCost
        {
            TOLL_FREE,
            STANDARD_RATE,
            PREMIUM_RATE,
            UNKNOWN_COST
        }

        static ShortNumberInfo INSTANCE = new ShortNumberInfo(_ShortNumberInfo.getInstance());

        private _ShortNumberInfo shortNumberInfo;

        private ShortNumberInfo(_ShortNumberInfo shortNumberInfo)
        {
            this.shortNumberInfo = shortNumberInfo;
        }

        /// <summary>
        /// Returns the singleton instance of the ShortNumberInfo.
        /// </summary>
        public static ShortNumberInfo Instance
        {
            get
            {
                return INSTANCE;
            }
        }

        /// <summary>
        /// Check whether a short number is a possible number, given the number in the form of a string,
        /// and the region where the number is dialed from. This provides a more lenient check than
        /// {@link #isValidShortNumber}.
        /// </summary>
        /// <param name="shortNumber">the short number to check as a string</param>
        /// <param name="regionDialingFrom">the region from which the number is dialed</param>
        /// <returns>whether the number is a possible short number</returns>
        public bool IsPossibleShortNumber(string shortNumber, string regionDialingFrom)
        {
            return shortNumberInfo.isPossibleShortNumber(shortNumber, regionDialingFrom);
        }

        /// <summary>
        /// Check whether a short number is a possible number. This provides a more lenient check than
        /// {@link #isValidShortNumber}. See {@link #isPossibleShortNumber(String, String)} for
        /// details.
        /// </summary>
        /// <param name="number">the short number to check</param>
        /// <returns>whether the number is a possible short number</returns>
        public bool IsPossibleShortNumber(PhoneNumber number)
        {
            return shortNumberInfo.isPossibleShortNumber(number._inner);
        }

        /// <summary>
        /// Tests whether a short number matches a valid pattern. Note that this doesn't verify the number
        /// is actually in use, which is impossible to tell by just looking at the number itself.
        /// </summary>
        /// <param name="shortNumber">the short number to check as a string</param>
        /// <param name="regionDialingFrom">the region from which the number is dialed</param>
        /// <returns>whether the short number matches a valid pattern</returns>
        public bool IsValidShortNumber(string shortNumber, string regionDialingFrom)
        {
            return shortNumberInfo.isValidShortNumber(shortNumber, regionDialingFrom);
        }

        /// <summary>
        /// Tests whether a short number matches a valid pattern. Note that this doesn't verify the number
        /// is actually in use, which is impossible to tell by just looking at the number itself. See
        /// {@link #isValidShortNumber(String, String)} for details.
        /// </summary>
        /// <param name="number">the short number for which we want to test the validity</param>
        /// <returns>whether the short number matches a valid pattern</returns>
        public bool IsValidShortNumber(PhoneNumber number)
        {
            return shortNumberInfo.isValidShortNumber(number._inner);
        }

        /// <summary>
        /// Gets the expected cost category of a short number (however, nothing is implied about its
        /// validity). If it is important that the number is valid, then its validity must first be checked
        /// using {@link isValidShortNumber}. Note that emergency numbers are always considered toll-free.
        /// Example usage:
        /// <pre>{@code
        /// PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance();
        /// ShortNumberInfo shortInfo = ShortNumberInfo.getInstance();
        /// PhoneNumber number = phoneUtil.parse("110", "FR");
        /// if (shortInfo.isValidShortNumber(number)) {
        ///   ShortNumberInfo.ShortNumberCost cost = shortInfo.getExpectedCost(number);
        ///   // Do something with the cost information here.
        /// }}</pre>
        /// </summary>
        /// <param name="number">the short number for which we want to know the expected cost category</param>
        /// <returns>the expected cost category of the short number. Returns UNKNOWN_COST if the number does
        ///     not match a cost category. Note that an invalid number may match any cost category.</returns>
        public ShortNumberCost GetExpectedCost(PhoneNumber number)
        {
            return (ShortNumberCost)shortNumberInfo.getExpectedCost(number._inner);
        }

        /// <summary>
        /// Returns true if the number might be used to connect to an emergency service in the given region.
        /// This method takes into account cases where the number might contain formatting, or might have additional digits appended (when it is okay to do that in the region specified).
        /// </summary>
        /// <param name="number">the phone number to test</param>
        /// <param name="regionCode">the region where the phone number is being dialed</param>
        /// <returns>whether the number might be used to connect to an emergency service in the given region</returns>
        public bool ConnectsToEmergencyNumber(string number, string regionCode)
        {
            return shortNumberInfo.connectsToEmergencyNumber(number, regionCode);
        }

        /// <summary>
        /// Returns true if the number exactly matches an emergency service number in the given region.
        /// 
        /// This method takes into account cases where the number might contain formatting, but doesn't
        /// allow additional digits to be appended.
        /// </summary>
        /// <param name="number">the phone number to test</param>
        /// <param name="regionCode">the region where the phone number is being dialed</param>
        /// <returns>whether the number exactly matches an emergency services number in the given region</returns>
        public bool IsEmergencyNumber(string number, string regionCode)
        {
            return shortNumberInfo.isEmergencyNumber(number, regionCode);
        }

        /// <summary>
        /// Given a valid short number, determines whether it is carrier-specific (however, nothing is
        /// implied about its validity). If it is important that the number is valid, then its validity
        /// must first be checked using {@link isValidShortNumber}.
        /// </summary>
        /// <param name="number">the valid short number to check</param>
        /// <returns>whether the short number is carrier-specific (assuming the input was a valid short number).</returns>
        public bool IsCarrierSpecific(PhoneNumber number)
        {
            return shortNumberInfo.isCarrierSpecific(number._inner);
        }
    }
}