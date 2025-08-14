using System;

namespace mah_boi.Tools.StringTableFormats;

struct CsfFileHeader
{
    public char[] CSFchars;
    public UInt32 FormatVersion;
    public UInt32 NumberOfLabels;
    public UInt32 NumberOfStrings;
    public UInt32 UnknownBytes;
    public CsfLanguageCode LanguageCode;
}
