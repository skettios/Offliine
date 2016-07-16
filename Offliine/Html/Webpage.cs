using System;
using System.IO;

namespace Offliine.Html
{
    public class Webpage
    {
        public delegate void Callback(StreamWriter writer);

        private event Callback _callback;

        public Webpage(Callback callback)
        {
            _callback = callback;
        }

        public void Serve(Stream output)
        {
            if (_callback == null)
                return;

            var writer = new StreamWriter(output);
            _callback(writer);
            writer.Flush();
            writer.Close();
        }
    }
}
