/*
 * Copyright (C) 2009 The Libphonenumber Authors
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

namespace com.google.i18n.phonenumbers {

using PhoneNumberType = com.google.i18n.phonenumbers.PhoneNumberUtil.PhoneNumberType;
using PhoneNumber = com.google.i18n.phonenumbers.Phonenumber.PhoneNumber;
using NumberFormat = com.google.i18n.phonenumbers.Phonemetadata.NumberFormat;
using PhoneMetadata = com.google.i18n.phonenumbers.Phonemetadata.PhoneMetadata;
using CountryCodeSource = com.google.i18n.phonenumbers.Phonenumber.PhoneNumber.CountryCodeSource;
using MatchType = com.google.i18n.phonenumbers.PhoneNumberUtil.MatchType;
using PhoneNumberFormat = com.google.i18n.phonenumbers.PhoneNumberUtil.PhoneNumberFormat;
using PhoneMetadataCollection = com.google.i18n.phonenumbers.Phonemetadata.PhoneMetadataCollection;
using PhoneNumberDesc = com.google.i18n.phonenumbers.Phonemetadata.PhoneNumberDesc;
using Leniency = com.google.i18n.phonenumbers.PhoneNumberUtil.Leniency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.google.i18n.phonenumbers;
using Test;
using java.lang;
using java.util.logging;
using java.util;






/**
 * @author Lara Rennie
 *
 * Verifies all of the example numbers in the metadata are valid and of the correct type. If no
 * example number exists for a particular type, the test still passes.
 */
