using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    class StrCategory : IStrCategory
    {
        public string CategoryName { get; set; }
        public List<StrString> stringsOfCategory;

        public StrCategory(string categoryName)
        {
            CategoryName = categoryName;
            stringsOfCategory = new List<StrString>();
        }

        public StrCategory(StrCategory categorySample)
            =>
                stringsOfCategory = new List<StrString>(categorySample.stringsOfCategory);

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
                stringsOfCategory.Add(new StrString(stringName));

        /// <summary>
        ///     Добавление в категорию строки с указанным названием и значением.
        /// </summary>
        public void AddString(string stringName, string stringValue)         
            =>
                stringsOfCategory.Add(new StrString(stringName, stringValue));

        /// <summary>
        ///     Добавление в категорию заранее сформированной строки.
        /// </summary>
        public void AddString(StrString stringSample)                       
            => 
                stringsOfCategory.Add(stringSample);

        /// <summary>
        ///     Удалении из категории строк с указанным названием.
        /// </summary>
        public void RemoveString(string stringName, string stringValue)      
            => 
                stringsOfCategory.Remove(new StrString(stringName, stringValue));

        /// <summary>
        ///     Удалении из категории строк, совпадающих со строкой, указанной в параметрах.
        /// </summary>
        public void RemoveString(StrString stringSample)                     
            => 
                stringsOfCategory.RemoveAll(elem => elem == stringSample);

        /// <summary>
        ///     Переименование строк, подходящих под указанное название.
        /// </summary>
        public void RenameString(string oldStringName, string newStringName)
        {
            foreach (var tmp in stringsOfCategory)
            {
                if (tmp.StringName == oldStringName)
                    tmp.StringName = newStringName;
            }
        }

        public static bool operator ==(StrCategory firstCategory, StrCategory secondCategory)
        {
            if (firstCategory.CategoryName != secondCategory.CategoryName) return false;

            if (firstCategory.stringsOfCategory.Count != secondCategory.stringsOfCategory.Count) return false;

            for (int i = 0; i < firstCategory.stringsOfCategory.Count; i++)
                if (firstCategory.stringsOfCategory[i] != secondCategory.stringsOfCategory[i])
                    return false;

            return true;
        }
        public static bool operator !=(StrCategory firstCategory, StrCategory secondCategory)
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
