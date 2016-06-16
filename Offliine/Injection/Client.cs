using System;
using System.Collections.Generic;
using System.IO;
using Android.Runtime;
using Android.Util;
using Java.IO;
using Java.Net;

namespace Offliine.Injection
{
    public class Client
    {
        private Socket _socket;

        public Client(Socket socket)
        {
            _socket = socket;
        }

        public void Run()
        {
            var input = _socket.InputStream;
            var output = _socket.OutputStream;
            var header = GetRequest(input);
            var path = header.Path;

            var version = SystemVersions.GetSystemVersion(header.GetPropriety("User-Agent"));

            if (version != null)
            {
                if (path.IndexOf("?") != -1)
                {
                    var payload = path.Substring(path.IndexOf("?") + 1);
                    Log.Debug("Offliine", payload);
                    if (payload.Equals("hbl"))
                        Serve(version, output, "hbl" + version.PayloadVersion + ".bin");
                    else if (payload.Equals("loadiine"))
                        Serve(version, output, "loadiine" + version.PayloadVersion + ".bin");
                    else
                        Serve(version, output, payload + ".bin");
                }
                else
                {
                    Serve(version, output, "hbl" + version.PayloadVersion + ".bin");
                }
            }

            input.Close();
            output.Close();
            _socket.Close();
        }

        private void WriteHeader(Writer writer, string contentType)
        {
            writer.Write("HTTP/1.1 200 OK\r\n");
            if (contentType != null)
            {
                if (contentType.Contains("video"))
                    writer.Write("Transfer-Encoding: chunked\r\n");

                writer.Write("Content-Type: ");
                writer.Write(contentType);
                writer.Write("\r\n");
            }

            writer.Write("\r\n");
            writer.Flush();
        }

        private void Serve(SystemVersion version, Stream output, string payloadName)
        {
            var writer = new BufferedWriter(new OutputStreamWriter(output));

            WriteHeader(writer, "video/mp4");
            if (StageFright.Serve(new OutputStreamAdapter(output), version, payloadName))
            {
                Log.Debug("Offliine", "SUCCESS");
            }

            writer.Close();
        }

        private HTTPRequest GetRequest(Stream input)
        {
            var reader = new BufferedReader(new InputStreamReader(input));

            var line = reader.ReadLine();

            var splitLine = line.Split(' ');
            var method = splitLine[0].Trim();
            var path = splitLine[1].Trim();
            var protocol = splitLine[2].Trim();

            List<HTTPPropriety> props = new List<HTTPPropriety>();
            for (line = reader.ReadLine(); !string.IsNullOrEmpty(line); line = reader.ReadLine())
            {
                splitLine = line.Split(new[] {':'}, 2);
                props.Add(new HTTPPropriety(splitLine[0].Trim(), splitLine[1].Trim()));
            }

            return new HTTPRequest(method, protocol, path, props);
        }
    }
}