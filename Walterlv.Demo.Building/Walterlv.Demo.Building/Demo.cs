using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Walterlv.Demo.Building
{
    public class Demo : Freezable
    {
        public Demo()
        {
            
        }

        protected override Freezable CreateInstanceCore()
        {
            return new Demo();
        }

        public override string ToString()
        {
            return GetHashCode().ToString("D");
        }
    }
}
