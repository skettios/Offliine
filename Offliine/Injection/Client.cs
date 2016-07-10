using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Android.Runtime;
using Android.Util;
using Java.IO;
using Java.Net;
using Offliine.Injection.Exploit;
using Offliine.Injection.Http;
using Offliine.Injection.Util;

namespace Offliine.Injection
{
    public class Client
    {
        private readonly Socket _socket;

        public Client(Socket socket)
        {
            _socket = socket;
        }

        public void Start()
        {
            var thread = new Thread(Run);
            Thread.Sleep(1000);
            thread.Start();
        }

        public void Run()
        {
            try
            {
                if (_socket.IsConnected)
                {
                    _socket.SendBufferSize = 50000;
                    _socket.ReceiveBufferSize = 50000;

                    var input = _socket.InputStream;
                    var output = _socket.OutputStream;
                    var header = _getRequest(input);
                    var path = header.Path;

                    var version = SystemVersions.GetSystemVersion(header.GetPropriety("User-Agent"));

                    if (version != null)
                    {
                        if (path.Equals("/"))
                        {
                            _serveWebPage(output, version);
                        }
                        else
                        {
                            var fixedPath = path.Substring(path.IndexOf('/') + 1);
                            if (fixedPath.Equals("sdcafiine") && (version == SystemVersions.Us540 || version == SystemVersions.Eu540 || version == SystemVersions.Jp540))
                                _serveHax(version, output, fixedPath + "_" + MainActivity.PayloadNames[fixedPath] + "/" + version.PayloadVersions[1] + ".bin");
                            else
                                _serveHax(version, output, fixedPath + "_" + MainActivity.PayloadNames[fixedPath] + "/" + version.PayloadVersions[0] + ".bin");
                        }
                    }

                    input.Close();
                    output.Close();
                }
            }
            catch (Exception e)
            {
                Log.Debug("Offliine", e.Message);
            }

            _socket.Close();
        }

        private void _writeHeader(Writer writer, string contentType)
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

        private void _serveWebPage(Stream output, SystemVersion version)
        {
            var writer = new OutputStreamWriter(output);

            HtmlHelper.BeginHtml(writer);
            HtmlHelper.BeginBody(writer);
            HtmlHelper.CreateHeader1(writer, "Offliine");
            HtmlHelper.BeginDiv(writer);
            foreach (var payload in MainActivity.PayloadNames)
            {
                if (MainActivity.FoundPayloads[payload.Key].Contains(version.PayloadVersions[0] + ".bin"))
                    HtmlHelper.CreateButton(writer, payload.Key, payload.Value);
            }
            HtmlHelper.EndDiv(writer);
            HtmlHelper.EndBody(writer);
            HtmlHelper.EndHtml(writer);

            writer.Flush();
        }

        private void _serveHax(SystemVersion version, Stream output, string payloadName)
        {
            var writer = new BufferedWriter(new OutputStreamWriter(output));

            _writeHeader(writer, "video/mp4");
            if (StageFright.Serve(new OutputStreamAdapter(output), version, payloadName))
            {
                Log.Debug("Offliine", payloadName);
            }

            writer.Close();
        }

        private HTTPRequest _getRequest(Stream input)
        {
            var reader = new BufferedReader(new InputStreamReader(input));

            var line = reader.ReadLine();

            var splitLine = line.Split(' ');
            var method = splitLine[0].Trim();
            var path = splitLine[1].Trim();
            var protocol = splitLine[2].Trim();

            var props = new List<HTTPPropriety>();
            for (line = reader.ReadLine(); !string.IsNullOrEmpty(line); line = reader.ReadLine())
            {
                splitLine = line.Split(new[] {':'}, 2);
                props.Add(new HTTPPropriety(splitLine[0].Trim(), splitLine[1].Trim()));
            }

            return new HTTPRequest(method, protocol, path, props);
        }
    }
}