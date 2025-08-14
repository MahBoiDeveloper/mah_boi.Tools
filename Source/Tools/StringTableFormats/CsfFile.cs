using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using mah_boi.Tools.Extensions;
using mah_boi.Tools.StringTableFormats.Exceptions;

namespace mah_boi.Tools.StringTableFormats;

/// <summary>
/// Class for parsing <u>.csf</u> file format.<br/>
/// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
/// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
/// </summary>
public class CsfFile : StringTable
{
    private static readonly char[] FSC  = {' ', 'F', 'S', 'C' }; // begin of the CSF file
    private static readonly char[] LBL  = {' ', 'L', 'B', 'L' }; // begin of the label
    private static readonly char[] RTS  = {' ', 'R', 'T', 'S' }; // begin of the value
    private static readonly char[] WRTS = {'W', 'R', 'T', 'S' }; // begin of the value with extra value

    private CsfFileHeader Header = new();

    #region Constructors
    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public CsfFile() : base()
    {
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public CsfFile(string fileName) : base(fileName)
    {
        Parse();
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public CsfFile(string fileName, Encoding encoding) : base(fileName, encoding)
    {
        Parse();
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public CsfFile(CsfFile csfFile) : base(csfFile)
    {
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public CsfFile(string fileName, List<StringTableEntry> strings) : base(fileName, strings)
    {
    }

    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    public CsfFile(string fileName, Encoding encoding, List<StringTableEntry> strings) : base(fileName, encoding, strings)
    {
    }
    #endregion

    #region Parsing
    /// <summary>
    /// Parse .csf file.
    /// </summary>
    public override void Parse()
    {
        using (BinaryReader br = new BinaryReader(File.Open(FileName, FileMode.Open)))
        {
            ParseHeader(br);
            ParseBody(br);
        }
    }

    /// <summary>
    /// Parsing header of .csf file.
    /// </summary>
    public void ParseHeader(BinaryReader br)
    {
        // read the header, that defines count of entries in the string table
        Header.CSFchars = br.ReadChars(4); // first 4 bytes must be " FSC"

        // read 4 chars until gets " FSC" string
        for (;;)
        {
            if (new string(Header.CSFchars) == new string(FSC))
                break;

            Header.CSFchars = br.ReadChars(4);
        }
        

        Header.FormatVersion = br.ReadUInt32(); // format version should be equal 3

        if (Header.FormatVersion != 3)
            parsingErrorsAndWarnings.AddMessage($"File format version doesn't used commonly (parsed value: {Header.FormatVersion}) "
                                              + "by C&C games, be aware of breaking this file.", StringTableParseException.MessageType.Warning);

        Header.NumberOfLabels = br.ReadUInt32();  // label count
        Header.NumberOfStrings = br.ReadUInt32(); // value count

        if (Header.NumberOfLabels < Header.NumberOfStrings)
            parsingErrorsAndWarnings.AddMessage("File contains strings with extra values. "
                                              + "Converting it to other string table formats would be only without extra values! "
                                              + "It is recommended to delete extra values from strings as depreceted feature."
                                              , StringTableParseException.MessageType.Warning);

        Header.UnknownBytes = br.ReadUInt32(); // unknown bytes
        Header.LanguageCode = (CsfLanguageCode)br.ReadUInt32(); // language code (see more in CsfLanguageCode)
    }

    /// <summary>
    /// Parsing the body of the .csf file.
    /// </summary>
    public void ParseBody(BinaryReader br)
    {
        // read until the end of file or error
        for (UInt32 i = 0; i < Header.NumberOfLabels || br.PeekChar() > -1; i++)
        {
            // read label
            char[] lbl = br.ReadChars(4); // should be ' LBL'

            // if not ' LBL', read next 4 bytes
            if (new string(lbl) != new string(LBL))
            {
                if (i > 0) i--;
                continue;
            }

            UInt32 countOfStrings = br.ReadUInt32();
            UInt32 labelNameLength = br.ReadUInt32();
            char[] labelName = br.ReadChars(labelNameLength);

            byte[] stringValue = FileEncoding.GetBytes(string.Empty);
            char[] extraStringValue = string.Empty.ToCharArray();

            if (countOfStrings != 0)
            {
                // read string's label and value
                char[] rtsOrWrts = br.ReadChars(4); // ' RTS' - hasn't extra value. 'WRTS' - has extra value
                UInt32 valueLength = br.ReadUInt32();
                stringValue = br.ReadBytes(Convert.ToInt32(valueLength * 2)); // data should be inverted

                stringValue.ForEach(x => x = (byte)~x); // inverting all bytes

                // read string's extra value
                if (new string(rtsOrWrts) == new string(WRTS))
                {
                    UInt32 extraValueLength = br.ReadUInt32();         // length of extra value
                    extraStringValue = br.ReadChars(extraValueLength); // extra value (always in Unicode encoding)
                }

                Table.Add(new StringTableEntry(new string(labelName), new string(FileEncoding.GetChars(stringValue)), new string(extraStringValue)));
            }
            else
            {
                Table.Add(new StringTableEntry(new string(labelName), null, null));
            }
        }
    }

    /// <summary>
    /// Save string table data into .csf file.
    /// </summary>
    public override void Save()
    {
        using (BinaryWriter bw = new BinaryWriter(File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
        {
            // write file header
            bw.Write(Header.CSFchars);
            bw.Write(Header.FormatVersion);        // format version
            bw.Write(Count());                     // label's count
            bw.Write(Count());                     // value's count
            bw.Write(Header.UnknownBytes);         // unknown bytes
            bw.Write((UInt32)Header.LanguageCode); // language code

            // write entries one by one
            foreach (var str in Table)
            {
                string labelName = str.Name;

                bw.Write(LBL);
                bw.Write((UInt32)1);                      // values count
                bw.Write(labelName.ToCharArray().Length); // label length
                bw.Write(labelName.ToCharArray());        // label name

                if (str.ExtraValue.IsNullOrEmpty())
                    bw.Write(RTS);
                else
                    bw.Write(WRTS);

                bw.Write(Convert.ToUInt32(str.Value.Length));        // get length of the value
                byte[] byteValue = FileEncoding.GetBytes(str.Value); // get bytes of the value
                byteValue.ForEach(x => x = (byte)~x);                // inverting the array
                bw.Write(byteValue);                                 // write inverted array

                if (str.ExtraValue.IsNullOrEmpty())
                {
                    bw.Write(Convert.ToUInt32(str.ExtraValue.Length));
                    bw.Write(str.ExtraValue.ToCharArray());
                }
            }
        }
    }

    /// <summary>
    /// Save data to the specific file by path.
    /// </summary>
    public override void SaveAs(string fileName)
    {
        string tmp = FileName;
        FileName = fileName;
        Save();
        FileName = tmp;
    }
    #endregion

    #region Other methods
    public static bool operator == (CsfFile firstFile, CsfFile secondFile) => (StringTable)firstFile == (StringTable)secondFile;
    public static bool operator != (CsfFile firstFile, CsfFile secondFile) => !(firstFile == secondFile);
    public override bool Equals(object obj) => (CsfFile)obj == this;
    public override int GetHashCode() => base.GetHashCode();
    public override string ToString() => base.ToString();
    #endregion
}
