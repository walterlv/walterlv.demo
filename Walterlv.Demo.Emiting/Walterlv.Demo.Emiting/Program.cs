using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Walterlv.Demo.Emiting
{
    class Program
    {
        static void Main(string[] args)
        {
            var func = GenerateIL();
            func(new BusinessBase());
        }

        private static Func<object, string> GenerateIL()
        {
            var asmName = new AssemblyName("iPoint.DynBus.Libary");
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName
                , AssemblyBuilderAccess.RunAndSave);//创建程序集
            var mdlBldr = asmBuilder.DefineDynamicModule("iPoint.DynBus.Libary", "Test.dll");//定义模块, 命名空间
            var typeBldr = mdlBldr.DefineType("Invoice", TypeAttributes.Public
                | TypeAttributes.Serializable);//定义类 


            //ConstructorInfo ctor = TypeBuilder.GetConstructor(typeOfBusinessBase
            //    , typeof(iWork.Library.CslaBaseTypes.BusinessBase<>).GetConstructor(new Type[] { }));

            //定义 GetIdValue 方法

            var method = typeBldr.DefineMethod("GetIdValue"
                , MethodAttributes.Family | MethodAttributes.HideBySig
                | MethodAttributes.Virtual);
            method.SetReturnType(typeof(object));
            ILGenerator methodIL = method.GetILGenerator();
            methodIL.Emit(OpCodes.Nop);
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldstr, "ID");
            var indexerDictionary = typeof(BusinessBase);
            methodIL.Emit(OpCodes.Call, indexerDictionary.GetMethod("get_Item", new Type[] { typeof(string) }));
            methodIL.Emit(OpCodes.Stloc_0);
            methodIL.Emit(OpCodes.Br_S, "IL_000f");
            methodIL.Emit(OpCodes.Ldloc_0);
            methodIL.Emit(OpCodes.Ret);

            return (Func<object, string>)method.CreateDelegate(typeof(Func<object, string>));
        }
    }
}
