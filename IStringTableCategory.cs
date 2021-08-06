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
        void AddString(StrString stringSample);
        void AddString(string stringName, string stringValue);
        void RemoveString(string stringName, string stringValue);
        void RemoveString(StrString stringSample);
        void RenameString(string oldStringName, string newStringName);
        void RenameAllStrings(string oldStringName, string newStringName);
        bool StringExist(string stringName);
        bool StringExist(StrString stringSample);
    }
}
