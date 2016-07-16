using Java.IO;

namespace Offliine.Android
{
    public class HtmlHelper
    {
        public static void BeginHtml(Writer writer)
        {
            writer.Write("<head>");
            writer.Write("<style>");
            writer.Write("body { background-color: lightblue; text-align: center; }");
            writer.Write("div { border: 3px solid gray; border-radius: 5px; background-color: white; margin: auto; width: 81%; text-align: left; }");
            writer.Write("button { background-color: white; border: 3px solid lightblue; border-radius: 5px; text-decoration: none; width: 250px; height: 50px; margin: 5px; font-size: 20px; }");
            writer.Write("</style>");
            writer.Write("</head>");
            writer.Write("<html>");
        }

        public static void BeginBody(Writer writer)
        {
            writer.Write("<body>");
        }

        public static void CreateHeader1(Writer writer, string value)
        {
            writer.Write("<h1>");
            writer.Write(value);
            writer.Write("</h1>");
        }

        public static void BeginDiv(Writer writer)
        {
            writer.Write("<div>");
        }

        public static void CreateButton(Writer writer, string path, string name)
        {
            writer.Write("<button onclick=\"window.open(\'" + path + "\', \'_self\');\">");
            writer.Write(name);
            writer.Write("</button>");
//            writer.Write("<br>");
        }

        public static void EndDiv(Writer writer)
        {
            writer.Write("</div>");
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