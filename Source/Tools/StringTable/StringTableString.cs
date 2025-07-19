using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools.StringTable
{
    public class StringTableString
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ExtraValue { get; set; } = null;

        public StringTableString()
        {
        }

        public StringTableString(string stringName)
        {
            Name  = stringName;
            Value = string.Empty;
        }

        public StringTableString(string stringName, string stringValue)
        {
            Name  = stringName;
            Value = stringValue;
        }

        /// <summary>
        /// Checks if string contains only ASCII characters.
        /// </summary>
        public static bool IsACIIString(string str) => Encoding.UTF8.GetByteCount(str) == str.Length;
        
        /// <summary>
        /// Checks if string contains any non ASCII characters.
        /// </summary>
        public static bool HasNonASCIIChars(string str) => Encoding.UTF8.GetByteCount(str) != str.Length;

        /// <summary>
        /// Checks if string name has only ASCII characters
        /// </summary>
        public bool IsACIIStringName() => IsACIIString(Name);

        public static bool operator == (StringTableString firstString, StringTableString secondString)
        {
            if (firstString.Name       != secondString.Name)       return false;
            if (firstString.Value      != secondString.Value)      return false;
            if (firstString.ExtraValue != secondString.ExtraValue) return false;

            return true;
        }

        public static bool operator != (StringTableString a, StringTableString b) => !(a == b);
        
        public override bool Equals(object obj) => (StringTableString)obj == this;
        
        public override int GetHashCode() => base.GetHashCode();
    }
}
