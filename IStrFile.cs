using System;
using System.Collections.Generic;
using System.Linq;
namespace mah_boi.Tools
{
    interface IStrFile : IStringTable
    {
        CsfFile ToCsf();
        CsfFile ToCsf(string fileName);
        CsfFile ToCsf(StrFile fileSample);
    }
}
