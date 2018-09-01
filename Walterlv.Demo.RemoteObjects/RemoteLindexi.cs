using System;
using System.Threading.Tasks;

namespace Walterlv.Demo.RemoteObjects
{
    public interface ILindexi
    {
        string OrderDinner(string cases);
        event EventHandler CaseOrdered;
    }

    [Serializable]
    public class NativeLindexi : MarshalByRefObject, ILindexi
    {
        private RemoteLindexi _lindexi;
        public EventHandler CaseOrderedHandler;

        public NativeLindexi(RemoteLindexi lindexi)
        {
            _lindexi = lindexi;
            CaseOrderedHandler = Lindexi_CaseOrdered;
            lindexi.CaseOrdered += CaseOrderedHandler;
        }

        public void Lindexi_CaseOrdered(object sender, EventArgs e)
        {
            CaseOrdered?.Invoke(sender, e);
        }

        public string OrderDinner(string cases)
        {
            return _lindexi.OrderDinner(cases);
        }

        public event EventHandler CaseOrdered;
    }

    [Serializable]
    public class RemoteLindexi : MarshalByRefObject, ILindexi
    {
        public string OrderDinner(string cases)
        {
            Console.WriteLine($"点餐：{cases}");
            //Task.Run(async () =>
            //{
            //    await Task.Delay(1000);
            //    CaseOrdered?.Invoke(null, null);
            //});
            return $"点餐：{cases}";
        }

        public event EventHandler CaseOrdered;
    }
}
