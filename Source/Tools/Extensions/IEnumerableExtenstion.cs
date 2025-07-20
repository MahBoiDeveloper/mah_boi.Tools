using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mah_boi.Tools.Extensions;

public static class IEnumerableExtenstion
{
    /// <summary>
    /// Apply action changes for all elements of collection container.
    /// </summary>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T element in source)
            action(element);

        return source;
    }
}
