using System;
using System.Collections.Generic;
using System.Linq;

namespace mah_boi.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            // Код для тестов
            string tmp = @"..\..\..\map.str";
            new StrFile(tmp).Save(@"..\..\..\_tmp.str");
            Console.WriteLine(new StrFile(tmp));
        }
    }
}
