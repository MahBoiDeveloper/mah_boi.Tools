using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mah_boi.Tools
{
    interface IStringTableCategory
    {
        string CategoryName { get; set; }
        string GetStringValue(string stringName);
        void AddString(string stringName);
        void AddString(StringTableString stringSample);
        void AddString(string stringName, string stringValue);
        void RemoveString(string stringName, string stringValue);
        void RemoveString(StringTableString stringSample);
        void RenameString(string oldStringName, string newStringName);
        void RenameAllStrings(string oldStringName, string newStringName);
        bool StringExist(string stringName);
        bool StringExist(StringTableString stringSample);
    }
}
