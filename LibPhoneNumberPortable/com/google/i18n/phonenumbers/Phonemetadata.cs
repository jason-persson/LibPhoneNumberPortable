﻿/*
 * Copyright (C) 2010 The Libphonenumber Authors
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

/**
 * Definition of the class representing metadata for international telephone numbers. This class is
 * hand created based on the class file compiled from phonemetadata.proto. Please refer to that file
 * for detailed descriptions of the meaning of each field.
 */

namespace com.google.i18n.phonenumbers {

using java.io;
using java.lang;
using java.util;
using String = java.lang.String;

public sealed class Phonemetadata {
  private Phonemetadata() {}
  public class NumberFormat : Externalizable {
    public NumberFormat() {}

    /**
     * Provides a dummy builder to 'emulate' the API of the code generated by the latest version of
     * Protocol Buffers. This lets BuildMetadataFromXml class to build with both this hand created
     * class and the one generated by the latest version of Protocol Buffers.
     */
    public sealed class Builder : NumberFormat {
      public NumberFormat build() {
        return this;
      }
    }

    public static Builder newBuilder() {
      return new Builder();
    }

    // required string pattern = 1;
    private boolean _hasPattern;
    private String pattern_ = "";
    public boolean hasPattern() { return _hasPattern; }
    public String getPattern() { return pattern_; }
    public NumberFormat setPattern(String value) {
      _hasPattern = true;
      pattern_ = value;
      return this;
    }

    // required string format = 2;
    private boolean _hasFormat;
    private String format_ = "";
    public boolean hasFormat() { return _hasFormat; }
    public String getFormat() { return format_; }
    public NumberFormat setFormat(String value) {
      _hasFormat = true;
      format_ = value;
      return this;
    }

    // repeated string leading_digits_pattern = 3;
    private java.util.List<String> leadingDigitsPattern_ = new java.util.ArrayList<String>();
    public java.util.List<String> leadingDigitPatterns() {
      return leadingDigitsPattern_;
    }
    public int leadingDigitsPatternSize() { return leadingDigitsPattern_.size(); }
    public String getLeadingDigitsPattern(int index) {
      return leadingDigitsPattern_.get(index);
    }
    public NumberFormat addLeadingDigitsPattern(String value) {
      if (value == null) {
        throw new NullPointerException();
      }
      leadingDigitsPattern_.add(value);
      return this;
    }

    // optional string national_prefix_formatting_rule = 4;
    private boolean _hasNationalPrefixFormattingRule;
    private String nationalPrefixFormattingRule_ = "";
    public boolean hasNationalPrefixFormattingRule() { return _hasNationalPrefixFormattingRule; }
    public String getNationalPrefixFormattingRule() { return nationalPrefixFormattingRule_; }
    public NumberFormat setNationalPrefixFormattingRule(String value) {
      _hasNationalPrefixFormattingRule = true;
      nationalPrefixFormattingRule_ = value;
      return this;
    }
    public NumberFormat clearNationalPrefixFormattingRule() {
      _hasNationalPrefixFormattingRule = false;
      nationalPrefixFormattingRule_ = "";
      return this;
    }

    // optional bool national_prefix_optional_when_formatting = 6;
    private boolean _hasNationalPrefixOptionalWhenFormatting;
    private boolean nationalPrefixOptionalWhenFormatting_ = false;
    public boolean hasNationalPrefixOptionalWhenFormatting() {
      return _hasNationalPrefixOptionalWhenFormatting; }
    public boolean isNationalPrefixOptionalWhenFormatting() {
      return nationalPrefixOptionalWhenFormatting_; }
    public NumberFormat setNationalPrefixOptionalWhenFormatting(boolean value) {
      _hasNationalPrefixOptionalWhenFormatting = true;
      nationalPrefixOptionalWhenFormatting_ = value;
      return this;
    }

