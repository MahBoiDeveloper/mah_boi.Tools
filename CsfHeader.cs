using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    class CsfHeader
    {
        public const UInt32 CNC_CSF_VERSION = 3;
        public readonly char[] CSF          = " FSC".ToCharArray();
        public readonly char[] LBL          = " LBL".ToCharArray();
        public readonly char[] STR          = " RTS".ToCharArray();
        public readonly char[] STRW         = "WRTS".ToCharArray();

        public enum LanguagesCodes : UInt32
        {
            US           = 0,
            UK           = 1,
            German       = 2,
            French       = 3,
            Spanish      = 4,
            Italian      = 5,
            Japanese     = 6,
            Jabberwockie = 7,
            Korean       = 8,
            Chinese      = 9,
            Unknown      = 10
        };

        public char[] csf;
        public UInt32 csfVersion;
        public UInt32 numberOfLabels;
        public UInt32 numberOfStrings;
        public UInt32 unusedBytes;
        public UInt32 languageCode;

        public CsfHeader()
        {
            csf             = "".ToCharArray();
            csfVersion      = 0;
            numberOfLabels  = 0;
            numberOfStrings = 0;
            unusedBytes     = 0;
            languageCode    = 0;
        }

        public CsfHeader
            (
                char[] csf,
                UInt32 csfVersion,
                UInt32 numberOfLabels,
                UInt32 numberOfStrings,
                UInt32 unusedBytes,
                UInt32 languageCode
            )
        {
            this.csf             = csf;
            this.csfVersion      = csfVersion;
            this.numberOfLabels  = numberOfLabels;
            this.numberOfStrings = numberOfStrings;
            this.unusedBytes     = unusedBytes;
            this.languageCode    = languageCode;
        }

        public CsfHeader(CsfHeader header)
        {
            csf             = header.csf;
            csfVersion      = header.csfVersion;
            numberOfLabels  = header.numberOfLabels;
            numberOfStrings = header.numberOfStrings;
            unusedBytes     = header.unusedBytes;
            languageCode    = header.languageCode;
        }

        public static bool operator ==(CsfHeader firstHeader, CsfHeader secondHeader)
        {
            if
            (
                   firstHeader.csf             == secondHeader.csf
                && firstHeader.csfVersion      == secondHeader.csfVersion
                && firstHeader.numberOfLabels  == secondHeader.numberOfLabels
                && firstHeader.numberOfStrings == secondHeader.numberOfStrings
                && firstHeader.unusedBytes     == secondHeader.unusedBytes
                && firstHeader.languageCode    == secondHeader.languageCode
            )
                return true;

            return false;
        }

        public static bool operator !=(CsfHeader firstHeader, CsfHeader secondHeader)
            =>
                !(firstHeader == secondHeader);

        public override bool Equals(object header)
            =>
                (CsfHeader)header == this;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
