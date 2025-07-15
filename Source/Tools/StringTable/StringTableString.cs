using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools.StringTable
{
    public class StringTableString
    {
        public string StringName { get; set; }
        public string StringValue { get; set; }
        public string ExtraStringValue { get; set; } = null;

        public StringTableString()
        {
        }

        public StringTableString(string stringName)
        {
            StringName       = stringName;
            StringValue      = string.Empty;
        }

        public StringTableString(string stringName, string stringValue)
        {
            StringName       = stringName;
            StringValue      = stringValue;
        }

        /// <summary>
        ///     Проверка на то, состоит ли строка только из ASCII символов.
        /// </summary>
        public static bool IsACIIString(string str)
        {
            // Создание байтового массива на основе символов строки
            byte[] bytesArray = Encoding.UTF8.GetBytes(str);

            // Если символ имеет номер больше 126, то он не является символом ASCII
            foreach (var tmp in bytesArray)
                if (tmp >= 127) return false;

            return true;
        }

        /// <summary>
        ///     Проверка на то, состоит ли название строки только из ASCII символов.
        /// </summary>
        public bool IsACIIStringName()
        {
            if (IsACIIString(StringName)) return true;
            
            return false;
        }

        /// <summary>
        ///     Проверка на то, состоит ли значение строки только из ASCII символов.
        /// </summary>
        public bool IsACIIStringValue()
        {
            if (IsACIIString(StringValue)) return true;

            return false;
        }

        /// <summary>
        ///     Проверка на то, состоит ли дополнительное значение строки только из ASCII символов.
        /// </summary>
        public bool IsACIIExtraStringValue()
        {
            if (IsACIIString(ExtraStringValue)) return true;

            return false;
        }

        public static bool operator== (StringTableString firstString, StringTableString secondString)
        {
            if (firstString.StringName  != secondString.StringName)  return false;
            if (firstString.StringValue != secondString.StringValue) return false;
            
            string extra1 = firstString?.ExtraStringValue ?? string.Empty;
            string extra2 = secondString?.ExtraStringValue ?? string.Empty;

            if (extra1 != extra2) return false;

            return true;
        }

        public static bool operator!= (StringTableString firstString, StringTableString secondString)
            =>
                !(firstString == secondString);
        
        public override bool Equals(object obj)
            =>
                (StringTableString)obj == this;
        
        public override int GetHashCode()
            =>
                base.GetHashCode();
    }
}
