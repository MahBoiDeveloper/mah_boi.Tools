using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    class CsfFile : StringTable, ICsfFile
    {
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
