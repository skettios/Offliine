using Java.IO;

namespace Offliine.Injection
{
    public class Payload
    {
        public static byte[] Generate(SystemVersion version, string payloadName)
        {
            var output = new ByteArrayOutputStream();

            var payload = Util.ReadFile(new File(MainActivity.Payloads.Path, payloadName));
            var loader = Util.ReadFile(new File(MainActivity.Loaders.Path, version.LoaderName));

            var padding = 0;
            while ((payload.Length + padding & 0x3) != 0)
                padding++;

            output.Write(loader);
            Util.WriteU32(payload.Length + padding, output);
            output.Write(payload);
            output.Write(new byte[padding]);

            output.Close();

            return output.ToByteArray();
        }
    }
}