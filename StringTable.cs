using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    abstract class StringTable
    {
        public enum StringTableFormats
        {
            csf,
            str
        }
        public Encoding FileEncoding { get; set; }
        public string FileName { get; set; }
        public const string NO_CATEGORY_STRINGS = ".NOCATEGORYSTRINGS";
        public const string STRING_TABLE_META_DATA = ".METADATA";
        protected List<StringTableCategory> categoriesOfTable;

        #region Конструкторы
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
                throw new StringTableParseException("Файл для парсинга не существует");

            FileEncoding = Encoding.Unicode;
            FileName     = fileName;
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
                throw new StringTableParseException("Файл для парсинга не существует");

            FileEncoding = encoding;
            FileName     = fileName;
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
            FileEncoding      = Encoding.Unicode;
            FileName          = stFile.FileName;
            categoriesOfTable = stFile.categoriesOfTable;
        }

        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StringTable(string fileName, List<StringTableCategory> stCategoties)
        {
            FileEncoding      = Encoding.Unicode;
            FileName          = fileName;
            categoriesOfTable = stCategoties;
        }

        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StringTable(string fileName, Encoding encoding, List<StringTableCategory> stCategoties)
        {
            FileEncoding      = encoding;
            FileName          = fileName;
            categoriesOfTable = stCategoties;
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

            foreach (var category in categoriesOfTable)
                foreach (var _string in category.stringsOfCategory)
                {
                    if (category.CategoryName != NO_CATEGORY_STRINGS)
                        sb.AppendLine(category.CategoryName + ":" + _string.StringName);
                    else
                        sb.AppendLine(_string.StringName);

                    sb.AppendLine("\t\"" + _string.StringValue + "\"")
                      .AppendLine("END")
                      .AppendLine(string.Empty);
                }

            return sb.ToString();
        }
        #endregion

        #region Методы работы с категориями и строками
        /// <summary>
        ///     Метод выдаёт список названий всех строк.
        /// </summary>
        public List<string> GetCategoriesNames()
        {
            var tmp = new List<string>();

            categoriesOfTable.ForEach(category => tmp.Add(category.CategoryName));

            return tmp;
        }

        /// <summary>
        ///     Поиск категории по указанному названию. Возвращает первое вхождение.
        /// </summary>
        public StringTableCategory GetCategory(string categoryName)
        {
            foreach (var tmp in categoriesOfTable)
                if (tmp.CategoryName == categoryName) return tmp;

            return null;
        }

        /// <summary>
        ///     Поиск категории по указанному названию. Возвращает все вхождения.
        /// </summary>
        public List<StringTableCategory> GetAllCategories(string categoryName) =>
            categoriesOfTable.Where(category => category.CategoryName == categoryName).ToList();

        /// <summary>
        ///     Поиск значения по указанному названию категории и строки. Возвращает первое вхождение.
        /// </summary>
        public string GetStringValue(string categoryName, string stringName)
        {
            foreach (var category in categoriesOfTable)
                if (category.CategoryName == categoryName)
                    return category.GetStringValue(stringName);

            return null;
        }

        /// <summary>
        ///     Возвращает все строки первой найденной категории.
        /// </summary>
        public List<StringTableString> GetCategoryStrings(string categoryName)
        {
            foreach (var category in categoriesOfTable)
                if (category.CategoryName == categoryName)
                    return category.stringsOfCategory;

            return null;
        }

        /// <summary>
        ///     Проверка на наличие определённой категории с<br/>
        ///     файле по указанному названию категории.<br/>
        ///     При первом вхождении возвращает истину.
        /// </summary>
        public bool CategoryExist(string categoryName)
        {
            foreach (var category in categoriesOfTable)
                if (category.CategoryName == categoryName)
                    return true;

            return false;
        }

        /// <summary>
        ///     Проверка на наличие определённой категории с<br/>
        ///     файле по указанному экземпляру категории.<br/>
        ///     При первом вхождении возвращает истину.
        /// </summary>
        public bool CategoryExist(StringTableCategory categorySample)
        {
            foreach (var category in categoriesOfTable)
                if (category == categorySample)
                    return true;

            return false;
        }

        /// <summary>
        ///     Проверка существовании строки в .str файле.
        /// </summary>
        public bool StringExist(string stringName)
        {
            foreach (var category in categoriesOfTable)
                if (category.StringExist(stringName))
                    return true;

            return false;
        }

        /// <summary>
        ///     Проверка существовании строки в .str файле.<br/>
        ///     При нахождении первого вхождения выходит, <br/>
        ///     выдавая положительный результат.
        /// </summary>
        public bool StringExist(string categoryName, string stringName)
        {
            foreach (var category in categoriesOfTable)
                if (category.CategoryName == categoryName)
                    if (category.StringExist(stringName))
                        return true;

            return false;
        }

        /// <summary>
        ///     Проверка существовании строки в .str файле.<br/>
        ///     При нахождении первого вхождения выходит, <br/>
        ///     выдавая положительный результат.
        /// </summary>
        public bool StringExist(string categoryName, StringTableString stringSample)
        {
            foreach (var category in categoriesOfTable)
                if (category.CategoryName == categoryName)
                    if (category.StringExist(stringSample))
                        return true;

            return false;
        }

        /// <summary>
        ///     Удаление категории вместе со строками по указанному названию.
        ///     Удаляется только первое вхождение.
        /// </summary>
        public void RemoveCategoryWithStrings(string categoryName)
        {
            foreach (var category in categoriesOfTable)
                if (category.CategoryName == categoryName)
                    categoriesOfTable.Remove(GetCategory(categoryName));
        }

        /// <summary>
        ///     Удаление категории вместе со строками по указанному экземпляру.
        ///     Удаляется только первое вхождение.
        /// </summary>
        public void RemoveCategoryWithStrings(StringTableCategory categorySample)
            =>
                categoriesOfTable.Remove(categorySample);

        /// <summary>
        ///     Удаление категории и перемещение строк в из удаляемой категории в буффер пустых строк.<br/>
        ///     Удаляется только первое вхождение.
        /// </summary>
        public void RemoveCategoryWithoutStrings(string categoryName)
        {
            if (!CategoryExist(categoryName)) return;

            StringTableCategory NoCategoryStrings = GetCategory(NO_CATEGORY_STRINGS);

            foreach (var category in categoriesOfTable)
                if (category.CategoryName == categoryName)
                {
                    category.stringsOfCategory.ForEach(elem => NoCategoryStrings.AddString(elem));
                    categoriesOfTable.Remove(category);
                    categoriesOfTable.Remove(GetCategory(NO_CATEGORY_STRINGS));
                    categoriesOfTable.Add(NoCategoryStrings);
                }
        }

        /// <summary>
        ///     Переименовка категории. Переименовывает первую категорию, попавшую под условие поиска.
        /// </summary>
        public void RenameCategory(string oldCategoryName, string newCategoryName)
        {
            foreach (var category in categoriesOfTable)
                if (category.CategoryName == oldCategoryName)
                    category.CategoryName = newCategoryName;
        }

        /// <summary>
        ///     Перемещает все подходящие строки из одной категории в другую. Нет учёта повторений.
        /// </summary>
        public void MoveToCategory(string stringName, string oldParentCategoryName, string newParentCategoryName)
        {
            if (!StringExist(oldParentCategoryName, stringName)) return;

            if (!CategoryExist(newParentCategoryName))
                categoriesOfTable.Add(new StringTableCategory(newParentCategoryName));

            List<StringTableCategory> list = categoriesOfTable.Where(elem => elem.CategoryName == oldParentCategoryName
                                                                 || elem.CategoryName == newParentCategoryName).ToList();

            foreach (var category in list)
            {
                if (category.CategoryName == oldParentCategoryName)
                {
                    foreach (var str in category.stringsOfCategory)
                    {
                        if (str.StringName == stringName)
                        {
                            foreach (var _category in list)
                            {
                                _category.AddString(str.StringName, str.StringValue);
                                category.RemoveString(str.StringName, str.StringValue);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Вспомогательные методы
        /// <summary>
        ///     Проверка конвертируемости текущего формата строковой таблицы в другой (из <u>.csf</u> в <u>.str</u> и наоборот).
        /// </summary>
        public abstract bool IsConvertable();

        /// <summary>
        ///     Проверка конвертируемости указанного списка категорий в текущую реализацию формата строковой таблицы.
        /// </summary>
        public abstract bool IsConvertable(List<StringTableCategory> stListOfCategories);

        /// <summary>
        ///     Проверка конвертируемости различных форматов строковых таблиц между собой.<br/>
        ///     Поддерживаются проверки конвертируемости:<br/>
        ///     1) <u>.csf</u> → <u>.str</u> <br/>
        ///     2) <u>.str</u> → <u>.csf</u>
        /// </summary>
        public static bool IsConvertableTo(Object stFormated, StringTableFormats format)
        {
            if (format == StringTableFormats.csf && stFormated is StrFile)
            {
                // основной критерий конвертируемости в .csf - отсутствие пробелов в названиях строк и категорий
                if ((stFormated as StrFile).categoriesOfTable.Where
                                (
                                    category =>
                                        category.CategoryName.Contains(' ')
                                        || category.stringsOfCategory.Where(str => str.StringName.Contains(' ')).ToList().Count > 0

                                ).Any())
                    return false;

                return true;
            }
            else if (format == StringTableFormats.str && stFormated is CsfFile csfFile)
            {
                // основной критерий конвертируемости в .str - отсутствие ковычек " в значении строки
                foreach (var category in csfFile.categoriesOfTable)
                    if (category.stringsOfCategory.Where(str => str.StringValue.Contains('\"')).Any())
                        return false;

                return true;
            }
            else { }

            return false;
        }

        public static bool operator ==(StringTable firstFile, StringTable secondFile)
        {
            if (firstFile.FileName != secondFile.FileName) return false;

            if (firstFile.categoriesOfTable.Count != secondFile.categoriesOfTable.Count) return false;

            if (firstFile.FileEncoding != secondFile.FileEncoding) return false;

            for (int i = 0; i < firstFile.categoriesOfTable.Count; i++)
                if (firstFile.categoriesOfTable[i] != secondFile.categoriesOfTable[i])
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

        /// <summary>
        ///     Вынесенная логика из метода <see cref="Parse"></see>. Метод комбинирует список<br/>
        ///     с категориями, где у категорий имеется только по 1 строке, <br/>
        ///     в полноценные категории с множеством строк внутри себя.
        /// </summary>
        protected void CombineStringsIntoCategories(List<StringTableCategory> list)
        {
            List<StringTableCategory> bufferList = new List<StringTableCategory>();
            StringTableCategory bufferCategory;

            for (; list.Count != 0;)
            {
                string categoryName = list[0].CategoryName;
                // создаём категорию с названием, как у первого элемента списка, т.к. список отсортирован
                bufferCategory = new StringTableCategory(categoryName);

                // выделяем из списка все категории с одним именем, и затем записываем значения из них в буфер
                list.Where(elem => elem.CategoryName == categoryName).ToList()
                    .ForEach(elem => bufferCategory.AddString(elem.stringsOfCategory[0].StringName,
                                                              elem.stringsOfCategory[0].StringValue));

                bufferList.Add(bufferCategory); // получив категорию с собранными вместе строками, записываем в буферный список

                list.RemoveAll(elem => elem.CategoryName == categoryName); // очищаем строку от уже скомбинированных строк
            }

            categoriesOfTable = bufferList;
        }
        #endregion
    }
}
