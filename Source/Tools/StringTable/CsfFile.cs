using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using mah_boi.Tools.StringTable.Exceptions;

namespace mah_boi.Tools.StringTable
{
    /// <summary>
    /// Class for parsing <u>.csf</u> file format.<br/>
    /// Supported games: RA2YR, GZH, TW, KW, RA3.<br/><br/>
    /// Read more about string table format <see href="https://modenc2.markjfox.net/CSF_File_Format">here</see>.<br/>
    /// </summary>
    // 
    
    public class CsfFile : StringTable
    {
        private static readonly char[] FSC  = {' ', 'F', 'S', 'C' }; // begin of the CSF file
        private static readonly char[] LBL  = {' ', 'L', 'B', 'L' }; // begin of the label
        private static readonly char[] RTS  = {' ', 'R', 'T', 'S' }; // begin of the value
        private static readonly char[] WRTS = {'W', 'R', 'T', 'S' }; // begin of the value with extra value
        private static UInt32 CNC_CSF_VERSION = 3;                   // file format version for C&C games

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
                char[] labelName = br.ReadChars((int)labelNameLength);

                byte[] stringValue = FileEncoding.GetBytes(string.Empty);
                char[] extraStringValue = string.Empty.ToCharArray();

                if (countOfStrings != 0)
                {
                    // read label value
                    char[] rtsOrWrts = br.ReadChars(4); // ' RTS' - hasn't extra value. 'WRTS' - has extra value
                    UInt32 valueLength = br.ReadUInt32();
                    stringValue = br.ReadBytes(Convert.ToInt32(valueLength * 2)); // data should be inverted

                    InvertAllBytesInArray(stringValue);

                    // чтение дополнительного значения лейбла
                    if (new string(rtsOrWrts) == new string(WRTS))
                    {
                        UInt32 extraValueLength = br.ReadUInt32();                          // length of extra value
                        extraStringValue = br.ReadChars(Convert.ToInt32(extraValueLength)); // extra value (always in Unicode encoding)
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
        ///     Сохранение текущего .csf файла согласно общему стандарту.
        /// </summary>
        public override void Save()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
                uint countOfLables = Convert.ToUInt32(Count());

                // записываем хедер файла
                bw.Write(FSC);                                                // ' FSC'
                bw.Write(CNC_CSF_VERSION);                                    // Версия формата
                bw.Write(countOfLables);                                      // Количество строк
                bw.Write(countOfLables + Convert.ToUInt32(ExtraTable.Count)); // Количество значений
                bw.Write((uint)0);                                          // ХЗ-байты
                bw.Write((uint)0);                                          // Код языка

                // записываем построчно значения из строковой таблицы в файл
                uint labelCounter = 0;
                do
                {
                    // запись нормальных строк
                    foreach (var str in Table)
                    {
                        string labelName = str.Name;

                        bw.Write(LBL);                     // строка со значением ' LBL'
                        bw.Write((uint)1);                 // количество строк для дополнительного значения
                        bw.Write(labelName.Length);        // длина названия
                        bw.Write(labelName.ToCharArray()); // само название

                        bw.Write(RTS);  // строка со значением ' RTS'
                        bw.Write(Convert.ToUInt32(str.Value.Length));        // запись длины значения
                        byte[] byteValue = FileEncoding.GetBytes(str.Value); // получения байтового массива на основе строки
                        InvertAllBytesInArray(byteValue);                          // инвертирование полученного массива

                        bw.Write(byteValue);                                       // запись в файл инвертированных байтов значения строки

                        labelCounter++; // т.к. мы прошли лейбл, то мы обязаны увеличить счётчик на 1
                    }

                    // запись дополнительных строк
                    foreach (var str in ExtraTable)
                    {
                        string labelName = str.StringName;

                        bw.Write(LBL);                     // строка со значением ' LBL'
                        bw.Write((uint)1);                 // количество строк для дополнительного значения
                        bw.Write(labelName.Length);        // длина названия
                        bw.Write(labelName.ToCharArray()); // само название

                        if (str.StringExtraValue == string.Empty)
                            bw.Write(RTS);  // строка со значением ' RTS'
                        else
                            bw.Write(WRTS); // строка со значением 'WRTS'

                        bw.Write(Convert.ToUInt32(str.StringValue.Length));        // запись длины значения

                        byte[] byteValue = FileEncoding.GetBytes(str.StringValue); // получения байтового массива на основе строки
                        InvertAllBytesInArray(byteValue);                          // инвертирование полученного массива

                        bw.Write(byteValue);                                       // запись в файл инвертированных байтов значения строки

                        if (str.StringExtraValue != string.Empty)
                        {
                            bw.Write(Convert.ToUInt32(str.StringExtraValue.Length));
                            bw.Write(str.StringExtraValue.ToCharArray());
                        }
                        labelCounter++;
                    }
                } while (labelCounter < countOfLables);
            }
        }

