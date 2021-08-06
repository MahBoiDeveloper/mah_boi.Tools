using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace mah_boi.Tools
{
    class Program
    {
        struct CsfHeader
        {
            public char[] csf;
            public UInt32 csfVersion;
            public UInt32 numLabels;
            public UInt32 numStrings;
            public UInt32 unused;
            public UInt32 language;
        }

        static void Main(string[] args)
        {
            
            CsfHeader header;

            header.csf = "".ToArray();
            header.csfVersion = 0;
            header.numLabels = 0;
            header.numStrings = 0;
            header.unused = 0;
            header.language = 0;

            // Код для тестов
            //string path = @"..\..\..\gamestrings.csf";
            string path = @"..\..\..\ra3_original_gamestrings.csf";
            if (File.Exists(path)) Console.WriteLine("File exist? " + File.Exists(path));
            using(BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open)))
            {

                //int i = 0;
                //while(br.PeekChar() > -1)
                //{
                //    i++;
                //    if(i == 0)
                //    {
                        header.csf = br.ReadChars(4);
                        header.csfVersion = br.ReadUInt32();
                        header.numLabels = br.ReadUInt32();
                        header.numStrings = br.ReadUInt32();
                        header.unused = br.ReadUInt32();
                        header.language = br.ReadUInt32();
                //}
                //}

            }
            Console.WriteLine(header.csf);
            Console.WriteLine(header.csfVersion);
            Console.WriteLine(header.numLabels);
            Console.WriteLine(header.numStrings);
            Console.WriteLine(header.unused);
            Console.WriteLine(header.language);

            #region
            /*
            string tmp = @"..\..\..\map.str";
            new StrFile(tmp).Save(@"..\..\..\_tmp.str");
            Console.WriteLine(new StrFile(tmp));
            */
            #endregion
        }
    }
}
