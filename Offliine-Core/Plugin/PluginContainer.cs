using System;
using System.Reflection;
using Offliine.API.Plugin;

namespace Offliine.Core.Plugin
{
    public class PluginContainer
    {
        public PluginInfo PluginInfo;
        public Assembly PluginBinary;
        public Type PluginEntry;
        public object PluginObject;

        public PluginContainer(PluginInfo pluginInfo, Assembly pluginBinary, Type pluginEntry, object pluginObject)
        {
            PluginInfo = pluginInfo;
            PluginBinary = pluginBinary;
            PluginEntry = pluginEntry;
            PluginObject = pluginObject;
        }
    }
}
