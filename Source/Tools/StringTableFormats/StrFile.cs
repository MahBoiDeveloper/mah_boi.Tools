using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using mah_boi.Tools.Extensions;
using mah_boi.Tools.StringTableFormats.Exceptions;

namespace mah_boi.Tools.StringTableFormats;

/// <summary>
/// Class for parsing <u>.str</u> file format.<br/>
/// Supported games: GZH, TW, KW, RA3.<br/><br/>
/// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
/// </summary>
public class StrFile : StringTable
{
    private enum LineType
    {
        Label = 0,
        Value = 1,
        End = 2
    }

    #region Contsructors
    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile() : base()
    {
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(string fileName) : base(fileName)
    {
        Parse();
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(string fileName, Encoding encoding) : base(fileName, encoding)
    {
        Parse();
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(StrFile strFile) : base(strFile)
    {
        this.Format = strFile.Format;
        this.CategorySeparator = strFile.CategorySeparator;
        this.FileEncoding = strFile.FileEncoding;
        this.FileName = strFile.FileName;
        this.Table = strFile.Table;
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(string fileName, List<StringTableEntry> stStringsList) : base(fileName, stStringsList)
    {
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(string fileName, Encoding encoding, List<StringTableEntry> stStringsList) : base(fileName, encoding, stStringsList)
    {
    }

    /// <summary>
    /// Class for parsing <u>.str</u> file format.<br/>
    /// Supported games: GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://generals.projectperfectmod.com/genstr/">here</see>.<br/>
    /// </summary>
    public StrFile(StringTable stSample) : base(stSample)
    {
    }
    #endregion

    #region Parsing
    public override void Parse()
    {
        string stringName   = string.Empty;
        string stringValue  = string.Empty;

        LineType searchStatus = LineType.Label;

        // For each cycle through all lines in text file.
        uint currentLineNumber = 0;
        foreach (var currentLine in new StreamReader(FileName, FileEncoding).ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
        {
            currentLineNumber++;

            // skip line if it is a commentary or empty line
            if 
            (
                currentLine.StartsWith("//") 
                || currentLine.StartsWith(";") 
                || currentLine.Trim() == string.Empty
            )
                continue;

            // if read string has an error (multiple value)
            if
            (
                searchStatus == LineType.End 
                && !currentLine.Trim().Equals("end", StringComparison.InvariantCultureIgnoreCase)
            )
            {
                parsingErrorsAndWarnings.AddMessage($"Error in an entry formating ({currentLineNumber}): \"{currentLine}\" | "
                                                   + "After a label doesn't follow a value", StringTableParseException.MessageType.Error);
                searchStatus = LineType.Label;
            }

            // read entry contains error, because it hasn't value
            else if (currentLine.Trim().ToLower() == "end" && searchStatus == LineType.Value)
            {
                parsingErrorsAndWarnings.AddMessage($"Error in an entry formating ({currentLineNumber}): \"{currentLine}\" | "
                                                  + $"After the label follows closing \"{currentLine.Trim()}\", but not the value."
                                                  +  "Use quotation marks (\"\") to define an empty string", StringTableParseException.MessageType.Error);

                searchStatus = LineType.Label;
            }

            // read string is a label aka entry name
            else if
            (
                searchStatus == LineType.Label          // analyzed string is a label
                && !currentLine.Trim().StartsWith("\"") // analyzed string is not a value
            )
            {
                if (currentLine.IsACII())
                {
                    stringName   = currentLine.Trim();
                    searchStatus = LineType.Value;
                }
                else
                {
                    parsingErrorsAndWarnings.AddMessage($"Error in an entry formating ({currentLineNumber}): \"{currentLine}\" | "
                                                       + "Entry name (label) has non ASCII symbols, that is not allowed. "
                                                       + "Replace them to make file to be able parsed be the program.",
                                                        StringTableParseException.MessageType.Error);
                }
            }

            // read string is a value
            else if
            (
                searchStatus == LineType.Value         // analyzed string is a value
                && currentLine.Trim().StartsWith("\"") // starts with "
                && currentLine.Trim().EndsWith("\"")   // ends with "
            )
            {
                stringValue  = currentLine.Trim().Replace("\"", string.Empty);
                searchStatus = LineType.End;
            }

            // read entry has a splitted value on the several lines
            else if
            (
                searchStatus == LineType.Value         // analyzed string is a value
                && currentLine.Trim().StartsWith("\"") // starts with "
                && !currentLine.Trim().EndsWith("\"")  // and not ends with "
            )
            {
                stringValue += currentLine.Trim().Replace("\"", string.Empty);
            }

            else if
            (
                searchStatus == LineType.Value          // analyzed string is a value
                && currentLine.Trim().StartsWith("\\n") // value splitted by \n that placed in the start
                && !currentLine.Trim().EndsWith("\"")   // and doesn't have " in the end of string
            )
            {
                stringValue += currentLine.Trim();
            }

            else if
            (
                searchStatus == LineType.Value          // analyzed string is a value
                && currentLine.Trim().StartsWith("\\n") // value splitted by \n that placed in the start
                && currentLine.Trim().EndsWith("\"")    // and ends with "
            )
            {
                stringValue += currentLine.Trim().Replace("\"", string.Empty);
                searchStatus = LineType.End;
            }

            else if
            (
                searchStatus == LineType.Value           // analyzed string is a value
                && !currentLine.Trim().StartsWith("\\n") // and doesn't contain \n
            )
            {
                parsingErrorsAndWarnings.AddMessage($"Error in an entry formating ({currentLineNumber}): \"{currentLine}\" | "
                                                   + "Unable to find \"\\n\" symblos in the begining of the string.", StringTableParseException.MessageType.Error);
            }

            // read string is an end of the entry
            else if (searchStatus == LineType.End && currentLine.Trim().ToLower() == "end")
            {
                Table.Add(new StringTableEntry(stringName, stringValue));
                searchStatus = (int)LineType.Label;
            }

            // unknown errors
            else
            {
                parsingErrorsAndWarnings.AddMessage($"Error in an entry formating ({currentLineNumber}): \"{currentLine}\" | "
                                                   + "Unknown error", StringTableParseException.MessageType.Error);
            }
        }
    }

    /// <summary>
    /// Save .str file format data to the source file.
    /// </summary>
    public override void Save()
    {
        using (StreamWriter sw = new(File.OpenWrite(FileName), FileEncoding))
            sw.WriteLine(ToString());
    }
    #endregion

    #region Other methods

    public static bool operator == (StrFile a, StrFile b) => (StringTable)a == (StringTable)b;

    public static bool operator != (StrFile a, StrFile b) => !(a == b);

    public override bool Equals(object obj) => (StrFile)obj == this;

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => base.ToString();
    #endregion
}
