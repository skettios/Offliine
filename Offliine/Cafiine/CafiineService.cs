using Android.App;
using Android.Content;
using Android.Graphics;
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

            var notificationManager = (NotificationManager) GetSystemService(NotificationService);
            var notification = new Notification.Builder(this);
            var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(), 0);
            notification.SetOngoing(true);
            notification.SetContentIntent(pendingIntent);
            notification.SetContentTitle("Cafiine is currently running.");
            notification.SetContentText("IP Address: " + MainActivity.IpAddress);
            notification.SetSmallIcon(Resource.Drawable.icon);
            notification.SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.icon));

            notificationManager.Notify(0, notification.Build());

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

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(0);

            Toast.MakeText(this, "Cafiine Stopped", ToastLength.Short).Show();
        }
    }
}