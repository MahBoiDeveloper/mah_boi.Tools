﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mah_boi.Tools
{
    interface IStrCategory
    {
        string CategoryName { get; set; }
        string GetStringValue(string stringName);
        void AddString(string stringName);
        void AddString(StrString stringSample);
        void AddString(string stringName, string stringValue);
        void RemoveString(string stringName, string stringValue);
        void RemoveString(StrString stringSample);
        void RenameString(string oldStringName, string newStringName);
    }
}
