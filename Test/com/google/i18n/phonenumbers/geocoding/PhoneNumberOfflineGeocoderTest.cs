/*
 * Copyright (C) 2011 The Libphonenumber Authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using PhoneNumberType = com.google.i18n.phonenumbers.PhoneNumberUtil.PhoneNumberType;
using PhoneNumber = com.google.i18n.phonenumbers.Phonenumber.PhoneNumber;
using NumberFormat = com.google.i18n.phonenumbers.Phonemetadata.NumberFormat;
using PhoneMetadata = com.google.i18n.phonenumbers.Phonemetadata.PhoneMetadata;
using CountryCodeSource = com.google.i18n.phonenumbers.Phonenumber.PhoneNumber.CountryCodeSource;
using MatchType = com.google.i18n.phonenumbers.PhoneNumberUtil.MatchType;
using PhoneNumberFormat = com.google.i18n.phonenumbers.PhoneNumberUtil.PhoneNumberFormat;
using PhoneMetadataCollection = com.google.i18n.phonenumbers.Phonemetadata.PhoneMetadataCollection;
using PhoneNumberDesc = com.google.i18n.phonenumbers.Phonemetadata.PhoneNumberDesc;
using com.google.i18n.phonenumbers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using java.lang;
using java.util;
using com.google.i18n.phonenumbers.geocoding;

namespace Test
{
/**
 * Unit tests for PhoneNumberOfflineGeocoder.java
 *
 * @author Shaopeng Jia
 */
[TestClass]
public class PhoneNumberOfflineGeocoderTest : TestCase {
  private readonly PhoneNumberOfflineGeocoder geocoder =
      new PhoneNumberOfflineGeocoder(TEST_MAPPING_DATA_DIRECTORY);
  private static readonly String TEST_MAPPING_DATA_DIRECTORY =
      "/com/google/i18n/phonenumbers/geocoding/testing_data/";

  // Set up some test numbers to re-use.
  private static readonly PhoneNumber KO_NUMBER1 =
      new PhoneNumber().setCountryCode(82).setNationalNumber(22123456L);
  private static readonly PhoneNumber KO_NUMBER2 =
      new PhoneNumber().setCountryCode(82).setNationalNumber(322123456L);
  private static readonly PhoneNumber KO_NUMBER3 =
      new PhoneNumber().setCountryCode(82).setNationalNumber(6421234567L);
  private static readonly PhoneNumber KO_INVALID_NUMBER =
      new PhoneNumber().setCountryCode(82).setNationalNumber(1234L);
  private static readonly PhoneNumber US_NUMBER1 =
      new PhoneNumber().setCountryCode(1).setNationalNumber(6502530000L);
  private static readonly PhoneNumber US_NUMBER2 =
      new PhoneNumber().setCountryCode(1).setNationalNumber(6509600000L);
  private static readonly PhoneNumber US_NUMBER3 =
      new PhoneNumber().setCountryCode(1).setNationalNumber(2128120000L);
  private static readonly PhoneNumber US_NUMBER4 =
      new PhoneNumber().setCountryCode(1).setNationalNumber(6174240000L);
  private static readonly PhoneNumber US_INVALID_NUMBER =
      new PhoneNumber().setCountryCode(1).setNationalNumber(123456789L);
  private static readonly PhoneNumber BS_NUMBER1 =
      new PhoneNumber().setCountryCode(1).setNationalNumber(2423651234L);
  private static readonly PhoneNumber AU_NUMBER =
      new PhoneNumber().setCountryCode(61).setNationalNumber(236618300L);
  private static readonly PhoneNumber NUMBER_WITH_INVALID_COUNTRY_CODE =
      new PhoneNumber().setCountryCode(999).setNationalNumber(2423651234L);
  private static readonly PhoneNumber INTERNATIONAL_TOLL_FREE =
      new PhoneNumber().setCountryCode(800).setNationalNumber(12345678L);

