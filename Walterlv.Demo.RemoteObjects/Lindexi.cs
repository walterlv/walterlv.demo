using System;

namespace Walterlv.Demo.RemoteObjects
{
    [Serializable]
    public class Lindexi : MarshalByRefObject
    {
        public string OrderDinner(string cases)
        {
            Console.WriteLine($"点餐：{cases}");
            return $"点餐：{cases}";
        }
    }
}
