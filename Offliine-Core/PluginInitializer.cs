using System;
using Offliine.API;

namespace Offliine.Core
{
    public class PluginInitializer : IPluginInitializer
    {
        public void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
