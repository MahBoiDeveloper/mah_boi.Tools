using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    interface IStringTable
    {
        List<string> GetCategoriesNames();
        List<StringTableString> GetCategoryStrings(string categoryName);
        StringTableCategory GetCategory(string categoryName);
        List<StringTableCategory> GetAllCategories(string categoryName);
        string GetStringValue(string categoryName, string stringName);
        void Save();
        void Save(string fileName);
        bool StringExist(string stringName);
        bool StringExist(string categoryName, string stringName);
        bool StringExist(string categoryName, StringTableString stringSample);
        bool CategoryExist(string categoryName);
        bool CategoryExist(StringTableCategory categoryName);
        void RemoveCategoryWithStrings(string categoryName);
        void RemoveCategoryWithStrings(StringTableCategory categorySample);
        void RemoveCategoryWithoutStrings(string categoryName);
        void RenameCategory(string oldCategoryName, string newCategoryName);
        void MoveToCategory(string stringName, string oldParentCategoryName, string newParentCategoryName);
    }
}
