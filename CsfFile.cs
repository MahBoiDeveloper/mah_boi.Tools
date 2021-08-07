using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    class CsfFile : StringTable, ICsfFile
    {
        #region Хедеры, лейблы и строки формата .csf
        private class CsfFileHeader
        {
            public const UInt32 CNC_CSF_VERSION = 3;
            public readonly char[] CSF_REVERSED = " FSC".ToCharArray();

            public enum LanguagesCodes : UInt32
            {
                US = 0,
                UK = 1,
                German = 2,
                French = 3,
                Spanish = 4,
                Italian = 5,
                Japanese = 6,
                Jabberwockie = 7,
                Korean = 8,
                Chinese = 9,
                Unknown = 10
            };

            public char[] Csf { get; set; }
            public UInt32 CsfVersion { get; set; }
            public UInt32 NumberOfLabels { get; set; }
            public UInt32 NumberOfStrings { get; set; }
            public UInt32 UnusedBytes { get; set; }
            public UInt32 LanguageCode { get; set; }

            public CsfFileHeader()
            {
                Csf = "".ToCharArray();
                CsfVersion = 0;
                NumberOfLabels = 0;
                NumberOfStrings = 0;
                UnusedBytes = 0;
                LanguageCode = 0;
            }

            public CsfFileHeader
                (
                    char[] csf,
                    UInt32 csfVersion,
                    UInt32 numberOfLabels,
                    UInt32 numberOfStrings,
                    UInt32 unusedBytes,
                    UInt32 languageCode
                )
            {
                Csf             = csf;
                CsfVersion      = csfVersion;
                NumberOfLabels  = numberOfLabels;
                NumberOfStrings = numberOfStrings;
                UnusedBytes     = unusedBytes;
                LanguageCode    = languageCode;
            }

            public CsfFileHeader(CsfFileHeader header)
            {
                Csf             = header.Csf;
                CsfVersion      = header.CsfVersion;
                NumberOfLabels  = header.NumberOfLabels;
                NumberOfStrings = header.NumberOfStrings;
                UnusedBytes     = header.UnusedBytes;
                LanguageCode    = header.LanguageCode;
            }

            public static bool operator ==(CsfFileHeader firstHeader, CsfFileHeader secondHeader)
            {
                if
                (
                       firstHeader.Csf             == secondHeader.Csf
                    && firstHeader.CsfVersion      == secondHeader.CsfVersion
                    && firstHeader.NumberOfLabels  == secondHeader.NumberOfLabels
                    && firstHeader.NumberOfStrings == secondHeader.NumberOfStrings
                    && firstHeader.UnusedBytes     == secondHeader.UnusedBytes
                    && firstHeader.LanguageCode    == secondHeader.LanguageCode
                )
                    return true;

                return false;
            }

            public static bool operator !=(CsfFileHeader firstHeader, CsfFileHeader secondHeader)
                =>
                    !(firstHeader == secondHeader);

            public override bool Equals(object header)
                =>
                    (CsfFileHeader)header == this;

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        private class CsfLabelHeader
        {
            public readonly char[] LBL = " LBL".ToCharArray();

            public char[] Lbl { get; }
            public UInt32 NumberOfStringPairs { get; }
            public UInt32 LabelNameLength { get; }
            public char[] LabelName { get; }

            public CsfLabelHeader()
            {
                Lbl                 = "".ToCharArray();
                NumberOfStringPairs = 0;
                LabelNameLength     = 0;
                LabelName           = "".ToCharArray();
            }

            public CsfLabelHeader
                (
                    char[] lbl,
                    UInt32 numberOfStringPairs,
                    UInt32 labelNameLength,
                    char[] labelName
                )
            {
                Lbl                 = lbl;
                NumberOfStringPairs = numberOfStringPairs;
                LabelNameLength     = labelNameLength;
                LabelName           = labelName;
            }

            public CsfLabelHeader(CsfLabelHeader csfLabelHeader)
            {
                Lbl                 = csfLabelHeader.Lbl;
                NumberOfStringPairs = csfLabelHeader.NumberOfStringPairs;
                LabelNameLength     = csfLabelHeader.LabelNameLength;
                LabelName           = csfLabelHeader.LabelName;
            }
        }

        private class CsfLabelValue
        {
            public readonly char[] STR_REVERSED  = " RTS".ToCharArray();
            public readonly char[] STRW_REVERSED = "WRTS".ToCharArray();
            public UInt32 ValueLength { get; }
            public byte[] Value { get; }
            public UInt32 ExtraValueLength { get; }
            public char[] ExtraValue { get; }
        }
        #endregion

        private CsfFileHeader Header { get; set; }

        #region Конструкторы
        /// <summary>
        ///     Класс для парсинга <u>.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public CsfFile(string fileName) : base(fileName)
        {
            Parse();
        }

        /// <summary>
        ///     Класс для парсинга <u>.csf</u> файлов<br/>
        ///     Поддерживаются форматы игр: GZH, TW, KW, RA3.<br/><br/>
        ///     Подробнее про CSF/STR форматы <see href="https://modenc.renegadeprojects.com/CSF_File_Format">здесь</see><br/>
        ///     Подробнее про особенности парсинга 
        ///     <see href="https://github.com/MahBoiDeveloper/mah_boi.Tools/blob/main/StrFile.cs#L17">здесь</see>
        /// </summary>
        public CsfFile(CsfFile csfFile) : base(csfFile)
        {
        }
        #endregion

        #region Парсинг
        public override void Parse()
        {
            using (BinaryReader br = new BinaryReader(File.Open(FileName, FileMode.Open)))
            {
                Header = new CsfFileHeader();
                // читаем заголовок файла, который нам укажет количество считываний далее
                Header.Csf             = br.ReadChars(4); // по факту эта строка обязана всегда быть " FSC"
                Header.CsfVersion      = br.ReadUInt32(); // у ЦНЦ игр это число всегда равно 3
                Header.NumberOfLabels  = br.ReadUInt32(); // количество лейблов
                Header.NumberOfStrings = br.ReadUInt32(); // количество строк
                Header.UnusedBytes     = br.ReadUInt32(); // никто не знает, что это за байты, и никто их не использует
                Header.LanguageCode    = br.ReadUInt32(); // код языка (подробнее в CsfFileHeader.LanguagesCodes)

                // временная печать в консоль, чтобы увидеть баги
                Console.Write("Строка со словом ' FSC' = [");
                Console.Write(Header.Csf);
                Console.WriteLine(']');
                Console.WriteLine("Версия формата          = " + Header.CsfVersion);
                Console.WriteLine("Число лейблов           = " + Header.NumberOfLabels);
                Console.WriteLine("Число строк-значений    = " + Header.NumberOfStrings);
                Console.WriteLine("Неиспользованные байты  = " + Header.UnusedBytes);
                Console.WriteLine("Код языка               = " + Header.LanguageCode);
            }
        }

        public override void Save()
        {
        }

        public override void Save(string fileName)
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

        #region Конверторы

        public StrFile ToStr()
        {
            return null;
        }
        
        public StrFile ToStr(string fileName)
        {
            return null;
        }
        
        public StrFile ToStr(CsfFile fileSample)
        {
            return null;
        }
        #endregion

        #region Вспомогательные методы
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
        #endregion
    }
}
