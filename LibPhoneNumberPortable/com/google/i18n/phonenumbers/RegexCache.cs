/*
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

namespace com.google.i18n.phonenumbers {

using java.lang;
using java.util;
using java.util.regex;
using System.Runtime.CompilerServices;
using String = java.lang.String;

/**
 * LRU Cache for compiled regular expressions used by the libphonenumbers libary.
 *
 * @author Shaopeng Jia
 */
public class RegexCache {
  private LRUCache<String, Pattern> cache;

  public RegexCache(int size) {
    cache = new LRUCache<String, Pattern>(size);
  }

  public Pattern getPatternForRegex(String regex) {
    Pattern pattern = cache.get(regex);
    if (pattern == null) {
      pattern = Pattern.compile(regex);
      cache.put(regex, pattern);
    }
    return pattern;
  }

  // This method is used for testing.
  internal boolean containsRegex(String regex) {
    return cache.containsKey(regex);
  }

  private class LRUCache<K, V> where V : class {
    // LinkedHashMap offers a straightforward implementation of LRU cache.
    private LinkedHashMap<K, V> map;
    private int _size;
    private readonly object _syncLock = new object();

    public LRUCache(int _size) {
      this._size = _size;
      map = new LinkedHashMap<K, V>(_size * 4 / 3 + 1, 0.75f, true, () =>
      {
          return map.size() > _size;
      });
    }

    public V get(K key) {
        lock (_syncLock)
        {
            return map.get(key);
        }
    }

    public void put(K key, V value) {
        lock (_syncLock)
        {
            map.put(key, value);
        }
    }

    public boolean containsKey(K key) {
        lock (_syncLock)
        {
            return map.containsKey(key);
        }
    }
  }
}
}
