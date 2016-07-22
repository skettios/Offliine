using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Offliine.API.Plugin;

namespace Offliine.Core.Plugin
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

        public void Reload()
        {
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
                    if (type.GetCustomAttributes(typeof(PluginInfo), true).Length > 0)
                    {
                        var pluginInfo = (PluginInfo) Attribute.GetCustomAttribute(type, typeof(PluginInfo));

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
                    if (method.GetCustomAttribute<PluginInit>() != null)
                    {
                        if (method.GetParameters().Length != 1)
                            break;

                        if (method.GetParameters()[0].ParameterType != typeof(IPluginInit))
                            break;

                        method.Invoke(container.PluginObject, new object[] { new PluginInitialize() });
                        pluginInitialized = true;

                        break;
                    }
                }

                if (!pluginInitialized)
                    Console.WriteLine($"Plugin, {container.PluginInfo.Name}, does not contain an initialize function!");
            }
        }
    }
}
