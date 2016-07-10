namespace Offliine.Injection.Util
{
    public class Constants
    {
        public static readonly IConstants C532 = new Constants532();
        public static readonly IConstants C550 = new Constants550();
    }

    public interface IConstants
    {
        int PopjumplrStack12();
        int PopjumplrStack20();
        int Callfunc();
        int Callr28PopR28ToR31();
        int PopR28R29R30R31();
        int PopR27();
        int PopR24ToR31();
        int CallfuncptrWithargsFromR3Mem();
        int Setr3Tor31PopR31();

        int Memcpy();
        int DcFlushRange();
        int IcInvalidateRange();
        int OsSwitchSecCodeGenMode();
        int OsCodegenCopy();
        int OsGetCodegenVirtAddrRange();
        int OsGetCoreId();
        int OsGetCurrentThread();
        int OsSetThreadAffinity();
        int OsYieldThread();
        int OsFatal();
        int Exit();
        int OsScreenFlipBuffersEx();
        int OsScreenClearBufferEx();
        int OsDynLoadAcquire();
        int OsDynLoadFindExport();
        int OsSnprintf();
    }

    public class Constants532 : IConstants
    {
        public int PopjumplrStack12()
        {
            return 16895252;
        }

        public int PopjumplrStack20()
        {
            return 16928040;
        }

        public int Callfunc()
        {
            return 17299500;
        }

        public int Callr28PopR28ToR31()
        {
            return 17290024;
        }

        public int PopR28R29R30R31()
        {
            return 16898244;
        }

        public int PopR27()
        {
            return 16894704;
        }

        public int PopR24ToR31()
        {
            return 16909356;
        }

        public int CallfuncptrWithargsFromR3Mem()
        {
            return 16929632;
        }

        public int Setr3Tor31PopR31()
        {
            return 16894976;
        }

        public int Memcpy()
        {
            return 16996968;
        }

        public int DcFlushRange()
        {
            return 16924392;
        }

        public int IcInvalidateRange()
        {
            return 16924688;
        }

        public int OsSwitchSecCodeGenMode()
        {
            return 17002688;
        }

        public int OsCodegenCopy()
        {
            return 17002712;
        }

        public int OsGetCodegenVirtAddrRange()
        {
            return 17002432;
        }

        public int OsGetCoreId()
        {
            return 16928300;
        }

        public int OsGetCurrentThread()
        {
            return 17050060;
        }

        public int OsSetThreadAffinity()
        {
            return 17048196;
        }

        public int OsYieldThread()
        {
            return 17044048;
        }

        public int OsFatal()
        {
            return 16978792;
        }

        public int Exit()
        {
            return 16895344;
        }

        public int OsScreenFlipBuffersEx()
        {
            return 17017296;
        }

        public int OsScreenClearBufferEx()
        {
            return 17017488;
        }

        public int OsDynLoadAcquire()
        {
            return 16950044;
        }

        public int OsDynLoadFindExport()
        {
            return 16955280;
        }

        public int OsSnprintf()
        {
            return 16969884;
        }
    }

    public class Constants550 : IConstants
    {
        public int PopjumplrStack12()
        {
            return 16895268;
        }

        public int PopjumplrStack20()
        {
            return 16928136;
        }

        public int Callfunc()
        {
            return 17302132;
        }

        public int Callr28PopR28ToR31()
        {
            return 17292656;
        }

        public int PopR28R29R30R31()
        {
            return 16898260;
        }

        public int PopR27()
        {
            return 16894720;
        }

        public int PopR24ToR31()
        {
            return 16909512;
        }

        public int CallfuncptrWithargsFromR3Mem()
        {
            return 16929728;
        }

        public int Setr3Tor31PopR31()
        {
            return 16894992;
        }

        public int Memcpy()
        {
            return 16998344;
        }

        public int DcFlushRange()
        {
            return 16924552;
        }

        public int IcInvalidateRange()
        {
            return 16924848;
        }

        public int OsSwitchSecCodeGenMode()
        {
            return 17004224;
        }

        public int OsCodegenCopy()
        {
            return 17004248;
        }

        public int OsGetCodegenVirtAddrRange()
        {
            return 17003968;
        }

        public int OsGetCoreId()
        {
            return 16928396;
        }

        public int OsGetCurrentThread()
        {
            return 17051984;
        }

        public int OsSetThreadAffinity()
        {
            return 17050076;
        }

        public int OsYieldThread()
        {
            return 17045732;
        }

        public int OsFatal()
        {
            return 16979480;
        }

        public int Exit()
        {
            return 16895360;
        }

        public int OsScreenFlipBuffersEx()
        {
            return 17018832;
        }

        public int OsScreenClearBufferEx()
        {
            return 17019024;
        }

        public int OsDynLoadAcquire()
        {
            return 16950196;
        }

        public int OsDynLoadFindExport()
        {
            return 16955432;
        }

        public int OsSnprintf()
        {
            return 16970080;
        }
    }
}