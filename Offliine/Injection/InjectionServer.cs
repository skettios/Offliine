using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Offliine.Injection
{
    public class InjectionServer
    {
        private Thread _thread;

        private TcpListener _server;
        public bool Running { get; private set; }

        public void Start()
        {
            Running = true;

            _thread = new Thread(Run);
            _thread.Start();
        }

        public void Stop()
        {
            Running = false;

            _thread.Abort();
        }

        public void Run()
        {
            try
            {
                _server = new TcpListener(IPAddress.Any, 1337);
                _server.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _server.Start();

                while (Running)
                {
                    var client = _server.AcceptTcpClient();
                    var injection = new InjectionClient(client);
                    injection.Start();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
