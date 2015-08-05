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
 * Class containing string constants of region codes for easier testing.
 */
sealed class  RegionCode {
  // Region code for global networks (e.g. +800 numbers).
  internal static readonly String UN001  = "001";
  internal static readonly String AD  = "AD";
  internal static readonly String AE  = "AE";
  internal static readonly String AM  = "AM";
  internal static readonly String AO  = "AO";
  internal static readonly String AQ  = "AQ";
  internal static readonly String AR  = "AR";
  internal static readonly String AU  = "AU";
  internal static readonly String BR  = "BR";
  internal static readonly String BS  = "BS";
  internal static readonly String BY  = "BY";
  internal static readonly String CA  = "CA";
  internal static readonly String CH  = "CH";
  internal static readonly String CL  = "CL";
  internal static readonly String CN  = "CN";
  internal static readonly String CS  = "CS";
  internal static readonly String DE  = "DE";
  internal static readonly String FR  = "FR";
  internal static readonly String GB  = "GB";
  internal static readonly String IT  = "IT";
  internal static readonly String JP  = "JP";
  internal static readonly String KR  = "KR";
  internal static readonly String MX  = "MX";
  internal static readonly String NZ  = "NZ";
  internal static readonly String PG  = "PG";
  internal static readonly String PL  = "PL";
  internal static readonly String RE  = "RE";
  internal static readonly String SG  = "SG";
  internal static readonly String US  = "US";
  internal static readonly String YT  = "YT";
  internal static readonly String ZW  = "ZW";
  // Official code for the unknown region.
  internal static readonly String ZZ  = "ZZ";
}

}