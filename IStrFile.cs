﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mah_boi.Tools
{
    interface IStrFile
    {
        void Parse();
        List<string> GetCategoriesNames();
        List<StrString> GetCategoryStrings(string categoryName);
        StrCategory GetCategory(string categoryName);
        List<StrCategory> GetAllCategories(string categoryName);
        string GetStringValue(string categoryName, string stringName);
        void Save();
        void Save(string fileName);
        bool StringExist(string stringName);
        bool StringExist(string categoryName, string stringName);
        bool StringExist(string categoryName, StrString stringSample);
        bool CategoryExist(string categoryName);
        bool CategoryExist(StrCategory categoryName);
        void RemoveCategoryWithStrings(string categoryName);
        void RemoveCategoryWithStrings(StrCategory categorySample);
        void RemoveCategoryWithoutStrings(string categoryName);
        void MoveToCategory(string stringName, string oldParentCategoryName, string newParentCategoryName);
        CsfFile ConvertToCsf();
        CsfFile ConvertToCsf(string fileName);
        CsfFile ConvertToCsf(StrFile fileSample);
    }
}