    // optional string domestic_carrier_code_formatting_rule = 5;
    private boolean _hasDomesticCarrierCodeFormattingRule;
    private String domesticCarrierCodeFormattingRule_ = "";
    public boolean hasDomesticCarrierCodeFormattingRule() {
      return _hasDomesticCarrierCodeFormattingRule; }
    public String getDomesticCarrierCodeFormattingRule() {
      return domesticCarrierCodeFormattingRule_; }
    public NumberFormat setDomesticCarrierCodeFormattingRule(String value) {
      _hasDomesticCarrierCodeFormattingRule = true;
      domesticCarrierCodeFormattingRule_ = value;
      return this;
    }

    public NumberFormat mergeFrom(NumberFormat other) {
      if (other.hasPattern()) {
        setPattern(other.getPattern());
      }
      if (other.hasFormat()) {
        setFormat(other.getFormat());
      }
      int leadingDigitsPatternSize = other.leadingDigitsPatternSize();
      for (int i = 0; i < leadingDigitsPatternSize; i++) {
        addLeadingDigitsPattern(other.getLeadingDigitsPattern(i));
      }
      if (other.hasNationalPrefixFormattingRule()) {
        setNationalPrefixFormattingRule(other.getNationalPrefixFormattingRule());
      }
      if (other.hasDomesticCarrierCodeFormattingRule()) {
        setDomesticCarrierCodeFormattingRule(other.getDomesticCarrierCodeFormattingRule());
      }
      setNationalPrefixOptionalWhenFormatting(other.isNationalPrefixOptionalWhenFormatting());
      return this;
    }

    override public void writeExternal(ObjectOutput objectOutput) {
      objectOutput.writeUTF(pattern_);
      objectOutput.writeUTF(format_);
      int _leadingDigitsPatternSize = leadingDigitsPatternSize();
      objectOutput.writeInt(_leadingDigitsPatternSize);
      for (int i = 0; i < _leadingDigitsPatternSize; i++) {
        objectOutput.writeUTF(leadingDigitsPattern_.get(i));
      }

      objectOutput.writeBoolean(_hasNationalPrefixFormattingRule);
      if (_hasNationalPrefixFormattingRule) {
        objectOutput.writeUTF(nationalPrefixFormattingRule_);
      }
      objectOutput.writeBoolean(_hasDomesticCarrierCodeFormattingRule);
      if (_hasDomesticCarrierCodeFormattingRule) {
        objectOutput.writeUTF(domesticCarrierCodeFormattingRule_);
      }
      objectOutput.writeBoolean(nationalPrefixOptionalWhenFormatting_);
    }

    override public void readExternal(ObjectInput objectInput) {
      setPattern(objectInput.readUTF());
      setFormat(objectInput.readUTF());
      int leadingDigitsPatternSize = objectInput.readInt();
      for (int i = 0; i < leadingDigitsPatternSize; i++) {
        leadingDigitsPattern_.add(objectInput.readUTF());
      }
      if (objectInput.readBoolean()) {
        setNationalPrefixFormattingRule(objectInput.readUTF());
      }
      if (objectInput.readBoolean()) {
        setDomesticCarrierCodeFormattingRule(objectInput.readUTF());
      }
      setNationalPrefixOptionalWhenFormatting(objectInput.readBoolean());
    }
  }

  public class PhoneNumberDesc : Externalizable {
    public PhoneNumberDesc() {}

    /**
     * Provides a dummy builder.
     *
     * @see NumberFormat.Builder
     */
    public sealed class Builder : PhoneNumberDesc {
      public PhoneNumberDesc build() {
        return this;
      }
    }
    public static Builder newBuilder() {
      return new Builder();
    }

    // optional string national_number_pattern = 2;
    private boolean _hasNationalNumberPattern;
    private String nationalNumberPattern_ = "";
    public boolean hasNationalNumberPattern() { return _hasNationalNumberPattern; }
    public String getNationalNumberPattern() { return nationalNumberPattern_; }
    public PhoneNumberDesc setNationalNumberPattern(String value) {
      _hasNationalNumberPattern = true;
      nationalNumberPattern_ = value;
      return this;
    }

