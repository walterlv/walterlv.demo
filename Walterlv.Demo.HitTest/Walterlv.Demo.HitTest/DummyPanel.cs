using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Walterlv.Demo.HitTest
{
    public class DummyPanel : UserControl
    {
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return null;
            // return base.HitTestCore(hitTestParameters);
        }
    }

    public class DummyChild : UserControl
    {
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return null;
            // return base.HitTestCore(hitTestParameters);
        }
    }
}
