using System.Linq;

namespace Walterlv.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var instance = new 类的类型();
            SetPropertyValue(instance, 5);

            var propertyInfo = typeof(类的类型).GetProperties().First();
            QuickEmit.OutputPropertySetter(propertyInfo);
            var setValue = QuickEmit.CreatePropertySetter(propertyInfo);
            setValue(instance, 5);
        }

        static void SetPropertyValue(object @this, object value)
        {
            ((类的类型) @this).TempProperty = (double) value;
        }
    }

    public class 类的类型
    {
        public double TempProperty { get; set; }
    }
}