[TestClass] public class ExampleNumbersTest : TestCase {
  private static readonly Logger LOGGER  = Logger.getLogger("ExampleNumbersTest");
  private PhoneNumberUtil phoneNumberUtil;
  private ShortNumberInfo shortNumberInfo;
  private List<PhoneNumber> invalidCases = new ArrayList<PhoneNumber>();
  private List<PhoneNumber> wrongTypeCases = new ArrayList<PhoneNumber>();

  public ExampleNumbersTest() {
    PhoneNumberUtil.resetInstance();
    phoneNumberUtil = PhoneNumberUtil.getInstance();
    shortNumberInfo = new ShortNumberInfo(phoneNumberUtil);
  }

  /**
   * @param exampleNumberRequestedType  type we are requesting an example number for
   * @param possibleExpectedTypes       acceptable types that this number should match, such as
   *     FIXED_LINE and FIXED_LINE_OR_MOBILE for a fixed line example number.
   */
  private void checkNumbersValidAndCorrectType(PhoneNumberType exampleNumberRequestedType,
                                               Set<PhoneNumberType> possibleExpectedTypes) {
    foreach(String regionCode in phoneNumberUtil.getSupportedRegions()) {
      PhoneNumber exampleNumber =
          phoneNumberUtil.getExampleNumberForType(regionCode, exampleNumberRequestedType);
      if (exampleNumber != null) {
        if (!phoneNumberUtil.isValidNumber(exampleNumber)) {
          invalidCases.add(exampleNumber);
          LOGGER.log(Level.SEVERE, "Failed validation for " + exampleNumber.toString());
        } else {
          // We know the number is valid, now we check the type.
          PhoneNumberType exampleNumberType = phoneNumberUtil.getNumberType(exampleNumber);
          if (!possibleExpectedTypes.contains(exampleNumberType)) {
            wrongTypeCases.add(exampleNumber);
            LOGGER.log(Level.SEVERE, "Wrong type for " +
                       exampleNumber.toString() +
                       ": got " + exampleNumberType);
            LOGGER.log(Level.WARNING, "Expected types: ");
            foreach(PhoneNumberType type in possibleExpectedTypes) {
              LOGGER.log(Level.WARNING, type.ToString());
            }
          }
        }
      }
    }
  }

  [TestMethod] public void testFixedLine() {
    Set<PhoneNumberType> fixedLineTypes = EnumSet.of(PhoneNumberType.FIXED_LINE,
                                                     PhoneNumberType.FIXED_LINE_OR_MOBILE);
    checkNumbersValidAndCorrectType(PhoneNumberType.FIXED_LINE, fixedLineTypes);
    assertEquals(0, invalidCases.size());
    assertEquals(0, wrongTypeCases.size());
  }

  [TestMethod] public void testMobile() {
    Set<PhoneNumberType> mobileTypes = EnumSet.of(PhoneNumberType.MOBILE,
                                                  PhoneNumberType.FIXED_LINE_OR_MOBILE);
    checkNumbersValidAndCorrectType(PhoneNumberType.MOBILE, mobileTypes);
    assertEquals(0, invalidCases.size());
    assertEquals(0, wrongTypeCases.size());
  }

  [TestMethod] public void testTollFree() {
    Set<PhoneNumberType> tollFreeTypes = EnumSet.of(PhoneNumberType.TOLL_FREE);
    checkNumbersValidAndCorrectType(PhoneNumberType.TOLL_FREE, tollFreeTypes);
    assertEquals(0, invalidCases.size());
    assertEquals(0, wrongTypeCases.size());
  }

  [TestMethod] public void testPremiumRate() {
    Set<PhoneNumberType> premiumRateTypes = EnumSet.of(PhoneNumberType.PREMIUM_RATE);
    checkNumbersValidAndCorrectType(PhoneNumberType.PREMIUM_RATE, premiumRateTypes);
    assertEquals(0, invalidCases.size());
    assertEquals(0, wrongTypeCases.size());
  }

  [TestMethod] public void testVoip() {
    Set<PhoneNumberType> voipTypes = EnumSet.of(PhoneNumberType.VOIP);
    checkNumbersValidAndCorrectType(PhoneNumberType.VOIP, voipTypes);
    assertEquals(0, invalidCases.size());
    assertEquals(0, wrongTypeCases.size());
  }

  [TestMethod] public void testPager() {
    Set<PhoneNumberType> pagerTypes = EnumSet.of(PhoneNumberType.PAGER);
    checkNumbersValidAndCorrectType(PhoneNumberType.PAGER, pagerTypes);
    assertEquals(0, invalidCases.size());
    assertEquals(0, wrongTypeCases.size());
  }

  [TestMethod] public void testUan() {
    Set<PhoneNumberType> uanTypes = EnumSet.of(PhoneNumberType.UAN);
    checkNumbersValidAndCorrectType(PhoneNumberType.UAN, uanTypes);
    assertEquals(0, invalidCases.size());
    assertEquals(0, wrongTypeCases.size());
  }

  [TestMethod] public void testVoicemail() {
    Set<PhoneNumberType> voicemailTypes = EnumSet.of(PhoneNumberType.VOICEMAIL);
    checkNumbersValidAndCorrectType(PhoneNumberType.VOICEMAIL, voicemailTypes);
    assertEquals(0, invalidCases.size());
    assertEquals(0, wrongTypeCases.size());
  }

  [TestMethod] public void testSharedCost() {
    Set<PhoneNumberType> sharedCostTypes = EnumSet.of(PhoneNumberType.SHARED_COST);
    checkNumbersValidAndCorrectType(PhoneNumberType.SHARED_COST, sharedCostTypes);
    assertEquals(0, invalidCases.size());
    assertEquals(0, wrongTypeCases.size());
  }

  [TestMethod] public void testCanBeInternationallyDialled() {
    foreach(String regionCode in phoneNumberUtil.getSupportedRegions()) {
      PhoneNumber exampleNumber = null;
      PhoneNumberDesc desc =
          phoneNumberUtil.getMetadataForRegion(regionCode).getNoInternationalDialling();
      try {
        if (desc.hasExampleNumber()) {
          exampleNumber = phoneNumberUtil.parse(desc.getExampleNumber(), regionCode);
        }
      } catch (NumberParseException e) {
        LOGGER.log(Level.SEVERE, e.toString());
      }
      if (exampleNumber != null && phoneNumberUtil.canBeInternationallyDialled(exampleNumber)) {
        wrongTypeCases.add(exampleNumber);
        LOGGER.log(Level.SEVERE, "Number " + exampleNumber.toString()
                   + " should not be internationally diallable");
      }
    }
    assertEquals(0, wrongTypeCases.size());
  }

  [TestMethod] public void testEmergency() {
    int wrongTypeCounter = 0;
    foreach(String regionCode in shortNumberInfo.getSupportedRegions()) {
      if (regionCode == RegionCode.PG) {
        // The only short number for Papua New Guinea is 000, which fails the test, since the
        // national prefix is 0. This needs to be fixed.
        continue;
      }
      PhoneNumberDesc desc =
          MetadataManager.getShortNumberMetadataForRegion(regionCode).getEmergency();
      if (desc.hasExampleNumber()) {
        String exampleNumber = desc.getExampleNumber();
        if (!exampleNumber.matches(desc.getPossibleNumberPattern()) ||
            !shortNumberInfo.isEmergencyNumber(exampleNumber, regionCode)) {
          wrongTypeCounter++;
          LOGGER.log(Level.SEVERE, "Emergency example number test failed for " + regionCode);
        } else {
          PhoneNumber emergencyNumber = phoneNumberUtil.parse(exampleNumber, regionCode);
          if (shortNumberInfo.getExpectedCost(emergencyNumber) !=
                  ShortNumberInfo.ShortNumberCost.TOLL_FREE) {
            wrongTypeCounter++;
            LOGGER.log(Level.SEVERE, "Emergency example number not toll free for " + regionCode);
          }
        }
      }
    }
    assertEquals(0, wrongTypeCounter);
  }

  [TestMethod] public void testGlobalNetworkNumbers() {
    foreach(Integer callingCode in phoneNumberUtil.getSupportedGlobalNetworkCallingCodes()) {
      PhoneNumber exampleNumber =
          phoneNumberUtil.getExampleNumberForNonGeoEntity(callingCode);
      assertNotNull("No example phone number for calling code " + callingCode, exampleNumber);
      if (!phoneNumberUtil.isValidNumber(exampleNumber)) {
        invalidCases.add(exampleNumber);
        LOGGER.log(Level.SEVERE, "Failed validation for " + exampleNumber.toString());
      }
    }
    assertEquals(0, invalidCases.size());
  }

  [TestMethod] public void testEveryRegionHasAnExampleNumber() {
    foreach(String regionCode in phoneNumberUtil.getSupportedRegions()) {
      PhoneNumber exampleNumber = phoneNumberUtil.getExampleNumber(regionCode);
      assertNotNull("None found for region " + regionCode, exampleNumber);
    }
  }

  [TestMethod] public void testShortNumbersValidAndCorrectCost() {
    List<String> invalidStringCases = new ArrayList<String>();
    foreach(String regionCode in shortNumberInfo.getSupportedRegions()) {
      if (regionCode == RegionCode.PG) {
        // The only short number for Papua New Guinea is 000, which fails the test, since the
        // national prefix is 0. This needs to be fixed.
        continue;
      }
      String exampleShortNumber = shortNumberInfo.getExampleShortNumber(regionCode);
      if (!shortNumberInfo.isValidShortNumber(exampleShortNumber, regionCode)) {
        String invalidStringCase = "region_code: " + regionCode + ", national_number: " +
            exampleShortNumber;
        invalidStringCases.add(invalidStringCase);
        LOGGER.log(Level.SEVERE, "Failed validation for string " + invalidStringCase);
      }
      PhoneNumber phoneNumber = phoneNumberUtil.parse(exampleShortNumber, regionCode);
      if (!shortNumberInfo.isValidShortNumber(phoneNumber)) {
        invalidCases.add(phoneNumber);
        LOGGER.log(Level.SEVERE, "Failed validation for " + phoneNumber.toString());
      }

      foreach(ShortNumberInfo.ShortNumberCost cost in System.Enum.GetValues(typeof(ShortNumberInfo.ShortNumberCost))) {
        exampleShortNumber = shortNumberInfo.getExampleShortNumberForCost(regionCode, cost);
        if (!exampleShortNumber.equals("")) {
          phoneNumber = phoneNumberUtil.parse(exampleShortNumber, regionCode);
          if (cost != shortNumberInfo.getExpectedCost(phoneNumber)) {
            wrongTypeCases.add(phoneNumber);
            LOGGER.log(Level.SEVERE, "Wrong cost for " + phoneNumber.toString());
          }
        }
      }
    }
    assertEquals(0, invalidStringCases.size());
    assertEquals(0, invalidCases.size());
    assertEquals(0, wrongTypeCases.size());
  }
}

}