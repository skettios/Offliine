using System;
using Offliine.API.Plugin;

namespace Offliine.Core.Plugin
{
    public class PluginInitialize : IPluginInit
    {
        public void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
