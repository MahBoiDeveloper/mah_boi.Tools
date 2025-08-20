using System;
using System.Linq;

namespace mah_boi.Tools.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Returns next value of enum type.
    /// </summary>
    public static T Next<T>(this T src) where T : Enum
    {
        T[] values = EnumExtensions.Values<T>();
        return values[(Array.IndexOf(values, src) + 1) % values.Length];
    }

    /// <summary>
    /// Returns string of all available values in enum type.
    /// </summary>
    public static string GetNames<T>() where T : Enum
        => string.Join(", ", EnumExtensions.Values<T>().ForEach(e => e.ToString()));

    /// <summary>
    /// Returns list of available values of the specific enum type.
    /// </summary>
    public static T[] Values<T>() where T : Enum
        => (T[])Enum.GetValues(typeof(T));
}
