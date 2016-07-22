using System;

namespace Offliine.API.Plugin
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginInfo : Attribute
    {
        public string Name, Id, Version;
        public int VersionCode;

        public PluginInfo(string name, string id, string version = "unspecified", int versionCode = -1)
        {
            Name = name;
            Id = id;
            Version = version;
            VersionCode = versionCode;
        }
    }
}
