# File Formats

Document intended to different file formats used in the mah_boi's Tools.

## String Table

Description of string table file formats commonly used by C&C community.

### Compiled String Format (CSF)

> [!IMPORTANT]
> Reference: https://modenc2.markjfox.net/CSF_File_Format

The original binary file format for the in-game string table is actually initial format developed by Westwood Studios for Red Alert 2 game.

All `.csf` file format have header, labels, values. Header contains information for the further file parsing.

Header data ordering:

```
char[4] -- " FSC" string (reversed CSF). The beggining of the file header.
uint32  -- File format version. For NOX game equals 2, other C&C games are equal to 3.
           Metadata field, difference between 2 and 3 formats in unknown.
uint32  -- File's labels count.
uint32  -- File's values count. If values > labels, then file contains string(s) with extra values.
           Zero is not an error.
uint32  -- Unknown bytes. Could be used for metadata.
uint32  -- Language code. Check CsfLanguageCode.cs file for details about what code equals what country.
```

After the header follows labels with value data.

Label data ordering:

```
char[4] -- " LBL" string. If game haven't found string, it search string in the next 4 bytes.
uint32  -- Count of values for label. If value more than 2, it may be a corrupts in the file.
uint32  -- Length of the string byte array.
char*   -- Label name as byte array. Name is ASCII string without \0 terminator on the end.
```

Value data ordering:

```
char[4] -- " RTS" or "WRTS" string. "WRTS" means that the string has extra value. You can find example of this string in generals.csf --> DIALOGEVENT:MisGLA02Chatter18Subtitle
uint32  -- Length of the string value array.
uint16* -- String value represented as 2 byte array. All array contain values in inverted mode. Has Unicode encoding.
uint32  -- Length of the string value array. All array contain values in inverted mode. Exists only if it is a extra string.
uint16* -- Extra string value represented as 2 byte array. Alsmost all C&C games doesn't use extra value. Exists only if it is a extra string.
```

### Third Generation Text Format (STR)

> [!IMPORTANT]
> Reference: https://generals.projectperfectmod.com/genstr/

The text file format for the in-game string table was initially developed by WW/EA for C&C: Generals, and has since been used in all modern C&C games, including Tiberium Wars, Kane's Wrath and Red Alert 3. The `.str` file is actually a modified C&C Generals `.ini` file format that does not allow the use of a string table with entries that have an extra value or a name containing non-ASCII characters.

The string name can be repeated (case is not tested in-game, so be aware of bugs) and is case insensitive.

Entry name structure example:

```
                     Name (Label)
                       v
       *---------------v----------------*
       |HOTKEYNAME:SIDEBARWATERCRAFTPAGE|
       *----^-----^-----------^---------*
            ^     ^           ^
        Category  ^           ^
                  ^    String in category
             Separator
```

After the label, the value is contained between quotation marks. The value can be split across several lines, but the game wouldn't recognise it as text on multiple lines. A new line can be inserted as `\n` in the text. If the value is empty, it should be defined as `""`. The end of the string definition can be indicated by the word `end` on the new line after the closed quotation mark. In addition to `;` comments from `.ini` file format ancestor, `.str` file format allows C-like commentaries that starts with `//` characters.

Examples:

```ini
; Commentary
// Another commentary

SCRIPT:EXAMPLE
	"TEXT SAMPLE"
END

SCRIPT:EXAMPLE
  "TEXT 
  SAMPLE"
END

Text sample
"Value that would be displayed.
\n As long text.
\n With text on 3 lines total."
End

Script:example
""
end
```

### Starkku's File Format (TXT)



### CnCNet's File Format (INI)


