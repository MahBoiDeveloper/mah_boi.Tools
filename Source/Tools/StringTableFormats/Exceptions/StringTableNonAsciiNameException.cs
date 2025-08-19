using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mah_boi.Tools.StringTableFormats.Exceptions;

public class StringTableNonAsciiNameException : Exception
{
    public new string Message = "Provided string data has non-ascii name.";

    public StringTableNonAsciiNameException() : base()
    {
    }
}
