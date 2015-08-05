using java.lang;
using java.text;
using JavaPort.Collections;
using System;
using String = java.lang.String;
using StringBuilder = java.lang.StringBuilder;
using Math = java.lang.Math;
using IOException = java.io.IOException;
namespace java.util.regex
{
public sealed class Pattern
{
    public static readonly int UNIX_LINES = 0x01;
    public static readonly int CASE_INSENSITIVE = 0x02;
    public static readonly int COMMENTS = 0x04;
    public static readonly int MULTILINE = 0x08;
    public static readonly int LITERAL = 0x10;
    public static readonly int DOTALL = 0x20;
    public static readonly int UNICODE_CASE = 0x40;
    public static readonly int CANON_EQ = 0x80;
    public static readonly int UNICODE_CHARACTER_CLASS = 0x100;
    private String _pattern;
    private int _flags;
    private boolean compiled = false;
    private String normalizedPattern;
    internal Node root;
    internal Node matchRoot;
    int[] buffer;
    internal Map<String, Integer> _namedGroups;
    GroupHead[] groupNodes;
    private int[] temp;
    internal int capturingGroupCount;
    internal int localCount;
    private int _cursor;
    private int patternLength;
    private boolean hasSupplementary;

    public static Pattern compile(String regex) {
        return new Pattern(regex, 0);
    }

    public static Pattern compile(String regex, int flags) {
        return new Pattern(regex, flags);
    }

    public String pattern() {
        return _pattern;
    }

    public String toString() {
        return _pattern;
    }

    public Matcher matcher(CharSequence input) {
        if (!compiled) {
            lock(this) {
                if (!compiled)
                    compile();
            }
        }
        Matcher m = new Matcher(this, input);
        return m;
    }

    public int flags() {
        return _flags;
    }

    public static boolean matches(String regex, CharSequence input) {
        Pattern p = Pattern.compile(regex);
        Matcher m = p.matcher(input);
        return m.matches();
    }

    public String[] split(CharSequence input, int limit) {
        int index = 0;
        boolean matchLimited = limit > 0;
        ArrayList<String> matchList = new ArrayList<String>();
        Matcher m = matcher(input);
        // Add segments before each match found
        while(m.find()) {
            if (!matchLimited || matchList.size() < limit - 1) {
                String match = input.subSequence(index, m.start()).toString();
                matchList.add(match);
                index = m.end();
            } else if (matchList.size() == limit - 1) { // last one
                String match = input.subSequence(index,
                                                 input.length()).toString();
                matchList.add(match);
                index = m.end();
            }
        }
        // If no match was found, return this
        if (index == 0)
            return new String[] {input.toString()};
        // Add remaining segment
        if (!matchLimited || matchList.size() < limit)
            matchList.add(input.subSequence(index, input.length()).toString());
        // Construct result
        int resultSize = matchList.size();
        if (limit == 0)
            while (resultSize > 0 && matchList.get(resultSize-1).equals(""))
                resultSize--;
        String[] result = new String[resultSize];
        return matchList.subList(0, resultSize).toArray(result);
    }
    public String[] split(CharSequence input) {
        return split(input, 0);
    }
    public static String quote(String s) {
        int slashEIndex = s.indexOf(new String("\\E"));
        if (slashEIndex == -1)
            return new String("\\Q" + s + "\\E");
        StringBuilder sb = new StringBuilder(s.length() * 2);
        sb.append("\\Q");
        slashEIndex = 0;
        int current = 0;
        while ((slashEIndex = s.indexOf(new String("\\E"), current)) != -1) {
            sb.append(s.substring(current, slashEIndex));
            current = slashEIndex + 2;
            sb.append("\\E\\\\E\\Q");
        }
        sb.append(s.substring(current, s.length()));
        sb.append("\\E");
        return sb.toString();
    }
/*
    private void readObject(java.io.ObjectInputStream s) {
        // Read in all fields
        s.defaultReadObject();
        // Initialize counts
        capturingGroupCount = 1;
        localCount = 0;
        // if length > 0, the Pattern is lazily compiled
        compiled = false;
        if (pattern.length() == 0) {
            root = new Start(lastAccept);
            matchRoot = lastAccept;
            compiled = true;
        }
    }
*/
    private Pattern(String p, int f) {
        _pattern = p;
        _flags = f;
        // to use UNICODE_CASE if UNICODE_CHARACTER_CLASS present
        if ((_flags & UNICODE_CHARACTER_CLASS) != 0)
            _flags |= UNICODE_CASE;
        // Reset group index count
        capturingGroupCount = 1;
        localCount = 0;
        if (_pattern.length() > 0) {
            compile();
        } else {
            root = new Start(lastAccept);
            matchRoot = lastAccept;
        }
    }

    private void normalize() {
        boolean inCharClass = false;
        int lastCodePoint = -1;
        // Convert pattern into normalizedD form
        normalizedPattern = Normalizer.normalize(pattern, Normalizer.Form.NFD);
        patternLength = normalizedPattern.length();
        // Modify pattern to match canonical equivalences
        StringBuilder newPattern = new StringBuilder(patternLength);
        for(int i=0; i<patternLength; ) {
            int c = normalizedPattern.codePointAt(i);
            StringBuilder sequenceBuffer;
            if ((Character.getType(c) == Character.NON_SPACING_MARK)
                && (lastCodePoint != -1)) {
                sequenceBuffer = new StringBuilder();
                sequenceBuffer.appendCodePoint(lastCodePoint);
                sequenceBuffer.appendCodePoint(c);
                while(Character.getType(c) == Character.NON_SPACING_MARK) {
                    i += Character.charCount(c);
                    if (i >= patternLength)
                        break;
                    c = normalizedPattern.codePointAt(i);
                    sequenceBuffer.appendCodePoint(c);
                }
                String ea = produceEquivalentAlternation(
                                               sequenceBuffer.toString());
                newPattern.setLength(newPattern.length()-Character.charCount(lastCodePoint));
                newPattern.append("(?:").append(ea).append(")");
            } else if (c == '[' && lastCodePoint != '\\') {
                i = normalizeCharClass(newPattern, i);
            } else {
                newPattern.appendCodePoint(c);
            }
            lastCodePoint = c;
            i += Character.charCount(c);
        }
        normalizedPattern = newPattern.toString();
    }

    private int normalizeCharClass(StringBuilder newPattern, int i) {
        StringBuilder charClass = new StringBuilder();
        StringBuilder eq = null;
        int lastCodePoint = -1;
        String result;
        i++;
        charClass.append("[");
        while(true) {
            int c = normalizedPattern.codePointAt(i);
            StringBuilder sequenceBuffer;
            if (c == ']' && lastCodePoint != '\\') {
                charClass.append((char)c);
                break;
            } else if (Character.getType(c) == Character.NON_SPACING_MARK) {
                sequenceBuffer = new StringBuilder();
                sequenceBuffer.appendCodePoint(lastCodePoint);
                while(Character.getType(c) == Character.NON_SPACING_MARK) {
                    sequenceBuffer.appendCodePoint(c);
                    i += Character.charCount(c);
                    if (i >= normalizedPattern.length())
                        break;
                    c = normalizedPattern.codePointAt(i);
                }
                String ea = produceEquivalentAlternation(
                                                  sequenceBuffer.toString());
                charClass.setLength(charClass.length()-Character.charCount(lastCodePoint));
                if (eq == null)
                    eq = new StringBuilder();
                eq.append('|');
                eq.append(ea);
            } else {
                charClass.appendCodePoint(c);
                i++;
            }
            if (i == normalizedPattern.length())
                throw error(new String("Unclosed character class"));
            lastCodePoint = c;
        }
        if (eq != null) {
            result = new String("(?:"+charClass.toString()+eq.toString()+")");
        } else {
            result = charClass.toString();
        }
        newPattern.append(result);
        return i;
    }

    private String produceEquivalentAlternation(String source) {
        int len = countChars(source, 0, 1);
        if (source.length() == len)
            // source has one character.
            return source;
        String @base = source.substring(0,len);
        String combiningMarks = source.substring(len);
        String[] perms = producePermutations(combiningMarks);
        StringBuilder result = new StringBuilder(source);
        // Add combined permutations
        for(int x=0; x<perms.Length; x++) {
            String next = @base + perms[x];
            if (x>0)
                result.append("|"+next);
            next = composeOneStep(next);
            if (next != null)
                result.append("|"+produceEquivalentAlternation(next));
        }
        return result.toString();
    }

    private String[] producePermutations(String input) {
        if (input.length() == countChars(input, 0, 1))
            return new String[] {input};
        if (input.length() == countChars(input, 0, 2)) {
            int c0 = Character.codePointAt(input, 0);
            int c1 = Character.codePointAt(input, Character.charCount(c0));
            if (getClass(c1) == getClass(c0)) {
                return new String[] {input};
            }
            String[] result = new String[2];
            result[0] = input;
            StringBuilder sb = new StringBuilder(2);
            sb.appendCodePoint(c1);
            sb.appendCodePoint(c0);
            result[1] = sb.toString();
            return result;
        }
        int length = 1;
        int nCodePoints = countCodePoints(input);
        for(int x=1; x<nCodePoints; x++)
            length = length * (x+1);
        String[] temp = new String[length];
        int[] combClass = new int[nCodePoints];
        for(int x=0, i=0; x<nCodePoints; x++) {
            int c = Character.codePointAt(input, i);
            combClass[x] = getClass(c);
            i +=  Character.charCount(c);
        }
        // For each char, take it out and add the permutations
        // of the remaining chars
        int index = 0;
        int len;
        // offset maintains the index in code units.
        for(int x=0, offset=0; x<nCodePoints; x++, offset+=len) {
continue1:
            len = countChars(input, offset, 1);
            boolean skip = false;
            for(int y=x-1; y>=0; y--) {
                if (combClass[y] == combClass[x]) {
                    offset+=len;
                    if (x < nCodePoints) goto continue1;
                    goto end1;
                }
            }
            StringBuilder sb = new StringBuilder(input);
            String otherChars = sb.delete(offset, offset+len).toString();
            String[] subResult = producePermutations(otherChars);
            String prefix = input.substring(offset, offset+len);
            for(int y=0; y<subResult.Length; y++)
                temp[index++] =  prefix + subResult[y];
        }
end1:
        String[] _result = new String[index];
        for (int x=0; x<index; x++)
            _result[x] = temp[x];
        return _result;
    }

    private int getClass(int c) {
        throw new NotImplementedException();
        //return sun.text.Normalizer.getCombiningClass(c);
    }

    private String composeOneStep(String input) {
        int len = countChars(input, 0, 2);
        String firstTwoCharacters = input.substring(0, len);
        String result = Normalizer.normalize(firstTwoCharacters, Normalizer.Form.NFC);
        if (result.equals(firstTwoCharacters))
            return null;
        else {
            String remainder = input.substring(len);
            return result + remainder;
        }
    }

    private void RemoveQEQuoting() {
        int pLen = patternLength;
        int i = 0;
        while (i < pLen-1) {
            if (temp[i] != '\\')
                i += 1;
            else if (temp[i + 1] != 'Q')
                i += 2;
            else
                break;
        }
        if (i >= pLen - 1)    // No \Q sequence found
            return;
        int j = i;
        i += 2;
        int[] newtemp = new int[j + 2*(pLen-i) + 2];
        Array.Copy(temp, 0, newtemp, 0, j);
        boolean inQuote = true;
        while (i < pLen) {
            int c = temp[i++];
            if (! ASCII.isAscii(c) || ASCII.isAlnum(c)) {
                newtemp[j++] = c;
            } else if (c != '\\') {
                if (inQuote) newtemp[j++] = '\\';
                newtemp[j++] = c;
            } else if (inQuote) {
                if (temp[i] == 'E') {
                    i++;
                    inQuote = false;
                } else {
                    newtemp[j++] = '\\';
                    newtemp[j++] = '\\';
                }
            } else {
                if (temp[i] == 'Q') {
                    i++;
                    inQuote = true;
                } else {
                    newtemp[j++] = c;
                    if (i != pLen)
                        newtemp[j++] = temp[i++];
                }
            }
        }
        patternLength = j;
        temp = Arrays.copyOf(newtemp, j + 2); // double zero termination
    }
    private void compile() {
        // Handle canonical equivalences
        if (has(CANON_EQ) && !has(LITERAL)) {
            normalize();
        } else {
            normalizedPattern = _pattern;
        }
        patternLength = normalizedPattern.length();
        // Copy pattern to int array for convenience
        // Use double zero to terminate pattern
        temp = new int[patternLength + 2];
        hasSupplementary = false;
        int c, count = 0;
        // Convert all chars into code points
        for (int x = 0; x < patternLength; x += Character.charCount(c)) {
            c = normalizedPattern.codePointAt(x);
            if (isSupplementary(c)) {
                hasSupplementary = true;
            }
            temp[count++] = c;
        }
        patternLength = count;   // patternLength now in code points
        if (! has(LITERAL))
            RemoveQEQuoting();
        // Allocate all temporary objects here.
        buffer = new int[32];
        groupNodes = new GroupHead[10];
        _namedGroups = null;
        if (has(LITERAL)) {
            // Literal pattern handling
            matchRoot = newSlice(temp, patternLength, hasSupplementary);
            matchRoot.next = lastAccept;
        } else {
            // Start recursive descent parsing
            matchRoot = expr(lastAccept);
            // Check extra pattern characters
            if (patternLength != _cursor) {
                if (peek() == ')') {
                    throw error(new String("Unmatched closing ')'"));
                } else {
                    throw error(new String("Unexpected internal error"));
                }
            }
        }
        // Peephole optimization
        if (matchRoot is Slice) {
            root = BnM.optimize(matchRoot);
            if (root == matchRoot) {
                root = hasSupplementary ? new StartS(matchRoot) : new Start(matchRoot);
            }
        } else if (matchRoot is Begin || matchRoot is First) {
            root = matchRoot;
        } else {
            root = hasSupplementary ? new StartS(matchRoot) : new Start(matchRoot);
        }
        // Release temporary storage
        temp = null;
        buffer = null;
        groupNodes = null;
        patternLength = 0;
        compiled = true;
    }
    internal Map<String, Integer> namedGroups() {
        if (_namedGroups == null)
            _namedGroups = new HashMap<String, Integer>(2);
        return _namedGroups;
    }

