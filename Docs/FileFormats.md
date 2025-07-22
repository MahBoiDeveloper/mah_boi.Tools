# File Formats



## Compiled String Format (CSF)



## Third Generation Text Format (STR)

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

After the label, the value is contained between quotation marks. The value can be split across several lines, but the game wouldn't recognise it as text on multiple lines. A new line can be inserted as `\n` in the text. If the value is empty, it should be defined as `""`. The end of the string definition can be indicated by the word `end` on the new line after the closed quotation mark.

Examples:

```ini
; Commentary

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

## Starkku's File Format (TXT)



## CnCNet's File Format


