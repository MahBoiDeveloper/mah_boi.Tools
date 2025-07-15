using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools.StringTable
{
    class CsfFileHeader
    {
        public char[] CSFchars;
        public uint CSFformatVersion;
        public uint CSFnumberOfLabels;
        public uint CSFnumberOfStrings;
        public uint CSFunknownBytes;
        public uint CSFlanguageCode;

        public CsfFileHeader()
        {
        }
    }
}
