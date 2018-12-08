using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.CSharp;

namespace HwndHostDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            var demo0 = new TestClass();
            var property0 = demo0.GetType().GetRuntimeProperty("Test");
            //var setter0 = XXX(property0);

            //setter0();


            var demo = new SlideLayoutSaveInfo();

            var property = demo.GetType().GetProperty("Type");
            var setter = CreatePropertySetter(property);

            SaveInfo a = demo;
            int b = 0;

            SetThat(a, b);

            object c = a;
            object d = b;

            setter((object)demo, (object)SlideLayoutType.Comparision);
        }

        private static void SetThat(object target, object value)
        {
            var instance = (SlideLayoutSaveInfo) target;
            var originalValue = (double) value;
            double targetValue;

            if (value is ValueType)
            {
                // 拆箱。
                targetValue = (double) originalValue;
            }
            else
            {
                // 执行操作符转换。
                targetValue = (double) originalValue;
            }

            instance.Type = targetValue;
        }

static void SetPropertyValue(object @this, object value)
{
    （(类的类型) @this).属性名称 = (属性的类型) value;
}

        private static void IfElse(bool value)
        {
            if (value)
            {
            }
            else
            {
                Console.WriteLine("");
            }
        }

        public string Test { get; set; }

        /// <summary>
        /// 生成一个用于在完全不知道属性所属类型和属性本身类型的情况下，为属性赋值的方法。
        /// static void set_Property(object @this, object value)
        /// {
        ///     ((TargetType) @this).Property = (PropertyType) value;
        /// }
        /// </summary>
        /// <param name="propertyInfo">属性的信息。</param>
        /// <returns>一个委托，调用它可以为指定实例的属性赋值。</returns>
        public static Action<object, object> CreatePropertySetter(PropertyInfo propertyInfo)
        {
            var declaringType = propertyInfo.DeclaringType;
            if (declaringType is null)
            {
                throw new ArgumentException(@"It can not be a runtime property.", nameof(propertyInfo));
            }

            var method = new DynamicMethod("<set_Property>",
                typeof(void), new[] {typeof(object), typeof(object)});
            method.DefineParameter(1, ParameterAttributes.None, "target");
            method.DefineParameter(2, ParameterAttributes.None, "value");
            var il = method.GetILGenerator();

            il.DeclareLocal(typeof(bool));
            il.DeclareLocal(typeof(bool));
            il.DeclareLocal(typeof(bool));
            il.DeclareLocal(typeof(bool));

            var startOfElse = il.DefineLabel();
            var @return = il.DefineLabel();

            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Isinst, typeof(ValueType));
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Cgt_Un);
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Brfalse_S, startOfElse);

            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, declaringType);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
            il.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Br_S, @return);

            il.MarkLabel(startOfElse);
            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, declaringType);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
            il.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Nop);

            il.MarkLabel(@return);
            il.Emit(OpCodes.Ret);

            return (Action<object, object>) method.CreateDelegate(typeof(Action<object, object>));
        }

        public static Action<object, object> XXX(PropertyInfo propertyInfo)
        {
            if (propertyInfo.DeclaringType is null)
            {
                throw new ArgumentException(@"It can not be a runtime property.", nameof(propertyInfo));
            }

            var asmName = new AssemblyName("MyAssembly");
            var asm = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Save);
            var mod = asm.DefineDynamicModule(asmName.Name, asmName.Name + ".dll");
            var typeBuilder = mod.DefineType("MyType", TypeAttributes.Public);
            var method = typeBuilder.DefineMethod("MyMethod",
                MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard,
                typeof(void), new[] { typeof(object), typeof(object) });

            //var method = new DynamicMethod("<set_Property>",
            //    typeof(void), new[] { typeof(object), typeof(object) });
            method.DefineParameter(1, ParameterAttributes.None, "target");
            method.DefineParameter(2, ParameterAttributes.None, "value");
            var il = method.GetILGenerator();

            il.DeclareLocal(typeof(bool));

            var startOfElse = il.DefineLabel();
            var @return = il.DefineLabel();

            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, typeof(object).GetMethod(nameof(GetType)));
            il.Emit(OpCodes.Callvirt, typeof(Type).GetProperty(nameof(Type.IsValueType)).GetGetMethod());
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Brfalse_S, startOfElse);

            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
            il.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Br_S, @return);

            il.MarkLabel(startOfElse);
            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
            il.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Nop);

            il.MarkLabel(@return);
            il.Emit(OpCodes.Ret);
            typeBuilder.CreateType();

            asm.Save("MyAssembly.dll");

            return (Action<object, object>)method.CreateDelegate(typeof(Action<object, object>));
        }
    }

    public class TestClass
    {
        public int Test { get; set; }

        public void Test1(string x)
        {
        }
    }

    public abstract class SaveInfo
    {
        public IList<SaveInfo> Extensions { get; set; } = (IList<SaveInfo>)new List<SaveInfo>();
    }

    public class SlideLayoutSaveInfo : SaveInfo
    {
        public double Type { get; set; }
    }

    public enum SlideLayoutType
    {
        Blank,
        TitleOnly,
        TitleSlide,
        TitleAndContent,
        ContentOnly,
        Comparision
    }
}
