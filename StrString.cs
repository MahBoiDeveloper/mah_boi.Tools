using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mah_boi.Tools
{
    class StrString : IStrString
    {
        public string StringName { get; set; }
        public string StringValue { get; set; }

        public StrString()
        {
        }

        public StrString(string stringName)
            =>
                StringName = stringName;

        public StrString(string stringName, string stringValue)
        {
            StringName = stringName;
            StringValue = stringValue;
        }

        public static bool operator ==(StrString firstString, StrString secondString)
        {
            if (firstString.StringName != secondString.StringName) return false;
            if (firstString.StringValue != secondString.StringValue) return false;
            return true;
        }
        public static bool operator !=(StrString firstString, StrString secondString)
            =>
                !(firstString == secondString);
        public override bool Equals(object obj)
            =>
                obj == (object)this;
        public override int GetHashCode()
            =>
                base.GetHashCode();
    }
}
