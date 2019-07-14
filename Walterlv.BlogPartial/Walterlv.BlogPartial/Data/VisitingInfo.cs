using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Walterlv.BlogPartial.Data
{
    public class VisitingInfo
    {
        public int Id { set; get; }

        public DateTimeOffset Time { set; get; }

        public string Ip { get; set; }

        public string UserAgent { get; set; }
    }
}
