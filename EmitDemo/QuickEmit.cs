using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Walterlv.Demo
{
    public static class QuickEmit
    {
        public static Action<object, object> CreatePropertySetter(PropertyInfo propertyInfo)
        {
            var declaringType = propertyInfo.DeclaringType;
            var propertyType = propertyInfo.PropertyType;

            // 创建一个动态方法，参数依次为方法名、返回值类型、参数类型。
            // 对应着 IL 中的
            // .method private hidebysig static void
            //     SetPropertyValue(
            //     ) cil managed
            var method = new DynamicMethod("<set_Property>", typeof(void), new[] {typeof(object), typeof(object)});
            var il = method.GetILGenerator();

            // 定义形参。注意参数位置从 1 开始——即使现在在写静态方法。
            // 对应着 IL 中的
            //     object this,
            //     object 'value'
            method.DefineParameter(1, ParameterAttributes.None, "this");
            method.DefineParameter(2, ParameterAttributes.None, "value");

            // 用 Emit 生成 IL 代码。
            // 对应着 IL 中的各种操作符。
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, declaringType);
            il.Emit(OpCodes.Ldarg_1);
            var castingCode = propertyInfo.PropertyType.IsValueType
                ? OpCodes.Unbox_Any
                : OpCodes.Castclass;
            il.Emit(castingCode, propertyType);
            il.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);

            // 为生成的动态方法创建调用委托，返回返回这个委托。
            return (Action<object, object>) method.CreateDelegate(typeof(Action<object, object>));
        }

        public static void OutputPropertySetter(PropertyInfo propertyInfo)
        {
            var declaringType = propertyInfo.DeclaringType;
            var propertyType = propertyInfo.PropertyType;

            var assemblyName = new AssemblyName("Temp");
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save);
            var module = assembly.DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".dll");
            var type = module.DefineType("Temp", TypeAttributes.Public);
            var method = type.DefineMethod("<set_Property>",
                MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard,
                typeof(void), new[] { typeof(object), typeof(object) });
            var il = method.GetILGenerator();
            
            method.DefineParameter(1, ParameterAttributes.None, "this");
            method.DefineParameter(2, ParameterAttributes.None, "value");

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, declaringType);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Castclass, propertyType);
            il.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);

            // 为生成的动态方法创建调用委托，返回返回这个委托。
            type.CreateType();
            assembly.Save($"{assemblyName.Name}.dll");
        }
    }
}
