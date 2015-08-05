using System;
namespace java.util.regex
{
    public class PatternSyntaxException : Exception
    {
        private String s;
        private String normalizedPattern;
        private int p;

        public PatternSyntaxException(String s, String normalizedPattern, int p)
        {
            // TODO: Complete member initialization
            this.s = s;
            this.normalizedPattern = normalizedPattern;
            this.p = p;
        }
    }
}