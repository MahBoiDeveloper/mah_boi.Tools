using System;
using System.Collections.Generic;
using System.Text;

namespace mah_boi.Tools
{
    abstract class StringTable : IStringTable
    {
        public const string NOCATEGORYSTRINGS = ".NOCATEGORYSTRINGS";

        public virtual string FileName { get; set; }

        public abstract void Parse();

        public abstract void Save();
    }
}
