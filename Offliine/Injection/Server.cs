using System;
using System.Threading;
using Android.Util;
using Java.Lang;
using Java.Net;

namespace Offliine.Injection
{
    public class Server
    {
        public void Start()
        {
            var thread = new System.Threading.Thread(Run);
            thread.Start();
        }

        public void Run()
        {
            Log.Info("Offliine", "Offliine server started.");
            var server = new ServerSocket(1337);

            var clientCount = 0;

            while (true)
            {
                var socket = server.Accept();
                Log.Info("Offliine", "searching...");

                var client = new Client(socket);
                var thread = new System.Threading.Thread(client.Run);

                var builder = new StringBuilder();
                builder.Append("clientThread-");
                builder.Append(clientCount++);
                thread.Name = builder.ToString();

                thread.Start();
            }
        }
    }
}