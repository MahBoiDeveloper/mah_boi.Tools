using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using mah_boi.Tools.StringTable.Exceptions;

namespace mah_boi.Tools.StringTable
{
    public abstract class StringTable
    {
        protected StringTableParseException        parsingErrorsAndWarnings = new();
        protected StringTableNonAsciiNameException nonAsciiNameException    = new();

        /// <summary>
        /// Source file encoding.
        /// </summary>
        public Encoding FileEncoding { get; set; }
        
        /// <summary>
        /// Source file name.
        /// </summary>
        
        public string FileName { get; set; }
        
        /// <summary>
        /// String table data.
        /// </summary>
        public List<StringTableString> Table;

        #region Constructors
        /// <summary>
        ///     Class for parsing <u>.str/.csf</u> file formats.<br/>
        ///     Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        ///     Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        ///     Read more about parsing nuances
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable()
        {
            FileEncoding = Encoding.UTF8;
            FileName     = "TMP-" + DateTime.Now;
            Table        = new List<StringTableString>();
            ExtraTable   = new List<StringTableExtraString>();
        }

        /// <summary>
        ///     Class for parsing <u>.str/.csf</u> file formats.<br/>
        ///     Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        ///     Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        ///     Read more about parsing nuances
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
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
        ///     Class for parsing <u>.str/.csf</u> file formats.<br/>
        ///     Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        ///     Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        ///     Read more about parsing nuances
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
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
        ///     Class for parsing <u>.str/.csf</u> file formats.<br/>
        ///     Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        ///     Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        ///     Read more about parsing nuances
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(StringTable stFile)
        {
            FileEncoding = stFile.FileEncoding;
            FileName     = stFile.FileName;
            Table        = stFile.Table;
            ExtraTable   = stFile.ExtraTable;
        }

        /// <summary>
        ///     Class for parsing <u>.str/.csf</u> file formats.<br/>
        ///     Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        ///     Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        ///     Read more about parsing nuances
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(string fileName, List<StringTableString> strings)
        {
            FileEncoding = Encoding.UTF8;
            FileName     = fileName;
            Table        = strings;
            ExtraTable   = new List<StringTableExtraString>();
        }

        /// <summary>
        ///     Class for parsing <u>.str/.csf</u> file formats.<br/>
        ///     Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        ///     Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        ///     Read more about parsing nuances
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(string fileName, List<StringTableString> strings, List<StringTableExtraString> extraStrings)
        {
            FileEncoding = Encoding.UTF8;
            FileName     = fileName;
            Table        = strings;
            ExtraTable   = extraStrings;
        }

        /// <summary>
        ///     Class for parsing <u>.str/.csf</u> file formats.<br/>
        ///     Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        ///     Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        ///     Read more about parsing nuances
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(string fileName, Encoding encoding, List<StringTableString> strings)
        {
            FileEncoding = encoding;
            FileName     = fileName;
            Table        = strings;
            ExtraTable   = new List<StringTableExtraString>();
        }

        /// <summary>
        ///     Class for parsing <u>.str/.csf</u> file formats.<br/>
        ///     Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        ///     Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        ///     Read more about parsing nuances
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(string fileName, Encoding encoding, List<StringTableString> strings, List<StringTableExtraString> extraStrings)
        {
            FileEncoding = encoding;
            FileName     = fileName;
            Table        = strings;
            ExtraTable   = extraStrings;
        }
        #endregion

        #region Parsing
        /// <summary>
        /// Abstract method to parse special file format of string table.
        /// </summary>
        public abstract void Parse();

