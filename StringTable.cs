using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    abstract class StringTable
    {
        public const string NOCATEGORYSTRINGS = ".NOCATEGORYSTRINGS";

        public string FileName { get; set; }

        public virtual void Parse()
        {
        }
    }
}
