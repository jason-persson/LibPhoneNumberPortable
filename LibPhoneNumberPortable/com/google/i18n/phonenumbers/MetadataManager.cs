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

using PhoneMetadata = com.google.i18n.phonenumbers.Phonemetadata.PhoneMetadata;
using PhoneMetadataCollection = com.google.i18n.phonenumbers.Phonemetadata.PhoneMetadataCollection;

using java.io;
using java.lang;
using java.util;
using java.util.logging;
using JavaPort;
using IOException = java.io.IOException;
using String = java.lang.String;

/**
 * Class encapsulating loading of PhoneNumber Metadata information. Currently this is used only for
 * additional data files such as PhoneNumberAlternateFormats, but in the future it is envisaged it
 * would handle the main metadata file (PhoneNumberMetadata.xml) as well.
 *
 * @author Lara Rennie
 */
class MetadataManager {
  private static readonly  String ALTERNATE_FORMATS_FILE_PREFIX =
      "/com/google/i18n/phonenumbers/data/PhoneNumberAlternateFormatsProto";
  private static readonly  String SHORT_NUMBER_METADATA_FILE_PREFIX =
      "/com/google/i18n/phonenumbers/data/ShortNumberMetadataProto";

  private static readonly Logger LOGGER = Logger.getLogger("MetadataManager");

  private static readonly Map<Integer, PhoneMetadata> callingCodeToAlternateFormatsMap =
      Collections.synchronizedMap(new HashMap<Integer, PhoneMetadata>());
  private static readonly Map<String, PhoneMetadata> regionCodeToShortNumberMetadataMap =
      Collections.synchronizedMap(new HashMap<String, PhoneMetadata>());

  // A set of which country calling codes there are alternate format data for. If the set has an
  // entry for a code, then there should be data for that code linked into the resources.
  private static readonly  Set<Integer> countryCodeSet =
      AlternateFormatsCountryCodeSet.getCountryCodeSet();

  // A set of which region codes there are short number data for. If the set has an entry for a
  // code, then there should be data for that code linked into the resources.
  private static readonly  Set<String> regionCodeSet = ShortNumbersRegionCodeSet.getRegionCodeSet();

  private MetadataManager() {
  }

  private static void close(InputStream @in) {
    if (@in != null) {
      try {
        @in.close();
      } catch (IOException e) {
        LOGGER.log(Level.WARNING, e.toString());
      }
    }
  }

  private static void loadAlternateFormatsMetadataFromFile(int countryCallingCode) {
    InputStream source = Extensions.getResourceAsStream(
        ALTERNATE_FORMATS_FILE_PREFIX + "_" + countryCallingCode);
    ObjectInputStream @in = null;
    try {
      @in = new ObjectInputStream(source);
      PhoneMetadataCollection alternateFormats = new PhoneMetadataCollection();
      alternateFormats.readExternal(@in);
      foreach (PhoneMetadata metadata in alternateFormats.getMetadataList()) {
        callingCodeToAlternateFormatsMap.put(metadata.getCountryCode(), metadata);
      }
    } catch (IOException e) {
      LOGGER.log(Level.WARNING, e.toString());
    } finally {
      close(@in);
    }
  }

  public static PhoneMetadata getAlternateFormatsForCountry(int countryCallingCode) {
    if (!countryCodeSet.contains(countryCallingCode)) {
      return null;
    }
    lock (callingCodeToAlternateFormatsMap) {
      if (!callingCodeToAlternateFormatsMap.containsKey(countryCallingCode)) {
        loadAlternateFormatsMetadataFromFile(countryCallingCode);
      }
    }
    return callingCodeToAlternateFormatsMap.get(countryCallingCode);
  }

  private static void loadShortNumberMetadataFromFile(String regionCode) {
    InputStream source = Extensions.getResourceAsStream(
        SHORT_NUMBER_METADATA_FILE_PREFIX + "_" + regionCode);
    ObjectInputStream @in = null;
    try {
      @in = new ObjectInputStream(source);
      PhoneMetadataCollection shortNumberMetadata = new PhoneMetadataCollection();
      shortNumberMetadata.readExternal(@in);
      foreach (PhoneMetadata metadata in shortNumberMetadata.getMetadataList()) {
        regionCodeToShortNumberMetadataMap.put(regionCode, metadata);
      }
    } catch (IOException e) {
      LOGGER.log(Level.WARNING, e.toString());
    } finally {
      close(@in);
    }
  }

  // @VisibleForTesting
  internal static Set<String> getShortNumberMetadataSupportedRegions() {
    return regionCodeSet;
  }

  internal static PhoneMetadata getShortNumberMetadataForRegion(String regionCode) {
    if (!regionCodeSet.contains(regionCode)) {
      return null;
    }
    lock (regionCodeToShortNumberMetadataMap) {
      if (!regionCodeToShortNumberMetadataMap.containsKey(regionCode)) {
        loadShortNumberMetadataFromFile(regionCode);
      }
    }
    return regionCodeToShortNumberMetadataMap.get(regionCode);
  }
}
}
