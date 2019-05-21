using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Walterlv.InputMethodInteraction
{
    /// <summary>
    /// 为控件提供输入法的支持
    /// </summary>
    public sealed class ImeSupport
    {
        private IntPtr _defaultImeWnd;
        private IntPtr _currentContext;
        private IntPtr _previousContext;
        private HwndSource _hwndSource;
        private readonly FrameworkElement _textArea;
        private bool _isUpdatingCompositionWindow;

        /// <summary>
        /// 构造一个IME支持对象，传入需要支持IME的控件即可
        /// </summary>
        /// <param name="textArea"></param>
        public ImeSupport(FrameworkElement textArea)
        {
            _textArea = textArea ?? throw new ArgumentNullException(nameof(textArea));
            _textArea.GotKeyboardFocus += (s, e) =>
            {
                if (_textArea.IsKeyboardFocused)
                {
                    if (_hwndSource != null)
                        return;
                    _isUpdatingCompositionWindow = true;
                    ClearContext();
                    CreateContext();
                }
                else
                {
                    ClearContext();
                    return;
                }
                UpdateCompositionWindow();
                _isUpdatingCompositionWindow = false;
            };

            _textArea.LostKeyboardFocus += (s, e) =>
            {
                if (_isUpdatingCompositionWindow)
                    return;
                if (Equals(e.OldFocus, _textArea) && _currentContext != IntPtr.Zero)
                {
                    ImeNative.ImmNotifyIME(_currentContext, ImeNative.NI_COMPOSITIONSTR, ImeNative.CPS_CANCEL);
                }
                ClearContext();
            };
            InputMethod.SetIsInputMethodSuspended(_textArea, true);
        }

        /// <summary>
        /// 更新CompositionWindow位置
        /// </summary>
        public void UpdateCompositionWindow()
        {
            if (_currentContext == IntPtr.Zero)
                return;
            //上面判断微软拼音的方法，会导致方法被切片，从而在快速得到焦点和失去焦点时，失去焦点清理的代码会先于此函数执行，导致引发错误
            if (_hwndSource == null)
                return;
            SetCompositionFont(_hwndSource, _currentContext, _textArea);
            SetCompositionWindow(_hwndSource, _currentContext, _textArea);
        }

        private static void SetCompositionWindow(HwndSource source, IntPtr hIMC, FrameworkElement textArea)
        {
            if (textArea == null)
                throw new ArgumentNullException(nameof(textArea));
            var textViewBounds = new Rect(16, 100, 20, 40); // ImeHelper.GetBounds(textArea, source);
            var characterBounds = new Rect(16, 200, 20, 40); // textArea.DocumentManager.CaretInfo.Bounds;
            characterBounds = textArea.TransformToAncestor(source.RootVisual).TransformBounds(characterBounds);
            //解决surface上输入法光标位置不正确
            //现象是surface上光标的位置需要乘以2才能正确，普通电脑上没有这个问题
            //且此问题与DPI无关，目前用CaretWidth可以有效判断
            var caretLeftTop = new Point(characterBounds.TopLeft.X / SystemParameters.CaretWidth,
                characterBounds.TopLeft.Y / SystemParameters.CaretWidth);

            const int CFS_DEFAULT = 0x0000;
            const int CFS_RECT = 0x0001;
            const int CFS_POINT = 0x0002;
            const int CFS_FORCE_POSITION = 0x0020;
            const int CFS_EXCLUDE = 0x0080;
            const int CFS_CANDIDATEPOS = 0x0040;

            var form = new ImeNative.CompositionForm();
            form.dwStyle = CFS_POINT;
            form.ptCurrentPos.x = (int)Math.Max(caretLeftTop.X, textViewBounds.Left);
            form.ptCurrentPos.y = (int)Math.Max(caretLeftTop.Y, textViewBounds.Top);
            form.ptCurrentPos.y += (int)characterBounds.Height;

            ImeNative.ImmSetCompositionWindow(hIMC, ref form);
        }

        private static void SetCompositionFont(HwndSource source, IntPtr hIMC, FrameworkElement textArea)
        {
            if (textArea == null)
                throw new ArgumentNullException(nameof(textArea));
            var lf = new ImeNative.LOGFONT();
            var characterBounds = new Rect(16, 200, 20, 40); // textArea.DocumentManager.CaretInfo.Bounds;
            lf.lfFaceName = "Microsoft YaHei";
            lf.lfHeight = (int)characterBounds.Height;

            var GCS_COMPSTR = 8;

            var length = ImeNative.ImmGetCompositionString(hIMC, GCS_COMPSTR, null, 0);
            if (length > 0)
            {
                var target = new byte[length];
                var count = ImeNative.ImmGetCompositionString(hIMC, GCS_COMPSTR, target, length);
                if (count > 0)
                {
                    var inputString = Encoding.Default.GetString(target);
                    if (string.IsNullOrWhiteSpace(inputString))
                    {
                        lf.lfWidth = 1;
                    }
                }
            }

            ImeNative.ImmSetCompositionFont(hIMC, ref lf);
        }

        private void ClearContext()
        {
            if (_hwndSource == null)
                return;
            ImeNative.ImmAssociateContext(_hwndSource.Handle, _previousContext);
            ImeNative.ImmReleaseContext(_defaultImeWnd, _currentContext);
            _currentContext = IntPtr.Zero;
            _defaultImeWnd = IntPtr.Zero;
            _hwndSource.RemoveHook(WndProc);
            _hwndSource = null;
        }

        private void CreateContext()
        {
            _hwndSource = (HwndSource)PresentationSource.FromVisual(_textArea);
            if (_hwndSource == null)
                return;
            _defaultImeWnd = ImeNative.ImmGetDefaultIMEWnd(IntPtr.Zero);
            _currentContext = ImeNative.ImmGetContext(_defaultImeWnd);
            _previousContext = ImeNative.ImmAssociateContext(_hwndSource.Handle, _currentContext);
            _hwndSource.AddHook(WndProc);

            //尽管文档说传递null是无效的，但这似乎有助于在与WPF共享的默认输入上下文中激活IME
            var threadMgr = ImeNative.GetTextFrameworkThreadManager();
            threadMgr?.SetFocus(IntPtr.Zero);
        }

        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (msg)
            {
                case ImeNative.WM_INPUTLANGCHANGE:
                    if (_hwndSource != null)
                    {
                        ClearContext();
                        CreateContext();
                    }

                    break;
                case ImeNative.WM_IME_COMPOSITION:
                    UpdateCompositionWindow();
                    break;
            }

            return IntPtr.Zero;
        }
    }
}