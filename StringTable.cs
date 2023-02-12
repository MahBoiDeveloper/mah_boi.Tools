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
        protected List<StringTableString> Table;
        protected List<StringTableExtraString> ExtraTable;

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
            FileEncoding = Encoding.UTF8;
            FileName     = "TMP-" + DateTime.Now;
            Table        = new List<StringTableString>();
            ExtraTable   = new List<StringTableExtraString>();
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
                File.Create(fileName);

            FileEncoding = Encoding.UTF8;
            FileName     = fileName;
            Table        = new List<StringTableString>();
            ExtraTable   = new List<StringTableExtraString>();
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
                File.Create(fileName);

            FileEncoding = encoding;
            FileName     = fileName;
            Table        = new List<StringTableString>();
            ExtraTable   = new List<StringTableExtraString>();
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
            Table        = stFile.Table;
            ExtraTable   = stFile.ExtraTable;
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
            FileEncoding = Encoding.UTF8;
            FileName     = fileName;
            Table        = strings;
            ExtraTable   = new List<StringTableExtraString>();
        }

        public StringTable(string fileName, List<StringTableString> strings, List<StringTableExtraString> extraStrings)
        {
            FileEncoding = Encoding.UTF8;
            FileName     = fileName;
            Table        = strings;
            ExtraTable   = extraStrings;
        }

        /// <summary>
        ///     Класс для парсинга <u>.str/.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public StringTable(string fileName, Encoding encoding, List<StringTableString> strings)
        {
            FileEncoding = encoding;
            FileName     = fileName;
            Table        = strings;
            ExtraTable   = new List<StringTableExtraString>();
        }

        public StringTable(string fileName, Encoding encoding, List<StringTableString> strings, List<StringTableExtraString> extraStrings)
        {
            FileEncoding = encoding;
            FileName     = fileName;
            Table        = strings;
            ExtraTable   = extraStrings;
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
            
            foreach (var str in Table)
            {
                str.StringValue = str.StringValue.Replace("\n", "\\n");

                sb.AppendLine(str.StringName)
                  .AppendLine("\t\"" + str.StringValue + "\"")
                  .AppendLine("END")
                  .AppendLine(string.Empty);          
            }
                
            foreach (var str in ExtraTable)
            {
                str.StringValue = str.StringValue.Replace("\n", "\\n");

                sb.AppendLine(str.StringName)
                  .AppendLine("\t\"" + str.StringValue + "\"")
                  .AppendLine("END")
                  .AppendLine(string.Empty);
            }

            return sb.ToString();
        }
        #endregion

        #region Методы добавления строк
        public void AddString(StringTableString stString)
        {
            if (stString.IsACIIStringName())
                Table.Add(stString);
        }

        public void AddString(List<StringTableString> stList)
            =>
                Table.AddRange(stList.Where(x => x.IsACIIStringName()));

        public void AddString(StringTable stImportTable)
        {
            if (stImportTable is CsfFile || stImportTable is StrFile)
            {
                Table.AddRange(stImportTable.Table);

                if (stImportTable is CsfFile && stImportTable.IsExtraStringsInStringTable())
                {
                    ExtraTable.AddRange(stImportTable.ExtraTable);
                }
            }
        }

        public void AddString(StringTableExtraString stString)
        {
            if (stString.IsACIIStringName())
                ExtraTable  .Add(stString);
        }

        public void AddString(List<StringTableExtraString> stList)
            =>
                ExtraTable.AddRange(stList.Where(x => x.IsACIIStringName()));

        public void AddEmptyString(string stringName)
            =>
                Table.Add(new StringTableString(stringName));
        #endregion

        #region Методы модификации строк
        public void ChangeStringName(string oldName, string newName)
        {
            if (!(StringTableString.IsACIIString(oldName) && StringTableString.IsACIIString(newName))) return;

            foreach (var str in Table)
            {
                if (str.StringName == oldName)
                {
                    str.StringName = newName;
                    return;
                }
            }

            foreach (var str in ExtraTable)
            {
                if (str.StringName == oldName)
                {
                    str.StringName = newName;
                    return;
                }
            }
        }

        public void ChangeStringNameOnMatch(string oldName, string newName)
        {
            if (!(StringTableString.IsACIIString(oldName) && StringTableString.IsACIIString(newName))) return;

            Table.Where(str => str.StringName == oldName).ToList().ForEach(str => str.StringName = newName);

            ExtraTable.Where(str => str.StringName == oldName).ToList().ForEach(str => str.StringName = newName);
        }

        public void ChangeStringValue(string stringName, string newValue)
        {
            if (!StringTableString.IsACIIString(stringName)) return;

            foreach (var str in Table)
            {
                if (str.StringName == stringName)
                {
                    str.StringValue = newValue;
                    return;
                }
            }

            foreach (var str in ExtraTable)
            {
                if (str.StringName == stringName)
                {
                    str.StringValue = newValue;
                    return;
                }
            }
        }

        public void ChangeStringValueOnMatch(string stringName, string newValue)
        {
            if (!StringTableString.IsACIIString(stringName)) return;

            Table.Where(str => str.StringName == stringName).ToList().ForEach(str => str.StringValue = newValue);

            ExtraTable.Where(str => str.StringName == stringName).ToList().ForEach(str => str.StringValue = newValue);
        }

        public void ChangeStringExtraValue(string stringName, string newExtraValue)
        {
            if (!StringTableString.IsACIIString(stringName)) return;

            foreach (var str in ExtraTable)
            {
                if (str.StringName == stringName)
                {
                    str.StringExtraValue = newExtraValue;
                    return;
                }
            }
        }

        public void ChangeStringExtraValueOnMatch(string stringName, string newExtraValue)
        {
            if (!StringTableString.IsACIIString(stringName)) return;

            ExtraTable.Where(str => str.StringName == stringName).ToList().ForEach(str => str.StringExtraValue = newExtraValue);
        }

        public void DeleteExtraValues()
        {
            ExtraTable.ForEach(str => Table.Add(new StringTableString(str.StringName, str.StringValue)));

            ExtraTable = new List<StringTableExtraString>();
        }
        #endregion

        #region Методы удаления строк
        public void DeleteStringByName(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                foreach (var str in Table)
                {
                    if (str.StringName == stringName)
                    {
                        Table.Remove(str);
                        return;
                    }
                }

                foreach (var str in ExtraTable)
                {
                    if (str.StringName == stringName)
                    {
                        Table.Remove(str);
                        return;
                    }
                }
            }
        }

        public void DeleteStringByNameOnMatch(string stringName)
        {
            if (!StringTableString.IsACIIString(stringName)) return;
                
            Table.RemoveAll(str => str.StringName == stringName);
            ExtraTable.RemoveAll(str => str.StringName == stringName);
        }

        public void DeleteStringOnMatch(StringTableString deleteString)
        {
            Table.RemoveAll(str => str == deleteString);
            ExtraTable.RemoveAll(str => str == deleteString);
        }

        public void DeleteStringOnMatch(List<StringTableString> deleteStringList)
        {
            foreach (var deleteString in deleteStringList)
            {
                Table.RemoveAll(str => str == deleteString);
                ExtraTable.RemoveAll(str => str == deleteString);
                deleteStringList.RemoveAll(str => str == deleteString);
            }
        }

        public void DeleteStringByValue(string stringValue)
        {
            foreach (var str in Table)
            {
                if (str.StringValue == stringValue)
                {
                    Table.Remove(str);
                    return;
                }
            }

            foreach (var str in ExtraTable)
            {
                if (str.StringValue == stringValue)
                {
                    ExtraTable.Remove(str);
                    return;
                }
            }
        }

        public void DeleteStringByValueOnMatch(string stringValue)
        {
            Table.RemoveAll(str => str.StringValue == stringValue);
            ExtraTable.RemoveAll(str => str.StringValue == stringValue);
        }

        #endregion

        #region Методы выборки строк
        public string GetStringValue(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                foreach (var str in Table)
                    if (str.StringName == stringName)
                        return str.StringValue;

                foreach (var str in ExtraTable)
                    if (str.StringName == stringName)
                        return str.StringValue;
            }

            return null;
        }

        public string GetStringName(string stringValue)
        {
            foreach (var str in Table)
                if (str.StringValue == stringValue)
                    return str.StringName;

            foreach (var str in ExtraTable)
                if (str.StringValue == stringValue)
                    return str.StringName;

            return null;
        }

        public StringTableString GetString(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                foreach (var str in Table)
                    if (str.StringName == stringName)
                        return str;

                foreach (var str in ExtraTable)
                    if (str.StringName == stringName)
                        return str;
            }

            return null;
        }

        public List<StringTableExtraString> GetStringsWithExtraValue()
            =>
                ExtraTable;

        public List<StringTableString> GetStringOnMatch(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                List<StringTableString> stsList = new List<StringTableString>();

                foreach (var str in Table)
                    if (str.StringName == stringName)
                        stsList.Add(str);

                return stsList;
            }

            return null;
        }

        public List<StringTableString> GetStringByNameOnMatch(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                List<StringTableString> stsList = new List<StringTableString>();

                foreach (var str in Table)
                    if (str.StringName == stringName)
                        stsList.Add(str);

                return stsList;
            }

            return null;
        }

        public List<StringTableString> GetStringByValueOnMatch(string stringValue)
        {
            List<StringTableString> stsList = new List<StringTableString>();

            foreach (var str in Table)
                if (str.StringValue == stringValue)
                    stsList.Add(str);

            return stsList;
        }

        public int GetStringIndexByName(string stringName)
        {
            if (!StringTableString.IsACIIString(stringName)) return -1;

            return Table.FindIndex(str => str == GetString(stringName));
        }

        public int GetExtraStringIndexByName(string stringName)
        {
            if (!StringTableString.IsACIIString(stringName)) return -1;

            int index = ExtraTable.FindIndex(str => str == GetString(stringName));
            if (index != -1)
                return Table.Count() + index;

            return -1;
        }

        public List<int> GetStringIndexByNameOnMatch(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                List<int> idList = new List<int>();

                int i = 0;
                foreach (var str in Table)
                {
                    i++;
                    if (str.StringName == stringName)
                        idList.Add(i);
                }

                return idList;
            }

            return null;
        }

        public int GetStringIndex(StringTableString _string)
            =>
                Table.FindIndex(str => str == _string);

        public List<int> GetStringIndexOnMatch(StringTableString _string)
        {
            List<int> idList = new List<int>();
            int i = 0;

            foreach (var str in Table)
            {
                i++;
                if (str == _string)
                    idList.Add(i);
            }
            return idList;
        }

        public List<string> GetStringNames()
        {
            List<string> nameList = new List<string>();
            Table.ForEach(str => nameList.Add(str.StringName));
            ExtraTable.ForEach(str => nameList.Add(str.StringName));
            return nameList;
        }

        public List<string> GetCategoryNames(char stringDelimiter)
        {
            List<string> categoryList = new List<string>();

            foreach (var str in Table)
            {
                if (str.StringName.Contains(stringDelimiter))
                {
                    str.StringName.Substring(0, str.StringName.LastIndexOf(stringDelimiter));
                }
                else
                {
                    categoryList.Add(str.StringName);
                }
            }

            foreach (var str in ExtraTable)
            {
                if (str.StringName.Contains(stringDelimiter))
                {
                    str.StringName.Substring(0, str.StringName.LastIndexOf(stringDelimiter));
                }
                else
                {
                    categoryList.Add(str.StringName);
                }
            }

            return categoryList;
        }

        public List<string> GetStringsInCategory(char stringDelimiter, string categoryName)
        {
            List<string> stringList = new List<string>();

            foreach (var str in Table)
                if (str.StringName.Contains(categoryName))
                    stringList.Add(str.StringName.Substring(str.StringName.LastIndexOf(stringDelimiter) + 1));

            foreach (var str in ExtraTable)
                if (str.StringName.Contains(categoryName))
                    stringList.Add(str.StringName.Substring(str.StringName.LastIndexOf(stringDelimiter) + 1));

            return stringList;
        }
        #endregion

        #region Вспомогательные методы
        /// <summary>
        ///     Проверка существовании строки в .csf/.str файле.
        /// </summary>
        public bool StringExist(string stringName)
        {
            foreach (var str in Table)
                if (str.StringName == stringName)
                    return true;

            foreach (var str in ExtraTable)
                if (str.StringName == stringName)
                    return true;

            return false;
        }

        /// <summary>
        ///     Проверка существовании строки в .csf/.str файле с использованием примера строки.
        /// </summary>
        public bool StringExist(StringTableString _string)
        {
            foreach (var str in Table)
                if (_string.StringName == str.StringName
                    && _string.StringValue == str.StringValue)
                    return true;

            foreach (var str in ExtraTable)
                if (_string.StringName == str.StringName
                    && _string.StringValue == str.StringValue)
                    return true;

            return false;
        }

        public bool StringExist(StringTableExtraString extraString)
        {
            foreach (var str in ExtraTable)
                if (extraString.StringName == str.StringName
                    && extraString.StringValue == str.StringValue
                    && extraString.StringExtraValue == str.StringExtraValue)
                    return true;

            return false;
        }

        /// <summary>
        ///     Метод подсчёта количества строк в таблице. Учитываются обычне и дополнительные строки.
        /// </summary>
        public int Count()
            =>
                Table.Count + ExtraTable.Count;

        /// <summary>
        ///     Получить все сообщения ошибок при парсинге файла определённого формата.
        /// </summary>
        public string GetParsingMessages()
            =>
                ParsingErrorsAndWarnings.GetExceptions();

        /// <summary>
        ///     Проверка существования строк с дополнительным значением.
        /// </summary>
        public bool IsExtraStringsInStringTable()
            =>
                ExtraTable.Count != 0 ? true : false;

        /// <summary>
        ///     Проверка конвертируемости текущего формата строковой таблицы в другой (из <u>.csf</u> в <u>.str</u> и наоборот).
        /// </summary>
        public abstract bool IsConvertable();

        /// <summary>
        ///     Проверка конвертируемости указанного списка категорий в текущую реализацию формата строковой таблицы.
        /// </summary>
        public abstract bool IsConvertable(List<StringTableString> TableSample);

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
                if (csfFile.IsExtraStringsInStringTable())
                    return false;

                return true;
            }
            else { }

            return false;
        }

        public static bool operator == (StringTable firstFile, StringTable secondFile)
        {
            if (firstFile.FileName != secondFile.FileName) return false;

            if (firstFile.Table.Count != secondFile.Table.Count) return false;

            if (firstFile.ExtraTable.Count != secondFile.ExtraTable.Count) return false;

            int countOfStrings = firstFile.Table.Count;
            for (int i = 0; i < countOfStrings; i++)
                if (firstFile.Table[i] != secondFile.Table[i])
                    return false;

            countOfStrings = firstFile.ExtraTable.Count;
            for (int i = 0; i < countOfStrings; i++)
                if (firstFile.ExtraTable[i] != secondFile.ExtraTable[i])
                    return false;

            return true;
        }
        public static bool operator != (StringTable firstFile, StringTable secondFile)
            =>
                !(firstFile == secondFile);
        public override bool Equals(object obj)
            =>
                (StringTable)obj == this;
        public override int GetHashCode()
            =>
                base.GetHashCode();
        #endregion
    }
}
