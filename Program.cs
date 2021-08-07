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
            // Код для тестов
            //string path = @"..\..\..\DataSamples\gamestrings.csf";
            string path = @"..\..\..\DataSamples\ra3_original_gamestrings.csf";
            if (File.Exists(path)) Console.WriteLine("File exists? " + File.Exists(path));

            CsfFile csf = new CsfFile(path);
        }
    }
}
