using Android.Util;
using Java.Lang;
using Java.Net;

namespace Offliine.Injection
{
    public class Server
    {
        private Thread _thread;
        private ServerSocket _server;

        private bool _isRunning;

        public void Start()
        {
            _isRunning = true;

            _thread = new Thread(Run);
            _thread.Start();
        }

        public void Stop()
        {
            _server.Close();
            _isRunning = false;
        }

        public void Run()
        {
            try
            {
                Thread.Sleep(1000);

                _server = new ServerSocket(1337) { ReceiveBufferSize = 50000, ReuseAddress = true };

                while (_isRunning)
                {
                    var socket = _server.Accept();

                    var client = new Client(socket);
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