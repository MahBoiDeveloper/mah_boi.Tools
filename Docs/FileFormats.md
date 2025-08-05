# File Formats

Document intended to describe string table file formats commonly used by C&C community.

## Compiled String Format (CSF)

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
uint32  -- Count of values for label. If value more than 2, it may be a corrupt in file.
uint32  -- Length of the string byte array.
char*   -- Label name as byte array.
```

Value data ordering:

```

```

Порядок данных в лейбле:
char[4] -- " LBL" - строка, без прочтения которой игра пойдёт искать следующие 4 байта на их наличие.
           Эта идея с чтением используется при ориентировке на пустых строках.
UInt32  -- количество значений лейбла. Почти всегда равно 1. Может быть равно 0. Код ниже никак не 
           учитывает вариант, когда больше 1, что является скорее намеренной порчей файла, нежели
           правильной записью строки в формате. 
           Пример строки с 0 значений из generals.csf: TOOLTIP:InvalidGameVersion
UInt32  -- длина названия лейбла в символах.
     v
     v
     v
char[ ] -- название лейбла. Значение всегда в ASCII. Пробелов нет. Строка не оканчивается \0 терминатором.

Порядок данных в значении:
char[4] -- " RTS" или "WRTS". Если "WRTS", то имеется дополнительное значение и движок их считывает, но в игре
           они нигде не используются. 
           Пример строки с дополнительным значением из generals.csf: DIALOGEVENT:MisGLA02Chatter18Subtitle
UInt32  -- длина значения лейбла в символах. 
     v
    x2
     v
byte[ ] -- значение лейбла в виде байтов. Длина массива в 2 раза больше количества символов, т.к. по умолчанию 
           подразумевается, что они записываются в кодировке Unicode. Все байты значения инвертированы, поэтому
           необходимо предварительно их инвертировать, чтобы конвертирвать в нормальную строку.

Опциональное.

UInt32  -- длина дополнительного значения в символах.
     v
     v
     v
char[ ] -- дополнительное значение лейбла. Строка не оканчивается \0 терминатором.
           Само по себе использование дополнительных значений было замечено ТОЛЬКО в оригинальном generals.csf.
           И значение нигде в игре не используется, так что можно считать, что это такой же рудимент, как и код языка.

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

## Starkku's File Format (TXT)



## CnCNet's File Format