    // optional string possible_number_pattern = 3;
    private boolean _hasPossibleNumberPattern;
    private String possibleNumberPattern_ = "";
    public boolean hasPossibleNumberPattern() { return _hasPossibleNumberPattern; }
    public String getPossibleNumberPattern() { return possibleNumberPattern_; }
    public PhoneNumberDesc setPossibleNumberPattern(String value) {
      _hasPossibleNumberPattern = true;
      possibleNumberPattern_ = value;
      return this;
    }

    // optional string example_number = 6;
    private boolean _hasExampleNumber;
    private String exampleNumber_ = "";
    public boolean hasExampleNumber() { return _hasExampleNumber; }
    public String getExampleNumber() { return exampleNumber_; }
    public PhoneNumberDesc setExampleNumber(String value) {
      _hasExampleNumber = true;
      exampleNumber_ = value;
      return this;
    }

    public PhoneNumberDesc mergeFrom(PhoneNumberDesc other) {
      if (other.hasNationalNumberPattern()) {
        setNationalNumberPattern(other.getNationalNumberPattern());
      }
      if (other.hasPossibleNumberPattern()) {
        setPossibleNumberPattern(other.getPossibleNumberPattern());
      }
      if (other.hasExampleNumber()) {
        setExampleNumber(other.getExampleNumber());
      }
      return this;
    }

    public boolean exactlySameAs(PhoneNumberDesc other) {
      return nationalNumberPattern_.equals(other.nationalNumberPattern_) &&
          possibleNumberPattern_.equals(other.possibleNumberPattern_) &&
          exampleNumber_.equals(other.exampleNumber_);
    }

    override public void writeExternal(ObjectOutput objectOutput) {
      objectOutput.writeBoolean(_hasNationalNumberPattern);
      if (_hasNationalNumberPattern) {
        objectOutput.writeUTF(nationalNumberPattern_);
      }

      objectOutput.writeBoolean(_hasPossibleNumberPattern);
      if (_hasPossibleNumberPattern) {
        objectOutput.writeUTF(possibleNumberPattern_);
      }

      objectOutput.writeBoolean(_hasExampleNumber);
      if (_hasExampleNumber) {
        objectOutput.writeUTF(exampleNumber_);
      }
    }

    override public void readExternal(ObjectInput objectInput) {
      if (objectInput.readBoolean()) {
        setNationalNumberPattern(objectInput.readUTF());
      }

      if (objectInput.readBoolean()) {
        setPossibleNumberPattern(objectInput.readUTF());
      }

      if (objectInput.readBoolean()) {
        setExampleNumber(objectInput.readUTF());
      }
    }
  }

  public class PhoneMetadata : Externalizable {
    public PhoneMetadata() {}

    /**
     * Provides a dummy builder.
     *
     * @see NumberFormat.Builder
     */
    public sealed class Builder : PhoneMetadata
    {
      public PhoneMetadata build() {
        return this;
      }
    }
    public static Builder newBuilder() {
      return new Builder();
    }

    // optional PhoneNumberDesc general_desc = 1;
    private boolean _hasGeneralDesc;
    private PhoneNumberDesc generalDesc_ = null;
    public boolean hasGeneralDesc() { return _hasGeneralDesc; }
    public PhoneNumberDesc getGeneralDesc() { return generalDesc_; }
    public PhoneMetadata setGeneralDesc(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasGeneralDesc = true;
      generalDesc_ = value;
      return this;
    }

    // optional PhoneNumberDesc fixed_line = 2;
    private boolean _hasFixedLine;
    private PhoneNumberDesc fixedLine_ = null;
    public boolean hasFixedLine() { return _hasFixedLine; }
    public PhoneNumberDesc getFixedLine() { return fixedLine_; }
    public PhoneMetadata setFixedLine(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasFixedLine = true;
      fixedLine_ = value;
      return this;
    }

