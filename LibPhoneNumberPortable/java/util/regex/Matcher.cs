using java.lang;
using System;
using String = java.lang.String;
using StringBuilder = java.lang.StringBuilder;
using Math = java.lang.Math;
using IOException = java.io.IOException;

namespace java.util.regex
{
public sealed class Matcher : MatchResult {
    internal Pattern parentPattern;
    internal int[] groups;
    internal int from, to;
    internal int lookbehindTo;
    internal CharSequence text;
    internal static readonly int ENDANCHOR = 1;
    internal static readonly int NOANCHOR = 0;
    internal int acceptMode = NOANCHOR;
    internal int first = -1, last = 0;
    internal int oldLast = -1;
    internal int lastAppendPosition = 0;
    internal int[] locals;
    internal boolean _hitEnd;
    internal boolean _requireEnd;
    internal boolean transparentBounds = false;
    internal boolean anchoringBounds = true;
    Matcher() {
    }
    internal Matcher(Pattern parent, CharSequence text) {
        this.parentPattern = parent;
        this.text = text;
        // Allocate state storage
        int parentGroupCount = Math.max(parent.capturingGroupCount, 10);
        groups = new int[parentGroupCount * 2];
        locals = new int[parent.localCount];
        // Put fields into initial states
        reset();
    }
    public Pattern pattern() {
        return parentPattern;
    }
    public MatchResult toMatchResult() {
        Matcher result = new Matcher(this.parentPattern, text);
        result.first = this.first;
        result.last = this.last;
        result.groups = (int[])this.groups.Clone();
        return result;
    }
    public Matcher usePattern(Pattern newPattern) {
        if (newPattern == null)
            throw new IllegalArgumentException("Pattern cannot be null");
        parentPattern = newPattern;
        // Reallocate state storage
        int parentGroupCount = Math.max(newPattern.capturingGroupCount, 10);
        groups = new int[parentGroupCount * 2];
        locals = new int[newPattern.localCount];
        for (int i = 0; i < groups.Length; i++)
            groups[i] = -1;
        for (int i = 0; i < locals.Length; i++)
            locals[i] = -1;
        return this;
    }
    public Matcher reset() {
        first = -1;
        last = 0;
        oldLast = -1;
        for(int i=0; i<groups.Length; i++)
            groups[i] = -1;
        for(int i=0; i<locals.Length; i++)
            locals[i] = -1;
        lastAppendPosition = 0;
        from = 0;
        to = getTextLength();
        return this;
    }
    public Matcher reset(CharSequence input) {
        text = input;
        return reset();
    }
    public int start() {
        if (first < 0)
            throw new InvalidOperationException("No match available");
        return first;
    }
    public int start(int group) {
        if (first < 0)
            throw new InvalidOperationException("No match available");
        if (group > groupCount())
            throw new IndexOutOfRangeException("No group " + group);
        return groups[group * 2];
    }
    public int end() {
        if (first < 0)
            throw new InvalidOperationException("No match available");
        return last;
    }
    public int end(int group) {
        if (first < 0)
            throw new InvalidOperationException("No match available");
        if (group > groupCount())
            throw new IndexOutOfRangeException("No group " + group);
        return groups[group * 2 + 1];
    }
    public String group() {
        return group(0);
    }
    public String group(int group) {
        if (first < 0)
            throw new InvalidOperationException("No match found");
        if (group < 0 || group > groupCount())
            throw new IndexOutOfRangeException("No group " + group);
        if ((groups[group*2] == -1) || (groups[group*2+1] == -1))
            return null;
        return getSubSequence(groups[group * 2], groups[group * 2 + 1]).toString();
    }
    public String group(String name) {
        if (name == null)
            throw new NullReferenceException("Null group name");
        if (first < 0)
            throw new InvalidOperationException("No match found");
        if (!parentPattern.namedGroups().containsKey(name))
            throw new IllegalArgumentException("No group with name <" + name + ">");
        int group = parentPattern.namedGroups().get(name);
        if ((groups[group*2] == -1) || (groups[group*2+1] == -1))
            return null;
        return getSubSequence(groups[group * 2], groups[group * 2 + 1]).toString();
    }
    public int groupCount() {
        return parentPattern.capturingGroupCount - 1;
    }
    public boolean matches() {
        return match(from, ENDANCHOR);
    }
    public boolean find() {
        int nextSearchIndex = last;
        if (nextSearchIndex == first)
            nextSearchIndex++;
        // If next search starts before region, start it at region
        if (nextSearchIndex < from)
            nextSearchIndex = from;
        // If next search starts beyond region then it fails
        if (nextSearchIndex > to) {
            for (int i = 0; i < groups.Length; i++)
                groups[i] = -1;
            return false;
        }
        return search(nextSearchIndex);
    }
    public boolean find(int start) {
        int limit = getTextLength();
        if ((start < 0) || (start > limit))
            throw new IndexOutOfRangeException("Illegal start index");
        reset();
        return search(start);
    }
    public boolean lookingAt() {
        return match(from, NOANCHOR);
    }
    public static String quoteReplacement(String s) {
        if ((s.indexOf('\\') == -1) && (s.indexOf('$') == -1))
            return s;
        StringBuilder sb = new StringBuilder();
        for (int i=0; i<s.length(); i++) {
            char c = s.charAt(i);
            if (c == '\\' || c == '$') {
                sb.append('\\');
            }
            sb.append(c);
        }
        return sb.toString();
    }
    public Matcher appendReplacement(StringBuffer sb, String replacement) {
        // If no match, return error
        if (first < 0)
            throw new InvalidOperationException("No match available");
        // Process substitution string to replace group references with groups
        int cursor = 0;
        StringBuilder result = new StringBuilder();
        while (cursor < replacement.length()) {
            char nextChar = replacement.charAt(cursor);
            if (nextChar == '\\') {
                cursor++;
                nextChar = replacement.charAt(cursor);
                result.append(nextChar);
                cursor++;
            } else if (nextChar == '$') {
                // Skip past $
                cursor++;
                // A StringIndexOutOfBoundsException is thrown if
                // this "$" is the last character in replacement
                // string in current implementation, a IAE might be
                // more appropriate.
                nextChar = replacement.charAt(cursor);
                int refNum = -1;
                if (nextChar == '{') {
                    cursor++;
                    StringBuilder gsb = new StringBuilder();
                    while (cursor < replacement.length()) {
                        nextChar = replacement.charAt(cursor);
                        if (ASCII.isLower(nextChar) ||
                            ASCII.isUpper(nextChar) ||
                            ASCII.isDigit(nextChar)) {
                            gsb.append(nextChar);
                            cursor++;
                        } else {
                            break;
                        }
                    }
                    if (gsb.length() == 0)
                        throw new IllegalArgumentException(
                            "named capturing group has 0 length name");
                    if (nextChar != '}')
                        throw new IllegalArgumentException(
                            "named capturing group is missing trailing '}'");
                    String gname = gsb.toString();
                    if (ASCII.isDigit(gname.charAt(0)))
                        throw new IllegalArgumentException(
                            "capturing group name {" + gname +
                            "} starts with digit character");
                    if (!parentPattern.namedGroups().containsKey(gname))
                        throw new IllegalArgumentException(
                            "No group with name {" + gname + "}");
                    refNum = parentPattern.namedGroups().get(gname);
                    cursor++;
                } else {
                    // The first number is always a group
                    refNum = (int)nextChar - '0';
                    if ((refNum < 0)||(refNum > 9))
                        throw new IllegalArgumentException(
                            "Illegal group reference");
                    cursor++;
                    // Capture the largest legal group string
                    boolean done = false;
                    while (!done) {
                        if (cursor >= replacement.length()) {
                            break;
                        }
                        int nextDigit = replacement.charAt(cursor) - '0';
                        if ((nextDigit < 0)||(nextDigit > 9)) { // not a number
                            break;
                        }
                        int newRefNum = (refNum * 10) + nextDigit;
                        if (groupCount() < newRefNum) {
                            done = true;
                        } else {
                            refNum = newRefNum;
                            cursor++;
                        }
                    }
                }
                // Append group
                if (start(refNum) != -1 && end(refNum) != -1)
                    result.append(text, start(refNum), end(refNum));
            } else {
                result.append(nextChar);
                cursor++;
            }
        }
        // Append the intervening text
        sb.append(text, lastAppendPosition, first);
        // Append the match substitution
        sb.append(result);
        lastAppendPosition = last;
        return this;
    }
    public StringBuffer appendTail(StringBuffer sb) {
        sb.append(text, lastAppendPosition, getTextLength());
        return sb;
    }
    public String replaceAll(String replacement) {
        reset();
        boolean result = find();
        if (result) {
            StringBuffer sb = new StringBuffer();
            do {
                appendReplacement(sb, replacement);
                result = find();
            } while (result);
            appendTail(sb);
            return sb.toString();
        }
        return text.toString();
    }
    public String replaceFirst(String replacement) {
        if (replacement == null)
            throw new NullPointerException("replacement");
        reset();
        if (!find())
            return text.toString();
        StringBuffer sb = new StringBuffer();
        appendReplacement(sb, replacement);
        appendTail(sb);
        return sb.toString();
    }
    public Matcher region(int start, int end) {
        if ((start < 0) || (start > getTextLength()))
            throw new IndexOutOfBoundsException("start");
        if ((end < 0) || (end > getTextLength()))
            throw new IndexOutOfBoundsException("end");
        if (start > end)
            throw new IndexOutOfBoundsException("start > end");
        reset();
        from = start;
        to = end;
        return this;
    }
    public int regionStart() {
        return from;
    }
    public int regionEnd() {
        return to;
    }
    public boolean hasTransparentBounds() {
        return transparentBounds;
    }
    public Matcher useTransparentBounds(boolean b) {
        transparentBounds = b;
        return this;
    }
    public boolean hasAnchoringBounds() {
        return anchoringBounds;
    }
    public Matcher useAnchoringBounds(boolean b) {
        anchoringBounds = b;
        return this;
    }
    public String toString() {
        StringBuilder sb = new StringBuilder();
        sb.append("java.util.regex.Matcher");
        sb.append("[pattern=" + pattern());
        sb.append(" region=");
        sb.append(regionStart() + "," + regionEnd());
        sb.append(" lastmatch=");
        if ((first >= 0) && (group() != null)) {
            sb.append(group());
        }
        sb.append("]");
        return sb.toString();
    }
    public boolean hitEnd() {
        return _hitEnd;
    }
    public boolean requireEnd() {
        return _requireEnd;
    }
    internal boolean search(int from)
    {
        this._hitEnd = false;
        this._requireEnd = false;
        from        = from < 0 ? 0 : from;
        this.first  = from;
        this.oldLast = oldLast < 0 ? from : oldLast;
        for (int i = 0; i < groups.Length; i++)
            groups[i] = -1;
        acceptMode = NOANCHOR;
        boolean result = parentPattern.root.match(this, from, text);
        if (!result)
            this.first = -1;
        this.oldLast = this.last;
        return result;
    }
    internal boolean match(int from, int anchor)
    {
        this._hitEnd = false;
        this._requireEnd = false;
        from        = from < 0 ? 0 : from;
        this.first  = from;
        this.oldLast = oldLast < 0 ? from : oldLast;
        for (int i = 0; i < groups.Length; i++)
            groups[i] = -1;
        acceptMode = anchor;
        boolean result = parentPattern.matchRoot.match(this, from, text);
        if (!result)
            this.first = -1;
        this.oldLast = this.last;
        return result;
    }
    internal int getTextLength() {
        return text.length();
    }
    internal CharSequence getSubSequence(int beginIndex, int endIndex)
    {
        return text.subSequence(beginIndex, endIndex);
    }
    internal char charAt(int i)
    {
        return text.charAt(i);
    }
}
}