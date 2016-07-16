using System;
using System.Globalization;
using System.IO;
using Offliine.Html;

namespace Offliine.Injection.Util
{
    public enum WebpageType
    {
        Error, Injection
    }

    public class InjectionHelper
    {
        public static Webpage GenerateWebpage(WebpageType type)
        {
            switch (type)
            {
                case WebpageType.Error:
                    return new Webpage(_generateErrorPage);
                case WebpageType.Injection:
                    return new Webpage(_generateInjectionPage);
                default:
                    return null;
            }
        }

        private static void _generateErrorPage(StreamWriter writer)
        {
            var html = new HtmlWriter(writer);

            html.Begin();
            html.BeginHeader();
            html.Write("<title>Error</title>");
            html.Write("<style>");
            html.Write("html { font-family: arial; }");
            html.Write("body { background-color: lightblue; text-align: center; }");
            html.Write("</style>");
            html.BeginBody();
            html.Write("<h1>Offliine could not find system version!<h1>");
            html.EndBody();
            html.EndHeader();
            html.End();
        }

        private static void _generateInjectionPage(StreamWriter writer)
        {
            var html = new HtmlWriter(writer);

            html.BeginHeader();
            html.Write("<title>Offliine</html>");
            html.Write("<style>");
            html.Write("html { font-family: arial; }");
            html.Write("body { background-color: lightblue; text-align: center; }");
            html.Write("div { border: 3px solid gray; border-radius: 5px; background-color: white; margin: auto; width: 81%; text-align: left; }");
            html.Write("button { background-color: white; border: 3px solid lightblue; border-radius: 5px; text-decoration: none; width: 250px; height: 50px; margin: 5px; font-size: 20px; }");
            html.Write("</style>");
            html.EndHeader();
            html.Begin();
            html.Write("<h1>Offliine</h1>");
            html.End();
        }

        public static void WriteU32(int value, StreamWriter output)
        {
            try
            {
                output.Write(value >> 24 & 0xff);
                output.Write(value >> 16 & 0xff);
                output.Write(value >> 8 & 0xff);
                output.Write(value >> 0 & 0xff);
            }
            catch (Exception e)
            {
            }
        }

        public static byte[] ToArray(string value)
        {
            var data = new byte[value.Length / 2];

            for (var i = 0; i < data.Length; i++)
            {
                var val = value.Substring(i*2, 2);
                data[i] = (byte) int.Parse(val, NumberStyles.HexNumber);
            }

            return data;
        }

        public static byte[] ReadFile(Stream input)
        {
            try
            {
                var output = new MemoryStream();

                var buffer = new byte['Ѐ'];

                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    output.Write(buffer, 0, read);

                input.Close();
                output.Close();

                return output.ToArray();
            }
            catch (Exception e)
            {
            }

            return null;
        }
    }
}