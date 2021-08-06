using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mah_boi.Tools
{
    interface IStrFile
    {
        List<string> GetCategoriesNames();
        List<StringTableString> GetCategoryStrings(string categoryName);
        StrCategory GetCategory(string categoryName);
        List<StrCategory> GetAllCategories(string categoryName);
        string GetStringValue(string categoryName, string stringName);
        void Save();
        void Save(string fileName);
        bool StringExist(string stringName);
        bool StringExist(string categoryName, string stringName);
        bool StringExist(string categoryName, StringTableString stringSample);
        bool CategoryExist(string categoryName);
        bool CategoryExist(StrCategory categoryName);
        void RemoveCategoryWithStrings(string categoryName);
        void RemoveCategoryWithStrings(StrCategory categorySample);
        void RemoveCategoryWithoutStrings(string categoryName);
        void RenameCategory(string oldCategoryName, string newCategoryName);
        void MoveToCategory(string stringName, string oldParentCategoryName, string newParentCategoryName);
        CsfFile ConvertToCsf();
        CsfFile ConvertToCsf(string fileName);
        CsfFile ConvertToCsf(StrFile fileSample);
    }
}
