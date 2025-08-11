using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mah_boi.Tools.Extensions;

public static class BinaryReaderExtension
{
    public static char[] ReadChars(this BinaryReader br, UInt32 length)
    {
        List<char> ret = new();

        for (UInt32 i = 0; i < length; i++)
            ret.Add(br.ReadChar());

        return ret.ToArray();
    }
}
