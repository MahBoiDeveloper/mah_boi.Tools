using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rampastring.Tools;
using mah_boi.Tools.Extensions;

namespace mah_boi.Tools.StringTableFormats;

public class StringTableIniFile : StringTable
{
    #region Constructors
    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public StringTableIniFile() : base()
    {
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public StringTableIniFile(string fileName) : base(fileName)
    {
        Parse();
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public StringTableIniFile(string fileName, Encoding encoding) : base(fileName, encoding)
    {
        Parse();
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public StringTableIniFile(StringTableIniFile csfFile) : base(csfFile)
    {
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public StringTableIniFile(string fileName, List<StringTableEntry> strings) : base(fileName, strings)
    {
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public StringTableIniFile(string fileName, Encoding encoding, List<StringTableEntry> strings) : base(fileName, encoding, strings)
    {
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public StringTableIniFile(StringTable stSample) : base(stSample)
    {
    }
    #endregion

    #region Parsing
    /// <summary>
    /// Parses .ini file as string table.
    /// </summary>
    public override void Parse()
    {
        IniFile ini = new(FileName);
        foreach (var sectionName in ini.GetSections())
        {
            var section = ini.GetSection(sectionName);
            section.Keys.ForEach(kvp => AddString($"{section.SectionName}:{kvp.Key}", kvp.Value.Replace("@", "\n").Replace("\\semicolon", ";")));
        }
    }

    /// <summary>
    /// Save string table data into .txt file.
    /// </summary>
    public override void Save()
    {
        IniFile ini = new();

        foreach (var entry in Table)
        {
            var split = entry.Name.Split(':');
            
            if (!ini.SectionExists(split[0]))
                ini.AddSection(split[0]);

            StringBuilder sb = new();
            split.Where(x => x != split.First()).ForEach(x => sb.Append(x));
            ini.SetStringValue(split[0], sb.ToString(), entry.Value.Replace("\n", "@").Replace(";", "\\semicolon"));
        }

        ini.WriteIniFile(FileName);
    }
    #endregion

    #region Other methods
    public static bool operator == (StringTableIniFile firstFile, StringTableIniFile secondFile) => (StringTable)firstFile == (StringTable)secondFile;
    public static bool operator != (StringTableIniFile firstFile, StringTableIniFile secondFile) => !(firstFile == secondFile);
    public override bool Equals(object obj) => (StringTableIniFile)obj == this;
    public override int GetHashCode() => base.GetHashCode();
    public override string ToString() => base.ToString();
    #endregion
}
