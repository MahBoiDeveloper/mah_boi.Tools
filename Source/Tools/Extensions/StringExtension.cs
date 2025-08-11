using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mah_boi.Tools.Extensions;

public static class StringExtension
{
    /// <summary>
    /// Checks if string contains only ASCII characters (symbols should id less than 127).
    /// </summary>
    public static bool IsACII(this string str) => Encoding.UTF8.GetByteCount(str) == str.Length;

    /// <summary>
    /// Checks if string has any non ASCII characters.
    /// </summary>
    public static bool HasNonASCIIChars(this string str) => Encoding.UTF8.GetByteCount(str) != str.Length;

    /// <summary>
    /// Indicates whether the specified string is <see href="null" /> or an empty string ("").
    /// </summary>
    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
}
