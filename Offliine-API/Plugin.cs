using System;

namespace Offliine.API
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Plugin : Attribute
    {
        public string Name, Id, Version;
        public int VersionCode;

        public Plugin(string name, string id, string version = "v0.0.1", int versionCode = 0)
        {
            Name = name;
            Id = id;
            Version = version;
            VersionCode = versionCode;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Initialize : Attribute
    {
    }
}
