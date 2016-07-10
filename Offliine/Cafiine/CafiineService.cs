using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Offliine.Cafiine
{
    [Service]
    public class CafiineService : Service
    {
        private Server _server;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _server = new Server();
            _server.Start();

            Toast.MakeText(this, "Cafiine Started", ToastLength.Short).Show();
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            _server.Stop();

            Toast.MakeText(this, "Cafiine Stopped", ToastLength.Short).Show();
        }
    }
}