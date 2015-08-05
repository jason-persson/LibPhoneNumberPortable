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
 * Tests for {@link PhoneNumberMatch}.
 *
 * @author Tom Hofmann
 */
[TestClass] public class PhoneNumberMatchTest : TestCase {
  /**
   * Tests the value type semantics. Equality and hash code must be based on the covered range and
   * corresponding phone number. Range and number correctness are tested by
   * {@link PhoneNumberMatcherTest}.
   */
  [TestMethod] public void testValueTypeSemantics() {
    PhoneNumber number = new PhoneNumber();
    PhoneNumberMatch match1 = new PhoneNumberMatch(10, "1 800 234 45 67", number);
    PhoneNumberMatch match2 = new PhoneNumberMatch(10, "1 800 234 45 67", number);

    assertEquals(match1, match2);
    assertEquals(match1.hashCode(), match2.hashCode());
    assertEquals(match1.start(), match2.start());
    assertEquals(match1.end(), match2.end());
    assertEquals(match1.number(), match2.number());
    assertEquals(match1.rawString(), match2.rawString());
    assertEquals("1 800 234 45 67", match1.rawString());
  }

  /**
   * Tests the value type semantics for matches with a null number.
   */
  [TestMethod] public void testIllegalArguments() {
    try {
      new PhoneNumberMatch(-110, "1 800 234 45 67", new PhoneNumber());
      fail();
    } catch (IllegalArgumentException) { /* success */ }

    try {
      new PhoneNumberMatch(10, "1 800 234 45 67", null);
      fail();
    } catch (NullPointerException) { /* success */ }

    try {
      new PhoneNumberMatch(10, null, new PhoneNumber());
      fail();
    } catch (NullPointerException) { /* success */ }

    try {
      new PhoneNumberMatch(10, null, null);
      fail();
    } catch (NullPointerException) { /* success */ }
  }
}

}