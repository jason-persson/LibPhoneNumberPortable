/*
 * Copyright (C) 2012 The Libphonenumber Authors
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
 * Some basic tests to check that the phone number metadata can be correctly loaded.
 *
 * @author Lara Rennie
 */
[TestClass] public class MetadataManagerTest : TestCase {

  [TestMethod] public void testAlternateFormatsContainsData() {
    // We should have some data for Germany.
    PhoneMetadata germanyAlternateFormats = MetadataManager.getAlternateFormatsForCountry(49);
    assertNotNull(germanyAlternateFormats);
    assertTrue(germanyAlternateFormats.numberFormats().size() > 0);
  }

  [TestMethod] public void testShortNumberMetadataContainsData() {
    // We should have some data for France.
    PhoneMetadata franceShortNumberMetadata = MetadataManager.getShortNumberMetadataForRegion("FR");
    assertNotNull(franceShortNumberMetadata);
    assertTrue(franceShortNumberMetadata.hasShortCode());
  }

  [TestMethod] public void testAlternateFormatsFailsGracefully() {
    PhoneMetadata noAlternateFormats = MetadataManager.getAlternateFormatsForCountry(999);
    assertNull(noAlternateFormats);
  }

  [TestMethod] public void testShortNumberMetadataFailsGracefully() {
    PhoneMetadata noShortNumberMetadata = MetadataManager.getShortNumberMetadataForRegion("XXX");
    assertNull(noShortNumberMetadata);
  }
}

}