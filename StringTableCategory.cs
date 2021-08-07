using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace mah_boi.Tools
{
    class StringTableCategory
    {
        public string CategoryName { get; set; }
        public List<StringTableString> stringsOfCategory;

        public StringTableCategory(string categoryName)
        {
            CategoryName = categoryName;
            stringsOfCategory = new List<StringTableString>();
        }

        public StringTableCategory(StringTableCategory categorySample)
            =>
                stringsOfCategory = new List<StringTableString>(categorySample.stringsOfCategory);

        /// <summary>
        ///     Возращает значение указанной строки.
        /// </summary>
        public string GetStringValue(string stringName)
        {
            foreach(var tmp in stringsOfCategory)
                if (tmp.StringName == stringName)
                    return tmp.StringValue;

            return null;
        }

        /// <summary>
        ///     Добавление в категорию строки с указанным названием, но без значения.
        /// </summary>
        public void AddString(string stringName)
            =>
                stringsOfCategory.Add(new StringTableString(stringName));

        /// <summary>
        ///     Добавление в категорию строки с указанным названием и значением.
        /// </summary>
        public void AddString(string stringName, string stringValue)         
            =>
                stringsOfCategory.Add(new StringTableString(stringName, stringValue));

        /// <summary>
        ///     Добавление в категорию заранее сформированной строки.
        /// </summary>
        public void AddString(StringTableString stringSample)                       
            => 
                stringsOfCategory.Add(stringSample);

        /// <summary>
        ///     Удалении из категории строк с указанным названием.
        /// </summary>
        public void RemoveString(string stringName, string stringValue)      
            => 
                stringsOfCategory.Remove(new StringTableString(stringName, stringValue));

        /// <summary>
        ///     Удалении из категории строк, совпадающих со строкой, указанной в параметрах.
        /// </summary>
        public void RemoveString(StringTableString stringSample)                     
            => 
                stringsOfCategory.RemoveAll(elem => elem == stringSample);

        /// <summary>
        ///     Переименование строки, подходящей под указанное название.
        /// </summary>
        public void RenameString(string oldStringName, string newStringName)
        {
            foreach (var tmp in stringsOfCategory)
            {
                if (tmp.StringName == oldStringName)
                {
                    tmp.StringName = newStringName;
                    return;
                }
            }
        }

        /// <summary>
        ///     Переименование строк, подходящих под указанное название.
        /// </summary>
        public void RenameAllStrings(string oldStringName, string newStringName)
        {
            foreach (var tmp in stringsOfCategory)
            {
                if (tmp.StringName == oldStringName)
                    tmp.StringName = newStringName;
            }
        }

        public bool StringExist(string stringName)
        {
            if (stringsOfCategory.Where(elem => elem.StringName == stringName).ToList().Count > 0)
                return true;
            else
                return false;
        }

        public bool StringExist(StringTableString stringSample)
        {
            if (stringsOfCategory.Where(elem => elem == stringSample).ToList().Count > 0)
                return true;
            else
                return false;
        }

        public static bool operator ==(StringTableCategory firstCategory, StringTableCategory secondCategory)
        {
            if (firstCategory.CategoryName != secondCategory.CategoryName) return false;

            if (firstCategory.stringsOfCategory.Count != secondCategory.stringsOfCategory.Count) return false;

            for (int i = 0; i < firstCategory.stringsOfCategory.Count; i++)
                if (firstCategory.stringsOfCategory[i] != secondCategory.stringsOfCategory[i])
                    return false;

            return true;
        }
        public static bool operator !=(StringTableCategory firstCategory, StringTableCategory secondCategory)
            =>
                !(firstCategory == secondCategory);
        public override bool Equals(object obj)
            =>
                obj == (object)this;
        public override int GetHashCode()
            =>
                base.GetHashCode();
    }
}
