using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Offliine.Injection
{
    [Service]
    public class InjectionService : Service
    {
        private Server _server;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _server = new Server();
            _server.Start();

            Toast.MakeText(this, "Offliine Started", ToastLength.Short).Show();
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            _server.Stop();

            Toast.MakeText(this, "Offliine Stopped", ToastLength.Short).Show();
        }
    }
}
