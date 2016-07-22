using Offliine.API.Plugin;

namespace Cafiine
{
    [PluginInfo("Cafiine", "cafiine")]
    public class Cafiine
    {
        [PluginInit]
        public void Init(IPluginInit init)
        {
            init.Log("CAFIINE");
        }
    }
}
