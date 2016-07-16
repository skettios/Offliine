using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Offliine.Injection.Http;
using Offliine.Injection.Util;

namespace Offliine.Injection
{
    public class InjectionClient
    {
        private readonly TcpClient _client;

        public InjectionClient(TcpClient client)
        {
            _client = client;
        }

        public void Start()
        {
            var thread = new Thread(Run);
            thread.Start();
        }

        public void Run()
        {
            try
            {
                _client.SendBufferSize = 5000;
                _client.ReceiveBufferSize = 5000;

                var stream = _client.GetStream();
                var request = _getRequest(stream);

                var version = SystemVersions.GetSystemVersion(request.GetPropriety("User-Agent"));
                if (version != null)
                {
                    var path = request.Path;

                    if (path.Equals("/"))
                    {   
                    }
                    else
                    {
                        var fixedPath = path.Substring(path.IndexOf('/') + 1);
                    }
                }
                else
                {
                    InjectionHelper.GenerateWebpage(WebpageType.Error).Serve(stream);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HttpRequest _getRequest(Stream input)
        {
            var reader = new StreamReader(input);

            var line = reader.ReadLine();

            if (line != null)
            {
                var splitLine = line.Split(' ');
                var method = splitLine[0].Trim();
                var path = splitLine[1].Trim();
                var protocol = splitLine[2].Trim();

                var props = new List<HttpPropriety>();
                for (line = reader.ReadLine(); !string.IsNullOrEmpty(line); line = reader.ReadLine())
                {
                    splitLine = line.Split(new[] {':'}, 2);
                    props.Add(new HttpPropriety(splitLine[0].Trim(), splitLine[1].Trim()));
                }

                return new HttpRequest(method, protocol, path, props);
            }

            return null;
        }
    }
}