        /// <summary>
        ///     Сохранение текущего .csf файла согласно общему стандарту по указанному пути.
        /// </summary>
        public override void SaveAs(string fileName)
        {
            string tmp = FileName;
            FileName = fileName;
            Save();
            FileName = tmp;
        }
        #endregion

        #region Конверторы
        /// <summary>
        ///     Конвертор из <u>.csf</u> в <u>.str</u> из текущего отпарсенного файла.
        /// </summary>
        public StrFile ToStr()
        {
            if (!StringTable.IsConvertableTo((StringTable)this, StringTableFormat.str))
                throw new StringTableParseException("Указанный экземпляр .csf файла не конвертируем в формат .str");

            // в str нет символов переводы на новую строку заменяются на \n
            List<StringTableEntry> tmp = Table;
            foreach (var str in tmp)
                str.Value = str.Value.Replace("\n", "\\n");

            return new StrFile(FileName, tmp, ExtraTable);
        }

        /// <summary>
        ///     Безопасный конвертор .csf файла в .str файл.
        /// </summary>
        public bool Safe_ToStr(out StrFile returnParam)
        {
            try
            {
                returnParam = ToStr();
            }
            catch (StringTableParseException)
            {
                returnParam = new StrFile();
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Конвертор из <u>.csf</u> в <u>.str</u> на основе указанного отпарсенного файла fileSample.
        /// </summary>
        public static StrFile ToStr(CsfFile fileSample)
        {
            if (!fileSample.IsConvertable())
                throw new StringTableParseException("Указанный экземпляр .csf файла не конвертируем в формат.str");

            // в str нет символов переводы на новую строку заменяются на \n
            List<StringTableEntry> tmp = fileSample.Table;
            foreach (var str in tmp)
                str.Value = str.Value.Replace("\n", "\\n");

            return new StrFile(fileSample.FileName, tmp, fileSample.ExtraTable);
        }

        /// <summary>
        ///     Безопасный конвертор .csf файла в .str файл.
        /// </summary>
        public static bool Safe_ToStr(CsfFile fileSample, out StrFile returnParam)
        {
            try
            {
                returnParam = fileSample.ToStr();
            }
            catch (StringTableParseException)
            {
                returnParam = new StrFile();
                return false;
            }
            return true;
        }
        #endregion

        #region Вспомогательные методы
        /// <summary>
        ///     Проверка конвертируемости текущего формата строковой таблицы в другой в <u>.str</u>.
        /// </summary>
        public override bool IsConvertable()
            =>
                StringTable.IsConvertableTo((StringTable)this, StringTableFormat.str);

        public override bool IsConvertable(List<StringTableEntry> strings)
            =>
                StringTable.IsConvertableTo((StringTable)new StrFile(string.Empty, strings), StringTableFormat.csf);

        /// <summary>
        ///     Согласно описанию формата <u>.csf</u> на <a href="https://modenc.renegadeprojects.com/CSF_File_Format">modenc</a>, 
        ///     все байты строки-значения необходимо<br/>
        ///     инвертировать перед тем, как записывать данные в переменные.<br/>
        ///     Отдельное спасибо человеку с никнеймом <b>pd</b> за подсказку.
        /// </summary>
        private void InvertAllBytesInArray(byte[] array)
        {
            if (array == null) return;

            for (int i = 0; i < array.Length; i++)
                array[i] = (byte)~array[i];
        }

        public static bool operator ==(CsfFile firstFile, CsfFile secondFile)
            =>
                (StringTable)firstFile == (StringTable)secondFile;

        public static bool operator !=(CsfFile firstFile, CsfFile secondFile)
            =>
                !(firstFile == secondFile);

        public override bool Equals(object obj)
            =>
                (CsfFile)obj == this;

        public override int GetHashCode()
            =>
                base.GetHashCode();

        /// <summary>
        ///     Метод формирует строку, равносильную .str/.csf файлу.
        /// </summary>
        public override string ToString()
            =>
                base.ToString();
        #endregion
    }
}