  [TestMethod] public void testGetDescriptionForNumberWithNoDataFile() {
    // No data file containing mappings for US numbers is available in Chinese for the unittests. As
    // a result, the country name of United States in simplified Chinese is returned.
    assertEquals("\u7F8E\u56FD",
        geocoder.getDescriptionForNumber(US_NUMBER1, Locale.SIMPLIFIED_CHINESE));
    assertEquals("Bahamas",
        geocoder.getDescriptionForNumber(BS_NUMBER1, new Locale("en", "US")));
    assertEquals("Australia",
        geocoder.getDescriptionForNumber(AU_NUMBER, new Locale("en", "US")));
    assertEquals("", geocoder.getDescriptionForNumber(NUMBER_WITH_INVALID_COUNTRY_CODE,
                                                      new Locale("en", "US")));
    assertEquals("", geocoder.getDescriptionForNumber(INTERNATIONAL_TOLL_FREE,
                                                      new Locale("en", "US")));
  }

  [TestMethod] public void testGetDescriptionForNumberWithMissingPrefix() {
    // Test that the name of the country is returned when the number passed in is valid but not
    // covered by the geocoding data file.
    assertEquals("United States",
        geocoder.getDescriptionForNumber(US_NUMBER4, new Locale("en", "US")));
  }

  [TestMethod] public void testGetDescriptionForNumber_en_US() {
    assertEquals("CA",
        geocoder.getDescriptionForNumber(US_NUMBER1, new Locale("en", "US")));
    assertEquals("Mountain View, CA",
        geocoder.getDescriptionForNumber(US_NUMBER2, new Locale("en", "US")));
    assertEquals("New York, NY",
        geocoder.getDescriptionForNumber(US_NUMBER3, new Locale("en", "US")));
  }

  [TestMethod] public void testGetDescriptionForKoreanNumber() {
    assertEquals("Seoul",
        geocoder.getDescriptionForNumber(KO_NUMBER1, Locale.ENGLISH));
    assertEquals("Incheon",
        geocoder.getDescriptionForNumber(KO_NUMBER2, Locale.ENGLISH));
    assertEquals("Jeju",
        geocoder.getDescriptionForNumber(KO_NUMBER3, Locale.ENGLISH));
    assertEquals("\uC11C\uC6B8",
        geocoder.getDescriptionForNumber(KO_NUMBER1, Locale.KOREAN));
    assertEquals("\uC778\uCC9C",
        geocoder.getDescriptionForNumber(KO_NUMBER2, Locale.KOREAN));
  }

  [TestMethod] public void testGetDescriptionForFallBack() {
    // No fallback, as the location name for the given phone number is available in the requested
    // language.
    assertEquals("Kalifornien",
        geocoder.getDescriptionForNumber(US_NUMBER1, Locale.GERMAN));
    // German falls back to English.
    assertEquals("New York, NY",
        geocoder.getDescriptionForNumber(US_NUMBER3, Locale.GERMAN));
    // Italian falls back to English.
    assertEquals("CA",
        geocoder.getDescriptionForNumber(US_NUMBER1, Locale.ITALIAN));
    // Korean doesn't fall back to English.
    assertEquals("\uB300\uD55C\uBBFC\uAD6D",
        geocoder.getDescriptionForNumber(KO_NUMBER3, Locale.KOREAN));
  }

  [TestMethod] public void testGetDescriptionForNumberWithUserRegion() {
    // User in Italy, American number. We should just show United States, in Spanish, and not more
    // detailed information.
    assertEquals("Estados Unidos",
        geocoder.getDescriptionForNumber(US_NUMBER1, new Locale("es", "ES"), "IT"));
    // Unknown region - should just show country name.
    assertEquals("Estados Unidos",
        geocoder.getDescriptionForNumber(US_NUMBER1, new Locale("es", "ES"), "ZZ"));
    // User in the States, language German, should show detailed data.
    assertEquals("Kalifornien",
        geocoder.getDescriptionForNumber(US_NUMBER1, Locale.GERMAN, "US"));
    // User in the States, language French, no data for French, so we fallback to English detailed
    // data.
    assertEquals("CA",
        geocoder.getDescriptionForNumber(US_NUMBER1, Locale.FRENCH, "US"));
    // Invalid number - return an empty string.
    assertEquals("", geocoder.getDescriptionForNumber(US_INVALID_NUMBER, Locale.ENGLISH,
                                                      "US"));
  }

  [TestMethod] public void testGetDescriptionForInvalidNumber() {
    assertEquals("", geocoder.getDescriptionForNumber(KO_INVALID_NUMBER, Locale.ENGLISH));
    assertEquals("", geocoder.getDescriptionForNumber(US_INVALID_NUMBER, Locale.ENGLISH));
  }
}
}