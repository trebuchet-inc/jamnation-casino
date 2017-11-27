using System;
using System.Linq;
using System.Collections.Generic;

static class Tools 
{
 	public static bool In(this string source, params string[] list)
    {
        if (null == source) throw new ArgumentNullException("source");
        return list.Contains(source, StringComparer.OrdinalIgnoreCase);
    }
}
