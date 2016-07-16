using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace Offliine.Android.Injection
{
    [Service]
    public class InjectionService : Service
    {
        private Server _server;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _server = new Server();
            _server.Start();

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            var notification = new Notification.Builder(this);
            var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(), 0);
            notification.SetOngoing(true);
            notification.SetContentIntent(pendingIntent);
            notification.SetContentTitle("Offliine is currently running.");
            notification.SetContentText("IP Address: " + MainActivity.IpAddress + ":1337");
            notification.SetSmallIcon(Resource.Drawable.icon);
            notification.SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.icon));

            notificationManager.Notify(1, notification.Build());

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

            var notificationManager = (NotificationManager) GetSystemService(NotificationService);
            notificationManager.Cancel(1);

            Toast.MakeText(this, "Offliine Stopped", ToastLength.Short).Show();
        }
    }
}
