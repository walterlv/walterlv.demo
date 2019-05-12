using System;
using System.Threading;

namespace Walterlv.BlogPartial.Services.Implements
{
    public class VisitingCounter : IVisitingCounter
    {
        private int _csdnCount;
        private int _blogCount;

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
            Console.WriteLine($"[PV] CSDN | {_csdnCount} , blog.walterlv.com | {_blogCount}");
        }
    }
}