        /// <summary>
        /// Abstract method to save string table to special file format.
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Abstract method to save string table to special file format with new name.
        /// </summary>
        public abstract void SaveAs(string fileName);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (var str in Table)
            {
                str.Value = str.Value.Replace("\n", "\\n");

                sb.AppendLine(str.Name)
                  .AppendLine("\t\"" + str.Value + "\"")
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

        #region String table data changing methods
        /// <summary>
        /// Adds string sample to the string table.<br/>
        /// If string have non-ascii name, method thorws <see cref="StringTableNonAsciiNameException"/>.
        /// </summary>
        public void AddString(StringTableString stString)
        {
            if (stString.IsACIIStringName())
                Table.Add(stString);
            else
                throw nonAsciiNameException;
        }

        /// <summary>
        /// Adds string range to the string table in the end of list.<br/>
        /// If even one string have non-ascii name, method thorws <see cref="StringTableNonAsciiNameException"/>.
        /// </summary>
        public void AddString(List<StringTableString> stList) => Table.AddRange(stList.Where(x => x.IsACIIStringName()));

        /// <summary>
        /// Transfer data from given string table.
        /// </summary>
        public void AddString(StringTable stImportTable) => Table.AddRange(stImportTable.Table);

        /// <summary>
        /// Add new entry to the string table with name but without value.
        /// </summary>
        public void AddEmptyString(string stringName) => Table.Add(new StringTableString(stringName));
        #endregion

        #region Методы модификации строк
        /// <summary>
        ///     Изменяет название одной строки.
        /// </summary>
        public void ChangeStringName(string oldName, string newName)
        {
            if (!(StringTableString.IsACIIString(oldName) && StringTableString.IsACIIString(newName))) return;

            foreach (var str in Table)
            {
                if (str.Name == oldName)
                {
                    str.Name = newName;
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

        /// <summary>
        ///     Изменяет название <u>всех</u> строк при совпадении.
        /// </summary>
        public void ChangeStringNameOnMatch(string oldName, string newName)
        {
            if (!(StringTableString.IsACIIString(oldName) && StringTableString.IsACIIString(newName))) return;

            Table.Where(str => str.Name == oldName).ToList().ForEach(str => str.Name = newName);

            ExtraTable.Where(str => str.StringName == oldName).ToList().ForEach(str => str.StringName = newName);
        }

        /// <summary>
        ///     Изменяет значение строки, ищя её по названию.
        /// </summary>
        public void ChangeStringValue(string stringName, string newValue)
        {
            if (!StringTableString.IsACIIString(stringName)) return;

            foreach (var str in Table)
            {
                if (str.Name == stringName)
                {
                    str.Value = newValue;
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

        /// <summary>
        ///     Изменяет значение <u>всех</u> строк при совпадении названияю
        /// </summary>
        public void ChangeStringValueOnMatch(string stringName, string newValue)
        {
            if (!StringTableString.IsACIIString(stringName)) return;

            Table.Where(str => str.Name == stringName).ToList().ForEach(str => str.Value = newValue);

            ExtraTable.Where(str => str.StringName == stringName).ToList().ForEach(str => str.StringValue = newValue);
        }

        /// <summary>
        ///     Изменяет дополнительное значение строки.
        /// </summary>
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

        /// <summary>
        ///     Изменяет дополнительное значение <u>всех</u> строк при совпадении.
        /// </summary>
        public void ChangeStringExtraValueOnMatch(string stringName, string newExtraValue)
        {
            if (!StringTableString.IsACIIString(stringName)) return;

            ExtraTable.Where(str => str.StringName == stringName).ToList().ForEach(str => str.StringExtraValue = newExtraValue);
        }

        /// <summary>
        ///     Удаляет дополнительные значения, преобразуя строки к нормализированным.
        /// </summary>
        public void DeleteExtraValues()
        {
            ExtraTable.ForEach(str => Table.Add(new StringTableString(str.StringName, str.StringValue)));

            ExtraTable = new List<StringTableExtraString>();
        }
        #endregion

        #region Методы удаления строк
        /// <summary>
        ///     Удаление строки по названию.<br/>
        ///     Удаляется первая найденная строка относительно начала.
        /// </summary>
        public void DeleteStringByName(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                foreach (var str in Table)
                {
                    if (str.Name == stringName)
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

        /// <summary>
        ///     Удаление строк, название которых совпадает с названием шаблона.
        /// </summary>
        public void DeleteStringByNameOnMatch(string stringName)
        {
            if (!StringTableString.IsACIIString(stringName)) return;
                
            Table.RemoveAll(str => str.Name == stringName);
            ExtraTable.RemoveAll(str => str.StringName == stringName);
        }

        /// <summary>
        ///     Удаление всех строк по полному совпадению названия.<br/>
        ///     Удаление происходит из основной и дополнительной таблиц.
        /// </summary>
        public void DeleteStringOnMatch(StringTableString deleteString)
        {
            Table.RemoveAll(str => str == deleteString);
            ExtraTable.RemoveAll(str => str == deleteString);
        }

        /// <summary>
        ///     Удаление всех строк по полному совпадению из строк коллекции.
        /// </summary>
        public void DeleteStringOnMatch(List<StringTableString> deleteStringList)
        {
            foreach (var deleteString in deleteStringList)
            {
                Table.RemoveAll(str => str == deleteString);
                ExtraTable.RemoveAll(str => str == deleteString);
                deleteStringList.RemoveAll(str => str == deleteString);
            }
        }

        /// <summary>
        ///     Удаление строки из таблицы, с поиском относительно её значения.
        /// </summary>
        /// <param name="stringValue"></param>
        public void DeleteStringByValue(string stringValue)
        {
            foreach (var str in Table)
            {
                if (str.Value == stringValue)
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

        /// <summary>
        ///     Удаление всех строк из таблицы, значения которых совпадают.
        /// </summary>
        public void DeleteStringByValueOnMatch(string stringValue)
        {
            Table.RemoveAll(str => str.Value == stringValue);
            ExtraTable.RemoveAll(str => str.StringValue == stringValue);
        }
        #endregion

        #region Методы выборки строк
        /// <summary>
        ///     Получение значения строки по её названию.
        /// </summary>
        public string GetStringValue(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                foreach (var str in Table)
                    if (str.Name == stringName)
                        return str.Value;

                foreach (var str in ExtraTable)
                    if (str.StringName == stringName)
                        return str.StringValue;
            }

            return null;
        }

        /// <summary>
        ///     Получение названия строки по её значению.
        /// </summary>
        public string GetStringName(string stringValue)
        {
            foreach (var str in Table)
                if (str.Value == stringValue)
                    return str.Name;

            foreach (var str in ExtraTable)
                if (str.StringValue == stringValue)
                    return str.StringName;

            return null;
        }

        /// <summary>
        ///     Получение полной строки по названию.
        /// </summary>
        public StringTableString GetString(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                foreach (var str in Table)
                    if (str.Name == stringName)
                        return str;

                foreach (var str in ExtraTable)
                    if (str.StringName == stringName)
                        return str;
            }

            return null;
        }

        /// <summary>
        ///     Получение всех дополнительных строк.
        /// </summary>
        public List<StringTableExtraString> GetStringsWithExtraValue()
            =>
                ExtraTable;

        /// <summary>
        ///     Получение всех строк, подходящих по названию.
        /// </summary>
        public List<StringTableString> GetStringOnMatch(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                List<StringTableString> stsList = new List<StringTableString>();

                foreach (var str in Table)
                    if (str.Name == stringName)
                        stsList.Add(str);

                return stsList;
            }

            return null;
        }

        /// <summary>
        ///     Получение всех строк, подходящих по названию.
        /// </summary>
        public List<StringTableString> GetStringByNameOnMatch(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                List<StringTableString> stsList = new List<StringTableString>();

                foreach (var str in Table)
                    if (str.Name == stringName)
                        stsList.Add(str);

                return stsList;
            }

            return null;
        }

        /// <summary>
        ///     Полчение всех строк, подходящих по значению.
        /// </summary>
        public List<StringTableString> GetStringByValueOnMatch(string stringValue)
        {
            List<StringTableString> stsList = new List<StringTableString>();

            foreach (var str in Table)
                if (str.Value == stringValue)
                    stsList.Add(str);

            return stsList;
        }

        /// <summary>
        ///     Получение общего индекса строки.
        /// </summary>
        public int GetStringIndexByName(string stringName)
        {
            if (!StringTableString.IsACIIString(stringName)) return -1;

            return Table.FindIndex(str => str == GetString(stringName));
        }

        /// <summary>
        ///     Получение общего индекса дополнительной строки.
        /// </summary>
        public int GetExtraStringIndexByName(string stringName)
        {
            if (!StringTableString.IsACIIString(stringName)) return -1;

            int index = ExtraTable.FindIndex(str => str == GetString(stringName));
            if (index != -1)
                return Table.Count() + index;

            return -1;
        }

        /// <summary>
        ///     Получение списка индексов строк по шаблону.
        /// </summary>
        public List<int> GetStringIndexByNameOnMatch(string stringName)
        {
            if (StringTableString.IsACIIString(stringName))
            {
                List<int> idList = new List<int>();

                int i = 0;
                foreach (var str in Table)
                {
                    i++;
                    if (str.Name == stringName)
                        idList.Add(i);
                }

                return idList;
            }

            return null;
        }

        /// <summary>
        ///     Получение индекса строки по полному совпадению.
        /// </summary>
        public int GetStringIndex(StringTableString _string)
            =>
                Table.FindIndex(str => str == _string);

        /// <summary>
        ///     Получение список индексов строк по полному совпадению.
        /// </summary>
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

        /// <summary>
        ///     Получение названий всех строк.
        /// </summary>
        public List<string> GetStringNames()
        {
            List<string> nameList = new List<string>();
            Table.ForEach(str => nameList.Add(str.Name));
            ExtraTable.ForEach(str => nameList.Add(str.StringName));
            return nameList;
        }

        /// <summary>
        ///     Возвращает список названий категорий.
        /// </summary>
        public List<string> GetCategoryNames(char stringDelimiter)
        {
            List<string> categoryList = new List<string>();

            foreach (var str in Table)
            {
                if (str.Name.Contains(stringDelimiter))
                {
                    str.Name.Substring(0, str.Name.LastIndexOf(stringDelimiter));
                }
                else
                {
                    categoryList.Add(str.Name);
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

        /// <summary>
        ///     Возвращает список с названиями строк в категории.
        /// </summary>
        public List<string> GetStringsInCategory(char stringDelimiter, string categoryName)
        {
            List<string> stringList = new List<string>();

            foreach (var str in Table)
                if (str.Name.Contains(categoryName))
                    stringList.Add(str.Name.Substring(str.Name.LastIndexOf(stringDelimiter) + 1));

            foreach (var str in ExtraTable)
                if (str.StringName.Contains(categoryName))
                    stringList.Add(str.StringName.Substring(str.StringName.LastIndexOf(stringDelimiter) + 1));

            return stringList;
        }
        #endregion

        #region Other methods
        /// <summary>
        /// Checks if string exist in string table by its name.
        /// </summary>
        public bool StringExist(string stringName)
        {
            foreach (var str in Table)
                if (str.Name == stringName)
                    return true;
            
            return false;
        }

        /// <summary>
        /// Checks if string exist in string table.
        /// </summary>
        public bool StringExist(StringTableString _string)
        {
            foreach (var str in Table)
                if (_string == str)
                    return true;

            return false;
        }

        /// <summary>
        /// Count the number of strings in string table.
        /// </summary>
        public int Count() => Table.Count;

        /// <summary>
        /// Returns parsing errors and warnings.
        /// </summary>
        public string GetParsingMessages() => parsingErrorsAndWarnings.GetExceptions();

        /// <summary>
        /// Checks if strings with extra values exist in string table.
        /// </summary>
        public bool AreExtraValuesExist()
        {
            foreach (var str in Table)
            {
                if (str.ExtraValue != null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if string table could be converted to other file formats.
        /// </summary>
        public abstract bool IsConvertable();

        /// <summary>
        /// Checks if category list could be converted to current string table format.
        /// </summary>
        public abstract bool IsConvertable(List<StringTableString> TableSample);

        /// <summary>
        /// Checks if string table could be converted to other file formats.
        /// </summary>
        public bool IsConvertableTo(StringTableFormats format) => IsConvertableTo(this, format);

        /// <summary>
        /// Checks if string table could be converted to other file formats.
        /// </summary>
        public static bool IsConvertableTo(StringTable stFormated, StringTableFormats format)
        {
            if (format == StringTableFormats.csf)
                return !stFormated.IsExtraStringsInStringTable();

            return true;
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
        public static bool operator != (StringTable a, StringTable b) => !(a == b);

        public override bool Equals(object obj) => (StringTable)obj == this;

        public override int GetHashCode() => base.GetHashCode();
        #endregion
    }
}
