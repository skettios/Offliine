using Android;
using Android.App;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using Java.IO;
using Offliine.Core.Plugin;

namespace Offliine.Android
{
    [Activity(Label = "Offliine_Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        public static File ExternalStorage = Environment.GetExternalStoragePublicDirectory("Offliine");
        public static File Plugins = new File(ExternalStorage, "Plugins");

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.Internet) != Permission.Granted)
                    RequestPermissions(new[] { Manifest.Permission.Internet }, 1);

                if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                    RequestPermissions(new[] { Manifest.Permission.WriteExternalStorage }, 1);
            }

            if (!ExternalStorage.Exists())
                ExternalStorage.Mkdir();

            if (!Plugins.Exists())
                Plugins.Mkdir();

            var pluginLoader = new PluginLoader(Plugins.AbsolutePath);

            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }
    }
}

