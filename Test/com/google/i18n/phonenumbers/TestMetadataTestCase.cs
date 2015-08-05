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
 * Root class for PhoneNumberUtil tests that depend on the test metadata file.
 * <p>
 * Note since tests that extend this class do not use the normal metadata file, they should not be
 * used for regression test purposes.
 *
 * @author Shaopeng Jia
 * @author Lara Rennie
 */
[TestClass] public class TestMetadataTestCase : TestCase {
  private static readonly String TEST_META_DATA_FILE_PREFIX  =
      "/com/google/i18n/phonenumbers/data/PhoneNumberMetadataProtoForTesting";

  protected readonly PhoneNumberUtil phoneUtil ;

  public TestMetadataTestCase() {
    phoneUtil = initializePhoneUtilForTesting();
  }

  static PhoneNumberUtil initializePhoneUtilForTesting() {
    PhoneNumberUtil.resetInstance();
    PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance(
        TEST_META_DATA_FILE_PREFIX,
        CountryCodeToRegionCodeMapForTesting.getCountryCodeToRegionCodeMap());
    return phoneUtil;
  }
}

}