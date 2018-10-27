using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace Walterlv.Demo.WindowsTasks
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void JumpList_OnJumpItemsRejected(object sender, JumpItemsRejectedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void JumpList_OnJumpItemsRemovedByUser(object sender, JumpItemsRemovedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