    // optional PhoneNumberDesc mobile = 3;
    private boolean _hasMobile;
    private PhoneNumberDesc mobile_ = null;
    public boolean hasMobile() { return _hasMobile; }
    public PhoneNumberDesc getMobile() { return mobile_; }
    public PhoneMetadata setMobile(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasMobile = true;
      mobile_ = value;
      return this;
    }

    // optional PhoneNumberDesc toll_free = 4;
    private boolean _hasTollFree;
    private PhoneNumberDesc tollFree_ = null;
    public boolean hasTollFree() { return _hasTollFree; }
    public PhoneNumberDesc getTollFree() { return tollFree_; }
    public PhoneMetadata setTollFree(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasTollFree = true;
      tollFree_ = value;
      return this;
    }

    // optional PhoneNumberDesc premium_rate = 5;
    private boolean _hasPremiumRate;
    private PhoneNumberDesc premiumRate_ = null;
    public boolean hasPremiumRate() { return _hasPremiumRate; }
    public PhoneNumberDesc getPremiumRate() { return premiumRate_; }
    public PhoneMetadata setPremiumRate(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasPremiumRate = true;
      premiumRate_ = value;
      return this;
    }

    // optional PhoneNumberDesc shared_cost = 6;
    private boolean _hasSharedCost;
    private PhoneNumberDesc sharedCost_ = null;
    public boolean hasSharedCost() { return _hasSharedCost; }
    public PhoneNumberDesc getSharedCost() { return sharedCost_; }
    public PhoneMetadata setSharedCost(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasSharedCost = true;
      sharedCost_ = value;
      return this;
    }

    // optional PhoneNumberDesc personal_number = 7;
    private boolean _hasPersonalNumber;
    private PhoneNumberDesc personalNumber_ = null;
    public boolean hasPersonalNumber() { return _hasPersonalNumber; }
    public PhoneNumberDesc getPersonalNumber() { return personalNumber_; }
    public PhoneMetadata setPersonalNumber(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasPersonalNumber = true;
      personalNumber_ = value;
      return this;
    }

    // optional PhoneNumberDesc voip = 8;
    private boolean _hasVoip;
    private PhoneNumberDesc voip_ = null;
    public boolean hasVoip() { return _hasVoip; }
    public PhoneNumberDesc getVoip() { return voip_; }
    public PhoneMetadata setVoip(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasVoip = true;
      voip_ = value;
      return this;
    }

    // optional PhoneNumberDesc pager = 21;
    private boolean _hasPager;
    private PhoneNumberDesc pager_ = null;
    public boolean hasPager() { return _hasPager; }
    public PhoneNumberDesc getPager() { return pager_; }
    public PhoneMetadata setPager(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasPager = true;
      pager_ = value;
      return this;
    }

    // optional PhoneNumberDesc uan = 25;
    private boolean _hasUan;
    private PhoneNumberDesc uan_ = null;
    public boolean hasUan() { return _hasUan; }
    public PhoneNumberDesc getUan() { return uan_; }
    public PhoneMetadata setUan(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasUan = true;
      uan_ = value;
      return this;
    }

    // optional PhoneNumberDesc emergency = 27;
    private boolean _hasEmergency;
    private PhoneNumberDesc emergency_ = null;
    public boolean hasEmergency() { return _hasEmergency; }
    public PhoneNumberDesc getEmergency() { return emergency_; }
    public PhoneMetadata setEmergency(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasEmergency = true;
      emergency_ = value;
      return this;
    }

    // optional PhoneNumberDesc voicemail = 28;
    private boolean _hasVoicemail;
    private PhoneNumberDesc voicemail_ = null;
    public boolean hasVoicemail() { return _hasVoicemail; }
    public PhoneNumberDesc getVoicemail() { return voicemail_; }
    public PhoneMetadata setVoicemail(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasVoicemail = true;
      voicemail_ = value;
      return this;
    }

    // optional PhoneNumberDesc short_code = 29;
    private boolean _hasShortCode;
    private PhoneNumberDesc shortCode_ = null;
    public boolean hasShortCode() { return _hasShortCode; }
    public PhoneNumberDesc getShortCode() { return shortCode_; }
    public PhoneMetadata setShortCode(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasShortCode = true;
      shortCode_ = value;
      return this;
    }

