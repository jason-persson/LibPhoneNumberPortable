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
 * Tests for the Phonenumber.PhoneNumber object itself.
 *
 * @author Lara Rennie
 */
[TestClass] public class PhonenumberTest : TestCase {

  [TestMethod] public void testEqualSimpleNumber() {
    PhoneNumber numberA = new PhoneNumber();
    numberA.setCountryCode(1).setNationalNumber(6502530000L);

    PhoneNumber numberB = new PhoneNumber();
    numberB.setCountryCode(1).setNationalNumber(6502530000L);

    assertEquals(numberA, numberB);
    assertEquals(numberA.hashCode(), numberB.hashCode());
  }

  [TestMethod] public void testEqualWithItalianLeadingZeroSetToDefault() {
    PhoneNumber numberA = new PhoneNumber();
    numberA.setCountryCode(1).setNationalNumber(6502530000L).setItalianLeadingZero(false);

    PhoneNumber numberB = new PhoneNumber();
    numberB.setCountryCode(1).setNationalNumber(6502530000L);

    // These should still be equal, since the default value for this field is false.
    assertEquals(numberA, numberB);
    assertEquals(numberA.hashCode(), numberB.hashCode());
  }

  [TestMethod] public void testEqualWithCountryCodeSourceSet() {
    PhoneNumber numberA = new PhoneNumber();
    numberA.setRawInput("+1 650 253 00 00").
        setCountryCodeSource(CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN);
    PhoneNumber numberB = new PhoneNumber();
    numberB.setRawInput("+1 650 253 00 00").
        setCountryCodeSource(CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN);
    assertEquals(numberA, numberB);
    assertEquals(numberA.hashCode(), numberB.hashCode());
  }

  [TestMethod] public void testNonEqualWithItalianLeadingZeroSetToTrue() {
    PhoneNumber numberA = new PhoneNumber();
    numberA.setCountryCode(1).setNationalNumber(6502530000L).setItalianLeadingZero(true);

    PhoneNumber numberB = new PhoneNumber();
    numberB.setCountryCode(1).setNationalNumber(6502530000L);

    assertFalse(numberA.equals(numberB));
    assertFalse(numberA.hashCode() == numberB.hashCode());
  }

  [TestMethod] public void testNonEqualWithDifferingRawInput() {
    PhoneNumber numberA = new PhoneNumber();
    numberA.setCountryCode(1).setNationalNumber(6502530000L).setRawInput("+1 650 253 00 00").
        setCountryCodeSource(CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN);

    PhoneNumber numberB = new PhoneNumber();
    // Although these numbers would pass an isNumberMatch test, they are not considered "equal" as
    // objects, since their raw input is different.
    numberB.setCountryCode(1).setNationalNumber(6502530000L).setRawInput("+1-650-253-00-00").
        setCountryCodeSource(CountryCodeSource.FROM_NUMBER_WITH_PLUS_SIGN);

    assertFalse(numberA.equals(numberB));
    assertFalse(numberA.hashCode() == numberB.hashCode());
  }

  [TestMethod] public void testNonEqualWithPreferredDomesticCarrierCodeSetToDefault() {
    PhoneNumber numberA = new PhoneNumber();
    numberA.setCountryCode(1).setNationalNumber(6502530000L).setPreferredDomesticCarrierCode("");

    PhoneNumber numberB = new PhoneNumber();
    numberB.setCountryCode(1).setNationalNumber(6502530000L);

    assertFalse(numberA.equals(numberB));
    assertFalse(numberA.hashCode() == numberB.hashCode());
  }

  [TestMethod] public void testEqualWithPreferredDomesticCarrierCodeSetToDefault() {
    PhoneNumber numberA = new PhoneNumber();
    numberA.setCountryCode(1).setNationalNumber(6502530000L).setPreferredDomesticCarrierCode("");

    PhoneNumber numberB = new PhoneNumber();
    numberB.setCountryCode(1).setNationalNumber(6502530000L).setPreferredDomesticCarrierCode("");

    assertEquals(numberA, numberB);
    assertEquals(numberA.hashCode(), numberB.hashCode());
  }
}

}