    internal sealed class TreeInfo {
        internal int minLength;
        internal int maxLength;
        internal boolean maxValid;
        internal boolean deterministic;
        internal TreeInfo() {
            reset();
        }
        internal void reset() {
            minLength = 0;
            maxLength = 0;
            maxValid = true;
            deterministic = true;
        }
    }
    /*
     * The following private methods are mainly used to improve the
     * readability of the code. In order to let the Java compiler easily
     * inline them, we should not put many assertions or error checks in them.
     */
    private boolean has(int f) {
        return (_flags & f) != 0;
    }
    private void accept(int ch, String s) {
        int testChar = temp[_cursor++];
        if (has(COMMENTS))
            testChar = parsePastWhitespace(testChar);
        if (ch != testChar) {
            throw error(s);
        }
    }
    private void mark(int c) {
        temp[patternLength] = c;
    }
    private int peek() {
        int ch = temp[_cursor];
        if (has(COMMENTS))
            ch = peekPastWhitespace(ch);
        return ch;
    }
    private int read() {
        int ch = temp[_cursor++];
        if (has(COMMENTS))
            ch = parsePastWhitespace(ch);
        return ch;
    }
    private int readEscaped() {
        int ch = temp[_cursor++];
        return ch;
    }
    private int next() {
        int ch = temp[++_cursor];
        if (has(COMMENTS))
            ch = peekPastWhitespace(ch);
        return ch;
    }
    private int nextEscaped() {
        int ch = temp[++_cursor];
        return ch;
    }
    private int peekPastWhitespace(int ch) {
        while (ASCII.isSpace(ch) || ch == '#') {
            while (ASCII.isSpace(ch))
                ch = temp[++_cursor];
            if (ch == '#') {
                ch = peekPastLine();
            }
        }
        return ch;
    }
    private int parsePastWhitespace(int ch) {
        while (ASCII.isSpace(ch) || ch == '#') {
            while (ASCII.isSpace(ch))
                ch = temp[_cursor++];
            if (ch == '#')
                ch = parsePastLine();
        }
        return ch;
    }
    private int parsePastLine() {
        int ch = temp[_cursor++];
        while (ch != 0 && !isLineSeparator(ch))
            ch = temp[_cursor++];
        return ch;
    }
    private int peekPastLine() {
        int ch = temp[++_cursor];
        while (ch != 0 && !isLineSeparator(ch))
            ch = temp[++_cursor];
        return ch;
    }
    private boolean isLineSeparator(int ch) {
        if (has(UNIX_LINES)) {
            return ch == '\n';
        } else {
            return (ch == '\n' ||
                    ch == '\r' ||
                    (ch|1) == '\u2029' ||
                    ch == '\u0085');
        }
    }
    private int skip() {
        int i = _cursor;
        int ch = temp[i+1];
        _cursor = i + 2;
        return ch;
    }
    private void unread() {
        _cursor--;
    }
    private PatternSyntaxException error(String s) {
        return new PatternSyntaxException(s, normalizedPattern,  _cursor - 1);
    }
    private boolean findSupplementary(int start, int end) {
        for (int i = start; i < end; i++) {
            if (isSupplementary(temp[i]))
                return true;
        }
        return false;
    }
    private static boolean isSupplementary(int ch) {
        return ch >= Character.MIN_SUPPLEMENTARY_CODE_POINT ||
               Character.isSurrogate((char)ch);
    }
    private Node expr(Node end) {
        Node prev = null;
        Node firstTail = null;
        Node branchConn = null;
        for (;;) {
            Node node = sequence(end);
            Node nodeTail = root;      //double return
            if (prev == null) {
                prev = node;
                firstTail = nodeTail;
            } else {
                // Branch
                if (branchConn == null) {
                    branchConn = new BranchConn();
                    branchConn.next = end;
                }
                if (node == end) {
                    // if the node returned from sequence() is "end"
                    // we have an empty expr, set a null atom into
                    // the branch to indicate to go "next" directly.
                    node = null;
                } else {
                    // the "tail.next" of each atom goes to branchConn
                    nodeTail.next = branchConn;
                }
                if (prev is Branch) {
                    ((Branch)prev).add(node);
                } else {
                    if (prev == end) {
                        prev = null;
                    } else {
                        // replace the "end" with "branchConn" at its tail.next
                        // when put the "prev" into the branch as the first atom.
                        firstTail.next = branchConn;
                    }
                    prev = new Branch(prev, node, branchConn);
                }
            }
            if (peek() != '|') {
                return prev;
            }
            next();
        }
    }
    private Node sequence(Node end) {
        Node head = null;
        Node tail = null;
        Node node = null;
        for (;;) {
            int ch = peek();
            switch (ch) {
            case '(':
                // Because group handles its own closure,
                // we need to treat it differently
                node = group0();
                // Check for comment or flag group
                if (node == null)
                    continue;
                if (head == null)
                    head = node;
                else
                    tail.next = node;
                // Double return: Tail was returned in root
                tail = root;
                continue;
            case '[':
                node = clazz(true);
                break;
            case '\\':
                ch = nextEscaped();
                if (ch == 'p' || ch == 'P') {
                    boolean oneLetter = true;
                    boolean comp = (ch == 'P');
                    ch = next(); // Consume { if present
                    if (ch != '{') {
                        unread();
                    } else {
                        oneLetter = false;
                    }
                    node = family(oneLetter, comp);
                } else {
                    unread();
                    node = atom();
                }
                break;
            case '^':
                next();
                if (has(MULTILINE)) {
                    if (has(UNIX_LINES))
                        node = new UnixCaret();
                    else
                        node = new Caret();
                } else {
                    node = new Begin();
                }
                break;
            case '$':
                next();
                if (has(UNIX_LINES))
                    node = new UnixDollar(has(MULTILINE));
                else
                    node = new Dollar(has(MULTILINE));
                break;
            case '.':
                next();
                if (has(DOTALL)) {
                    node = new All();
                } else {
                    if (has(UNIX_LINES))
                        node = new UnixDot();
                    else {
                        node = new Dot();
                    }
                }
                break;
            case '|':
            case ')':
                goto end2;
            case ']': // Now interpreting dangling ] and } as literals
            case '}':
                node = atom();
                break;
            case '?':
            case '*':
            case '+':
                next();
                throw error(new String("Dangling meta character '" + ((char)ch) + "'"));
            case 0:
                if (_cursor >= patternLength) {
                    goto end2;
                }
                // Fall through
                node = atom();
                break;
            default:
                node = atom();
                break;
            }
            node = closure(node);
            if (head == null) {
                head = tail = node;
            } else {
                tail.next = node;
                tail = node;
            }
        }
end2:
        if (head == null) {
            return end;
        }
        tail.next = end;
        root = tail;      //double return
        return head;
    }
    private Node atom() {
        int first = 0;
        int prev = -1;
        boolean hasSupplementary = false;
        int ch = peek();
        for (;;) {
            switch (ch) {
            case '*':
            case '+':
            case '?':
            case '{':
                if (first > 1) {
                    _cursor = prev;    // Unwind one character
                    first--;
                }
                break;
            case '$':
            case '.':
            case '^':
            case '(':
            case '[':
            case '|':
            case ')':
                break;
            case '\\':
                ch = nextEscaped();
                if (ch == 'p' || ch == 'P') { // Property
                    if (first > 0) { // Slice is waiting; handle it first
                        unread();
                        break;
                    } else { // No slice; just return the family node
                        boolean comp = (ch == 'P');
                        boolean oneLetter = true;
                        ch = next(); // Consume { if present
                        if (ch != '{')
                            unread();
                        else
                            oneLetter = false;
                        return family(oneLetter, comp);
                    }
                }
                unread();
                prev = _cursor;
                ch = escape(false, first == 0);
                if (ch >= 0) {
                    append(ch, first);
                    first++;
                    if (isSupplementary(ch)) {
                        hasSupplementary = true;
                    }
                    ch = peek();
                    continue;
                } else if (first == 0) {
                    return root;
                }
                // Unwind meta escape sequence
                _cursor = prev;
                break;
            case 0:
                if (_cursor >= patternLength) {
                    break;
                }
                // Fall through
                prev = _cursor;
                append(ch, first);
                first++;
                if (isSupplementary(ch))
                {
                    hasSupplementary = true;
                }
                ch = next();
                break;
            default:
                prev = _cursor;
                append(ch, first);
                first++;
                if (isSupplementary(ch)) {
                    hasSupplementary = true;
                }
                ch = next();
                continue;
            }
            break;
        }
        if (first == 1) {
            return newSingle(buffer[0]);
        } else {
            return newSlice(buffer, first, hasSupplementary);
        }
    }
    private void append(int ch, int len) {
        if (len >= buffer.Length) {
            int[] tmp = new int[len+len];
            Array.Copy(buffer, 0, tmp, 0, len);
            buffer = tmp;
        }
        buffer[len] = ch;
    }
    private Node @ref(int refNum) {
        boolean done = false;
        while(!done) {
            int ch = peek();
            switch(ch) {
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                int newRefNum = (refNum * 10) + (ch - '0');
                // Add another number if it doesn't make a group
                // that doesn't exist
                if (capturingGroupCount - 1 < newRefNum) {
                    done = true;
                    break;
                }
                refNum = newRefNum;
                read();
                break;
            default:
                done = true;
                break;
            }
        }
        if (has(CASE_INSENSITIVE))
            return new CIBackRef(refNum, has(UNICODE_CASE));
        else
            return new BackRef(refNum);
    }
    private int escape(boolean inclass, boolean create) {
        int ch = skip();
        switch (ch) {
        case '0':
            return o();
        case '1':
        case '2':
        case '3':
        case '4':
        case '5':
        case '6':
        case '7':
        case '8':
        case '9':
            if (inclass) break;
            if (create) {
                root = @ref((ch - '0'));
            }
            return -1;
        case 'A':
            if (inclass) break;
            if (create) root = new Begin();
            return -1;
        case 'B':
            if (inclass) break;
            if (create) root = new Bound(Bound.NONE, has(UNICODE_CHARACTER_CLASS));
            return -1;
        case 'C':
            break;
        case 'D':
            if (create) root = has(UNICODE_CHARACTER_CLASS)
                               ? new Utype(UnicodeProp.DIGIT).complement()
                               : new Ctype(ASCII.DIGIT).complement();
            return -1;
        case 'E':
        case 'F':
            break;
        case 'G':
            if (inclass) break;
            if (create) root = new LastMatch();
            return -1;
        case 'H':
        case 'I':
        case 'J':
        case 'K':
        case 'L':
        case 'M':
        case 'N':
        case 'O':
        case 'P':
        case 'Q':
        case 'R':
            break;
        case 'S':
            if (create) root = has(UNICODE_CHARACTER_CLASS)
                               ? new Utype(UnicodeProp.WHITE_SPACE).complement()
                               : new Ctype(ASCII.SPACE).complement();
            return -1;
        case 'T':
        case 'U':
        case 'V':
            break;
        case 'W':
            if (create) root = has(UNICODE_CHARACTER_CLASS)
                               ? new Utype(UnicodeProp.WORD).complement()
                               : new Ctype(ASCII.WORD).complement();
            return -1;
        case 'X':
        case 'Y':
            break;
        case 'Z':
            if (inclass) break;
            if (create) {
                if (has(UNIX_LINES))
                    root = new UnixDollar(false);
                else
                    root = new Dollar(false);
            }
            return -1;
        case 'a':
            return '\u0007';
        case 'b':
            if (inclass) break;
            if (create) root = new Bound(Bound.BOTH, has(UNICODE_CHARACTER_CLASS));
            return -1;
        case 'c':
            return c();
        case 'd':
            if (create) root = has(UNICODE_CHARACTER_CLASS)
                               ? (Node)new Utype(UnicodeProp.DIGIT)
                               : (Node)new Ctype(ASCII.DIGIT);
            return -1;
        case 'e':
            return '\u001B';
        case 'f':
            return '\f';
        case 'g':
        case 'h':
        case 'i':
        case 'j':
            break;
        case 'k':
            if (inclass)
                break;
            if (read() != '<')
                throw error(new String("\\k is not followed by '<' for named capturing group"));
            String name = groupname(read());
            if (!namedGroups().containsKey(name))
                throw error(new String("(named capturing group <"+ name+"> does not exit"));
            if (create) {
                if (has(CASE_INSENSITIVE))
                    root = new CIBackRef(namedGroups().get(name), has(UNICODE_CASE));
                else
                    root = new BackRef(namedGroups().get(name));
            }
            return -1;
        case 'l':
        case 'm':
            break;
        case 'n':
            return '\n';
        case 'o':
        case 'p':
        case 'q':
            break;
        case 'r':
            return '\r';
        case 's':
            if (create) root = has(UNICODE_CHARACTER_CLASS)
                               ? (Node)new Utype(UnicodeProp.WHITE_SPACE)
                               : (Node)new Ctype(ASCII.SPACE);
            return -1;
        case 't':
            return '\t';
        case 'u':
            return u();
        case 'v':
            return '\u000B';
        case 'w':
            if (create) root = has(UNICODE_CHARACTER_CLASS)
                               ? (Node)new Utype(UnicodeProp.WORD)
                               : (Node)new Ctype(ASCII.WORD);
            return -1;
        case 'x':
            return x();
        case 'y':
            break;
        case 'z':
            if (inclass) break;
            if (create) root = new End();
            return -1;
        default:
            return ch;
        }
        throw error(new String("Illegal/unsupported escape sequence"));
    }
    private CharProperty clazz(boolean consume) {
        CharProperty prev = null;
        CharProperty node = null;
        BitClass bits = new BitClass();
        boolean include = true;
        boolean firstInClass = true;
        int ch = next();
        for (;;) {
            switch (ch) {
                case '^':
                    // Negates if first char in a class, otherwise literal
                    if (firstInClass) {
                        if (temp[_cursor-1] != '[')
                            break;
                        ch = next();
                        include = !include;
                        continue;
                    } else {
                        // ^ not first in class, treat as literal
                        break;
                    }
                case '[':
                    firstInClass = false;
                    node = clazz(true);
                    if (prev == null)
                        prev = node;
                    else
                        prev = union(prev, node);
                    ch = peek();
                    continue;
                case '&':
                    firstInClass = false;
                    ch = next();
                    if (ch == '&') {
                        ch = next();
                        CharProperty rightNode = null;
                        while (ch != ']' && ch != '&') {
                            if (ch == '[') {
                                if (rightNode == null)
                                    rightNode = clazz(true);
                                else
                                    rightNode = union(rightNode, clazz(true));
                            } else { // abc&&def
                                unread();
                                rightNode = clazz(false);
                            }
                            ch = peek();
                        }
                        if (rightNode != null)
                            node = rightNode;
                        if (prev == null) {
                            if (rightNode == null)
                                throw error(new String("Bad class syntax"));
                            else
                                prev = rightNode;
                        } else {
                            prev = intersection(prev, node);
                        }
                    } else {
                        // treat as a literal &
                        unread();
                        break;
                    }
                    continue;
                case 0:
                    firstInClass = false;
                    if (_cursor >= patternLength)
                        throw error(new String("Unclosed character class"));
                    break;
                case ']':
                    firstInClass = false;
                    if (prev != null) {
                        if (consume)
                            next();
                        return prev;
                    }
                    break;
                default:
                    firstInClass = false;
                    break;
            }
            node = range(bits);
            if (include) {
                if (prev == null) {
                    prev = node;
                } else {
                    if (prev != node)
                        prev = union(prev, node);
                }
            } else {
                if (prev == null) {
                    prev = node.complement();
                } else {
                    if (prev != node)
                        prev = setDifference(prev, node);
                }
            }
            ch = peek();
        }
    }
    private CharProperty bitsOrSingle(BitClass bits, int ch) {
        /* Bits can only handle codepoints in [u+0000-u+00ff] range.
           Use "single" node instead of bits when dealing with unicode
           case folding for codepoints listed below.
           (1)Uppercase out of range: u+00ff, u+00b5
              toUpperCase(u+00ff) -> u+0178
              toUpperCase(u+00b5) -> u+039c
           (2)LatinSmallLetterLongS u+17f
              toUpperCase(u+017f) -> u+0053
           (3)LatinSmallLetterDotlessI u+131
              toUpperCase(u+0131) -> u+0049
           (4)LatinCapitalLetterIWithDotAbove u+0130
              toLowerCase(u+0130) -> u+0069
           (5)KelvinSign u+212a
              toLowerCase(u+212a) ==> u+006B
           (6)AngstromSign u+212b
              toLowerCase(u+212b) ==> u+00e5
        */
        //int d;
        if (ch < 256 &&
            !(has(CASE_INSENSITIVE) && has(UNICODE_CASE) &&
              (ch == 0xff || ch == 0xb5 ||
               ch == 0x49 || ch == 0x69 ||  //I and i
               ch == 0x53 || ch == 0x73 ||  //S and s
               ch == 0x4b || ch == 0x6b ||  //K and k
               ch == 0xc5 || ch == 0xe5)))  //A+ring
            return bits.add(ch, flags());
        return newSingle(ch);
    }
    private CharProperty range(BitClass bits) {
        int ch = peek();
        if (ch == '\\') {
            ch = nextEscaped();
            if (ch == 'p' || ch == 'P') { // A property
                boolean comp = (ch == 'P');
                boolean oneLetter = true;
                // Consume { if present
                ch = next();
                if (ch != '{')
                    unread();
                else
                    oneLetter = false;
                return family(oneLetter, comp);
            } else { // ordinary escape
                unread();
                ch = escape(true, true);
                if (ch == -1)
                    return (CharProperty) root;
            }
        } else {
            ch = single();
        }
        if (ch >= 0) {
            if (peek() == '-') {
                int endRange = temp[_cursor+1];
                if (endRange == '[') {
                    return bitsOrSingle(bits, ch);
                }
                if (endRange != ']') {
                    next();
                    int m = single();
                    if (m < ch)
                        throw error(new String("Illegal character range"));
                    if (has(CASE_INSENSITIVE))
                        return caseInsensitiveRangeFor(ch, m);
                    else
                        return rangeFor(ch, m);
                }
            }
            return bitsOrSingle(bits, ch);
        }
        throw error(new String("Unexpected character '"+((char)ch)+"'"));
    }
    private int single() {
        int ch = peek();
        switch (ch) {
        case '\\':
            return escape(true, false);
        default:
            next();
            return ch;
        }
    }
    private CharProperty family(boolean singleLetter,
                                boolean maybeComplement)
    {
        next();
        String name;
        CharProperty node = null;
        int i;
        if (singleLetter) {
            int c = temp[_cursor];
            if (!Character.isSupplementaryCodePoint(c)) {
                name = String.valueOf((char)c);
            } else {
                name = new String(temp, _cursor, 1);
            }
            read();
        } else {
            i = _cursor;
            mark('}');
            while(read() != '}') {
            }
            mark('\u0000');
            int j = _cursor;
            if (j > patternLength)
                throw error(new String("Unclosed character family"));
            if (i + 1 >= j)
                throw error(new String("Empty character family"));
            name = new String(temp, i, j-i-1);
        }
        i = name.indexOf('=');
        if (i != -1) {
            // property construct \p{name=value}
            String value = name.substring(i + 1);
            name = name.substring(0, i).toLowerCase();
            if (new String("sc").equals(name) || new String("script").equals(name)) {
                node = unicodeScriptPropertyFor(value);
            } else if (new String("blk").equals(name) || new String("block").equals(name)) {
                node = unicodeBlockPropertyFor(value);
            } else if (new String("gc").equals(name) || new String("general_category").equals(name)) {
                node = charPropertyNodeFor(value);
            } else {
                throw error(new String("Unknown Unicode property {name=<" + name + ">, "
                             + "value=<" + value + ">}"));
            }
        } else {
            if (name.startsWith(new String("In"))) {
                // \p{inBlockName}
                node = unicodeBlockPropertyFor(name.substring(2));
            } else if (name.startsWith(new String("Is"))) {
                // \p{isGeneralCategory} and \p{isScriptName}
                name = name.substring(2);
                UnicodeProp uprop = UnicodeProp.forName(name);
                if (uprop != null)
                    node = new Utype(uprop);
                if (node == null)
                    node = CharPropertyNames.charPropertyFor(name);
                if (node == null)
                    node = unicodeScriptPropertyFor(name);
            } else {
                if (has(UNICODE_CHARACTER_CLASS)) {
                    UnicodeProp uprop = UnicodeProp.forPOSIXName(name);
                    if (uprop != null)
                        node = new Utype(uprop);
                }
                if (node == null)
                    node = charPropertyNodeFor(name);
            }
        }
        if (maybeComplement) {
            if (node is Category || node is Block)
                hasSupplementary = true;
            node = node.complement();
        }
        return node;
    }
    private CharProperty unicodeScriptPropertyFor(String name) {
        Character.UnicodeScript script;
        try {
            script = Character.UnicodeScript.forName(name);
        } catch (IllegalArgumentException) {
            throw error(new String("Unknown character script name {" + name + "}"));
        }
        return new Script(script);
    }
    private CharProperty unicodeBlockPropertyFor(String name) {
        Character.UnicodeBlock block;
        try {
            block = Character.UnicodeBlock.forName(name);
        } catch (IllegalArgumentException) {
            throw error(new String("Unknown character block name {" + name + "}"));
        }
        return new Block(block);
    }
    private CharProperty charPropertyNodeFor(String name) {
        CharProperty p = CharPropertyNames.charPropertyFor(name);
        if (p == null)
            throw error(new String("Unknown character property name {" + name + "}"));
        return p;
    }
    private String groupname(int ch) {
        StringBuilder sb = new StringBuilder();
        sb.append(Character.toChars(ch));
        while (ASCII.isLower(ch=read()) || ASCII.isUpper(ch) ||
               ASCII.isDigit(ch)) {
            sb.append(Character.toChars(ch));
        }
        if (sb.length() == 0)
            throw error(new String("named capturing group has 0 length name"));
        if (ch != '>')
            throw error(new String("named capturing group is missing trailing '>'"));
        return sb.toString();
    }
    private Node group0() {
        boolean capturingGroup = false;
        Node head = null;
        Node tail = null;
        int save = _flags;
        root = null;
        int ch = next();
        if (ch == '?') {
            ch = skip();
            switch (ch) {
            case ':':   //  (?:xxx) pure group
                head = createGroup(true);
                tail = root;
                head.next = expr(tail);
                break;
            case '=':   // (?=xxx) and (?!xxx) lookahead
            case '!':
                head = createGroup(true);
                tail = root;
                head.next = expr(tail);
                if (ch == '=') {
                    head = tail = new Pos(head);
                } else {
                    head = tail = new Neg(head);
                }
                break;
            case '>':   // (?>xxx)  independent group
                head = createGroup(true);
                tail = root;
                head.next = expr(tail);
                head = tail = new Ques(head, INDEPENDENT);
                break;
            case '<':   // (?<xxx)  look behind
                ch = read();
                if (ASCII.isLower(ch) || ASCII.isUpper(ch)) {
                    // named captured group
                    String name = groupname(ch);
                    if (namedGroups().containsKey(name))
                        throw error(new String("Named capturing group <" + name
                                    + "> is already defined"));
                    capturingGroup = true;
                    head = createGroup(false);
                    tail = root;
                    namedGroups().put(name, new Integer(capturingGroupCount-1));
                    head.next = expr(tail);
                    break;
                }
                int start = _cursor;
                head = createGroup(true);
                tail = root;
                head.next = expr(tail);
                tail.next = lookbehindEnd;
                TreeInfo info = new TreeInfo();
                head.study(info);
                if (info.maxValid == false) {
                    throw error(new String("Look-behind group does not have "
                                + "an obvious maximum length"));
                }
                boolean hasSupplementary = findSupplementary(start, patternLength);
                if (ch == '=') {
                    head = tail = (hasSupplementary ?
                                   new BehindS(head, info.maxLength,
                                               info.minLength) :
                                   new Behind(head, info.maxLength,
                                              info.minLength));
                } else if (ch == '!') {
                    head = tail = (hasSupplementary ?
                                   new NotBehindS(head, info.maxLength,
                                                  info.minLength) :
                                   new NotBehind(head, info.maxLength,
                                                 info.minLength));
                } else {
                    throw error(new String("Unknown look-behind group"));
                }
                break;
            case '$':
            case '@':
                throw error(new String("Unknown group type"));
            default:    // (?xxx:) inlined match flags
                unread();
                addFlag();
                ch = read();
                if (ch == ')') {
                    return null;    // Inline modifier only
                }
                if (ch != ':') {
                    throw error(new String("Unknown inline modifier"));
                }
                head = createGroup(true);
                tail = root;
                head.next = expr(tail);
                break;
            }
        } else { // (xxx) a regular group
            capturingGroup = true;
            head = createGroup(false);
            tail = root;
            head.next = expr(tail);
        }
        accept(')', new String("Unclosed group"));
        _flags = save;
        // Check for quantifiers
        Node node = closure(head);
        if (node == head) { // No closure
            root = tail;
            return node;    // Dual return
        }
        if (head == tail) { // Zero length assertion
            root = node;
            return node;    // Dual return
        }
        if (node is Ques) {
            Ques ques = (Ques) node;
            if (ques.type == POSSESSIVE) {
                root = node;
                return node;
            }
            tail.next = new BranchConn();
            tail = tail.next;
            if (ques.type == GREEDY) {
                head = new Branch(head, null, tail);
            } else { // Reluctant quantifier
                head = new Branch(null, head, tail);
            }
            root = tail;
            return head;
        } else if (node is Curly) {
            Curly curly = (Curly) node;
            if (curly.type == POSSESSIVE) {
                root = node;
                return node;
            }
            // Discover if the group is deterministic
            TreeInfo info = new TreeInfo();
            if (head.study(info)) { // Deterministic
                GroupTail temp = (GroupTail) tail;
                head = root = new GroupCurly(head.next, curly.cmin,
                                   curly.cmax, curly.type,
                                   ((GroupTail)tail).localIndex,
                                   ((GroupTail)tail).groupIndex,
                                             capturingGroup);
                return head;
            } else { // Non-deterministic
                int temp = ((GroupHead) head).localIndex;
                Loop loop;
                if (curly.type == GREEDY)
                    loop = new Loop(this.localCount, temp);
                else  // Reluctant Curly
                    loop = new LazyLoop(this.localCount, temp);
                Prolog prolog = new Prolog(loop);
                this.localCount += 1;
                loop.cmin = curly.cmin;
                loop.cmax = curly.cmax;
                loop.body = head;
                tail.next = loop;
                root = loop;
                return prolog; // Dual return
            }
        }
        throw error(new String("Internal logic error"));
    }
    private Node createGroup(boolean anonymous) {
        int localIndex = localCount++;
        int groupIndex = 0;
        if (!anonymous)
            groupIndex = capturingGroupCount++;
        GroupHead head = new GroupHead(localIndex);
        root = new GroupTail(localIndex, groupIndex);
        if (!anonymous && groupIndex < 10)
            groupNodes[groupIndex] = head;
        return head;
    }
    private void addFlag() {
        int ch = peek();
        for (;;) {
            switch (ch) {
            case 'i':
                _flags |= CASE_INSENSITIVE;
                break;
            case 'm':
                _flags |= MULTILINE;
                break;
            case 's':
                _flags |= DOTALL;
                break;
            case 'd':
                _flags |= UNIX_LINES;
                break;
            case 'u':
                _flags |= UNICODE_CASE;
                break;
            case 'c':
                _flags |= CANON_EQ;
                break;
            case 'x':
                _flags |= COMMENTS;
                break;
            case 'U':
                _flags |= (UNICODE_CHARACTER_CLASS | UNICODE_CASE);
                break;
            case '-': 
                ch = next();
                subFlag();
                // subFlag then fall through
                return;
            default:
                return;
            }
            ch = next();
        }
    }
    private void subFlag() {
        int ch = peek();
        for (;;) {
            switch (ch) {
            case 'i':
                _flags &= ~CASE_INSENSITIVE;
                break;
            case 'm':
                _flags &= ~MULTILINE;
                break;
            case 's':
                _flags &= ~DOTALL;
                break;
            case 'd':
                _flags &= ~UNIX_LINES;
                break;
            case 'u':
                _flags &= ~UNICODE_CASE;
                break;
            case 'c':
                _flags &= ~CANON_EQ;
                break;
            case 'x':
                _flags &= ~COMMENTS;
                break;
            case 'U':
                _flags &= ~(UNICODE_CHARACTER_CLASS | UNICODE_CASE);
                return;
            default:
                return;
            }
            ch = next();
        }
    }
    const int MAX_REPS   = 0x7FFFFFFF;
    const int GREEDY     = 0;
    const int LAZY       = 1;
    const int POSSESSIVE = 2;
    const int INDEPENDENT = 3;
    private Node closure(Node prev) {
        //Node atom;
        int ch = peek();
        switch (ch) {
        case '?':
            ch = next();
            if (ch == '?') {
                next();
                return new Ques(prev, LAZY);
            } else if (ch == '+') {
                next();
                return new Ques(prev, POSSESSIVE);
            }
            return new Ques(prev, GREEDY);
        case '*':
            ch = next();
            if (ch == '?') {
                next();
                return new Curly(prev, 0, MAX_REPS, LAZY);
            } else if (ch == '+') {
                next();
                return new Curly(prev, 0, MAX_REPS, POSSESSIVE);
            }
            return new Curly(prev, 0, MAX_REPS, GREEDY);
        case '+':
            ch = next();
            if (ch == '?') {
                next();
                return new Curly(prev, 1, MAX_REPS, LAZY);
            } else if (ch == '+') {
                next();
                return new Curly(prev, 1, MAX_REPS, POSSESSIVE);
            }
            return new Curly(prev, 1, MAX_REPS, GREEDY);
        case '{':
            ch = temp[_cursor+1];
            if (ASCII.isDigit(ch)) {
                skip();
                int cmin = 0;
                do {
                    cmin = cmin * 10 + (ch - '0');
                } while (ASCII.isDigit(ch = read()));
                int cmax = cmin;
                if (ch == ',') {
                    ch = read();
                    cmax = MAX_REPS;
                    if (ch != '}') {
                        cmax = 0;
                        while (ASCII.isDigit(ch)) {
                            cmax = cmax * 10 + (ch - '0');
                            ch = read();
                        }
                    }
                }
                if (ch != '}')
                    throw error(new String("Unclosed counted closure"));
                if (((cmin) | (cmax) | (cmax - cmin)) < 0)
                    throw error(new String("Illegal repetition range"));
                Curly curly;
                ch = peek();
                if (ch == '?') {
                    next();
                    curly = new Curly(prev, cmin, cmax, LAZY);
                } else if (ch == '+') {
                    next();
                    curly = new Curly(prev, cmin, cmax, POSSESSIVE);
                } else {
                    curly = new Curly(prev, cmin, cmax, GREEDY);
                }
                return curly;
            } else {
                throw error(new String("Illegal repetition"));
            }
        default:
            return prev;
        }
    }
    private int c() {
        if (_cursor < patternLength) {
            return read() ^ 64;
        }
        throw error(new String("Illegal control escape sequence"));
    }
    private int o() {
        int n = read();
        if (((n-'0')|('7'-n)) >= 0) {
            int m = read();
            if (((m-'0')|('7'-m)) >= 0) {
                int o = read();
                if ((((o-'0')|('7'-o)) >= 0) && (((n-'0')|('3'-n)) >= 0)) {
                    return (n - '0') * 64 + (m - '0') * 8 + (o - '0');
                }
                unread();
                return (n - '0') * 8 + (m - '0');
            }
            unread();
            return (n - '0');
        }
        throw error(new String("Illegal octal escape sequence"));
    }
    private int x() {
        int n = read();
        if (ASCII.isHexDigit(n)) {
            int m = read();
            if (ASCII.isHexDigit(m)) {
                return ASCII.toDigit(n) * 16 + ASCII.toDigit(m);
            }
        } else if (n == '{' && ASCII.isHexDigit(peek())) {
            int ch = 0;
            while (ASCII.isHexDigit(n = read())) {
                ch = (ch << 4) + ASCII.toDigit(n);
                if (ch > Character.MAX_CODE_POINT)
                    throw error(new String("Hexadecimal codepoint is too big"));
            }
            if (n != '}')
                throw error(new String("Unclosed hexadecimal escape sequence"));
            return ch;
        }
        throw error(new String("Illegal hexadecimal escape sequence"));
    }
    private int cursor() {
        return _cursor;
    }
    private void setcursor(int pos) {
        _cursor = pos;
    }
    private int uxxxx() {
        int n = 0;
        for (int i = 0; i < 4; i++) {
            int ch = read();
            if (!ASCII.isHexDigit(ch)) {
                throw error(new String("Illegal Unicode escape sequence"));
            }
            n = n * 16 + ASCII.toDigit(ch);
        }
        return n;
    }
    private int u() {
        int n = uxxxx();
        if (Character.isHighSurrogate((char)n)) {
            int cur = cursor();
            if (read() == '\\' && read() == 'u') {
                int n2 = uxxxx();
                if (Character.isLowSurrogate((char)n2))
                    return Character.toCodePoint((char)n, (char)n2);
            }
            setcursor(cur);
        }
        return n;
    }
    //
    // Utility methods for code point support
    //
    private static int countChars(CharSequence seq, int index,
                                        int lengthInCodePoints) {
        // optimization
        if (lengthInCodePoints == 1 && !Character.isHighSurrogate(seq.charAt(index))) {
            return 1;
        }
        int length = seq.length();
        int x = index;
        if (lengthInCodePoints >= 0) {
            for (int i = 0; x < length && i < lengthInCodePoints; i++) {
                if (Character.isHighSurrogate(seq.charAt(x++))) {
                    if (x < length && Character.isLowSurrogate(seq.charAt(x))) {
                        x++;
                    }
                }
            }
            return x - index;
        }
        if (index == 0) {
            return 0;
        }
        int len = -lengthInCodePoints;
        for (int i = 0; x > 0 && i < len; i++) {
            if (Character.isLowSurrogate(seq.charAt(--x))) {
                if (x > 0 && Character.isHighSurrogate(seq.charAt(x-1))) {
                    x--;
                }
            }
        }
        return index - x;
    }
    private static int countCodePoints(CharSequence seq) {
        int length = seq.length();
        int n = 0;
        for (int i = 0; i < length; ) {
            n++;
            if (Character.isHighSurrogate(seq.charAt(i++))) {
                if (i < length && Character.isLowSurrogate(seq.charAt(i))) {
                    i++;
                }
            }
        }
        return n;
    }
    private sealed class BitClass : BmpCharProperty {
        readonly boolean[] bits;
        internal BitClass() { bits = new boolean[256]; }
        private BitClass(boolean[] bits) { this.bits = bits; }
        internal BitClass add(int c, int flags) {
            if ((flags & CASE_INSENSITIVE) != 0) {
                if (ASCII.isAscii(c)) {
                    bits[ASCII.toUpper(c)] = true;
                    bits[ASCII.toLower(c)] = true;
                } else if ((flags & UNICODE_CASE) != 0) {
                    bits[Character.toLowerCase(c)] = true;
                    bits[Character.toUpperCase(c)] = true;
                }
            }
            bits[c] = true;
            return this;
        }
        internal override boolean isSatisfiedBy(int ch) {
            return ch < 256 && bits[ch];
        }
    }
    private CharProperty newSingle(int ch) {
        if (has(CASE_INSENSITIVE)) {
            int lower, upper;
            if (has(UNICODE_CASE)) {
                upper = Character.toUpperCase(ch);
                lower = Character.toLowerCase(upper);
                if (upper != lower)
                    return new SingleU(lower);
            } else if (ASCII.isAscii(ch)) {
                lower = ASCII.toLower(ch);
                upper = ASCII.toUpper(ch);
                if (lower != upper)
                    return new SingleI(lower, upper);
            }
        }
        if (isSupplementary(ch))
            return new SingleS(ch);    // Match a given Unicode character
        return new Single(ch);         // Match a given BMP character
    }
    private Node newSlice(int[] buf, int count, boolean hasSupplementary) {
        int[] tmp = new int[count];
        if (has(CASE_INSENSITIVE)) {
            if (has(UNICODE_CASE)) {
                for (int i = 0; i < count; i++) {
                    tmp[i] = Character.toLowerCase(
                                 Character.toUpperCase(buf[i]));
                }
                return hasSupplementary ? (Node)new SliceUS(tmp) : (Node)new SliceU(tmp);
            }
            for (int i = 0; i < count; i++) {
                tmp[i] = ASCII.toLower(buf[i]);
            }
            return hasSupplementary ? (Node)new SliceIS(tmp) : (Node)new SliceI(tmp);
        }
        for (int i = 0; i < count; i++) {
            tmp[i] = buf[i];
        }
        return hasSupplementary ? (Node)new SliceS(tmp) : (Node)new Slice(tmp);
    }
    internal class Node {
        
