using System.Runtime.InteropServices;

namespace Walterlv.Windows.Native
{
    public static partial class Win32
    {
        internal static class Properties
        {
#if !ANSI
            public const CharSet BuildCharSet = CharSet.Unicode;
#else
            public const CharSet BuildCharSet = CharSet.Ansi;
#endif
        }
    }
}