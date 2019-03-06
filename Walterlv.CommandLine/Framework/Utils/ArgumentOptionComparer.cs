using System;
using System.Collections.Generic;

namespace Walterlv.Framework.Utils
{
    internal class ArgumentOptionComparer : IEqualityComparer<string>
    {
        internal static readonly IEqualityComparer<string> Instance = new ArgumentOptionComparer();

        bool IEqualityComparer<string>.Equals(string x, string y)
            => string.Equals(x, y, StringComparison.InvariantCulture);

        int IEqualityComparer<string>.GetHashCode(string option)
            => option?.GetHashCode() ?? 0;
    }
}