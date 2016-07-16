using System.IO;

namespace Offliine.Html
{
    public class HtmlWriter
    {
        private readonly StreamWriter _writer;

        public HtmlWriter(StreamWriter writer)
        {
            _writer = writer;
        }

        public void Begin()
        {
            _writer.Write("<html>");
        }

        public void BeginHeader()
        {
            _writer.Write("<header>");
        }

        public void BeginBody()
        {
            _writer.Write("<body>");
        }

        public void Write(string value)
        {
            _writer.Write(value);
        }

        public void EndBody()
        {
            _writer.Write("</body>");
        }

        public void EndHeader()
        {
            _writer.Write("</header>");
        }

        public void End()
        {
            _writer.Write("</html>");
        }
    }
}