        Func<Matcher, int, CharSequence, boolean> match_funk = (matcher, i, seq)=>
        {
            matcher.last = i;
            matcher.groups[0] = matcher.first;
            matcher.groups[1] = matcher.last;
            return true;
        };

        internal Node next;
        internal Node(Func<Matcher, int, CharSequence, boolean> match_funk)
        {
            this.match_funk = match_funk;
            next = Pattern._accept;
        }
        internal Node() {
            next = Pattern._accept;
        }
        internal virtual boolean match(Matcher matcher, int i, CharSequence seq) {
            return match_funk(matcher, i, seq);
        }
        internal virtual boolean study(TreeInfo info) {
            if (next != null) {
                return next.study(info);
            } else {
                return info.deterministic;
            }
        }
    }
    internal class LastNode : Node {
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            if (matcher.acceptMode == Matcher.ENDANCHOR && i != matcher.to)
                return false;
            matcher.last = i;
            matcher.groups[0] = matcher.first;
            matcher.groups[1] = matcher.last;
            return true;
        }
    }
    internal class Start : Node {
        internal int minLength;
        internal Start(Node node) {
            this.next = node;
            TreeInfo info = new TreeInfo();
            next.study(info);
            minLength = info.minLength;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            if (i > matcher.to - minLength) {
                matcher._hitEnd = true;
                return false;
            }
            int guard = matcher.to - minLength;
            for (; i <= guard; i++) {
                if (next.match(matcher, i, seq)) {
                    matcher.first = i;
                    matcher.groups[0] = matcher.first;
                    matcher.groups[1] = matcher.last;
                    return true;
                }
            }
            matcher._hitEnd = true;
            return false;
        }
        internal override boolean study(TreeInfo info)
        {
            next.study(info);
            info.maxValid = false;
            info.deterministic = false;
            return false;
        }
    }
    /*
     * StartS supports supplementary characters, including unpaired surrogates.
     */
    sealed class StartS : Start {
        internal StartS(Node node) : base(node) {
            
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            if (i > matcher.to - minLength) {
                matcher._hitEnd = true;
                return false;
            }
            int guard = matcher.to - minLength;
            while (i <= guard) {
                //if ((ret = next.match(matcher, i, seq)) || i == guard)
                if (next.match(matcher, i, seq)) {
                    matcher.first = i;
                    matcher.groups[0] = matcher.first;
                    matcher.groups[1] = matcher.last;
                    return true;
                }
                if (i == guard)
                    break;
                // Optimization to move to the next character. This is
                // faster than countChars(seq, i, 1).
                if (Character.isHighSurrogate(seq.charAt(i++))) {
                    if (i < seq.length() &&
                        Character.isLowSurrogate(seq.charAt(i))) {
                        i++;
                    }
                }
            }
            matcher._hitEnd = true;
            return false;
        }
    }
    sealed class Begin : Node {
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int fromIndex = (matcher.anchoringBounds) ?
                matcher.from : 0;
            if (i == fromIndex && next.match(matcher, i, seq)) {
                matcher.first = i;
                matcher.groups[0] = i;
                matcher.groups[1] = matcher.last;
                return true;
            } else {
                return false;
            }
        }
    }
    sealed class End : Node {
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int endIndex = (matcher.anchoringBounds) ?
                matcher.to : matcher.getTextLength();
            if (i == endIndex) {
                matcher._hitEnd = true;
                return next.match(matcher, i, seq);
            }
            return false;
        }
    }
    sealed class Caret : Node {
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int startIndex = matcher.from;
            int endIndex = matcher.to;
            if (!matcher.anchoringBounds) {
                startIndex = 0;
                endIndex = matcher.getTextLength();
            }
            // Perl does not match ^ at end of input even after newline
            if (i == endIndex) {
                matcher._hitEnd = true;
                return false;
            }
            if (i > startIndex) {
                char ch = seq.charAt(i-1);
                if (ch != '\n' && ch != '\r'
                    && (ch|1) != '\u2029'
                    && ch != '\u0085' ) {
                    return false;
                }
                // Should treat /r/n as one newline
                if (ch == '\r' && seq.charAt(i) == '\n')
                    return false;
            }
            return next.match(matcher, i, seq);
        }
    }
    sealed class UnixCaret : Node {
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int startIndex = matcher.from;
            int endIndex = matcher.to;
            if (!matcher.anchoringBounds) {
                startIndex = 0;
                endIndex = matcher.getTextLength();
            }
            // Perl does not match ^ at end of input even after newline
            if (i == endIndex) {
                matcher._hitEnd = true;
                return false;
            }
            if (i > startIndex) {
                char ch = seq.charAt(i-1);
                if (ch != '\n') {
                    return false;
                }
            }
            return next.match(matcher, i, seq);
        }
    }
    sealed class LastMatch : Node {
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            if (i != matcher.oldLast)
                return false;
            return next.match(matcher, i, seq);
        }
    }
    sealed class Dollar : Node {
        boolean multiline;
        internal Dollar(boolean mul) {
            multiline = mul;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int endIndex = (matcher.anchoringBounds) ?
                matcher.to : matcher.getTextLength();
            if (!multiline) {
                if (i < endIndex - 2)
                    return false;
                if (i == endIndex - 2) {
                    char ch = seq.charAt(i);
                    if (ch != '\r')
                        return false;
                    ch = seq.charAt(i + 1);
                    if (ch != '\n')
                        return false;
                }
            }
            // Matches before any line terminator; also matches at the
            // end of input
            // Before line terminator:
            // If multiline, we match here no matter what
            // If not multiline, fall through so that the end
            // is marked as hit; this must be a /r/n or a /n
            // at the very end so the end was hit; more input
            // could make this not match here
            if (i < endIndex) {
                char ch = seq.charAt(i);
                 if (ch == '\n') {
                     // No match between \r\n
                     if (i > 0 && seq.charAt(i-1) == '\r')
                         return false;
                     if (multiline)
                         return next.match(matcher, i, seq);
                 } else if (ch == '\r' || ch == '\u0085' ||
                            (ch|1) == '\u2029') {
                     if (multiline)
                         return next.match(matcher, i, seq);
                 } else { // No line terminator, no match
                     return false;
                 }
            }
            // Matched at current end so hit end
            matcher._hitEnd = true;
            // If a $ matches because of end of input, then more input
            // could cause it to fail!
            matcher._requireEnd = true;
            return next.match(matcher, i, seq);
        }
        internal override boolean study(TreeInfo info)
        {
            next.study(info);
            return info.deterministic;
        }
    }
    sealed class UnixDollar : Node {
        boolean multiline;
        internal UnixDollar(boolean mul) {
            multiline = mul;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int endIndex = (matcher.anchoringBounds) ?
                matcher.to : matcher.getTextLength();
            if (i < endIndex) {
                char ch = seq.charAt(i);
                if (ch == '\n') {
                    // If not multiline, then only possible to
                    // match at very end or one before end
                    if (multiline == false && i != endIndex - 1)
                        return false;
                    // If multiline return next.match without setting
                    // matcher.hitEnd
                    if (multiline)
                        return next.match(matcher, i, seq);
                } else {
                    return false;
                }
            }
            // Matching because at the end or 1 before the end;
            // more input could change this so set hitEnd
            matcher._hitEnd = true;
            // If a $ matches because of end of input, then more input
            // could cause it to fail!
            matcher._requireEnd = true;
            return next.match(matcher, i, seq);
        }
        internal override boolean study(TreeInfo info)
        {
            next.study(info);
            return info.deterministic;
        }
    }
    private class CharProperty : Node {

        Func<int, boolean> isSatisfiedBy_funk;

        internal CharProperty(Func<int, boolean> isSatisfiedBy_funk)
        {
            this.isSatisfiedBy_funk = isSatisfiedBy_funk;
        }

        internal virtual boolean isSatisfiedBy(int ch)
        {
            return isSatisfiedBy_funk(ch);
        }

        internal virtual CharProperty complement()
        {
            return new CharProperty( (ch)=> { return (boolean)!isSatisfiedBy(ch);} );
        }

        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            if (i < matcher.to) {
                int ch = Character.codePointAt(seq, i);
                return isSatisfiedBy(ch)
                    && next.match(matcher, i+Character.charCount(ch), seq);
            } else {
                matcher._hitEnd = true;
                return false;
            }
        }
        internal override boolean study(TreeInfo info)
        {
            info.minLength++;
            info.maxLength++;
            return next.study(info);
        }
    }

    private abstract class BmpCharProperty : CharProperty {
        internal BmpCharProperty()
            : base(null)
        {
        }

        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            if (i < matcher.to) {
                return isSatisfiedBy(seq.charAt(i))
                    && next.match(matcher, i+1, seq);
            } else {
                matcher._hitEnd = true;
                return false;
            }
        }
    }

    sealed class SingleS : CharProperty {
        readonly int c;
        internal SingleS(int c) : base((ch)=> { return ch == c; }) { this.c = c; }
    }
    sealed class Single : BmpCharProperty {
        readonly int c;
        internal Single(int c) { this.c = c; }
        internal override boolean isSatisfiedBy(int ch)
        {
            return ch == c;
        }
    }
    sealed class SingleI : BmpCharProperty {
        readonly int lower;
        readonly int upper;
        internal SingleI(int lower, int upper)
        {
            this.lower = lower;
            this.upper = upper;
        }
        internal override boolean isSatisfiedBy(int ch)
        {
            return ch == lower || ch == upper;
        }
    }
    sealed class SingleU : CharProperty {
        readonly int lower;
        internal SingleU(int lower) : base((ch)=> { return lower == ch || lower == Character.toLowerCase(Character.toUpperCase(ch)); }) {
            this.lower = lower;
        }
    }
    sealed class Block : CharProperty {
        readonly Character.UnicodeBlock block;
        internal Block(Character.UnicodeBlock block)
            : base((ch) => { return block == Character.UnicodeBlock.of(ch); })
        {
            this.block = block;
        }
    }
    sealed class Script : CharProperty {
        readonly Character.UnicodeScript script;
        internal Script(Character.UnicodeScript script)
            : base((ch) => { return script == Character.UnicodeScript.of(ch); })
        {
            this.script = script;
        }
    }
    sealed class Category : CharProperty {
        readonly int typeMask;
        internal Category(int typeMask)
            : base((ch) => { return (typeMask & (1 << Character.getType(ch))) != 0; })
        { this.typeMask = typeMask; }
    }
    sealed class Utype : CharProperty {
        readonly UnicodeProp uprop;
        internal Utype(UnicodeProp uprop)
            : base((ch) => { return uprop.@is(ch); })
        { this.uprop = uprop; }
    }
    sealed class Ctype : BmpCharProperty {
        readonly int ctype;
        internal Ctype(int ctype) { this.ctype = ctype; }
        internal override boolean isSatisfiedBy(int ch)
        {
            return ch < 128 && ASCII.isType(ch, ctype);
        }
    }

    class SliceNode : Node {
        internal int[] buffer;
        internal SliceNode(int[] buf) {
            buffer = buf;
        }
        internal override boolean study(TreeInfo info)
        {
            info.minLength += buffer.Length;
            info.maxLength += buffer.Length;
            return next.study(info);
        }
    }

    sealed class Slice : SliceNode {
        internal Slice(int[] buf)
            : base(buf)
        {
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int[] buf = buffer;
            int len = buf.Length;
            for (int j=0; j<len; j++) {
                if ((i+j) >= matcher.to) {
                    matcher._hitEnd = true;
                    return false;
                }
                if (buf[j] != seq.charAt(i+j))
                    return false;
            }
            return next.match(matcher, i+len, seq);
        }
    }
    class SliceI : SliceNode {
        internal SliceI(int[] buf) : base(buf) {
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int[] buf = buffer;
            int len = buf.Length;
            for (int j=0; j<len; j++) {
                if ((i+j) >= matcher.to) {
                    matcher._hitEnd = true;
                    return false;
                }
                int c = seq.charAt(i+j);
                if (buf[j] != c &&
                    buf[j] != ASCII.toLower(c))
                    return false;
            }
            return next.match(matcher, i+len, seq);
        }
    }
    sealed class SliceU : SliceNode {
        internal SliceU(int[] buf)
            : base(buf)
        {
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int[] buf = buffer;
            int len = buf.Length;
            for (int j=0; j<len; j++) {
                if ((i+j) >= matcher.to) {
                    matcher._hitEnd = true;
                    return false;
                }
                int c = seq.charAt(i+j);
                if (buf[j] != c &&
                    buf[j] != Character.toLowerCase(Character.toUpperCase(c)))
                    return false;
            }
            return next.match(matcher, i+len, seq);
        }
    }
    sealed class SliceS : SliceNode {
        internal SliceS(int[] buf)
            : base(buf)
        {
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int[] buf = buffer;
            int x = i;
            for (int j = 0; j < buf.Length; j++) {
                if (x >= matcher.to) {
                    matcher._hitEnd = true;
                    return false;
                }
                int c = Character.codePointAt(seq, x);
                if (buf[j] != c)
                    return false;
                x += Character.charCount(c);
                if (x > matcher.to) {
                    matcher._hitEnd = true;
                    return false;
                }
            }
            return next.match(matcher, x, seq);
        }
    }
    class SliceIS : SliceNode {
        internal SliceIS(int[] buf)
            : base(buf)
        {
        }
        int toLower(int c) {
            return ASCII.toLower(c);
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int[] buf = buffer;
            int x = i;
            for (int j = 0; j < buf.Length; j++) {
                if (x >= matcher.to) {
                    matcher._hitEnd = true;
                    return false;
                }
                int c = Character.codePointAt(seq, x);
                if (buf[j] != c && buf[j] != toLower(c))
                    return false;
                x += Character.charCount(c);
                if (x > matcher.to) {
                    matcher._hitEnd = true;
                    return false;
                }
            }
            return next.match(matcher, x, seq);
        }
    }
    sealed class SliceUS : SliceIS {
        internal SliceUS(int[] buf)
            : base(buf)
        {
        }
        int toLower(int c) {
            return Character.toLowerCase(Character.toUpperCase(c));
        }
    }
    private static boolean inRange(int lower, int ch, int upper) {
        return lower <= ch && ch <= upper;
    }
    private static CharProperty rangeFor(int lower,
                                         int upper) {
        return new CharProperty((ch)=> { return inRange(lower, ch, upper); });
    }
    private CharProperty caseInsensitiveRangeFor(int lower,
                                                 int upper) {
        if (has(UNICODE_CASE))
            return new CharProperty((ch)=>
            {
                if (inRange(lower, ch, upper))
                        return true;
                    int up = Character.toUpperCase(ch);
                    return inRange(lower, up, upper) ||
                           inRange(lower, Character.toLowerCase(up), upper);
            });
        return new CharProperty((ch)=>
            {
                return inRange(lower, ch, upper) ||
                    ASCII.isAscii(ch) &&
                        (inRange(lower, ASCII.toUpper(ch), upper) ||
                         inRange(lower, ASCII.toLower(ch), upper));
            });
    }
    sealed class All : CharProperty {
        internal All()
            : base(null)
        {
        }
        internal override boolean isSatisfiedBy(int ch)
        {
            return true;
        }
    }
    sealed class Dot : CharProperty {
        internal Dot()
            : base(null)
        {
        }
        internal override boolean isSatisfiedBy(int ch)
        {
            return (ch != '\n' && ch != '\r'
                    && (ch|1) != '\u2029'
                    && ch != '\u0085');
        }
    }
    sealed class UnixDot : CharProperty {
        internal UnixDot()
            : base(null)
        {
        }
        internal override boolean isSatisfiedBy(int ch)
        {
            return ch != '\n';
        }
    }
    sealed class Ques : Node {
        Node atom;
        internal int type;
        internal Ques(Node node, int type) {
            this.atom = node;
            this.type = type;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            switch (type) {
            case GREEDY:
                return (atom.match(matcher, i, seq) && next.match(matcher, matcher.last, seq))
                    || next.match(matcher, i, seq);
            case LAZY:
                return next.match(matcher, i, seq)
                    || (atom.match(matcher, i, seq) && next.match(matcher, matcher.last, seq));
            case POSSESSIVE:
                if (atom.match(matcher, i, seq)) i = matcher.last;
                return next.match(matcher, i, seq);
            default:
                return atom.match(matcher, i, seq) && next.match(matcher, matcher.last, seq);
            }
        }
        internal override boolean study(TreeInfo info)
        {
            if (type != INDEPENDENT) {
                int minL = info.minLength;
                atom.study(info);
                info.minLength = minL;
                info.deterministic = false;
                return next.study(info);
            } else {
                atom.study(info);
                return next.study(info);
            }
        }
    }
    sealed class Curly : Node {
        Node atom;
        internal int type;
        internal int cmin;
        internal int cmax;
        internal Curly(Node node, int cmin, int cmax, int type) {
            this.atom = node;
            this.type = type;
            this.cmin = cmin;
            this.cmax = cmax;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int j;
            for (j = 0; j < cmin; j++) {
                if (atom.match(matcher, i, seq)) {
                    i = matcher.last;
                    continue;
                }
                return false;
            }
            if (type == GREEDY)
                return match0(matcher, i, j, seq);
            else if (type == LAZY)
                return match1(matcher, i, j, seq);
            else
                return match2(matcher, i, j, seq);
        }
        // Greedy match.
        // i is the index to start matching at
        // j is the number of atoms that have matched
        boolean match0(Matcher matcher, int i, int j, CharSequence seq) {
            if (j >= cmax) {
                // We have matched the maximum... continue with the rest of
                // the regular expression
                return next.match(matcher, i, seq);
            }
            int backLimit = j;
            while (atom.match(matcher, i, seq)) {
                // k is the length of this match
                int k = matcher.last - i;
                if (k == 0) // Zero length match
                    break;
                // Move up index and number matched
                i = matcher.last;
                j++;
                // We are greedy so match as many as we can
                while (j < cmax) {
                    if (!atom.match(matcher, i, seq))
                        break;
                    if (i + k != matcher.last) {
                        if (match0(matcher, matcher.last, j+1, seq))
                            return true;
                        break;
                    }
                    i += k;
                    j++;
                }
                // Handle backing off if match fails
                while (j >= backLimit) {
                   if (next.match(matcher, i, seq))
                        return true;
                    i -= k;
                    j--;
                }
                return false;
            }
            return next.match(matcher, i, seq);
        }
        // Reluctant match. At this point, the minimum has been satisfied.
        // i is the index to start matching at
        // j is the number of atoms that have matched
        boolean match1(Matcher matcher, int i, int j, CharSequence seq) {
            for (;;) {
                // Try finishing match without consuming any more
                if (next.match(matcher, i, seq))
                    return true;
                // At the maximum, no match found
                if (j >= cmax)
                    return false;
                // Okay, must try one more atom
                if (!atom.match(matcher, i, seq))
                    return false;
                // If we haven't moved forward then must break out
                if (i == matcher.last)
                    return false;
                // Move up index and number matched
                i = matcher.last;
                j++;
            }
        }
        boolean match2(Matcher matcher, int i, int j, CharSequence seq) {
            for (; j < cmax; j++) {
                if (!atom.match(matcher, i, seq))
                    break;
                if (i == matcher.last)
                    break;
                i = matcher.last;
            }
            return next.match(matcher, i, seq);
        }
        internal override boolean study(TreeInfo info)
        {
            // Save original info
            int minL = info.minLength;
            int maxL = info.maxLength;
            boolean maxV = info.maxValid;
            boolean detm = info.deterministic;
            info.reset();
            atom.study(info);
            int temp = info.minLength * cmin + minL;
            if (temp < minL) {
                temp = 0xFFFFFFF; // arbitrary large number
            }
            info.minLength = temp;
            if (maxV & info.maxValid) {
                temp = info.maxLength * cmax + maxL;
                info.maxLength = temp;
                if (temp < maxL) {
                    info.maxValid = false;
                }
            } else {
                info.maxValid = false;
            }
            if (info.deterministic && cmin == cmax)
                info.deterministic = detm;
            else
                info.deterministic = false;
            return next.study(info);
        }
    }
    sealed class GroupCurly : Node {
        Node atom;
        int type;
        int cmin;
        int cmax;
        int localIndex;
        int groupIndex;
        boolean capture;
        internal GroupCurly(Node node, int cmin, int cmax, int type, int local,
                   int group, boolean capture) {
            this.atom = node;
            this.type = type;
            this.cmin = cmin;
            this.cmax = cmax;
            this.localIndex = local;
            this.groupIndex = group;
            this.capture = capture;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int[] groups = matcher.groups;
            int[] locals = matcher.locals;
            int save0 = locals[localIndex];
            int save1 = 0;
            int save2 = 0;
            if (capture) {
                save1 = groups[groupIndex];
                save2 = groups[groupIndex+1];
            }
            // Notify GroupTail there is no need to setup group info
            // because it will be set here
            locals[localIndex] = -1;
            boolean ret = true;
            for (int j = 0; j < cmin; j++) {
                if (atom.match(matcher, i, seq)) {
                    if (capture) {
                        groups[groupIndex] = i;
                        groups[groupIndex+1] = matcher.last;
                    }
                    i = matcher.last;
                } else {
                    ret = false;
                    break;
                }
            }
            if (ret) {
                if (type == GREEDY) {
                    ret = match0(matcher, i, cmin, seq);
                } else if (type == LAZY) {
                    ret = match1(matcher, i, cmin, seq);
                } else {
                    ret = match2(matcher, i, cmin, seq);
                }
            }
            if (!ret) {
                locals[localIndex] = save0;
                if (capture) {
                    groups[groupIndex] = save1;
                    groups[groupIndex+1] = save2;
                }
            }
            return ret;
        }
        // Aggressive group match
        boolean match0(Matcher matcher, int i, int j, CharSequence seq) {
            int[] groups = matcher.groups;
            int save0 = 0;
            int save1 = 0;
            if (capture) {
                save0 = groups[groupIndex];
                save1 = groups[groupIndex+1];
            }
            for (;;) {
                if (j >= cmax)
                    break;
                if (!atom.match(matcher, i, seq))
                    break;
                int k = matcher.last - i;
                if (k <= 0) {
                    if (capture) {
                        groups[groupIndex] = i;
                        groups[groupIndex+1] = i + k;
                    }
                    i = i + k;
                    break;
                }
                for (;;) {
                    if (capture) {
                        groups[groupIndex] = i;
                        groups[groupIndex+1] = i + k;
                    }
                    i = i + k;
                    if (++j >= cmax)
                        break;
                    if (!atom.match(matcher, i, seq))
                        break;
                    if (i + k != matcher.last) {
                        if (match0(matcher, i, j, seq))
                            return true;
                        break;
                    }
                }
                while (j > cmin) {
                    if (next.match(matcher, i, seq)) {
                        if (capture) {
                            groups[groupIndex+1] = i;
                            groups[groupIndex] = i - k;
                        }
                        i = i - k;
                        return true;
                    }
                    // backing off
                    if (capture) {
                        groups[groupIndex+1] = i;
                        groups[groupIndex] = i - k;
                    }
                    i = i - k;
                    j--;
                }
                break;
            }
            if (capture) {
                groups[groupIndex] = save0;
                groups[groupIndex+1] = save1;
            }
            return next.match(matcher, i, seq);
        }
        // Reluctant matching
        boolean match1(Matcher matcher, int i, int j, CharSequence seq) {
            for (;;) {
                if (next.match(matcher, i, seq))
                    return true;
                if (j >= cmax)
                    return false;
                if (!atom.match(matcher, i, seq))
                    return false;
                if (i == matcher.last)
                    return false;
                if (capture) {
                    matcher.groups[groupIndex] = i;
                    matcher.groups[groupIndex+1] = matcher.last;
                }
                i = matcher.last;
                j++;
            }
        }
        // Possessive matching
        boolean match2(Matcher matcher, int i, int j, CharSequence seq) {
            for (; j < cmax; j++) {
                if (!atom.match(matcher, i, seq)) {
                    break;
                }
                if (capture) {
                    matcher.groups[groupIndex] = i;
                    matcher.groups[groupIndex+1] = matcher.last;
                }
                if (i == matcher.last) {
                    break;
                }
                i = matcher.last;
            }
            return next.match(matcher, i, seq);
        }
        internal override boolean study(TreeInfo info)
        {
            // Save original info
            int minL = info.minLength;
            int maxL = info.maxLength;
            boolean maxV = info.maxValid;
            boolean detm = info.deterministic;
            info.reset();
            atom.study(info);
            int temp = info.minLength * cmin + minL;
            if (temp < minL) {
                temp = 0xFFFFFFF; // Arbitrary large number
            }
            info.minLength = temp;
            if (maxV & info.maxValid) {
                temp = info.maxLength * cmax + maxL;
                info.maxLength = temp;
                if (temp < maxL) {
                    info.maxValid = false;
                }
            } else {
                info.maxValid = false;
            }
            if (info.deterministic && cmin == cmax) {
                info.deterministic = detm;
            } else {
                info.deterministic = false;
            }
            return next.study(info);
        }
    }
    sealed class BranchConn : Node {
        internal BranchConn() {}
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            return next.match(matcher, i, seq);
        }
        internal override boolean study(TreeInfo info)
        {
            return info.deterministic;
        }
    }
    sealed class Branch : Node {
        Node[] atoms = new Node[2];
        int size = 2;
        Node conn;
        internal Branch(Node first, Node second, Node branchConn) {
            conn = branchConn;
            atoms[0] = first;
            atoms[1] = second;
        }
        internal void add(Node node) {
            if (size >= atoms.Length) {
                Node[] tmp = new Node[atoms.Length*2];
                Array.Copy(atoms, 0, tmp, 0, atoms.Length);
                atoms = tmp;
            }
            atoms[size++] = node;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            for (int n = 0; n < size; n++) {
                if (atoms[n] == null) {
                    if (conn.next.match(matcher, i, seq))
                        return true;
                } else if (atoms[n].match(matcher, i, seq)) {
                    return true;
                }
            }
            return false;
        }
        internal override boolean study(TreeInfo info)
        {
            int minL = info.minLength;
            int maxL = info.maxLength;
            boolean maxV = info.maxValid;
            int minL2 = Integer.MAX_VALUE; //arbitrary large enough num
            int maxL2 = -1;
            for (int n = 0; n < size; n++) {
                info.reset();
                if (atoms[n] != null)
                    atoms[n].study(info);
                minL2 = Math.min(minL2, info.minLength);
                maxL2 = Math.max(maxL2, info.maxLength);
                maxV = (maxV & info.maxValid);
            }
            minL += minL2;
            maxL += maxL2;
            info.reset();
            conn.next.study(info);
            info.minLength += minL;
            info.maxLength += maxL;
            info.maxValid &= maxV;
            info.deterministic = false;
            return false;
        }
    }
    sealed class GroupHead : Node {
        internal int localIndex;
        internal GroupHead(int localCount) {
            localIndex = localCount;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int save = matcher.locals[localIndex];
            matcher.locals[localIndex] = i;
            boolean ret = next.match(matcher, i, seq);
            matcher.locals[localIndex] = save;
            return ret;
        }
        internal boolean matchRef(Matcher matcher, int i, CharSequence seq) {
            int save = matcher.locals[localIndex];
            matcher.locals[localIndex] = ~i; // HACK
            boolean ret = next.match(matcher, i, seq);
            matcher.locals[localIndex] = save;
            return ret;
        }
    }
    sealed class GroupRef : Node {
        GroupHead head;
        GroupRef(GroupHead head) {
            this.head = head;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            return head.matchRef(matcher, i, seq)
                && next.match(matcher, matcher.last, seq);
        }
        internal override boolean study(TreeInfo info)
        {
            info.maxValid = false;
            info.deterministic = false;
            return next.study(info);
        }
    }
    sealed class GroupTail : Node {
        internal int localIndex;
        internal int groupIndex;
        internal GroupTail(int localCount, int groupCount) {
            localIndex = localCount;
            groupIndex = groupCount + groupCount;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int tmp = matcher.locals[localIndex];
            if (tmp >= 0) { // This is the normal group case.
                // Save the group so we can unset it if it
                // backs off of a match.
                int groupStart = matcher.groups[groupIndex];
                int groupEnd = matcher.groups[groupIndex+1];
                matcher.groups[groupIndex] = tmp;
                matcher.groups[groupIndex+1] = i;
                if (next.match(matcher, i, seq)) {
                    return true;
                }
                matcher.groups[groupIndex] = groupStart;
                matcher.groups[groupIndex+1] = groupEnd;
                return false;
            } else {
                // This is a group reference case. We don't need to save any
                // group info because it isn't really a group.
                matcher.last = i;
                return true;
            }
        }
    }
    sealed class Prolog : Node {
        Loop loop;
        internal Prolog(Loop loop)
        {
            this.loop = loop;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            return loop.matchInit(matcher, i, seq);
        }
        internal override boolean study(TreeInfo info)
        {
            return loop.study(info);
        }
    }
    class Loop : Node {
        internal Node body;
        internal int countIndex; // local count index in matcher locals
        internal int beginIndex; // group beginning index
        internal int cmin, cmax;
        internal Loop(int countIndex, int beginIndex) {
            this.countIndex = countIndex;
            this.beginIndex = beginIndex;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            // Avoid infinite loop in zero-length case.
            if (i > matcher.locals[beginIndex]) {
                int count = matcher.locals[countIndex];
                // This block is for before we reach the minimum
                // iterations required for the loop to match
                if (count < cmin) {
                    matcher.locals[countIndex] = count + 1;
                    boolean b = body.match(matcher, i, seq);
                    // If match failed we must backtrack, so
                    // the loop count should NOT be incremented
                    if (!b)
                        matcher.locals[countIndex] = count;
                    // Return success or failure since we are under
                    // minimum
                    return b;
                }
                // This block is for after we have the minimum
                // iterations required for the loop to match
                if (count < cmax) {
                    matcher.locals[countIndex] = count + 1;
                    boolean b = body.match(matcher, i, seq);
                    // If match failed we must backtrack, so
                    // the loop count should NOT be incremented
                    if (!b)
                        matcher.locals[countIndex] = count;
                    else
                        return true;
                }
            }
            return next.match(matcher, i, seq);
        }
        internal virtual boolean matchInit(Matcher matcher, int i, CharSequence seq) {
            int save = matcher.locals[countIndex];
            boolean ret = false;
            if (0 < cmin) {
                matcher.locals[countIndex] = 1;
                ret = body.match(matcher, i, seq);
            } else if (0 < cmax) {
                matcher.locals[countIndex] = 1;
                ret = body.match(matcher, i, seq);
                if (ret == false)
                    ret = next.match(matcher, i, seq);
            } else {
                ret = next.match(matcher, i, seq);
            }
            matcher.locals[countIndex] = save;
            return ret;
        }
        internal override boolean study(TreeInfo info)
        {
            info.maxValid = false;
            info.deterministic = false;
            return false;
        }
    }
    sealed class LazyLoop : Loop {
        internal LazyLoop(int countIndex, int beginIndex)
            : base(countIndex, beginIndex)
        {
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            // Check for zero length group
            if (i > matcher.locals[beginIndex]) {
                int count = matcher.locals[countIndex];
                if (count < cmin) {
                    matcher.locals[countIndex] = count + 1;
                    boolean result = body.match(matcher, i, seq);
                    // If match failed we must backtrack, so
                    // the loop count should NOT be incremented
                    if (!result)
                        matcher.locals[countIndex] = count;
                    return result;
                }
                if (next.match(matcher, i, seq))
                    return true;
                if (count < cmax) {
                    matcher.locals[countIndex] = count + 1;
                    boolean result = body.match(matcher, i, seq);
                    // If match failed we must backtrack, so
                    // the loop count should NOT be incremented
                    if (!result)
                        matcher.locals[countIndex] = count;
                    return result;
                }
                return false;
            }
            return next.match(matcher, i, seq);
        }
        internal override boolean matchInit(Matcher matcher, int i, CharSequence seq)
        {
            int save = matcher.locals[countIndex];
            boolean ret = false;
            if (0 < cmin) {
                matcher.locals[countIndex] = 1;
                ret = body.match(matcher, i, seq);
            } else if (next.match(matcher, i, seq)) {
                ret = true;
            } else if (0 < cmax) {
                matcher.locals[countIndex] = 1;
                ret = body.match(matcher, i, seq);
            }
            matcher.locals[countIndex] = save;
            return ret;
        }
        internal override boolean study(TreeInfo info)
        {
            info.maxValid = false;
            info.deterministic = false;
            return false;
        }
    }
    class BackRef : Node {
        int groupIndex;
        internal BackRef(int groupCount)
            : base()
        {
            groupIndex = groupCount + groupCount;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int j = matcher.groups[groupIndex];
            int k = matcher.groups[groupIndex+1];
            int groupSize = k - j;
            // If the referenced group didn't match, neither can this
            if (j < 0)
                return false;
            // If there isn't enough input left no match
            if (i + groupSize > matcher.to) {
                matcher._hitEnd = true;
                return false;
            }
            // Check each new char to make sure it matches what the group
            // referenced matched last time around
            for (int index=0; index<groupSize; index++)
                if (seq.charAt(i+index) != seq.charAt(j+index))
                    return false;
            return next.match(matcher, i+groupSize, seq);
        }
        internal override boolean study(TreeInfo info)
        {
            info.maxValid = false;
            return next.study(info);
        }
    }
    class CIBackRef : Node {
        int groupIndex;
        boolean doUnicodeCase;
        internal CIBackRef(int groupCount, boolean doUnicodeCase)
            : base()
        {
            groupIndex = groupCount + groupCount;
            this.doUnicodeCase = doUnicodeCase;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int j = matcher.groups[groupIndex];
            int k = matcher.groups[groupIndex+1];
            int groupSize = k - j;
            // If the referenced group didn't match, neither can this
            if (j < 0)
                return false;
            // If there isn't enough input left no match
            if (i + groupSize > matcher.to) {
                matcher._hitEnd = true;
                return false;
            }
            // Check each new char to make sure it matches what the group
            // referenced matched last time around
            int x = i;
            for (int index=0; index<groupSize; index++) {
                int c1 = Character.codePointAt(seq, x);
                int c2 = Character.codePointAt(seq, j);
                if (c1 != c2) {
                    if (doUnicodeCase) {
                        int cc1 = Character.toUpperCase(c1);
                        int cc2 = Character.toUpperCase(c2);
                        if (cc1 != cc2 &&
                            Character.toLowerCase(cc1) !=
                            Character.toLowerCase(cc2))
                            return false;
                    } else {
                        if (ASCII.toLower(c1) != ASCII.toLower(c2))
                            return false;
                    }
                }
                x += Character.charCount(c1);
                j += Character.charCount(c2);
            }
            return next.match(matcher, i+groupSize, seq);
        }
        internal override boolean study(TreeInfo info)
        {
            info.maxValid = false;
            return next.study(info);
        }
    }
    sealed class First : Node {
        Node atom;
        First(Node node) {
            this.atom = BnM.optimize(node);
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            if (atom is BnM) {
                return atom.match(matcher, i, seq)
                    && next.match(matcher, matcher.last, seq);
            }
            for (;;) {
                if (i > matcher.to) {
                    matcher._hitEnd = true;
                    return false;
                }
                if (atom.match(matcher, i, seq)) {
                    return next.match(matcher, matcher.last, seq);
                }
                i += countChars(seq, i, 1);
                matcher.first++;
            }
        }
        internal override boolean study(TreeInfo info)
        {
            atom.study(info);
            info.maxValid = false;
            info.deterministic = false;
            return next.study(info);
        }
    }

    sealed class Conditional : Node {
        Node cond, yes, not;
        Conditional(Node cond, Node yes, Node not) {
            this.cond = cond;
            this.yes = yes;
            this.not = not;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            if (cond.match(matcher, i, seq)) {
                return yes.match(matcher, i, seq);
            } else {
                return not.match(matcher, i, seq);
            }
        }
        internal override boolean study(TreeInfo info)
        {
            int minL = info.minLength;
            int maxL = info.maxLength;
            boolean maxV = info.maxValid;
            info.reset();
            yes.study(info);
            int minL2 = info.minLength;
            int maxL2 = info.maxLength;
            boolean maxV2 = info.maxValid;
            info.reset();
            not.study(info);
            info.minLength = minL + Math.min(minL2, info.minLength);
            info.maxLength = maxL + Math.max(maxL2, info.maxLength);
            info.maxValid = (maxV & maxV2 & info.maxValid);
            info.deterministic = false;
            return next.study(info);
        }
    }

    sealed class Pos : Node {
        Node cond;
        internal Pos(Node cond) {
            this.cond = cond;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int savedTo = matcher.to;
            boolean conditionMatched = false;
            // Relax transparent region boundaries for lookahead
            if (matcher.transparentBounds)
                matcher.to = matcher.getTextLength();
            try {
                conditionMatched = cond.match(matcher, i, seq);
            } finally {
                // Reinstate region boundaries
                matcher.to = savedTo;
            }
            return conditionMatched && next.match(matcher, i, seq);
        }
    }

    sealed class Neg : Node {
        Node cond;
        internal Neg(Node cond) {
            this.cond = cond;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int savedTo = matcher.to;
            boolean conditionMatched = false;
            // Relax transparent region boundaries for lookahead
            if (matcher.transparentBounds)
                matcher.to = matcher.getTextLength();
            try {
                if (i < matcher.to) {
                    conditionMatched = !cond.match(matcher, i, seq);
                } else {
                    // If a negative lookahead succeeds then more input
                    // could cause it to fail!
                    matcher._requireEnd = true;
                    conditionMatched = !cond.match(matcher, i, seq);
                }
            } finally {
                // Reinstate region boundaries
                matcher.to = savedTo;
            }
            return conditionMatched && next.match(matcher, i, seq);
        }
    }

    static Node lookbehindEnd = new Node((matcher, i, seq)=>
            {
                return i == matcher.lookbehindTo;
            });

    class Behind : Node {
        internal Node cond;
        internal int rmax, rmin;

        internal Behind(Node cond, int rmax, int rmin) {
            this.cond = cond;
            this.rmax = rmax;
            this.rmin = rmin;
        }

        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int savedFrom = matcher.from;
            boolean conditionMatched = false;
            int startIndex = (!matcher.transparentBounds) ?
                             matcher.from : 0;
            int from = Math.max(i - rmax, startIndex);
            // Set end boundary
            int savedLBT = matcher.lookbehindTo;
            matcher.lookbehindTo = i;
            // Relax transparent region boundaries for lookbehind
            if (matcher.transparentBounds)
                matcher.from = 0;
            for (int j = i - rmin; !conditionMatched && j >= from; j--) {
                conditionMatched = cond.match(matcher, j, seq);
            }
            matcher.from = savedFrom;
            matcher.lookbehindTo = savedLBT;
            return conditionMatched && next.match(matcher, i, seq);
        }
    }

    sealed class BehindS : Behind {
        internal BehindS(Node cond, int rmax, int rmin)
            : base(cond, rmax, rmin)
        {
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int rmaxChars = countChars(seq, i, -rmax);
            int rminChars = countChars(seq, i, -rmin);
            int savedFrom = matcher.from;
            int startIndex = (!matcher.transparentBounds) ?
                             matcher.from : 0;
            boolean conditionMatched = false;
            int from = Math.max(i - rmaxChars, startIndex);
            // Set end boundary
            int savedLBT = matcher.lookbehindTo;
            matcher.lookbehindTo = i;
            // Relax transparent region boundaries for lookbehind
            if (matcher.transparentBounds)
                matcher.from = 0;
            for (int j = i - rminChars;
                 !conditionMatched && j >= from;
                 j -= j>from ? countChars(seq, j, -1) : 1) {
                conditionMatched = cond.match(matcher, j, seq);
            }
            matcher.from = savedFrom;
            matcher.lookbehindTo = savedLBT;
            return conditionMatched && next.match(matcher, i, seq);
        }
    }

    class NotBehind : Node {
        internal Node cond;
        internal int rmax, rmin;
        internal NotBehind(Node cond, int rmax, int rmin) {
            this.cond = cond;
            this.rmax = rmax;
            this.rmin = rmin;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int savedLBT = matcher.lookbehindTo;
            int savedFrom = matcher.from;
            boolean conditionMatched = false;
            int startIndex = (!matcher.transparentBounds) ?
                             matcher.from : 0;
            int from = Math.max(i - rmax, startIndex);
            matcher.lookbehindTo = i;
            // Relax transparent region boundaries for lookbehind
            if (matcher.transparentBounds)
                matcher.from = 0;
            for (int j = i - rmin; !conditionMatched && j >= from; j--) {
                conditionMatched = cond.match(matcher, j, seq);
            }
            // Reinstate region boundaries
            matcher.from = savedFrom;
            matcher.lookbehindTo = savedLBT;
            return !conditionMatched && next.match(matcher, i, seq);
        }
    }

    sealed class NotBehindS : NotBehind {
        internal NotBehindS(Node cond, int rmax, int rmin)
            : base(cond, rmax, rmin)
        {
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int rmaxChars = countChars(seq, i, -rmax);
            int rminChars = countChars(seq, i, -rmin);
            int savedFrom = matcher.from;
            int savedLBT = matcher.lookbehindTo;
            boolean conditionMatched = false;
            int startIndex = (!matcher.transparentBounds) ?
                             matcher.from : 0;
            int from = Math.max(i - rmaxChars, startIndex);
            matcher.lookbehindTo = i;
            // Relax transparent region boundaries for lookbehind
            if (matcher.transparentBounds)
                matcher.from = 0;
            for (int j = i - rminChars;
                 !conditionMatched && j >= from;
                 j -= j>from ? countChars(seq, j, -1) : 1) {
                conditionMatched = cond.match(matcher, j, seq);
            }
            //Reinstate region boundaries
            matcher.from = savedFrom;
            matcher.lookbehindTo = savedLBT;
            return !conditionMatched && next.match(matcher, i, seq);
        }
    }

    private static CharProperty union(CharProperty lhs,
                                      CharProperty rhs) {
        return new CharProperty((int ch)=>
            {
                return lhs.isSatisfiedBy(ch) || rhs.isSatisfiedBy(ch);
            });
    }
    private static CharProperty intersection(CharProperty lhs,
                                             CharProperty rhs) {
        return new CharProperty((int ch)=>
            {
                return lhs.isSatisfiedBy(ch) && rhs.isSatisfiedBy(ch);
            });
    }
    private static CharProperty setDifference(CharProperty lhs,
                                              CharProperty rhs) {
        return new CharProperty((int ch)=>
            {
                return ! rhs.isSatisfiedBy(ch) && lhs.isSatisfiedBy(ch);
            });
    }
    sealed class Bound : Node {
        internal static int LEFT = 0x1;
        internal static int RIGHT = 0x2;
        internal static int BOTH = 0x3;
        internal static int NONE = 0x4;
        int type;
        boolean useUWORD;
        internal Bound(int n, boolean useUWORD) {
            type = n;
            this.useUWORD = useUWORD;
        }
        boolean isWord(int ch) {
            return useUWORD ? UnicodeProp.WORD_is(ch)
                            : (boolean)(ch == '_' || Character.isLetterOrDigit(ch));
        }
        int check(Matcher matcher, int i, CharSequence seq) {
            int ch;
            boolean left = false;
            int startIndex = matcher.from;
            int endIndex = matcher.to;
            if (matcher.transparentBounds) {
                startIndex = 0;
                endIndex = matcher.getTextLength();
            }
            if (i > startIndex) {
                ch = Character.codePointBefore(seq, i);
                left = (isWord(ch) ||
                    ((Character.getType(ch) == Character.NON_SPACING_MARK)
                     && hasBaseCharacter(matcher, i-1, seq)));
            }
            boolean right = false;
            if (i < endIndex) {
                ch = Character.codePointAt(seq, i);
                right = (isWord(ch) ||
                    ((Character.getType(ch) == Character.NON_SPACING_MARK)
                     && hasBaseCharacter(matcher, i, seq)));
            } else {
                // Tried to access char past the end
                matcher._hitEnd = true;
                // The addition of another char could wreck a boundary
                matcher._requireEnd = true;
            }
            return ((left ^ right) ? (right ? LEFT : RIGHT) : NONE);
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            return (check(matcher, i, seq) & type) > 0
                && next.match(matcher, i, seq);
        }
    }
    private static boolean hasBaseCharacter(Matcher matcher, int i,
                                            CharSequence seq)
    {
        int start = (!matcher.transparentBounds) ?
            matcher.from : 0;
        for (int x=i; x >= start; x--) {
            int ch = Character.codePointAt(seq, x);
            if (Character.isLetterOrDigit(ch))
                return true;
            if (Character.getType(ch) == Character.NON_SPACING_MARK)
                continue;
            return false;
        }
        return false;
    }
    internal class BnM : Node {
        internal int[] buffer;
        internal int[] lastOcc;
        internal int[] optoSft;
        internal static Node optimize(Node node)
        {
            if (!(node is Slice)) {
                return node;
            }
            int[] src = ((Slice) node).buffer;
            int patternLength = src.Length;
            // The BM algorithm requires a bit of overhead;
            // If the pattern is short don't use it, since
            // a shift larger than the pattern length cannot
            // be used anyway.
            if (patternLength < 4) {
                return node;
            }
            int i, j/*, k*/;
            int[] lastOcc = new int[128];
            int[] optoSft = new int[patternLength];
            // Precalculate part of the bad character shift
            // It is a table for where in the pattern each
            // lower 7-bit value occurs
            for (i = 0; i < patternLength; i++) {
                lastOcc[src[i]&0x7F] = i + 1;
            }
            // Precalculate the good suffix shift
            // i is the shift amount being considered
            for (i = patternLength; i > 0; i--) {
                // j is the beginning index of suffix being considered
                for (j = patternLength - 1; j >= i; j--) {
                    // Testing for good suffix
                    if (src[j] == src[j-i]) {
                        // src[j..len] is a good suffix
                        optoSft[j-1] = i;
                    } else {
                        // No match. The array has already been
                        // filled up with correct values before.
                        goto continue3;
                    }
                }
                // This fills up the remaining of optoSft
                // any suffix can not have larger shift amount
                // then its sub-suffix. Why???
                while (j > 0) {
                    optoSft[--j] = i;
                }
            continue3:
                ;
            }
            // Set the guard value because of unicode compression
            optoSft[patternLength-1] = 1;
            if (node is SliceS)
                return new BnMS(src, lastOcc, optoSft, node.next);
            return new BnM(src, lastOcc, optoSft, node.next);
        }
        internal BnM(int[] src, int[] lastOcc, int[] optoSft, Node next)
        {
            this.buffer = src;
            this.lastOcc = lastOcc;
            this.optoSft = optoSft;
            this.next = next;
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int[] src = buffer;
            int patternLength = src.Length;
            int last = matcher.to - patternLength;
            // Loop over all possible match positions in text
            while (i <= last) {
                // Loop over pattern from right to left
                for (int j = patternLength - 1; j >= 0; j--) {
                    int ch = seq.charAt(i+j);
                    if (ch != src[j]) {
                        // Shift search to the right by the maximum of the
                        // bad character shift and the good suffix shift
                        i += Math.max(j + 1 - lastOcc[ch&0x7F], optoSft[j]);
                        goto continue4;
                    }
                }
                // Entire pattern matched starting at i
                matcher.first = i;
                boolean ret = next.match(matcher, i + patternLength, seq);
                if (ret) {
                    matcher.first = i;
                    matcher.groups[0] = matcher.first;
                    matcher.groups[1] = matcher.last;
                    return true;
                }
                i++;
            continue4:
                ;
            }
            // BnM is only used as the leading node in the unanchored case,
            // and it replaced its Start() which always searches to the end
            // if it doesn't find what it's looking for, so hitEnd is true.
            matcher._hitEnd = true;
            return false;
        }
        internal override boolean study(TreeInfo info)
        {
            info.minLength += buffer.Length;
            info.maxValid = false;
            return next.study(info);
        }
    }
    sealed class BnMS : BnM {
        internal int lengthInChars;
        internal BnMS(int[] src, int[] lastOcc, int[] optoSft, Node next)
            : base(src, lastOcc, optoSft, next)
        {
            for (int x = 0; x < buffer.Length; x++) {
                lengthInChars += Character.charCount(buffer[x]);
            }
        }
        internal override boolean match(Matcher matcher, int i, CharSequence seq)
        {
            int[] src = buffer;
            int patternLength = src.Length;
            int last = matcher.to - lengthInChars;
            // Loop over all possible match positions in text
            while (i <= last) {
                // Loop over pattern from right to left
                int ch;
                for (int j = countChars(seq, i, patternLength), x = patternLength - 1;
                     j > 0; j -= Character.charCount(ch), x--) {
                    ch = Character.codePointBefore(seq, i+j);
                    if (ch != src[x]) {
                        // Shift search to the right by the maximum of the
                        // bad character shift and the good suffix shift
                        int n = Math.max(x + 1 - lastOcc[ch&0x7F], optoSft[x]);
                        i += countChars(seq, i, n);
                        goto continue5;
                    }
                }
                // Entire pattern matched starting at i
                matcher.first = i;
                boolean ret = next.match(matcher, i + lengthInChars, seq);
                if (ret) {
                    matcher.first = i;
                    matcher.groups[0] = matcher.first;
                    matcher.groups[1] = matcher.last;
                    return true;
                }
                i += countChars(seq, i, 1);
            continue5:
                ;
            }
            matcher._hitEnd = true;
            return false;
        }
    }
