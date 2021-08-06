using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace mah_boi.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            
            CsfHeader header = new CsfHeader();

            // Код для тестов
            //string path = @"..\..\..\DataSamples\gamestrings.csf";
            string path = @"..\..\..\DataSamples\ra3_original_gamestrings.csf";
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
                        header.numberOfLabels = br.ReadUInt32();
                        header.numberOfStrings = br.ReadUInt32();
                        header.unusedBytes = br.ReadUInt32();
                        header.languageCode = br.ReadUInt32();
                //}
                //}

            }
            Console.WriteLine(header.csf);
            Console.WriteLine(header.csfVersion);
            Console.WriteLine(header.numberOfLabels);
            Console.WriteLine(header.numberOfStrings);
            Console.WriteLine(header.unusedBytes);
            Console.WriteLine(header.languageCode);

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
