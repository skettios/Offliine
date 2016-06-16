using System;
using System.Runtime.InteropServices;

namespace Offliine.Cafiine
{
    public class Constants
    {
        public static readonly byte BYTE_NORMAL = 0xff;
        public static readonly byte BYTE_SPECIAL = 0xfe;
        public static readonly byte BYTE_OPEN = 0x00;
        public static readonly byte BYTE_READ = 0x01;
        public static readonly byte BYTE_CLOSE = 0x02;
        public static readonly byte BYTE_OK = 0x03;
        public static readonly byte BYTE_SETPOS = 0x04;
        public static readonly byte BYTE_STATFILE = 0x05;
        public static readonly byte BYTE_EOF = 0x06;
        public static readonly byte BYTE_GETPOS = 0x07;
        public static readonly byte BYTE_REQUEST = 0x08;
        public static readonly byte BYTE_REQUEST_SLOW = 0x09;
        public static readonly byte BYTE_HANDLE = 0x0A;
        public static readonly byte BYTE_DUMP = 0x0B;
        public static readonly byte BYTE_PING = 0x0C;

        [Flags]
        public enum FsStatFlag : uint
        {
            None = 0,
            unk_14_present = 0x01000000,
            mtime_present = 0x04000000,
            ctime_present = 0x08000000,
            entid_present = 0x10000000,
            directory = 0x80000000,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FsStat
        {
            public FsStatFlag flags;
            public uint permission;
            public uint owner;
            public uint group;
            public uint file_size;
            public uint unk_14_nonzero;
            public uint unk_18_zero;
            public uint unk_1c_zero;
            public uint ent_id;
            public uint ctime_u;
            public uint ctime_l;
            public uint mtime_u;
            public uint mtime_l;
            public uint unk_34_zero;
            public uint unk_38_zero;
            public uint unk_3c_zero;
            public uint unk_40_zero;
            public uint unk_44_zero;
            public uint unk_48_zero;
            public uint unk_4c_zero;
            public uint unk_50_zero;
            public uint unk_54_zero;
            public uint unk_58_zero;
            public uint unk_5c_zero;
            public uint unk_60_zero;
        }
    }
}