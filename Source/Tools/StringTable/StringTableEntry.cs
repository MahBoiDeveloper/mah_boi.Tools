using mah_boi.Tools.StringTable.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools.StringTable
{
    public class StringTableEntry
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ExtraValue { get; set; } = null;

        public StringTableEntry()
        {
        }

        public StringTableEntry(string stringName)
        {
            Name  = stringName;
            Value = string.Empty;
        }

        public StringTableEntry(string stringName, string stringValue)
        {
            Name  = stringName;
            Value = stringValue;
        }

        /// <summary>
        /// Checks if string name has only ASCII characters
        /// </summary>
        public bool IsACIIName() => Name.IsACII();

        public static bool operator == (StringTableEntry firstString, StringTableEntry secondString)
        {
            if (firstString.Name       != secondString.Name)       return false;
            if (firstString.Value      != secondString.Value)      return false;
            if (firstString.ExtraValue != secondString.ExtraValue) return false;

            return true;
        }

        public static bool operator != (StringTableEntry a, StringTableEntry b) => !(a == b);
        
        public override bool Equals(object obj) => (StringTableEntry)obj == this;
        
        public override int GetHashCode() => base.GetHashCode();
    }
}