    // optional PhoneNumberDesc standard_rate = 30;
    private boolean _hasStandardRate;
    private PhoneNumberDesc standardRate_ = null;
    public boolean hasStandardRate() { return _hasStandardRate; }
    public PhoneNumberDesc getStandardRate() { return standardRate_; }
    public PhoneMetadata setStandardRate(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasStandardRate = true;
      standardRate_ = value;
      return this;
    }

    // optional PhoneNumberDesc carrier_specific = 31;
    private boolean _hasCarrierSpecific;
    private PhoneNumberDesc carrierSpecific_ = null;
    public boolean hasCarrierSpecific() { return _hasCarrierSpecific; }
    public PhoneNumberDesc getCarrierSpecific() { return carrierSpecific_; }
    public PhoneMetadata setCarrierSpecific(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasCarrierSpecific = true;
      carrierSpecific_ = value;
      return this;
    }

    // optional PhoneNumberDesc noInternationalDialling = 24;
    private boolean _hasNoInternationalDialling;
    private PhoneNumberDesc noInternationalDialling_ = null;
    public boolean hasNoInternationalDialling() { return _hasNoInternationalDialling; }
    public PhoneNumberDesc getNoInternationalDialling() { return noInternationalDialling_; }
    public PhoneMetadata setNoInternationalDialling(PhoneNumberDesc value) {
      if (value == null) {
        throw new NullPointerException();
      }
      _hasNoInternationalDialling = true;
      noInternationalDialling_ = value;
      return this;
    }

    // required string id = 9;
    private boolean _hasId;
    private String id_ = "";
    public boolean hasId() { return _hasId; }
    public String getId() { return id_; }
    public PhoneMetadata setId(String value) {
      _hasId = true;
      id_ = value;
      return this;
    }

    // required int32 country_code = 10;
    private boolean _hasCountryCode;
    private int countryCode_ = 0;
    public boolean hasCountryCode() { return _hasCountryCode; }
    public int getCountryCode() { return countryCode_; }
    public PhoneMetadata setCountryCode(int value) {
      _hasCountryCode = true;
      countryCode_ = value;
      return this;
    }

    // required string international_prefix = 11;
    private boolean _hasInternationalPrefix;
    private String internationalPrefix_ = "";
    public boolean hasInternationalPrefix() { return _hasInternationalPrefix; }
    public String getInternationalPrefix() { return internationalPrefix_; }
    public PhoneMetadata setInternationalPrefix(String value) {
      _hasInternationalPrefix = true;
      internationalPrefix_ = value;
      return this;
    }

    // optional string preferred_international_prefix = 17;
    private boolean _hasPreferredInternationalPrefix;
    private String preferredInternationalPrefix_ = "";
    public boolean hasPreferredInternationalPrefix() { return _hasPreferredInternationalPrefix; }
    public String getPreferredInternationalPrefix() { return preferredInternationalPrefix_; }
    public PhoneMetadata setPreferredInternationalPrefix(String value) {
      _hasPreferredInternationalPrefix = true;
      preferredInternationalPrefix_ = value;
      return this;
    }

    // optional string national_prefix = 12;
    private boolean _hasNationalPrefix;
    private String nationalPrefix_ = "";
    public boolean hasNationalPrefix() { return _hasNationalPrefix; }
    public String getNationalPrefix() { return nationalPrefix_; }
    public PhoneMetadata setNationalPrefix(String value) {
      _hasNationalPrefix = true;
      nationalPrefix_ = value;
      return this;
    }

    // optional string preferred_extn_prefix = 13;
    private boolean _hasPreferredExtnPrefix;
    private String preferredExtnPrefix_ = "";
    public boolean hasPreferredExtnPrefix() { return _hasPreferredExtnPrefix; }
    public String getPreferredExtnPrefix() { return preferredExtnPrefix_; }
    public PhoneMetadata setPreferredExtnPrefix(String value) {
      _hasPreferredExtnPrefix = true;
      preferredExtnPrefix_ = value;
      return this;
    }

