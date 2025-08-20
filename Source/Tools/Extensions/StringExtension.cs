using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mah_boi.Tools.Extensions;

public static class StringExtension
{
    private static readonly Regex extractLinksRE = new Regex(@"((http[s]?)|([s]?ftp))\S+");
    private static readonly char[] forbiddenChars = ['/', '\\', ':', '*', '?', '<', '>', '|'];
    private static readonly HashSet<string> reservedFileNames = new(new List<string>()
    {
        "CON",
        "PRN",
        "AUX",
        "NUL",
        "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM¹", "COM²", "COM³",
        "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9", "LPT¹", "LPT²", "LPT³"
    }, StringComparer.InvariantCultureIgnoreCase);

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

    /// <summary>
    /// Converts string to enum value.
    /// </summary>
    public static T ToEnum<T>(this string value) where T : Enum
        => (T)Enum.Parse(typeof(T), value, true);

    /// <summary>
    /// Checks if filename could be used as WIN32API filename.
    /// </summary>
    /// <param name="defaultValue">The default string value.</param>
    /// <returns>True, if filename doesn't contain any reserved characters and their combinations.</returns>
    /// <remarks>
    /// Reference: <a href="https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file">Naming Files, Paths, and Namespaces</a>.
    /// </remarks>
    public static bool IsWin32FileName(this string filename)
    {
        foreach (var ch in forbiddenChars)
        {
            if (filename.Contains(ch))
                return false;
        }

        foreach (var fn in reservedFileNames)
        {
            if (filename.Contains(fn))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Replace special characters with spaces in the filename to avoid conflicts with WIN32API.
    /// </summary>
    /// <param name="defaultValue">The default string value.</param>
    /// <returns>File name without special characters or reserved combinations.</returns>
    /// <remarks>
    /// Reference: <a href="https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file">Naming Files, Paths, and Namespaces</a>.
    /// </remarks>
    public static string ToWin32FileName(this string filename)
    {
        forbiddenChars.ForEach(ch => filename.Replace(ch, '_'));

        // If the user is somehow using "con" or any other filename that is
        // reserved by WIN32API, it would be better to rename it.
        filename = reservedFileNames.Contains(filename) ? "_" : filename;

        return filename;
    }

    /// <summary>
    /// Returns array of links, contained in string.
    /// </summary>
    public static string[] GetLinks(this string text)
    {
        string[] empty = { };

        if (string.IsNullOrWhiteSpace(text))
            return empty;

        var matches = extractLinksRE.Matches(text);

        if (matches.Count == 0)
            return empty; // No link found

        string[] links = new string[matches.Count];
        for (int i = 0; i < links.Length; i++)
            links[i] = matches[i].Value.Trim();

        return links;
    }
}
