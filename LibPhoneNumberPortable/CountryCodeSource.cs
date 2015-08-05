using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libphonenumber
{
    public enum CountryCodeSource
    {
        FROM_NUMBER_WITH_PLUS_SIGN,
        FROM_NUMBER_WITH_IDD,
        FROM_NUMBER_WITHOUT_PLUS_SIGN,
        FROM_DEFAULT_COUNTRY
    }
}
