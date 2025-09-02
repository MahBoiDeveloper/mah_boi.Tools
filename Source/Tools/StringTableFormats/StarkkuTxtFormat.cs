using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace mah_boi.Tools.StringTableFormats;

public class StarkkuTxtFormat : StringTable
{
    #region Contsructors
    /// <summary>
    /// Class for parsing <u>.txt</u> string table file format.<br/>
    /// Read more about string table format <see href="https://github.com/Starkku/CSFTool">here</see>.<br/>
    /// </summary>
    public StarkkuTxtFormat() : base()
    {
    }

    /// <summary>
    /// Class for parsing <u>.txt</u> string table file format.<br/>
    /// Read more about string table format <see href="https://github.com/Starkku/CSFTool">here</see>.<br/>
    /// </summary>
    public StarkkuTxtFormat(string fileName) : base(fileName)
    {
        Parse();
    }

    /// <summary>
    /// Class for parsing <u>.txt</u> string table file format.<br/>
    /// Read more about string table format <see href="https://github.com/Starkku/CSFTool">here</see>.<br/>
    /// </summary>
    public StarkkuTxtFormat(string fileName, Encoding encoding) : base(fileName, encoding)
    {
        Parse();
    }

    /// <summary>
    /// Class for parsing <u>.txt</u> string table file format.<br/>
    /// Read more about string table format <see href="https://github.com/Starkku/CSFTool">here</see>.<br/>
    /// </summary>
    public StarkkuTxtFormat(StarkkuTxtFormat strFile) : base(strFile)
    {
        this.Format = strFile.Format;
        this.CategorySeparator = strFile.CategorySeparator;
        this.FileEncoding = strFile.FileEncoding;
        this.FileName = strFile.FileName;
        this.Table = strFile.Table;
    }

    /// <summary>
    /// Class for parsing <u>.txt</u> string table file format.<br/>
    /// Read more about string table format <see href="https://github.com/Starkku/CSFTool">here</see>.<br/>
    /// </summary>
    public StarkkuTxtFormat(string fileName, List<StringTableEntry> stStringsList) : base(fileName, stStringsList)
    {
    }

    /// <summary>
    /// Class for parsing <u>.txt</u> string table file format.<br/>
    /// Read more about string table format <see href="https://github.com/Starkku/CSFTool">here</see>.<br/>
    /// </summary>
    public StarkkuTxtFormat(string fileName, Encoding encoding, List<StringTableEntry> stStringsList) : base(fileName, encoding, stStringsList)
    {
    }

    /// <summary>
    /// Class for parsing <u>.txt</u> string table file format.<br/>
    /// Read more about string table format <see href="https://github.com/Starkku/CSFTool">here</see>.<br/>
    /// </summary>
    public StarkkuTxtFormat(StringTable stSample) : base(stSample)
    {
    }
    #endregion

    #region Parsing
    public override void Parse()
    {
        foreach (var line in new StreamReader(FileName, FileEncoding).ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
        {
            var split = line.Split('|');
            Add(split[0], split[1].Replace("\\n", "\n"));
        }
    }

    public override void Save()
    {
        StringBuilder sb = new();

        Table.ForEach(entry => sb.Append(entry.Name).Append('|').AppendLine(entry.Value.Replace("\n", "\\n")));

        using (StreamWriter sw = new(File.OpenWrite(FileName), FileEncoding))
            sw.WriteLine(sb.ToString());
    }
    #endregion
}
