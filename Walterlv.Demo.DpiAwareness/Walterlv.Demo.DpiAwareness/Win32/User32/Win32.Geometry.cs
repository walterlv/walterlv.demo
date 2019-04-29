using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Walterlv.Windows.Native
{
    public static partial class Win32
    {
        /// <summary>
        /// 在 Win32 函数使用的矩形
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public partial struct Rectangle : IEquatable<Rectangle>
        {
            /// <summary>
            ///  创建在 Win32 函数使用的矩形
            /// </summary>
            /// <param name="left"></param>
            /// <param name="top"></param>
            /// <param name="right"></param>
            /// <param name="bottom"></param>
            public Rectangle(int left = 0, int top = 0, int right = 0, int bottom = 0)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            /// <summary>
            /// 创建在 Win32 函数使用的矩形
            /// </summary>
            /// <param name="width">矩形的宽度</param>
            /// <param name="height">矩形的高度</param>
            public Rectangle(int width = 0, int height = 0) : this(0, 0, width, height) { }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public bool Equals(Rectangle other)
            {
                return (Left == other.Left) && (Right == other.Right) && (Top == other.Top) && (Bottom == other.Bottom);
            }

            public override bool Equals(object obj)
            {
                return obj is Rectangle && Equals((Rectangle)obj);
            }

            public static bool operator ==(Rectangle left, Rectangle right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Rectangle left, Rectangle right)
            {
                return !(left == right);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (int)Left;
                    hashCode = (hashCode * 397) ^ (int)Top;
                    hashCode = (hashCode * 397) ^ (int)Right;
                    hashCode = (hashCode * 397) ^ (int)Bottom;
                    return hashCode;
                }
            }

            public Size Size
            {
                get { return new Size(Width, Height); }
                set
                {
                    Width = value.Width;
                    Height = value.Height;
                }
            }

            /// <summary>
            /// 获取当前矩形是否空矩形
            /// </summary>
            public bool IsEmpty => this.Left == 0 && this.Top == 0 && this.Right == 0 && this.Bottom == 0;
            /// <summary>
            /// 矩形的宽度
            /// </summary>
            public int Width { get { return unchecked((int)(Right - Left)); } set { Right = unchecked((int)(Left + value)); } }
            /// <summary>
            /// 矩形的高度
            /// </summary>
            public int Height { get { return unchecked((int)(Bottom - Top)); } set { Bottom = unchecked((int)(Top + value)); } }

            /// <summary>
            /// 通过 x、y 坐标和宽度高度创建矩形
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <returns></returns>
            public static Rectangle Create(int x, int y, int width, int height)
            {
                unchecked
                {
                    return new Rectangle(x, y, (int)(width + x), (int)(height + y));
                }
            }

            public override string ToString()
            {
                var culture = CultureInfo.CurrentCulture;
                return $"{{ Left = {Left.ToString(culture)}, Top = {Top.ToString(culture)} , Right = {Right.ToString(culture)}, Bottom = {Bottom.ToString(culture)} }}, {{ Width: {Width.ToString(culture)}, Height: {Height.ToString(culture)} }}";
            }

            public static Rectangle From(ref Rectangle lvalue, ref Rectangle rvalue,
                Func<int, int, int> leftTopOperation,
                Func<int, int, int> rightBottomOperation = null)
            {
                if (rightBottomOperation == null)
                    rightBottomOperation = leftTopOperation;
                return new Rectangle(
                    leftTopOperation(lvalue.Left, rvalue.Left),
                    leftTopOperation(lvalue.Top, rvalue.Top),
                    rightBottomOperation(lvalue.Right, rvalue.Right),
                    rightBottomOperation(lvalue.Bottom, rvalue.Bottom)
                );
            }

            public void Add(Rectangle value)
            {
                Add(ref this, ref value);
            }

            public void Subtract(Rectangle value)
            {
                Subtract(ref this, ref value);
            }

            public void Multiply(Rectangle value)
            {
                Multiply(ref this, ref value);
            }

            public void Divide(Rectangle value)
            {
                Divide(ref this, ref value);
            }

            public void Deflate(Rectangle value)
            {
                Deflate(ref this, ref value);
            }

            public void Inflate(Rectangle value)
            {
                Inflate(ref this, ref value);
            }

            public void Offset(int x, int y)
            {
                Offset(ref this, x, y);
            }

            public void OffsetTo(int x, int y)
            {
                OffsetTo(ref this, x, y);
            }

            public void Scale(int x, int y)
            {
                Scale(ref this, x, y);
            }

            public void ScaleTo(int x, int y)
            {
                ScaleTo(ref this, x, y);
            }

            public static void Add(ref Rectangle lvalue, ref Rectangle rvalue)
            {
                lvalue.Left += rvalue.Left;
                lvalue.Top += rvalue.Top;
                lvalue.Right += rvalue.Right;
                lvalue.Bottom += rvalue.Bottom;
            }

            public static void Subtract(ref Rectangle lvalue, ref Rectangle rvalue)
            {
                lvalue.Left -= rvalue.Left;
                lvalue.Top -= rvalue.Top;
                lvalue.Right -= rvalue.Right;
                lvalue.Bottom -= rvalue.Bottom;
            }

            public static void Multiply(ref Rectangle lvalue, ref Rectangle rvalue)
            {
                lvalue.Left *= rvalue.Left;
                lvalue.Top *= rvalue.Top;
                lvalue.Right *= rvalue.Right;
                lvalue.Bottom *= rvalue.Bottom;
            }

            public static void Divide(ref Rectangle lvalue, ref Rectangle rvalue)
            {
                lvalue.Left /= rvalue.Left;
                lvalue.Top /= rvalue.Top;
                lvalue.Right /= rvalue.Right;
                lvalue.Bottom /= rvalue.Bottom;
            }

            public static void Deflate(ref Rectangle target, ref Rectangle deflation)
            {
                target.Top += deflation.Top;
                target.Left += deflation.Left;
                target.Bottom -= deflation.Bottom;
                target.Right -= deflation.Right;
            }

            public static void Inflate(ref Rectangle target, ref Rectangle inflation)
            {
                target.Top -= inflation.Top;
                target.Left -= inflation.Left;
                target.Bottom += inflation.Bottom;
                target.Right += inflation.Right;
            }

            public static void Offset(ref Rectangle target, int x, int y)
            {
                target.Top += y;
                target.Left += x;
                target.Bottom += y;
                target.Right += x;
            }

            public static void OffsetTo(ref Rectangle target, int x, int y)
            {
                var width = target.Width;
                var height = target.Height;
                target.Left = x;
                target.Top = y;
                target.Right = width;
                target.Bottom = height;
            }

            public static void Scale(ref Rectangle target, int x, int y)
            {
                target.Top *= y;
                target.Left *= x;
                target.Bottom *= y;
                target.Right *= x;
            }

            public static void ScaleTo(ref Rectangle target, int x, int y)
            {
                unchecked
                {
                    x = (int)(target.Left / x);
                    y = (int)(target.Top / y);
                }
                Scale(ref target, x, y);
            }

        }
        
        /// <summary>
        /// 在 Win32 函数使用的 Point 类，使用 int 表示数据
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public partial struct Point : IEquatable<Point>
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public bool Equals(Point other)
            {
                return (X == other.X) && (Y == other.Y);
            }

            public override bool Equals(object obj)
            {
                return obj is Point && Equals((Point)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                { return ((int)X * 397) ^ (int)Y; }
            }

            public int X, Y;

            public static bool operator ==(Point left, Point right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Point left, Point right)
            {
                return !(left == right);
            }

            public void Offset(int x, int y)
            {
                X += x;
                Y += y;
            }

            public void Set(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override string ToString()
            {
                var culture = CultureInfo.CurrentCulture;
                return $"{{ X = {X.ToString(culture)}, Y = {Y.ToString(culture)} }}";
            }

            public bool IsEmpty => this.X == 0 && this.Y == 0;
        }
        
        /// <summary>
        /// 在 Win32 函数使用的 Size 类，使用 int 表示数据
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public partial struct Size : IEquatable<Size>
        {

            public Size(int width, int height)
            {
                this.Width = width;
                this.Height = height;
            }

            public bool Equals(Size other)
            {
                return (this.Width == other.Width) && (this.Height == other.Height);
            }

            public override bool Equals(object obj)
            {
                return obj is Size && this.Equals((Size)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                { return ((int)this.Width * 397) ^ (int)this.Height; }
            }

            public int Width;
            public int Height;

            public static bool operator ==(Size left, Size right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Size left, Size right)
            {
                return !(left == right);
            }

            public void Offset(int width, int height)
            {
                Width += width;
                Height += height;
            }

            public void Set(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public override string ToString()
            {
                var culture = CultureInfo.CurrentCulture;
                return $"{{ Width = {Width.ToString(culture)}, Height = {Height.ToString(culture)} }}";
            }

            public bool IsEmpty => this.Width == 0 && this.Height == 0;
        }
    }
}