    // optional string national_prefix_for_parsing = 15;
    private boolean _hasNationalPrefixForParsing;
    private String nationalPrefixForParsing_ = "";
    public boolean hasNationalPrefixForParsing() { return _hasNationalPrefixForParsing; }
    public String getNationalPrefixForParsing() { return nationalPrefixForParsing_; }
    public PhoneMetadata setNationalPrefixForParsing(String value) {
      _hasNationalPrefixForParsing = true;
      nationalPrefixForParsing_ = value;
      return this;
    }

    // optional string national_prefix_transform_rule = 16;
    private boolean _hasNationalPrefixTransformRule;
    private String nationalPrefixTransformRule_ = "";
    public boolean hasNationalPrefixTransformRule() { return _hasNationalPrefixTransformRule; }
    public String getNationalPrefixTransformRule() { return nationalPrefixTransformRule_; }
    public PhoneMetadata setNationalPrefixTransformRule(String value) {
      _hasNationalPrefixTransformRule = true;
      nationalPrefixTransformRule_ = value;
      return this;
    }

    // optional bool same_mobile_and_fixed_line_pattern = 18 [default = false];
    private boolean _hasSameMobileAndFixedLinePattern;
    private boolean sameMobileAndFixedLinePattern_ = false;
    public boolean hasSameMobileAndFixedLinePattern() { return _hasSameMobileAndFixedLinePattern; }
    public boolean isSameMobileAndFixedLinePattern() { return sameMobileAndFixedLinePattern_; }
    public PhoneMetadata setSameMobileAndFixedLinePattern(boolean value) {
      _hasSameMobileAndFixedLinePattern = true;
      sameMobileAndFixedLinePattern_ = value;
      return this;
    }

    // repeated NumberFormat number_format = 19;
    private List<NumberFormat> numberFormat_ = new ArrayList<NumberFormat>();
    public List<NumberFormat> numberFormats() {
      return numberFormat_;
    }
    public int numberFormatSize() { return numberFormat_.size(); }
    public NumberFormat getNumberFormat(int index) {
      return numberFormat_.get(index);
    }
    public PhoneMetadata addNumberFormat(NumberFormat value) {
      if (value == null) {
        throw new NullPointerException();
      }
      numberFormat_.add(value);
      return this;
    }

    // repeated NumberFormat intl_number_format = 20;
    private java.util.List<NumberFormat> intlNumberFormat_ =
        new java.util.ArrayList<NumberFormat>();
    public java.util.List<NumberFormat> intlNumberFormats() {
      return intlNumberFormat_;
    }
    public int intlNumberFormatSize() { return intlNumberFormat_.size(); }
    public NumberFormat getIntlNumberFormat(int index) {
      return intlNumberFormat_.get(index);
    }

    public PhoneMetadata addIntlNumberFormat(NumberFormat value) {
      if (value == null) {
        throw new NullPointerException();
      }
      intlNumberFormat_.add(value);
      return this;
    }
    public PhoneMetadata clearIntlNumberFormat() {
      intlNumberFormat_.clear();
      return this;
    }

    // optional bool main_country_for_code = 22 [default = false];
    private boolean _hasMainCountryForCode;
    private boolean mainCountryForCode_ = false;
    public boolean hasMainCountryForCode() { return _hasMainCountryForCode; }
    public boolean isMainCountryForCode() { return mainCountryForCode_; }
    // Method that lets this class have the same interface as the one generated by Protocol Buffers
    // which is used by C++ build tools.
    public boolean getMainCountryForCode() { return mainCountryForCode_; }
    public PhoneMetadata setMainCountryForCode(boolean value) {
      _hasMainCountryForCode = true;
      mainCountryForCode_ = value;
      return this;
    }

