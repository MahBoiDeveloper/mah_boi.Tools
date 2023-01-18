using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    abstract class StringTable
    {
        protected StringTableParseException ParsingErrorsAndWarnings = new StringTableParseException();
        public enum StringTableFormats
        {
            csf,
            str
        }
        protected Encoding FileEncoding { get; set; }
        protected string FileName { get; set; }
        protected List<StringTableString> stStrings;

        #region Конструкторы
        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StringTable()
        {
            FileEncoding = Encoding.Unicode;
            FileName     = "TMP-" + DateTime.Now;
        }

        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StringTable(string fileName)
        {
            if (!File.Exists(fileName))
            {
                FileEncoding = Encoding.Unicode;
                FileName = fileName;
            }
        }

        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StringTable(string fileName, Encoding encoding)
        {
            if (!File.Exists(fileName))
            {
                FileEncoding = encoding;
                FileName = fileName;
            }
        }

        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StringTable(StringTable stFile)
        {
            FileEncoding = stFile.FileEncoding;
            FileName     = stFile.FileName;
            stStrings    = stFile.stStrings;
        }

        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StringTable(string fileName, List<StringTableString> strings)
        {
            FileEncoding = Encoding.Unicode;
            FileName     = fileName;
            stStrings    = strings;
        }

        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StringTable(string fileName, Encoding encoding, List<StringTableString> Strings)
        {
            FileEncoding = encoding;
            FileName     = fileName;
            stStrings    = Strings;
        }
        #endregion

        #region Парсинг
        public abstract void Parse();

        public abstract void Save();

        public abstract void Save(string fileName);

        /// <summary>
        ///     Метод формирует строку, равносильную .str/.csf файлу.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach(var str in stStrings)
                sb.AppendLine(str.StringName)
                  .AppendLine("\t\"" + str.StringValue + "\"")
                  .AppendLine("END")
                  .AppendLine(string.Empty);

            return sb.ToString();
        }
        #endregion

        #region Методы добавления строк
        public void AddString(StringTableString stString)
        {
            if(stString.IsACIIStringName())
                stStrings.Add(stString);
        }

        public void AddString(List<StringTableString> stList)
            =>
                stStrings.AddRange(stList.Where(x => x.IsACIIStringName()));

        public void AddString(StringTable stTable)
        {
            if(stTable is CsfFile || stTable is StrFile)
                stStrings.AddRange(stTable.stStrings);
        }

        public void AddEmptyString(string stringName)
            =>
                stStrings.Add(new StringTableString(stringName));
        #endregion

        #region Методы модификации строк
        public void ChangeStringName(string oldName, string newName)
        {
            if (!StringTableString.IsACIIString(oldName) || StringTableString.IsACIIString(newName)) return;

            foreach(var str in stStrings)
            {
                if(str.StringName == oldName)
                {
                    str.StringName = newName;
                    break;
                }
            }
        }

        public void ChangeStringNamesOnMatch(string oldName, string newName)
        {
            if (!StringTableString.IsACIIString(oldName) || StringTableString.IsACIIString(newName)) return;

            stStrings.Where(str => str.StringName == oldName).ToList().ForEach(str => str.StringName = newName);
        }

        public void ChangeStringValue(string stringName, string newValue)
        {
            if (!StringTableString.IsACIIString(stringName)) return;

            foreach (var str in stStrings)
            {
                if (str.StringName == stringName)
                {
                    str.StringValue = newValue;
                    break;
                }
            }
        }

        public void ChangeStringValueOnMatch(string stringName, string newValue)
        {
            if (!StringTableString.IsACIIString(stringName)) return;

            stStrings.Where(str => str.StringName == stringName).ToList().ForEach(str => str.StringValue = newValue);
        }
        #endregion

        #region Методы удаления строк
        public void DeleteStringByName(string stringName)
        { 
            if(StringTableString.IsACIIString(stringName))
            {
                foreach (var str in stStrings)
                {
                    if (str.StringName == stringName)
                    {
                        stStrings.Remove(str);
                        break;
                    }
                }
            }
        }

        public void DeleteStringByNameOnMatch(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
                stStrings.RemoveAll(str => str.StringName == stringName);
        }

        public void DeleteStringOnMatch(StringTableString deleteString)
            =>
                stStrings.RemoveAll(str => str == deleteString);

        public void DeleteStringByValue(string stringValue)
        { 
            foreach(var str in stStrings)
            {
                if (str.StringValue == stringValue)
                {
                    stStrings.Remove(str);
                    break;
                }
            }
        }

        public void DeleteStringByValueOnMatch(string stringValue)
            =>
                stStrings.RemoveAll(str => str.StringValue == stringValue);

        #endregion

        #region Методы выборки строк
        #endregion

        #region Методы работы с категориями и строками

        /// <summary>
        ///     Поиск значения по указанному названию категории и строки. Возвращает первое вхождение.
        /// </summary>
        public string GetStringValue(string stringName)
        {
            foreach (var str in stStrings)
                if (str.StringName == stringName)
                    return str.StringValue;

            return null;
        }

        ///// <summary>
        /////     Метод выдаёт список названий всех строк.
        ///// </summary>
        //public List<string> GetCategoriesNames()
        //{
        //    var tmp = new List<string>();

        //    categoriesOfTable.ForEach(category => tmp.Add(category.CategoryName));

        //    return tmp;
        //}

        ///// <summary>
        /////     Поиск категории по указанному названию. Возвращает первое вхождение.
        ///// </summary>
        //public StringTableCategory GetCategory(string categoryName)
        //{
        //    foreach (var tmp in categoriesOfTable)
        //        if (tmp.CategoryName == categoryName) return tmp;

        //    return null;
        //}

        ///// <summary>
        /////     Поиск категории по указанному названию. Возвращает все вхождения.
        ///// </summary>
        //public List<StringTableCategory> GetAllCategories(string categoryName) =>
        //    categoriesOfTable.AsParallel().Where(category => category.CategoryName == categoryName).ToList();

        ///// <summary>
        /////     Возвращает все строки первой найденной категории.
        ///// </summary>
        //public List<StringTableString> GetCategoryStrings(string categoryName)
        //{
        //    foreach (var category in categoriesOfTable)
        //        if (category.CategoryName == categoryName)
        //            return category.stringsOfCategory;

        //    return null;
        //}

        ///// <summary>
        /////     Проверка на наличие определённой категории с<br/>
        /////     файле по указанному названию категории.<br/>
        /////     При первом вхождении возвращает истину.
        ///// </summary>
        //public bool CategoryExist(string categoryName)
        //{
        //    foreach (var category in categoriesOfTable)
        //        if (category.CategoryName == categoryName)
        //            return true;

        //    return false;
        //}

        ///// <summary>
        /////     Проверка на наличие определённой категории с<br/>
        /////     файле по указанному экземпляру категории.<br/>
        /////     При первом вхождении возвращает истину.
        ///// </summary>
        //public bool CategoryExist(StringTableCategory categorySample)
        //{
        //    foreach (var category in categoriesOfTable)
        //        if (category == categorySample)
        //            return true;

        //    return false;
        //}

        /// <summary>
        ///     Проверка существовании строки в .str файле.
        /// </summary>
        public bool StringExist(string stringName)
        {
            foreach (var str in stStrings)
                if (str.StringName == stringName)
                    return true;

            return false;
        }

        /// <summary>
        ///     Проверка существовании строки в .str файле с использованием примера строки.
        /// </summary>
        public bool StringExist(StringTableString stString)
        {
            foreach (var str in stStrings)
                if (stString.StringName == str.StringName
                    && stString.StringValue == str.StringValue
                    && stString.ExtraStringValue == str.ExtraStringValue)
                    return true;

            return false;
        }

        ///// <summary>
        /////     Проверка существовании строки в .str файле.<br/>
        /////     При нахождении первого вхождения выходит, <br/>
        /////     выдавая положительный результат.
        ///// </summary>
        //public bool StringExist(string categoryName, string stringName)
        //{
        //    foreach (var category in categoriesOfTable)
        //        if (category.CategoryName == categoryName)
        //            if (category.StringExist(stringName))
        //                return true;

        //    return false;
        //}

        ///// <summary>
        /////     Проверка существовании строки в .str файле.<br/>
        /////     При нахождении первого вхождения выходит, <br/>
        /////     выдавая положительный результат.
        ///// </summary>
        //public bool StringExist(string categoryName, StringTableString stringSample)
        //{
        //    foreach (var category in categoriesOfTable)
        //        if (category.CategoryName == categoryName)
        //            if (category.StringExist(stringSample))
        //                return true;

        //    return false;
        //}

        ///// <summary>
        /////     Удаление категории вместе со строками по указанному названию.
        /////     Удаляется только первое вхождение.
        ///// </summary>
        //public StringTable RemoveCategoryWithStrings(string categoryName)
        //{
        //    foreach (var category in categoriesOfTable)
        //        if (category.CategoryName == categoryName)
        //            categoriesOfTable.Remove(GetCategory(categoryName));
            
        //    return this;
        //}

        ///// <summary>
        /////     Удаление категории вместе со строками по указанному экземпляру.
        /////     Удаляется только первое вхождение.
        ///// </summary>
        //public StringTable RemoveCategoryWithStrings(StringTableCategory categorySample)
        //{
        //    categoriesOfTable.Remove(categorySample);

        //    return this;
        //}

        ///// <summary>
        /////     Удаление категории и перемещение строк в из удаляемой категории в буффер пустых строк.<br/>
        /////     Удаляется только первое вхождение.
        ///// </summary>
        //public StringTable RemoveCategoryWithoutStrings(string categoryName)
        //{
        //    if (!CategoryExist(categoryName)) return;

        //    StringTableCategory NoCategoryStrings = GetCategory(NO_CATEGORY_STRINGS);

        //    foreach (var category in categoriesOfTable)
        //        if (category.CategoryName == categoryName)
        //        {
        //            category.stringsOfCategory.ForEach(elem => NoCategoryStrings.AddString(elem));
        //            categoriesOfTable.Remove(category);
        //            categoriesOfTable.Remove(GetCategory(NO_CATEGORY_STRINGS));
        //            categoriesOfTable.Add(NoCategoryStrings);
        //        }

        //    return this;
        //}

        ///// <summary>
        /////     Переименовка категории. Переименовывает первую категорию, попавшую под условие поиска.
        ///// </summary>
        //public StringTable RenameCategory(string oldCategoryName, string newCategoryName)
        //{
        //    foreach (var category in categoriesOfTable)
        //        if (category.CategoryName == oldCategoryName)
        //            category.CategoryName = newCategoryName;

        //    return this;
        //}

        ///// <summary>
        /////     Перемещает все подходящие строки из одной категории в другую. Нет учёта повторений.
        ///// </summary>
        //public StringTable MoveToCategory(string stringName, string oldParentCategoryName, string newParentCategoryName)
        //{
        //    if (!StringExist(oldParentCategoryName, stringName)) return;

        //    if (!CategoryExist(newParentCategoryName))
        //        categoriesOfTable.Add(new StringTableCategory(newParentCategoryName));

        //    List<StringTableCategory> list = categoriesOfTable.Where(elem => elem.CategoryName == oldParentCategoryName
        //                                                         || elem.CategoryName == newParentCategoryName).ToList();

        //    foreach (var category in list)
        //    {
        //        if (category.CategoryName == oldParentCategoryName)
        //        {
        //            foreach (var str in category.stringsOfCategory)
        //            {
        //                if (str.StringName == stringName)
        //                {
        //                    foreach (var _category in list)
        //                    {
        //                        _category.AddString(str.StringName, str.StringValue);
        //                        category.RemoveString(str.StringName, str.StringValue);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return this;
        //}

        /// <summary>
        ///     Метод подсчёта количества строк в файле без учёта пустых и дополнительных значений.
        /// </summary>
        public int Count()
            =>
                stStrings.Count;
        #endregion

        #region Вспомогательные методы
        /// <summary>
        ///     Получить все сообщения ошибок при парсинге файла определённого формата.
        /// </summary>
        public string GetParsingMessages()
            =>
                ParsingErrorsAndWarnings.GetExceptions();

        /// <summary>
        ///     Проверка конвертируемости текущего формата строковой таблицы в другой (из <u>.csf</u> в <u>.str</u> и наоборот).
        /// </summary>
        public abstract bool IsConvertable();

        /// <summary>
        ///     Проверка конвертируемости указанного списка категорий в текущую реализацию формата строковой таблицы.
        /// </summary>
        public abstract bool IsConvertable(List<StringTableString> stStringsSample);

        /// <summary>
        ///     Проверка конвертируемости различных форматов строковых таблиц между собой.<br/>
        ///     Поддерживаются проверки конвертируемости:<br/>
        ///     1) <u>.csf</u> → <u>.str</u> <br/>
        ///     2) <u>.str</u> → <u>.csf</u>
        /// </summary>
        public static bool IsConvertableTo(Object stFormated, StringTableFormats format)
        {
            if (format == StringTableFormats.csf && stFormated is StrFile strFile)
            {
                //// основные критерии конвертируемости в .csf - отсутствие пробелов в названиях строк и категорий
                //// однако и без этого игры нормально воспринимают строки с пробелами в названиях
                //if (strFile.stStrings.Where(str => str.StringName.Contains(' ')).Any())
                //    return false;
                
                return true;
            }
            else if (format == StringTableFormats.str && stFormated is CsfFile csfFile)
            {
                // основной критерий конвертируемости в .str - отсутствие дополнительных значений
                if (csfFile.stStrings.Where(str => str.ExtraStringValue != string.Empty).Any())
                    return false;

                return true;
            }
            else { }

            return false;
        }

        public static bool operator ==(StringTable firstFile, StringTable secondFile)
        {
            if (firstFile.FileName != secondFile.FileName) return false;

            if (firstFile.stStrings.Count != secondFile.stStrings.Count) return false;

            int countOfStrings = firstFile.stStrings.Count;

            for (int i = 0; i < countOfStrings; i++)
                if (firstFile.stStrings[i] != secondFile.stStrings[i])
                    return false;

            return true;
        }
        public static bool operator !=(StringTable firstFile, StringTable secondFile)
        {
            return !(firstFile == secondFile);
        }
        public override bool Equals(object obj)
            =>
                (StringTable)obj == this;
        public override int GetHashCode()
            =>
                base.GetHashCode();
        #endregion
    }
}
