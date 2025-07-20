using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using mah_boi.Tools.StringTable.Exceptions;
using mah_boi.Tools.StringTable.Extensions;

namespace mah_boi.Tools.StringTable
{
    public abstract class StringTable
    {
        protected StringTableParseException        parsingErrorsAndWarnings = new();
        protected StringTableNonAsciiNameException nonAsciiNameException    = new();

        public StringTableFormat Format { get; set; } = StringTableFormat.csf;

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
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        /// Read more about parsing nuances
        /// <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable()
        {
            FileEncoding = Encoding.UTF8;
            FileName     = "TMP-" + DateTime.Now;
            Table        = new List<StringTableString>();
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        /// Read more about parsing nuances
        /// <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(string fileName)
        {
            if (!File.Exists(fileName))
                File.Create(fileName);

            FileEncoding = Encoding.UTF8;
            FileName     = fileName;
            Table        = new List<StringTableString>();
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        /// Read more about parsing nuances
        /// <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(string fileName, Encoding encoding)
        {
            if (!File.Exists(fileName))
                File.Create(fileName);

            FileEncoding = encoding;
            FileName     = fileName;
            Table        = new List<StringTableString>();
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        /// Read more about parsing nuances
        /// <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(StringTable stFile)
        {
            FileEncoding = stFile.FileEncoding;
            FileName     = stFile.FileName;
            Table        = stFile.Table;
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        /// Read more about parsing nuances
        /// <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(string fileName, List<StringTableString> strings)
        {
            FileEncoding = Encoding.UTF8;
            FileName     = fileName;
            Table        = strings;
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        /// Read more about parsing nuances
        /// <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(string fileName, List<StringTableString> strings, List<StringTableExtraString> extraStrings)
        {
            FileEncoding = Encoding.UTF8;
            FileName     = fileName;
            Table        = strings;
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        /// Read more about parsing nuances
        /// <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(string fileName, Encoding encoding, List<StringTableString> strings)
        {
            FileEncoding = encoding;
            FileName     = fileName;
            Table        = strings;
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about CSF/STR formats <see href="https://modenc.renegadeprojects.com/CSF_File_Format">here</see>.<br/>
        /// Read more about parsing nuances
        /// <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">here</see>.
        /// </summary>
        public StringTable(string fileName, Encoding encoding, List<StringTableString> strings, List<StringTableExtraString> extraStrings)
        {
            FileEncoding = encoding;
            FileName     = fileName;
            Table        = strings;
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

        #region String table add methods
        /// <summary>
        /// Adds string sample to the string table.<br/>
        /// If string have non-ascii name, method thorws <see cref="StringTableNonAsciiNameException"/>.
        /// </summary>
        public void AddString(StringTableString stString)
        {
            if (stString.IsACIIName())
                Table.Add(stString);
            else
                throw nonAsciiNameException;
        }

        /// <summary>
        /// Adds string range to the string table in the end of list.<br/>
        /// If even one string have non-ascii name, method thorws <see cref="StringTableNonAsciiNameException"/>.
        /// </summary>
        public void AddString(List<StringTableString> stList) => Table.AddRange(stList.Where(x => x.IsACIIName()));

        /// <summary>
        /// Transfer data from given string table.
        /// </summary>
        public void AddString(StringTable stImportTable) => Table.AddRange(stImportTable.Table);

        /// <summary>
        /// Add new entry to the string table with name but without value.
        /// </summary>
        public void AddEmptyString(string stringName) => Table.Add(new StringTableString(stringName));
        #endregion

        #region String table modify methods
        /// <summary>
        /// Renames string name.
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
        }

        /// <summary>
        /// Change string name for each match.
        /// </summary>
        public void ChangeStringNameOnMatch(string oldName, string newName)
        {
            if (oldName.HasNonASCIIChars() || oldName.HasNonASCIIChars())
                throw nonAsciiNameException;

            Table.Where(str => str.Name == oldName).ToList().ForEach(str => str.Name = newName);
        }

        /// <summary>
        /// Change string value by name match.
        /// </summary>
        public void ChangeStringValue(string stringName, string newValue)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;

            foreach (var str in Table)
            {
                if (str.Name == stringName)
                {
                    str.Value = newValue;
                    return;
                }
            }
        }

        /// <summary>
        /// Change string value for each name match.
        /// </summary>
        public void ChangeStringValueOnMatch(string stringName, string newValue)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;

            Table.Where(str => str.Name == stringName).ToList().ForEach(str => str.Value = newValue);
        }

        /// <summary>
        /// Change string extra value by name match.
        /// </summary>
        public void ChangeStringExtraValue(string stringName, string newExtraValue)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;

            foreach (var str in Table)
            {
                if (str.Name == stringName)
                {
                    str.ExtraValue = newExtraValue;
                    return;
                }
            }
        }

        /// <summary>
        /// Change string value for each name match.
        /// </summary>
        public void ChangeStringExtraValueOnMatch(string stringName, string newExtraValue)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;

            Table.Where(str => str.Name == stringName).ToList().ForEach(str => str.ExtraValue = newExtraValue);
        }

        /// <summary>
        /// Delete all extra values from string table and make it convertable to text file formats.
        /// </summary>
        public void DeleteExtraValues() => Table.ForEach(str => str.ExtraValue = null);
        #endregion

        #region String table delete methods
        /// <summary>
        /// Removes entry from string table by name match.
        /// </summary>
        public void DeleteStringByName(string stringName)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;
            
            foreach (var str in Table)
            {
                if (str.Name == stringName)
                {
                    Table.Remove(str);
                    return;
                }
            }
        }

        /// <summary>
        /// Removes all entries from string table by name match.
        /// </summary>
        public void DeleteStringsByNameOnMatch(string stringName)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;

            Table.RemoveAll(str => str.Name == stringName);
        }

        /// <summary>
        /// Removes all entries from string table by match.
        /// </summary>
        public void DeleteStringsOnMatch(StringTableString deleteString) => Table.RemoveAll(str => str == deleteString);

        /// <summary>
        /// Removes entries range from string table by match.
        /// </summary>
        public void DeleteStringRange(List<StringTableString> list) => list.ForEach(input => Table.RemoveAll(str => str == input));

        /// <summary>
        /// Removes entry from string table by value match.
        /// </summary>
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
        }

        /// <summary>
        /// Removes all entries from string table by value match.
        /// </summary>
        public void DeleteStringsByValueOnMatch(string stringValue) => Table.RemoveAll(str => str.Value == stringValue);

        /// <summary>
        /// Removes all string table data.
        /// </summary>
        public void Clear() => Table.Clear();
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
        public bool IsConvertableTo(StringTableFormat format) => IsConvertableTo(this, format);

        /// <summary>
        /// Checks if string table could be converted to other file formats.
        /// </summary>
        public static bool IsConvertableTo(StringTable stFormated, StringTableFormat format)
        {
            if (format == StringTableFormat.csf)
                return !stFormated.AreExtraValuesExist();

            return true;
        }

        public static bool operator == (StringTable firstFile, StringTable secondFile)
        {
            if (firstFile.FileName != secondFile.FileName) return false;

            if (firstFile.Table.Count != secondFile.Table.Count) return false;

            int countOfStrings = firstFile.Table.Count;
            for (int i = 0; i < countOfStrings; i++)
                if (firstFile.Table[i] != secondFile.Table[i])
                    return false;

            return true;
        }
        public static bool operator != (StringTable a, StringTable b) => !(a == b);

        public override bool Equals(object obj) => (StringTable)obj == this;

        public override int GetHashCode() => base.GetHashCode();
        #endregion
    }
}