///////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////
    static Node _accept = new Node();
    static Node lastAccept = new LastNode();
    private class CharPropertyNames {

        internal static CharProperty charPropertyFor(String name) {
            CharPropertyFactory m = map.get(name);
            return m == null ? null : m.make();
        }

        private class CharPropertyFactory {
            private Func<CharProperty> make_func;

            internal CharPropertyFactory(Func<CharProperty> make_func)
            {
                this.make_func = make_func;
            }

            internal CharProperty make() {
                return make_func();
            }
        }

        private static void defCategory(String name,
                                        int typeMask) {
            map.put(name, new CharPropertyFactory(()=>
            {
                return new Category(typeMask);
            }));
        }

        private static void defRange(String name,
                                     int lower, int upper) {
            map.put(name, new CharPropertyFactory(()=>
            {
                return rangeFor(lower, upper);
            }));
        }
        private static void defCtype(String name,
                                     int ctype) {
            map.put(name, new CharPropertyFactory(()=>
            {
                return new Ctype(ctype);
            }));
        }
        private class CloneableProperty
            : CharProperty/*, Cloneable*/
        {
            internal CloneableProperty(Func<int, boolean> funk)
                : base(funk)
            {
            }

            public CloneableProperty clone() {
                throw new NotImplementedException();
                /*
                try {
                    return (CloneableProperty) super.clone();
                } catch (CloneNotSupportedException e) {
                    throw new AssertionError(e);
                }
                 */
            }
        }
        private static void defClone(String name,
                                     CloneableProperty p) {
            map.put(name, new CharPropertyFactory(()=>
            {
                return p.clone();
            }));
        }
        private static readonly HashMap<String, CharPropertyFactory> map
            = new HashMap<String, CharPropertyFactory>();
        static CharPropertyNames() {
            // Unicode character property aliases, defined in
            // http://www.unicode.org/Public/UNIDATA/PropertyValueAliases.txt
            defCategory("Cn", 1<<Character.UNASSIGNED);
            defCategory("Lu", 1<<Character.UPPERCASE_LETTER);
            defCategory("Ll", 1<<Character.LOWERCASE_LETTER);
            defCategory("Lt", 1<<Character.TITLECASE_LETTER);
            defCategory("Lm", 1<<Character.MODIFIER_LETTER);
            defCategory("Lo", 1<<Character.OTHER_LETTER);
            defCategory("Mn", 1<<Character.NON_SPACING_MARK);
            defCategory("Me", 1<<Character.ENCLOSING_MARK);
            defCategory("Mc", 1<<Character.COMBINING_SPACING_MARK);
            defCategory("Nd", 1<<Character.DECIMAL_DIGIT_NUMBER);
            defCategory("Nl", 1<<Character.LETTER_NUMBER);
            defCategory("No", 1<<Character.OTHER_NUMBER);
            defCategory("Zs", 1<<Character.SPACE_SEPARATOR);
            defCategory("Zl", 1<<Character.LINE_SEPARATOR);
            defCategory("Zp", 1<<Character.PARAGRAPH_SEPARATOR);
            defCategory("Cc", 1<<Character.CONTROL);
            defCategory("Cf", 1<<Character.FORMAT);
            defCategory("Co", 1<<Character.PRIVATE_USE);
            defCategory("Cs", 1<<Character.SURROGATE);
            defCategory("Pd", 1<<Character.DASH_PUNCTUATION);
            defCategory("Ps", 1<<Character.START_PUNCTUATION);
            defCategory("Pe", 1<<Character.END_PUNCTUATION);
            defCategory("Pc", 1<<Character.CONNECTOR_PUNCTUATION);
            defCategory("Po", 1<<Character.OTHER_PUNCTUATION);
            defCategory("Sm", 1<<Character.MATH_SYMBOL);
            defCategory("Sc", 1<<Character.CURRENCY_SYMBOL);
            defCategory("Sk", 1<<Character.MODIFIER_SYMBOL);
            defCategory("So", 1<<Character.OTHER_SYMBOL);
            defCategory("Pi", 1<<Character.INITIAL_QUOTE_PUNCTUATION);
            defCategory("Pf", 1<<Character.FINAL_QUOTE_PUNCTUATION);
            defCategory("L", ((1<<Character.UPPERCASE_LETTER) |
                              (1<<Character.LOWERCASE_LETTER) |
                              (1<<Character.TITLECASE_LETTER) |
                              (1<<Character.MODIFIER_LETTER)  |
                              (1<<Character.OTHER_LETTER)));
            defCategory("M", ((1<<Character.NON_SPACING_MARK) |
                              (1<<Character.ENCLOSING_MARK)   |
                              (1<<Character.COMBINING_SPACING_MARK)));
            defCategory("N", ((1<<Character.DECIMAL_DIGIT_NUMBER) |
                              (1<<Character.LETTER_NUMBER)        |
                              (1<<Character.OTHER_NUMBER)));
            defCategory("Z", ((1<<Character.SPACE_SEPARATOR) |
                              (1<<Character.LINE_SEPARATOR)  |
                              (1<<Character.PARAGRAPH_SEPARATOR)));
            defCategory("C", ((1<<Character.CONTROL)     |
                              (1<<Character.FORMAT)      |
                              (1<<Character.PRIVATE_USE) |
                              (1<<Character.SURROGATE))); // Other
            defCategory("P", ((1<<Character.DASH_PUNCTUATION)      |
                              (1<<Character.START_PUNCTUATION)     |
                              (1<<Character.END_PUNCTUATION)       |
                              (1<<Character.CONNECTOR_PUNCTUATION) |
                              (1<<Character.OTHER_PUNCTUATION)     |
                              (1<<Character.INITIAL_QUOTE_PUNCTUATION) |
                              (1<<Character.FINAL_QUOTE_PUNCTUATION)));
            defCategory("S", ((1<<Character.MATH_SYMBOL)     |
                              (1<<Character.CURRENCY_SYMBOL) |
                              (1<<Character.MODIFIER_SYMBOL) |
                              (1<<Character.OTHER_SYMBOL)));
            defCategory("LC", ((1<<Character.UPPERCASE_LETTER) |
                               (1<<Character.LOWERCASE_LETTER) |
                               (1<<Character.TITLECASE_LETTER)));
            defCategory("LD", ((1<<Character.UPPERCASE_LETTER) |
                               (1<<Character.LOWERCASE_LETTER) |
                               (1<<Character.TITLECASE_LETTER) |
                               (1<<Character.MODIFIER_LETTER)  |
                               (1<<Character.OTHER_LETTER)     |
                               (1<<Character.DECIMAL_DIGIT_NUMBER)));
            defRange("L1", 0x00, 0xFF); // Latin-1
            map.put("all", new CharPropertyFactory(()=>
            {
                return new All();
            }));
            // Posix regular expression character classes, defined in
            // http://www.unix.org/onlinepubs/009695399/basedefs/xbd_chap09.html
            defRange("ASCII", 0x00, 0x7F);   // ASCII
            defCtype("Alnum", ASCII.ALNUM);  // Alphanumeric characters
            defCtype("Alpha", ASCII.ALPHA);  // Alphabetic characters
            defCtype("Blank", ASCII.BLANK);  // Space and tab characters
            defCtype("Cntrl", ASCII.CNTRL);  // Control characters
            defRange("Digit", '0', '9');     // Numeric characters
            defCtype("Graph", ASCII.GRAPH);  // printable and visible
            defRange("Lower", 'a', 'z');     // Lower-case alphabetic
            defRange("Print", 0x20, 0x7E);   // Printable characters
            defCtype("Punct", ASCII.PUNCT);  // Punctuation characters
            defCtype("Space", ASCII.SPACE);  // Space characters
            defRange("Upper", 'A', 'Z');     // Upper-case alphabetic
            defCtype("XDigit",ASCII.XDIGIT); // hexadecimal digits
            // Java character properties, defined by methods in Character.java
            defClone("javaLowerCase", new CloneableProperty((int ch)=>
            {
                return Character.isLowerCase(ch);
            }));
            defClone("javaUpperCase", new CloneableProperty((int ch)=>
            {
                return Character.isUpperCase(ch);
            }));
            defClone("javaAlphabetic", new CloneableProperty((int ch)=>
            {
                return Character.isAlphabetic(ch);
            }));
            defClone("javaIdeographic", new CloneableProperty((int ch)=>
            {
                return Character.isIdeographic(ch);
            }));
            defClone("javaTitleCase", new CloneableProperty((int ch)=>
            {
                return Character.isTitleCase(ch);
            }));
            defClone("javaDigit", new CloneableProperty((int ch)=>
            {
                return Character.isDigit(ch);
            }));
            defClone("javaDefined", new CloneableProperty((int ch)=>
            {
                return Character.isDefined(ch);
            }));
            defClone("javaLetter", new CloneableProperty((int ch)=>
            {
                return Character.isLetter(ch);
            }));
            defClone("javaLetterOrDigit", new CloneableProperty((int ch)=>
            {
                return Character.isLetterOrDigit(ch);
            }));
            defClone("javaJavaIdentifierStart", new CloneableProperty((int ch)=>
            {
                return Character.isJavaIdentifierStart(ch);
            }));
            defClone("javaJavaIdentifierPart", new CloneableProperty((int ch)=>
            {
                return Character.isJavaIdentifierPart(ch);
            }));
            defClone("javaUnicodeIdentifierStart", new CloneableProperty((int ch)=>
            {
                return Character.isUnicodeIdentifierStart(ch);
            }));
            defClone("javaUnicodeIdentifierPart", new CloneableProperty((int ch)=>
            {
                return Character.isUnicodeIdentifierPart(ch);
            }));
            defClone("javaIdentifierIgnorable", new CloneableProperty((int ch)=>
            {
                return Character.isIdentifierIgnorable(ch);
            }));
            defClone("javaSpaceChar", new CloneableProperty((int ch)=>
            {
                return Character.isSpaceChar(ch);
            }));
            defClone("javaWhitespace", new CloneableProperty((int ch)=>
            {
                return Character.isWhitespace(ch);
            }));
            defClone("javaISOControl", new CloneableProperty((int ch)=>
            {
                return Character.isISOControl(ch);
            }));
            defClone("javaMirrored", new CloneableProperty((int ch)=>
            {
                return Character.isMirrored(ch);
            }));
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////

    public override string ToString()
    {
        return toString();
    }
}
}