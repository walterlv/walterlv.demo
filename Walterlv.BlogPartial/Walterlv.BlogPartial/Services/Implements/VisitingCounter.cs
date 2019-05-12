using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Walterlv.BlogPartial.Services.Implements
{
    public class VisitingCounter : IVisitingCounter
    {
        private DateTimeOffset _dateTime;
        private int _csdnCount;
        private int _blogCount;
        private readonly ConcurrentQueue<int> _csdnPvInHours = new ConcurrentQueue<int>();
        private readonly ConcurrentQueue<int> _blogPvInHours = new ConcurrentQueue<int>();

        public void AddCsdnPv()
        {
            Interlocked.Increment(ref _csdnCount);
        }

        public void AddBlogPv()
        {
            Interlocked.Increment(ref _blogCount);
        }

        public void PrintSummary()
        {
            Console.WriteLine($@"[PV | CSDN] {_csdnCount} | ...
[PV | blog] {_blogCount} | ...");
        }
    }
}