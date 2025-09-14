using System;
using System.Collections.Generic;
using System.IO;

namespace mah_boi.Tools.Extensions;

public static class BinaryReaderExtension
{
    /// <summary>
    /// Reads char array of uint32 length.
    /// </summary>
    public static char[] ReadChars(this BinaryReader br, UInt32 length)
    {
        char[] ret = new char[length];

        for (UInt32 i = 0; i < length; i++)
            ret[i] = br.ReadChar();

        return ret;
    }
}
