using System;
using String = java.lang.String;
using StringBuilder = java.lang.StringBuilder;
using Math = java.lang.Math;
using IOException = java.io.IOException;

namespace java.util.regex
{
    public interface MatchResult
    {
        int start();
        int start(int group);
        int end();
        int end(int group);
        String group();
        String group(int group);
        int groupCount();
    }
}