using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    class StringTableExtraString : StringTableString
    {
        public string StringExtraValue { get; set; }

        public StringTableExtraString() : base()
        {
            StringExtraValue = string.Empty;
        }

        public StringTableExtraString(string strName) : base(strName)
        {
            StringExtraValue = string.Empty;
        }

        public StringTableExtraString(string strName, string strValue, string strExtraValue) : base(strName, strValue)
        {
            StringExtraValue = strExtraValue;
        }

        public static bool operator == (StringTableString firstString, StringTableExtraString secondString)
        {
            if (firstString.StringName != secondString.StringName) return false;
            if (firstString.StringValue != secondString.StringValue) return false;
            if (secondString.StringExtraValue == string.Empty) return false;
            return true;
        }
        public static bool operator != (StringTableString firstString, StringTableExtraString secondString)
            =>
                !(firstString == secondString);

        public static bool operator == (StringTableExtraString firstString, StringTableString secondString)
        {
            if (firstString.StringName != secondString.StringName) return false;
            if (firstString.StringValue != secondString.StringValue) return false;
            if (firstString.StringExtraValue == string.Empty) return false;
            return true;
        }
        public static bool operator !=(StringTableExtraString firstString, StringTableString secondString)
            =>
                !(firstString == secondString);

        public static bool operator ==(StringTableExtraString firstString, StringTableExtraString secondString)
        {
            if (firstString.StringName != secondString.StringName) return false;
            if (firstString.StringValue != secondString.StringValue) return false;
            if (firstString.StringExtraValue != secondString.StringExtraValue) return false;
            return true;
        }
        public static bool operator !=(StringTableExtraString firstString, StringTableExtraString secondString)
            =>
                !(firstString == secondString);

        public override bool Equals(object obj)
            =>
                (StringTableExtraString)obj == this;

        public override int GetHashCode()
            =>
                base.GetHashCode();
    }
}
