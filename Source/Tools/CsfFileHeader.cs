using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    class CsfFileHeader
    {
        public char[] CSFchars;
        public UInt32 CSFformatVersion;
        public UInt32 CSFnumberOfLabels;
        public UInt32 CSFnumberOfStrings;
        public UInt32 CSFunknownBytes;
        public UInt32 CSFlanguageCode;

        public CsfFileHeader()
        {
        }
    }
}
