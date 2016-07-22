using System;

namespace Offliine.API.Plugin
{
    public interface IPluginInit
    {
        void Log(string msg);
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PluginInit : Attribute
    {
    }
}
