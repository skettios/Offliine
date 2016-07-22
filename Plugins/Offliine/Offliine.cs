using Offliine.API;

namespace Offliine
{
    [Plugin("Offliine", "offliine")]
    public class Offliine
    {
        [Initialize]
        public void Initialize(IPluginInitializer initializer)
        {
            initializer.Log("TEST");
        }
    }
}
