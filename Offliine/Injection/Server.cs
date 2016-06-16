using Android.Util;
using Java.Lang;
using Java.Net;
using Exception = System.Exception;

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
            try
            {
                var server = new ServerSocket(1337) {ReceiveBufferSize = 50000};

                var clientCount = 0;

                while (true)
                {
                    var socket = server.Accept();

                    var builder = new StringBuilder();
                    builder.Append("clientThread-");
                    builder.Append(clientCount++);

                    var client = new Client(socket, builder.ToString());
                    client.Start();
                }
            }
            catch (Exception e)
            {
                Log.Debug("Offliine", e.Message);
            }
        }
    }
}