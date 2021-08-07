using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    class CsfFile : StringTable, ICsfFile
    {
        #region Хедеры, лейблы, строки формата .csf
        public class CsfFileHeader
        {
            public const UInt32 CNC_CSF_VERSION = 3;
            public readonly char[] CSF = " FSC".ToCharArray();
            public readonly char[] LBL = " LBL".ToCharArray();
            public readonly char[] STR = " RTS".ToCharArray();
            public readonly char[] STRW = "WRTS".ToCharArray();

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

            public char[] csf;
            public UInt32 csfVersion;
            public UInt32 numberOfLabels;
            public UInt32 numberOfStrings;
            public UInt32 unusedBytes;
            public UInt32 languageCode;

            public CsfFileHeader()
            {
                csf = "".ToCharArray();
                csfVersion = 0;
                numberOfLabels = 0;
                numberOfStrings = 0;
                unusedBytes = 0;
                languageCode = 0;
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
                this.csf = csf;
                this.csfVersion = csfVersion;
                this.numberOfLabels = numberOfLabels;
                this.numberOfStrings = numberOfStrings;
                this.unusedBytes = unusedBytes;
                this.languageCode = languageCode;
            }

            public CsfFileHeader(CsfFileHeader header)
            {
                csf = header.csf;
                csfVersion = header.csfVersion;
                numberOfLabels = header.numberOfLabels;
                numberOfStrings = header.numberOfStrings;
                unusedBytes = header.unusedBytes;
                languageCode = header.languageCode;
            }

            public static bool operator ==(CsfFileHeader firstHeader, CsfFileHeader secondHeader)
            {
                if
                (
                       firstHeader.csf == secondHeader.csf
                    && firstHeader.csfVersion == secondHeader.csfVersion
                    && firstHeader.numberOfLabels == secondHeader.numberOfLabels
                    && firstHeader.numberOfStrings == secondHeader.numberOfStrings
                    && firstHeader.unusedBytes == secondHeader.unusedBytes
                    && firstHeader.languageCode == secondHeader.languageCode
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

        public class CsfLabelHeader
        {
        }

        public class CsfLabelValue
        {
        }
        #endregion

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
