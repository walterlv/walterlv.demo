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
    public class FastachalreMerweserewhai : MarshalByRefObject
    {
        public EventHandler _handler;
        public Lindexi _lindexi;

        public FastachalreMerweserewhai(MarshalByRefObject source, EventHandler handler)
        {
            _lindexi = (Lindexi) source;
            _lindexi.CaseOrdered += LindexiOnCaseOrdered;
            _handler = handler;
        }

        public void LindexiOnCaseOrdered(object sender, EventArgs e)
        {
            _handler?.Invoke(null, null);
        }
    }
}
