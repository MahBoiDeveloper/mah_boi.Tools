using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mah_boi.Tools
{
    abstract class StringTableString : IStringTableString
    {
        public string StringName { get; set; }
        public string StringValue { get; set; }

        public StringTableString()
        {
        }

        public StringTableString(string stringName)
            =>
                StringName = stringName;

        public StringTableString(string stringName, string stringValue)
        {
            StringName = stringName;
            StringValue = stringValue;
        }

        public static bool operator ==(StringTableString firstString, StringTableString secondString)
        {
            if (firstString.StringName != secondString.StringName) return false;
            if (firstString.StringValue != secondString.StringValue) return false;
            return true;
        }
        public static bool operator !=(StringTableString firstString, StringTableString secondString)
            =>
                !(firstString == secondString);
        public override bool Equals(object stString)
            =>
                (StringTableString)stString == this;
        public override int GetHashCode()
            =>
                base.GetHashCode();
    }
}
