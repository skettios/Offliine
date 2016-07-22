using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Offliine.Core;

namespace Offliine.Windows
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var pluginLoader = new PluginLoader("Plugins");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
