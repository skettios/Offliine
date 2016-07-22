using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Offliine.API;

namespace Offliine.Core
{
    public class PluginLoader
    {
        public readonly DirectoryInfo PluginDirectory;

        public readonly List<string> PluginIds = new List<string>();
        public readonly Dictionary<string, PluginContainer> PluginContainers = new Dictionary<string, PluginContainer>();

        public PluginLoader(string path)
        {
            PluginDirectory = new DirectoryInfo(path);

            _discoverPlugins();
            _initializePlugins();
        }

        private void _discoverPlugins()
        {
            foreach (var plugin in PluginDirectory.GetFiles())
            {
                var assembly = Assembly.LoadFile(plugin.FullName);
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(Plugin), true).Length > 0)
                    {
                        var pluginInfo = (Plugin) Attribute.GetCustomAttribute(type, typeof(Plugin));

                        PluginIds.Add(pluginInfo.Id);
                        PluginContainers.Add(pluginInfo.Id, new PluginContainer(pluginInfo, assembly, type, Activator.CreateInstance(type)));

                        break;
                    }
                }
            }
        }

        private void _initializePlugins()
        {
            foreach (var container in PluginContainers.Values)
            {
                var pluginInitialized = false;

                var methods = container.PluginEntry.GetMethods();
                foreach (var method in methods)
                {
                    if (method.GetCustomAttribute<Initialize>() != null)
                    {
                        if (method.GetParameters().Length != 1)
                            break;

                        if (method.GetParameters()[0].ParameterType != typeof(IPluginInitializer))
                            break;

                        method.Invoke(container.PluginObject, new object[] { new PluginInitializer() });
                        pluginInitialized = true;

                        break;
                    }
                }

                if (!pluginInitialized)
                    Console.WriteLine($"Plugin, {container.PluginInfo.Name}, does not contain an initialize function!");
            }
        }
    }

    public class PluginContainer
    {
        public Plugin PluginInfo;
        public Assembly PluginBinary;
        public Type PluginEntry;
        public object PluginObject;

        public PluginContainer(Plugin pluginInfo, Assembly pluginBinary, Type pluginEntry, object pluginObject)
        {
            PluginInfo = pluginInfo;
            PluginBinary = pluginBinary;
            PluginEntry = pluginEntry;
            PluginObject = pluginObject;
        }
    }
}
