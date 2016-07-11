using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Offliine.Cafiine;
using Offliine.Injection;
using Environment = Android.OS.Environment;

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

        public static Dictionary<string, string> PayloadNames = new Dictionary<string, string>();
        public static Dictionary<string, List<string>> FoundPayloads = new Dictionary<string, List<string>>();

        private bool _injectionRunning = false;
        private Intent _injection;

        private bool _cafiineRunning = false;
        private Intent _cafiine;

        private PowerManager.WakeLock _wakeLock;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            if ((int) Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.Internet) != Permission.Granted)
                    RequestPermissions(new[] {Manifest.Permission.Internet}, 1);

                if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                    RequestPermissions(new[] {Manifest.Permission.WriteExternalStorage}, 1);
            }

            if (!ExternalStorage.Exists())
                ExternalStorage.Mkdir();

            if (!Cafiine.Exists())
                Cafiine.Mkdir();

            _copyRequired();

            DiscoverPayloads();

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IpAddress = ip.ToString();
                    break;
                }
            }

            var powerManager = (PowerManager)GetSystemService(PowerService);
            _wakeLock = powerManager.NewWakeLock(WakeLockFlags.Partial, "Offliine-Wakelock");
            _wakeLock.Acquire();

            _injection = new Intent(this, typeof(InjectionService));
            _cafiine = new Intent(this, typeof(CafiineService));

            var view = FindViewById<TextView>(Resource.Id.IpAddressText);
            view.Text = "IP Address\n" + IpAddress + ":1337";
            view.Gravity = GravityFlags.Center;

            var offliine = FindViewById<Button>(Resource.Id.OffliineButton);
            offliine.SetTextColor(Color.Red);
            offliine.Click += (sender, args) =>
            {
                if (!_injectionRunning)
                {
                    _injectionRunning = true;

                    offliine.Enabled = false;
                    offliine.SetTextColor(Color.Green);
                    offliine.Text = "Offliine: ON";

                    new Timer(o =>
                    {
                        RunOnUiThread(() =>
                        {
                            offliine.Enabled = true;
                        });
                    }, null, 1000, 0);

                    StartService(_injection);
                }
                else
                {
                    _injectionRunning = false;

                    offliine.Enabled = false;
                    offliine.SetTextColor(Color.Red);
                    offliine.Text = "Offliine: OFF";

                    new Timer(o =>
                    {
                        RunOnUiThread(() =>
                        {
                            offliine.Enabled = true;
                        });
                    }, null, 1000, 0);

                    StopService(_injection);
                }
            };

            var cafiine = FindViewById<Button>(Resource.Id.CafiineButton);
            cafiine.SetTextColor(Color.Red);
            cafiine.Click += delegate
            {
                if (!_cafiineRunning)
                {
                    _cafiineRunning = true;

                    cafiine.Enabled = false;
                    cafiine.SetTextColor(Color.Green);
                    cafiine.Text = "Cafiine: ON";

                    new Timer(o =>
                    {
                        RunOnUiThread(() =>
                        {
                            cafiine.Enabled = true;
                        });
                    }, null, 1000, 0);

                    StartService(_cafiine);
                }
                else
                {
                    _cafiineRunning = false;

                    cafiine.Enabled = false;
                    cafiine.SetTextColor(Color.Red);
                    cafiine.Text = "Cafiine: OFF";

                    new Timer(o =>
                    {
                        RunOnUiThread(() =>
                        {
                            cafiine.Enabled = true;
                        });
                    }, null, 1000, 0);

                    StopService(_cafiine);
                }
            };
        }

        private void _copyRequired()
        {
            try
            {
                var assetManager = Assets;
                var rootAssetList = assetManager.List("");
                foreach (var s in rootAssetList)
                {
                    if (s.Equals("Loaders"))
                    {
                        var folder = new File(ExternalStorage, s);
                        if (!folder.Exists())
                            folder.Mkdir();
                        else
                            continue;

                        var innerAssetList = assetManager.List(s);
                        foreach (var name in innerAssetList)
                        {
                            var input = assetManager.Open(s + "/" + name);
                            var output = System.IO.File.Create(folder + "/" + name);
                            input.CopyTo(output);
                            output.Flush();
                            output.Close();
                            input.Close();
                        }
                    }
                    else if (s.Equals("Payloads"))
                    {
                        var folder = new File(ExternalStorage, s);
                        if (!folder.Exists())
                            folder.Mkdir();
                        else
                            continue;

                        var innerAssetList = assetManager.List(s);
                        foreach (var name in innerAssetList)
                        {
                            if (name.Equals("hbl_Homebrew Launcher") || name.Equals("loadiine_Loadiine") || name.Equals("ksploit_Kernel Exploit") || name.Equals("cafiine_Cafiine") || name.Equals("sdcafiine_SD Cafiine"))
                            {
                                var payloadFolder = new File(folder, name);
                                if (!payloadFolder.Exists())
                                    payloadFolder.Mkdir();
                                else
                                    continue;

                                var innerInnerAssetList = assetManager.List(s + "/" + name);
                                foreach (var file in innerInnerAssetList)
                                {
                                    var input = assetManager.Open(s + "/" + name + "/" + file);
                                    var output = System.IO.File.Create(payloadFolder + "/" + file);
                                    input.CopyTo(output);
                                    output.Flush();
                                    output.Close();
                                    input.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Debug("Offliine", e.Message);
            }
        }

        public static void DiscoverPayloads()
        {
            foreach (var folder in Payloads.ListFiles())
            {
                var match = Regex.Match(folder.Name, "[a-zA-Z]_[a-zA-Z]", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    var path = folder.Name.Substring(0, folder.Name.IndexOf('_'));
                    var name = folder.Name.Substring(folder.Name.IndexOf('_') + 1);
                    PayloadNames.Add(path, name);

                    var payloads = folder.ListFiles();
                    var foundPayloads = new List<string>();
                    foreach (var payload in payloads)
                        foundPayloads.Add(payload.Name);

                    FoundPayloads.Add(path, foundPayloads);
                }
            }
        }

        protected override void OnDestroy()
        {
            if (_injectionRunning)
                StopService(_injection);

            if (_cafiineRunning)
                StopService(_cafiine);

            _wakeLock.Release();

            base.OnDestroy();
        }
    }
}