    // optional string leading_digits = 23;
    private boolean _hasLeadingDigits;
    private String leadingDigits_ = "";
    public boolean hasLeadingDigits() { return _hasLeadingDigits; }
    public String getLeadingDigits() { return leadingDigits_; }
    public PhoneMetadata setLeadingDigits(String value) {
      _hasLeadingDigits = true;
      leadingDigits_ = value;
      return this;
    }

    // optional bool leading_zero_possible = 26 [default = false];
    private boolean _hasLeadingZeroPossible;
    private boolean leadingZeroPossible_ = false;
    public boolean hasLeadingZeroPossible() { return _hasLeadingZeroPossible; }
    public boolean isLeadingZeroPossible() { return leadingZeroPossible_; }
    public PhoneMetadata setLeadingZeroPossible(boolean value) {
      _hasLeadingZeroPossible = true;
      leadingZeroPossible_ = value;
      return this;
    }

    override public void writeExternal(ObjectOutput objectOutput) {
      objectOutput.writeBoolean(_hasGeneralDesc);
      if (_hasGeneralDesc) {
        generalDesc_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasFixedLine);
      if (_hasFixedLine) {
        fixedLine_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasMobile);
      if (_hasMobile) {
        mobile_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasTollFree);
      if (_hasTollFree) {
        tollFree_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasPremiumRate);
      if (_hasPremiumRate) {
        premiumRate_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasSharedCost);
      if (_hasSharedCost) {
        sharedCost_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasPersonalNumber);
      if (_hasPersonalNumber) {
        personalNumber_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasVoip);
      if (_hasVoip) {
        voip_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasPager);
      if (_hasPager) {
        pager_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasUan);
      if (_hasUan) {
        uan_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasEmergency);
      if (_hasEmergency) {
        emergency_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasVoicemail);
      if (_hasVoicemail) {
        voicemail_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasShortCode);
      if (_hasShortCode) {
        shortCode_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasStandardRate);
      if (_hasStandardRate) {
        standardRate_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasCarrierSpecific);
      if (_hasCarrierSpecific) {
        carrierSpecific_.writeExternal(objectOutput);
      }
      objectOutput.writeBoolean(_hasNoInternationalDialling);
      if (_hasNoInternationalDialling) {
        noInternationalDialling_.writeExternal(objectOutput);
      }

      objectOutput.writeUTF(id_);
      objectOutput.writeInt(countryCode_);
      objectOutput.writeUTF(internationalPrefix_);

      objectOutput.writeBoolean(_hasPreferredInternationalPrefix);
      if (_hasPreferredInternationalPrefix) {
        objectOutput.writeUTF(preferredInternationalPrefix_);
      }

      objectOutput.writeBoolean(_hasNationalPrefix);
      if (_hasNationalPrefix) {
        objectOutput.writeUTF(nationalPrefix_);
      }

      objectOutput.writeBoolean(_hasPreferredExtnPrefix);
      if (_hasPreferredExtnPrefix) {
        objectOutput.writeUTF(preferredExtnPrefix_);
      }

      objectOutput.writeBoolean(_hasNationalPrefixForParsing);
      if (_hasNationalPrefixForParsing) {
        objectOutput.writeUTF(nationalPrefixForParsing_);
      }

      objectOutput.writeBoolean(_hasNationalPrefixTransformRule);
      if (_hasNationalPrefixTransformRule) {
        objectOutput.writeUTF(nationalPrefixTransformRule_);
      }

      objectOutput.writeBoolean(sameMobileAndFixedLinePattern_);

      int _numberFormatSize = numberFormatSize();
      objectOutput.writeInt(_numberFormatSize);
      for (int i = 0; i < _numberFormatSize; i++) {
        numberFormat_.get(i).writeExternal(objectOutput);
      }

      int _intlNumberFormatSize = intlNumberFormatSize();
      objectOutput.writeInt(_intlNumberFormatSize);
      for (int i = 0; i < _intlNumberFormatSize; i++) {
        intlNumberFormat_.get(i).writeExternal(objectOutput);
      }

      objectOutput.writeBoolean(mainCountryForCode_);

      objectOutput.writeBoolean(_hasLeadingDigits);
      if (_hasLeadingDigits) {
        objectOutput.writeUTF(leadingDigits_);
      }

      objectOutput.writeBoolean(leadingZeroPossible_);
    }

