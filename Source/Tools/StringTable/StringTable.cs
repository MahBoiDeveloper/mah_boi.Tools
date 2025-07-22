using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using mah_boi.Tools.Extensions;
using mah_boi.Tools.StringTable.Exceptions;

namespace mah_boi.Tools.StringTable
{
    public abstract class StringTable
    {
        #region Fields and properties
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
        public List<StringTableEntry> Table;

        /// <summary>
        /// Categories separator.
        /// </summary>
        public char CategorySeparator { get; set; } = ':';
        #endregion

        #region Constructors
        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
        /// </summary>
        public StringTable()
        {
            FileEncoding = Encoding.UTF8;
            FileName     = "TMP-" + DateTime.Now.ToString();
            Table        = new List<StringTableEntry>();
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
        /// </summary>
        public StringTable(string fileName)
        {
            if (!File.Exists(fileName))
                File.Create(fileName);

            FileEncoding = Encoding.UTF8;
            FileName     = fileName;
            Table        = new List<StringTableEntry>();
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
        /// </summary>
        public StringTable(string fileName, Encoding encoding)
        {
            if (!File.Exists(fileName))
                File.Create(fileName);

            FileEncoding = encoding;
            FileName     = fileName;
            Table        = new List<StringTableEntry>();
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
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
        /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
        /// </summary>
        public StringTable(string fileName, List<StringTableEntry> strings)
        {
            FileEncoding = Encoding.UTF8;
            FileName     = fileName;
            Table        = strings;
        }

        /// <summary>
        /// Class for parsing <u>.str/.csf</u> file formats.<br/>
        /// Supported games: RA2, GZH, TW, KW, RA3.<br/><br/>
        /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
        /// </summary>
        public StringTable(string fileName, Encoding encoding, List<StringTableEntry> strings)
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
                  .AppendLine("END");

                if (str.ExtraValue != null)
                    sb.Append("; Extra value: ")
                      .AppendLine(str.ExtraValue);

                sb.AppendLine(string.Empty);          
            }
                
            return sb.ToString();
        }
        #endregion

        #region String table add methods
        /// <summary>
        /// Adds string sample to the string table.<br/>
        /// If string have non-ascii name, method thorws <see cref="StringTableNonAsciiNameException"/>.
        /// </summary>
        public void AddString(StringTableEntry stString)
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
        public void AddString(List<StringTableEntry> stList) => Table.AddRange(stList.Where(x => x.IsACIIName()));

        /// <summary>
        /// Transfer data from given string table.
        /// </summary>
        public void AddString(StringTable stImportTable) => Table.AddRange(stImportTable.Table);

        /// <summary>
        /// Add new entry to the string table with name but without value.
        /// </summary>
        public void AddEmptyString(string stringName) => Table.Add(new StringTableEntry(stringName));
        #endregion

        #region String table modify methods
        /// <summary>
        /// Renames string name.
        /// </summary>
        public void ChangeStringName(string oldName, string newName)
        {
            if (oldName.HasNonASCIIChars() || newName.HasNonASCIIChars()) 
                throw nonAsciiNameException;

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
        public void DeleteStringsOnMatch(StringTableEntry deleteString) => Table.RemoveAll(str => str == deleteString);

        /// <summary>
        /// Removes entries range from string table by match.
        /// </summary>
        public void DeleteStringRange(List<StringTableEntry> list) => list.ForEach(input => Table.RemoveAll(str => str == input));

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

        #region String table selection methods
        /// <summary>
        /// Get string value by value match.
        /// </summary>
        public string GetStringValue(string stringValue)
        {
            foreach (var str in Table)
                if (str.Value == stringValue)
                    return str.Value;

            return null;
        }

        /// <summary>
        /// Get string value by name match.
        /// </summary>
        public string GetStringName(string stringName)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;

            foreach (var str in Table)
                if (str.Name == stringName)
                    return str.Name;

            return null;
        }

        /// <summary>
        /// Get string by name match.
        /// </summary>
        public StringTableEntry GetString(string stringName)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;

            foreach (var str in Table)
                if (str.Name == stringName)
                    return str;

            return null;
        }

        /// <summary>
        /// Returns all string table entries with extra value.
        /// </summary>
        public List<StringTableEntry> GetStringsWithExtraValue() => Table.Where(str => str.ExtraValue != null).ToList();

        /// <summary>
        /// Returns first string table entry by name match.
        /// </summary>
        public StringTableEntry GetStringOnMatch(string stringName)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;

            StringTableEntry str = new();

            foreach (var elem in Table)
            {
                if (elem.Name == stringName)
                    return elem;
            }

            return null;
        }

        /// <summary>
        /// Returns all string table entries by name match.
        /// </summary>
        public List<StringTableEntry> GetStringsByNameOnMatch(string stringName)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;

            List<StringTableEntry> stsList = new();

            Table.Where(str => str.Name == stringName).ForEach(str => stsList.Add(str)).ToList();

            return stsList;
        }

        /// <summary>
        /// Returns all string table entries by value match.
        /// </summary>
        public List<StringTableEntry> GetStringsByValueOnMatch(string stringValue)
        {

            List<StringTableEntry> stsList = new();

            Table.Where(str => str.Value == stringValue).ForEach(str => stsList.Add(str)).ToList();

            return stsList;
        }

        /// <summary>
        /// Returns entry index by name match.
        /// </summary>
        public int GetStringIndexByName(string stringName)
        {
            if (stringName.HasNonASCIIChars())
                return -1;

            return GetStringIndex(GetString(stringName));
        }

        /// <summary>
        /// Returns list of entries indexes by name match.
        /// </summary>
        public List<int> GetStringIndexByNameOnMatch(string stringName)
        {
            if (stringName.HasNonASCIIChars())
                throw nonAsciiNameException;

            List<int> idList = new List<int>();

            Table.Where(str => str.Name == stringName).ForEach(str => GetStringIndexByName(stringName));

            return idList;
        }

        /// <summary>
        /// Returns index of entry by all field match.
        /// </summary>
        public int GetStringIndex(StringTableEntry _string) => Table.FindIndex(str => str == _string);

        /// <summary>
        /// Returns list of entries indexes by all field match.
        /// </summary>
        public List<int> GetStringIndexOnMatch(StringTableEntry _string)
        {
            List<int> idList = new List<int>();

            Table.Where(str => str == _string).ForEach(str => idList.Add(GetStringIndex(_string)));

            return idList;
        }

        /// <summary>
        /// Returns list of entries names.
        /// </summary>
        public List<string> GetStringNames()
        {
            List<string> nameList = new List<string>();
            Table.ForEach(str => nameList.Add(str.Name));
            return nameList;
        }

        /// <summary>
        /// Returns list of categories.
        /// </summary>
        public List<string> GetCategoryNames()
        {
            List<string> categoryList = new List<string>();

            Table.ForEach(str => categoryList.Add(str.Name.Split(CategorySeparator)[0]));

            return categoryList;
        }

        /// <summary>
        /// Returens list of entries in category.
        /// </summary>
        public List<string> GetStringsInCategory(string categoryName)
        {
            List<string> stringList = new List<string>();

            string searchString = categoryName + CategorySeparator;

            Table.Where(str => str.Name.StartsWith(searchString)).ForEach(str => stringList.Add(str.Name));

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
        public bool StringExist(StringTableEntry _string)
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
