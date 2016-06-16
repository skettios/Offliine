using System.Net;
using System.Net.Sockets;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using Offliine.Injection;

namespace Offliine
{
    [Activity(Label = "Offliine", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static File ExternalStorage = Environment.GetExternalStoragePublicDirectory("Offliine");
        public static File Cafiine = new File(ExternalStorage, "Cafiine");
        public static File Payloads = new File(ExternalStorage, "Payloads");
        public static File Loaders = new File(ExternalStorage, "Loaders");
        public static string IpAddress;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            if (CheckSelfPermission(Manifest.Permission.Internet) != Permission.Granted)
                RequestPermissions(new[] { Manifest.Permission.Internet }, 1);

            if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                RequestPermissions(new[] { Manifest.Permission.WriteExternalStorage }, 1);

            if (!ExternalStorage.Exists())
                ExternalStorage.Mkdir();

            if (!Cafiine.Exists())
                Cafiine.Mkdir();

            if (!Payloads.Exists())
                Payloads.Mkdir();
            
            if (!Loaders.Exists())
                Loaders.Mkdir();

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IpAddress = ip.ToString();
                    break;
                }
            }

            var view = FindViewById<TextView>(Resource.Id.IpAddressText);
            view.Text = "IP Address\n" + IpAddress;
            view.Gravity = GravityFlags.Center;

            var offline = FindViewById<Button>(Resource.Id.OffliineButton);
            offline.SetTextColor(Color.Red);
            offline.Click += delegate
            {
                Log.Info("Offliine", "Click");

                offline.Enabled = false;
                offline.SetTextColor(Color.Green);
                offline.Text = "Offliine: ON";

                var server = new Server();
                server.Start();
            };

            var cafiine = FindViewById<Button>(Resource.Id.CafiineButton);
            cafiine.SetTextColor(Color.Red);
            cafiine.Click += delegate
            {
                cafiine.Enabled = false;
                cafiine.SetTextColor(Color.Green);
                cafiine.Text = "Cafiine: ON";
            };
        }
    }
}