using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rampastring.Tools;

namespace mah_boi.Tools.Benchmarks;

public class ConversionsExtension : Conversions
{
    private readonly char[] default_separators = [','];

    public override object ValueFromString(string str, Type type)
        => type.Name switch
        {
            "String[]" => str.Split(default_separators),
            _ => base.ValueFromString(str, type)
        };
}
