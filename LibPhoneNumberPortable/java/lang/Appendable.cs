using System;
namespace java.lang
{
public interface Appendable {
    Appendable append(CharSequence csq);
    Appendable append(CharSequence csq, int start, int end);
    Appendable append(char c);
}
}