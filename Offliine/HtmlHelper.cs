using System.IO;
using Java.IO;

namespace Offliine
{
    public class HtmlHelper
    {
        public static void BeginHtml(OutputStreamWriter writer)
        {
            writer.Write("<head>");
            writer.Write("<style>");
            writer.Write("body { background-color: lightblue; }");
            writer.Write("button { width: 250px; height: 50px; margin-bottom: 5px; font-size: 20px; }");
            writer.Write("</style>");
            writer.Write("</head>");
            writer.Write("<html>");
        }

        public static void BeginBody(OutputStreamWriter writer)
        {
            writer.Write("<body>");
        }

        public static void CreateHeader1(OutputStreamWriter writer, string value)
        {
            writer.Write("<h1>");
            writer.Write(value);
            writer.Write("</h1>");
        }

        public static void CreateButton(OutputStreamWriter writer, string path, string name)
        {
            writer.Write("<button onclick=\"window.open(\'" + path + "\', \'_self\');\">");
            writer.Write(name);
            writer.Write("</button>");
            writer.Write("<br>");
        }

        public static void EndBody(OutputStreamWriter writer)
        {
            writer.Write("</body>");
        }

        public static void EndHtml(OutputStreamWriter writer)
        {
            writer.Write("</html>");
        }
    }
}