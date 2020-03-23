using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Walterlv.Demo.Windows.Native;

namespace Walterlv.Demo.Windows
{
    public class WindowAccentCompositor
    {
        public static void DisableDwm(IntPtr handle)
        {
            var value1 = DWMNCRENDERINGPOLICY.DWMNCRP_ENABLED;
            var errorCode1 = DwmSetWindowAttribute(
                handle,
                DWMWINDOWATTRIBUTE.DWMWA_NCRENDERING_POLICY,
                in value1,
                sizeof(DWMNCRENDERINGPOLICY));
            if (errorCode1 != 0)
            {
                throw new Win32Exception(errorCode1);
            }

            var value2 = true;
            var errorCode2 = DwmSetWindowAttribute(
                handle,
                DWMWINDOWATTRIBUTE.DWMWA_ALLOW_NCPAINT,
                in value2,
                sizeof(DWMNCRENDERINGPOLICY));
            if (errorCode2 != 0)
            {
                throw new Win32Exception(errorCode2);
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            in bool pvAttribute,
            uint cbAttribute);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            in DWMNCRENDERINGPOLICY pvAttribute,
            uint cbAttribute);
    }

    namespace Native
    {
        internal enum DWMWINDOWATTRIBUTE
        {
            /// <summary>
            /// Use with DwmGetWindowAttribute.
            /// Discovers whether non-client rendering is enabled.
            /// The retrieved value is of type BOOL. TRUE if non-client rendering is enabled; otherwise, FALSE.
            /// </summary>
            DWMWA_NCRENDERING_ENABLED = 1,

            /// <summary>
            /// Use with DwmSetWindowAttribute. Sets the non-client rendering policy.
            /// The pvAttribute parameter points to a value from the DWMNCRENDERINGPOLICY enumeration.
            /// </summary>
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        };

        internal enum DwmBooleanValue
        {
            Disable,
            Enable,
        }

        internal enum DWMNCRENDERINGPOLICY
        {
            /// <summary>
            /// The non-client rendering area is rendered based on the window style.
            /// </summary>
            DWMNCRP_USEWINDOWSTYLE,

            /// <summary>
            /// The non-client area rendering is disabled; the window style is ignored.
            /// </summary>
            DWMNCRP_DISABLED,

            /// <summary>
            /// The non-client area rendering is enabled; the window style is ignored.
            /// </summary>
            DWMNCRP_ENABLED,

            /// <summary>
            /// The maximum recognized DWMNCRENDERINGPOLICY value, used for validation purposes.
            /// </summary>
            DWMNCRP_LAST
        };
    }
}
