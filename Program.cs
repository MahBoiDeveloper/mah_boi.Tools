using System;
using System.Collections.Generic;
using System.Linq;

namespace mah_boi.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            //string tmp = @"C:\D-Drive\_Downloads\map.str";
            
            string tmp = @"C:\D-Drive\_Github\mah_boi.Tools\mod.str";
            new StrFile(tmp).Save(@"C:\D-Drive\_Github\mah_boi.Tools\_tmp.str");
            Console.WriteLine(new StrFile(tmp));
        }
    }
}
