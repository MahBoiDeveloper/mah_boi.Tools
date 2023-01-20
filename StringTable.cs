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

        public void ChangeStringNameOnMatch(string oldName, string newName)
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

        public void DeleteStringOnMatch(List<StringTableString> deleteStringList)
        {
            foreach(var deleteString in deleteStringList)
            {
                stStrings.RemoveAll(str => str == deleteString);
                deleteStringList.RemoveAll(str => str == deleteString);
            }
        }

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
        public string GetStringValue(string stringName)
        {
            if(StringTableString.IsACIIString(stringName))
                foreach (var str in stStrings)
                    if (str.StringName == stringName)
                        return str.StringValue;

            return null;
        }

        public string GetStringName(string stringValue)
        {
            foreach (var str in stStrings)
                if (str.StringValue == stringValue)
                    return str.StringName;

            return null;
        }

        public StringTableString GetString(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
                foreach (var str in stStrings)
                    if (str.StringName == stringName)
                        return str;

            return null;
        }

        public List<StringTableString> GetStringsWithExtraValue()
            =>
                stStrings.Where(str => str.ExtraStringValue != string.Empty).ToList();

        public List<StringTableString> GetStringOnMatch(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                List<StringTableString> stsList = new List<StringTableString>();

                foreach (var str in stStrings)
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

                foreach (var str in stStrings)
                    if (str.StringName == stringName)
                        stsList.Add(str);

                return stsList;
            }

            return null;
        }

        public List<StringTableString> GetStringByValueOnMatch(string stringValue)
        {
            List<StringTableString> stsList = new List<StringTableString>();

            foreach (var str in stStrings)
                if (str.StringValue == stringValue)
                    stsList.Add(str);

            return stsList;
        }

        public int GetStringIndexByName(string stringName)
            =>
                stStrings.FindIndex(str => str == GetString(stringName));

        public List<int> GetStringIndexByNameOnMatch(string stringName)
        {
            if(StringTableString.IsACIIString(stringName))
            {
                List<int> idList = new List<int>();

                int i = 0;
                foreach (var str in stStrings)
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
                stStrings.FindIndex(str => str == _string);

        public List<int> GetStringIndexOnMatch(StringTableString _string)
        {
            List<int> idList = new List<int>();
            int i = 0;

            foreach (var str in stStrings)
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
            stStrings.ForEach(str => nameList.Add(str.StringName));
            return nameList;
        }
        #endregion

        #region Вспомогательные методы
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

        /// <summary>
        ///     Метод подсчёта количества строк в файле без учёта пустых и дополнительных значений.
        /// </summary>
        public int Count()
            =>
                stStrings.Count;
        
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