    override public void readExternal(ObjectInput objectInput) {
      boolean hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setGeneralDesc(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setFixedLine(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setMobile(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setTollFree(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setPremiumRate(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setSharedCost(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setPersonalNumber(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setVoip(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setPager(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setUan(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setEmergency(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setVoicemail(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setShortCode(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setStandardRate(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setCarrierSpecific(desc);
      }
      hasDesc = objectInput.readBoolean();
      if (hasDesc) {
        PhoneNumberDesc desc = new PhoneNumberDesc();
        desc.readExternal(objectInput);
        setNoInternationalDialling(desc);
      }

      setId(objectInput.readUTF());
      setCountryCode(objectInput.readInt());
      setInternationalPrefix(objectInput.readUTF());

      boolean hasString = objectInput.readBoolean();
      if (hasString) {
        setPreferredInternationalPrefix(objectInput.readUTF());
      }

      hasString = objectInput.readBoolean();
      if (hasString) {
        setNationalPrefix(objectInput.readUTF());
      }

      hasString = objectInput.readBoolean();
      if (hasString) {
        setPreferredExtnPrefix(objectInput.readUTF());
      }

      hasString = objectInput.readBoolean();
      if (hasString) {
        setNationalPrefixForParsing(objectInput.readUTF());
      }

      hasString = objectInput.readBoolean();
      if (hasString) {
        setNationalPrefixTransformRule(objectInput.readUTF());
      }

      setSameMobileAndFixedLinePattern(objectInput.readBoolean());

      int nationalFormatSize = objectInput.readInt();
      for (int i = 0; i < nationalFormatSize; i++) {
        NumberFormat numFormat = new NumberFormat();
        numFormat.readExternal(objectInput);
        numberFormat_.add(numFormat);
      }

      int intlNumberFormatSize = objectInput.readInt();
      for (int i = 0; i < intlNumberFormatSize; i++) {
        NumberFormat numFormat = new NumberFormat();
        numFormat.readExternal(objectInput);
        intlNumberFormat_.add(numFormat);
      }

      setMainCountryForCode(objectInput.readBoolean());

      hasString = objectInput.readBoolean();
      if (hasString) {
        setLeadingDigits(objectInput.readUTF());
      }

      setLeadingZeroPossible(objectInput.readBoolean());
    }
  }

  public class PhoneMetadataCollection : Externalizable {
    public PhoneMetadataCollection() {}

    /**
     * Provides a dummy builder.
     *
     * @see NumberFormat.Builder
     */
    public sealed class Builder : PhoneMetadataCollection {
      public PhoneMetadataCollection build() {
        return this;
      }
    }
    public static Builder newBuilder() {
      return new Builder();
    }

    // repeated PhoneMetadata metadata = 1;
    private java.util.List<PhoneMetadata> metadata_ = new java.util.ArrayList<PhoneMetadata>();

    public java.util.List<PhoneMetadata> getMetadataList() {
      return metadata_;
    }
    public int getMetadataCount() { return metadata_.size(); }

    public PhoneMetadataCollection addMetadata(PhoneMetadata value) {
      if (value == null) {
        throw new NullPointerException();
      }
      metadata_.add(value);
      return this;
    }

    override public void writeExternal(ObjectOutput objectOutput) {
      int size = getMetadataCount();
      objectOutput.writeInt(size);
      for (int i = 0; i < size; i++) {
        metadata_.get(i).writeExternal(objectOutput);
      }
    }

    override public void readExternal(ObjectInput objectInput) {
      int size = objectInput.readInt();
      for (int i = 0; i < size; i++) {
        PhoneMetadata metadata = new PhoneMetadata();
        metadata.readExternal(objectInput);
        metadata_.add(metadata);
      }
    }

    public PhoneMetadataCollection clear() {
      metadata_.clear();
      return this;
    }
  }
}
}