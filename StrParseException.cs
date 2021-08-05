using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mah_boi.Tools
{
    class StrParseException : Exception
    {
        public StrParseException(string message) : base(message)
        {
        }
    }
}
