using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mah_boi.Tools.StringTable.Exceptions;

public class StringTableNonAsciiNameException : Exception
{
    public string Message = "Provided string data has non-ascii name.";

    public StringTableNonAsciiNameException() : base()
    {
    }
}
