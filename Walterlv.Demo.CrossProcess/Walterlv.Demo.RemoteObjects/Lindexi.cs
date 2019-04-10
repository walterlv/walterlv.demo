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
    public class RemoteEventHandler<TSource, TEventArgs> : MarshalByRefObject
    where TSource : MarshalByRefObject where TEventArgs : EventArgs
    {
        public TSource _lindexi;
        public EventHandler<TEventArgs> _handler;

        public RemoteEventHandler(
            MarshalByRefObject source,
            string eventName,
            EventHandler<TEventArgs> handler)
        {
            _lindexi = (TSource) source;

            var eventInfo = typeof(TSource).GetEvent(eventName);
            var hand = Delegate.CreateDelegate(eventInfo.EventHandlerType,
                this, GetType().GetMethod(nameof(LindexiOnCaseOrdered)));
            eventInfo.AddEventHandler(_lindexi, hand);
            _handler = handler;
        }

        public void LindexiOnCaseOrdered(object sender, TEventArgs e)
        {
            _handler?.Invoke(null, null);

            var walterlv = new Walterlv();
            var args = new object[] { "key", null };
            var value = (string) typeof(Walterlv).GetMethod("Get").Invoke(walterlv, args);
            // 在这里可以从 args 里面取出被 ref 或者 out 修改的参数。
        }

    }
    public class Walterlv
    {
        public string Get(string key)
        {
        }
    }
}
