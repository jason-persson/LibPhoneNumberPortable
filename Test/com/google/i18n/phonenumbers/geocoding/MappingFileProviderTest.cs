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
using java.util.logging;
using java.util;
using java.lang;
using java.io;
using com.google.i18n.phonenumbers.geocoding;

namespace Test
{
/**
 * Unittests for MappingFileProvider.java
 *
 * @author Shaopeng Jia
 */
[TestClass]
public class MappingFileProviderTest : TestCase {
  private readonly MappingFileProvider mappingProvider = new MappingFileProvider();
  private static readonly Logger LOGGER = Logger.getLogger("MappingFileProviderTest.");

  public MappingFileProviderTest() {
    SortedMap<Integer, Set<String>> mapping = new TreeMap<Integer, Set<String>>();
    mapping.put(1, newHashSet("en"));
    mapping.put(86, newHashSet("zh", "en", "zh_Hant"));
    mapping.put(41, newHashSet("de", "fr", "it", "rm"));
    mapping.put(65, newHashSet("en", "zh_Hans", "ms", "ta"));

    mappingProvider.readFileConfigs(mapping);
  }

  private static HashSet<String> newHashSet(params String[] strings) {
    HashSet<String> set = new HashSet<String>();
    set.addAll(Arrays.asList(strings));
    return set;
  }

  [TestMethod] public void testReadWriteExternal() {
    try {
      ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
      ObjectOutputStream objectOutputStream = new ObjectOutputStream(byteArrayOutputStream);
      mappingProvider.writeExternal(objectOutputStream);
      objectOutputStream.flush();

      MappingFileProvider newMappingProvider = new MappingFileProvider();
      newMappingProvider.readExternal(
          new ObjectInputStream(new ByteArrayInputStream(byteArrayOutputStream.toByteArray())));
      assertEquals(mappingProvider.toString(), newMappingProvider.toString());
    } catch (IOException e) {
      LOGGER.log(Level.SEVERE, e.getMessage());
      fail();
    }
  }

  [TestMethod] public void testGetFileName() {
    assertEquals("1_en", mappingProvider.getFileName(1, "en", "", ""));
    assertEquals("1_en", mappingProvider.getFileName(1, "en", "", "US"));
    assertEquals("1_en", mappingProvider.getFileName(1, "en", "", "GB"));
    assertEquals("41_de", mappingProvider.getFileName(41, "de", "", "CH"));
    assertEquals("", mappingProvider.getFileName(44, "en", "", "GB"));
    assertEquals("86_zh", mappingProvider.getFileName(86, "zh", "", ""));
    assertEquals("86_zh", mappingProvider.getFileName(86, "zh", "Hans", ""));
    assertEquals("86_zh", mappingProvider.getFileName(86, "zh", "", "CN"));
    assertEquals("", mappingProvider.getFileName(86, "", "", "CN"));
    assertEquals("86_zh", mappingProvider.getFileName(86, "zh", "Hans", "CN"));
    assertEquals("86_zh", mappingProvider.getFileName(86, "zh", "Hans", "SG"));
    assertEquals("86_zh", mappingProvider.getFileName(86, "zh", "", "SG"));
    assertEquals("86_zh_Hant", mappingProvider.getFileName(86, "zh", "", "TW"));
    assertEquals("86_zh_Hant", mappingProvider.getFileName(86, "zh", "", "HK"));
    assertEquals("86_zh_Hant", mappingProvider.getFileName(86, "zh", "Hant", "TW"));
  }
}
}