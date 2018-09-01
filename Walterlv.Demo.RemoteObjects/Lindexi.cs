using System;
using System.Threading.Tasks;

namespace Walterlv.Demo.RemoteObjects
{
    [Serializable]
    public class Lindexi : MarshalByRefObject
    {
        public string OrderDinner(string cases)
        {
            Console.WriteLine($"点餐：{cases}");
            //Task.Run(async () =>
            //{
            //    await Task.Delay(1000);
            //    CaseOrdered?.Invoke(null, null);
            //});
            CaseOrdered?.Invoke(null, null);
            return $"点餐：{cases}";
        }

        public event EventHandler CaseOrdered;
    }

    [Serializable]
    public class FastachalreMerweserewhai<TSource, TEventArgs> : MarshalByRefObject
    where TSource : MarshalByRefObject where TEventArgs : EventArgs
    {
        public TSource _lindexi;
        public EventHandler<TEventArgs> _handler;

        public FastachalreMerweserewhai(
            MarshalByRefObject source,
            EventHandler<TEventArgs> handler)
        {
            _lindexi = (TSource) source;
            Lindexi lindexi = (Lindexi) source;
            lindexi.CaseOrdered += LindexiOnCaseOrdered;
            _handler = handler;
        }

        public void LindexiOnCaseOrdered(object sender, EventArgs e)
        {
            _handler?.Invoke(null, null);
        }
    }
}
