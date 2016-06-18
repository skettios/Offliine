using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using Android.Util;
using Console = System.Console;
using File = Java.IO.File;

namespace Offliine.Cafiine
{
    class Server
    {
        public const byte BYTE_NORMAL = 0xff;
        public const byte BYTE_SPECIAL = 0xfe;
        public const byte BYTE_OPEN = 0x00;
        public const byte BYTE_READ = 0x01;
        public const byte BYTE_CLOSE = 0x02;
        public const byte BYTE_OK = 0x03;
        public const byte BYTE_SETPOS = 0x04;
        public const byte BYTE_STATFILE = 0x05;
        public const byte BYTE_EOF = 0x06;
        public const byte BYTE_GETPOS = 0x07;
        public const byte BYTE_REQUEST = 0x08;
        public const byte BYTE_REQUEST_SLOW = 0x09;
        public const byte BYTE_HANDLE = 0x0A;
        public const byte BYTE_DUMP = 0x0B;
        public const byte BYTE_PING = 0x0C;

        [Flags]
        public enum FSStatFlag : uint
        {
            None = 0,
            unk_14_present = 0x01000000,
            mtime_present = 0x04000000,
            ctime_present = 0x08000000,
            entid_present = 0x10000000,
            directory = 0x80000000,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FSStat
        {
            public FSStatFlag flags;
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

        private File _root = MainActivity.Cafiine;

        public void Start()
        {
            var thread = new System.Threading.Thread(Run);
            thread.Start();
        }

        public void Run()
        {
            var name = "[listener]";
            try
            {
                var listener = new TcpListener(IPAddress.Any, 7332);
                listener.Start();
                Console.WriteLine(name + " Listening on 7332");

                var index = 0;
                while (true)
                {
                    var client = listener.AcceptTcpClient();
                    var thread = new Thread(_handle) {Name = "[" + index.ToString() + "]"};
                    thread.Start(client);
                    index++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(name + " " + e.Message);
            }
        }

        private void _handle(object clientObj)
        {
            var name = Thread.CurrentThread.Name;
            var files = new FileStream[256];
            var filesRequest = new Dictionary<int, FileStream>();

            try
            {
                var client = (TcpClient) clientObj;
                using (var stream = client.GetStream())
                {
                    var reader = new EndianBinaryReader(stream);
                    var writer = new EndianBinaryWriter(stream);

                    var ids = reader.ReadUInt32s(4);

                    if (!new File(_root, ids[0].ToString("X8") + "-" + ids[1].ToString("X8")).Exists())
                    {
                        writer.Write(BYTE_NORMAL);
                        throw new Exception("Not interested.");
                    }

                    var localRoot = new File(_root, ids[0].ToString("X8") + "-" + ids[1].ToString("X8"));
                    writer.Write(BYTE_SPECIAL);

                    while (true)
                    {
                        var cmdByte = reader.ReadByte();
                        switch (cmdByte)
                        {
                            case BYTE_OPEN:
                            {
                                var requestSlow = false;

                                var lenPath = reader.ReadInt32();
                                var lenMode = reader.ReadInt32();
                                var path = reader.ReadString(Encoding.ASCII, lenPath - 1);
                                if (reader.ReadByte() != 0) throw new InvalidDataException();
                                var mode = reader.ReadString(Encoding.ASCII, lenMode - 1);
                                if (reader.ReadByte() != 0) throw new InvalidDataException();
                                if (new File(localRoot, path).Exists())
                                {
                                    var handle = -1;
                                    for (var i = 0; i < files.Length; i++)
                                    {
                                        if (files[i] == null)
                                        {
                                            handle = i;
                                            break;
                                        }
                                    }
                                    if (handle == -1)
                                    {
                                        writer.Write(BYTE_SPECIAL);
                                        writer.Write(-19);
                                        writer.Write(0);
                                        break;
                                    }

                                    files[handle] = new FileStream(localRoot + path, FileMode.Open, FileAccess.Read,
                                        FileShare.Read);

                                    writer.Write(BYTE_SPECIAL);
                                    writer.Write(0);
                                    writer.Write(0x0fff00ff | (handle << 8));
                                }
                                else if (new File(localRoot, path + "-request").Exists() || (requestSlow = new File(localRoot, path + "-request_slow").Exists()))
                                {
                                    if (new File(localRoot, path + "-dump").Exists())
                                    {
                                        writer.Write(!requestSlow ? BYTE_REQUEST : BYTE_REQUEST_SLOW);
                                    }
                                    else
                                    {
                                        writer.Write(BYTE_NORMAL);
                                    }
                                }
                                else
                                {
                                    writer.Write(BYTE_NORMAL);
                                }
                                break;
                            }
                            case BYTE_HANDLE:
                            {
                                var fd = reader.ReadInt32();
                                var lenPath = reader.ReadInt32();
                                var path = reader.ReadString(Encoding.ASCII, lenPath - 1);
                                if (reader.ReadByte() != 0) throw new InvalidDataException();

                                filesRequest.Add(fd,
                                    new FileStream(localRoot + path + "-dump", FileMode.OpenOrCreate, FileAccess.Write,
                                        FileShare.Write));

                                writer.Write(BYTE_SPECIAL);
                                break;
                            }
                            case BYTE_DUMP:
                            {
                                // Read buffer params : fd, size, file data
                                int fd = reader.ReadInt32();
                                int sz = reader.ReadInt32();
                                byte[] buffer = new byte[sz];
                                buffer = reader.ReadBytes(sz);

                                // Look for file descriptor
                                foreach (var item in filesRequest)
                                {
                                    if (item.Key == fd)
                                    {
                                        FileStream dump_file = item.Value;
                                        if (dump_file == null)
                                            break;

                                        // Write to file
                                        dump_file.Write(buffer, 0, sz);

                                        break;
                                    }
                                }

                                // Send response
                                writer.Write(BYTE_SPECIAL);
                                break;
                            }
                            case BYTE_READ:
                            {
                                int size = reader.ReadInt32();
                                int count = reader.ReadInt32();
                                int fd = reader.ReadInt32();
                                if ((fd & 0x0fff00ff) == 0x0fff00ff)
                                {
                                    int handle = (fd >> 8) & 0xff;
                                    if (files[handle] == null)
                                    {
                                        writer.Write(BYTE_SPECIAL);
                                        writer.Write(-19);
                                        writer.Write(0);
                                        break;
                                    }
                                    FileStream f = files[handle];

                                    byte[] buffer = new byte[size*count];
                                    int sz = f.Read(buffer, 0, buffer.Length);
                                    writer.Write(BYTE_SPECIAL);
                                    writer.Write(sz/size);
                                    writer.Write(sz);
                                    writer.Write(buffer, 0, sz);
                                    if (reader.ReadByte() != BYTE_OK)
                                        throw new InvalidDataException();
                                }
                                else
                                {
                                    writer.Write(BYTE_NORMAL);
                                }
                                break;
                            }
                            case BYTE_CLOSE:
                            {
                                int fd = reader.ReadInt32();
                                if ((fd & 0x0fff00ff) == 0x0fff00ff)
                                {
                                    int handle = (fd >> 8) & 0xff;
                                    if (files[handle] == null)
                                    {
                                        writer.Write(BYTE_SPECIAL);
                                        writer.Write(-38);
                                        break;
                                    }
                                    FileStream f = files[handle];

                                    writer.Write(BYTE_SPECIAL);
                                    writer.Write(0);
                                    f.Close();
                                    files[handle] = null;
                                }
                                else
                                {
                                    // Check if it is a file to dump
                                    foreach (var item in filesRequest)
                                    {
                                        if (item.Key == fd)
                                        {
                                            FileStream dump_file = item.Value;
                                            if (dump_file == null)
                                                break;

                                            // Close file and remove from request list
                                            dump_file.Close();
                                            filesRequest.Remove(fd);

                                            break;
                                        }
                                    }

                                    // Send response
                                    writer.Write(BYTE_NORMAL);
                                }
                                break;
                            }
                            case BYTE_SETPOS:
                            {
                                int fd = reader.ReadInt32();
                                int pos = reader.ReadInt32();
                                if ((fd & 0x0fff00ff) == 0x0fff00ff)
                                {
                                    int handle = (fd >> 8) & 0xff;
                                    if (files[handle] == null)
                                    {
                                        writer.Write(BYTE_SPECIAL);
                                        writer.Write(-38);
                                        break;
                                    }
                                    FileStream f = files[handle];

                                    f.Position = pos;
                                    writer.Write(BYTE_SPECIAL);
                                    writer.Write(0);
                                }
                                else
                                {
                                    writer.Write(BYTE_NORMAL);
                                }
                                break;
                            }
                            case BYTE_STATFILE:
                            {
                                int fd = reader.ReadInt32();
                                if ((fd & 0x0fff00ff) == 0x0fff00ff)
                                {
                                    int handle = (fd >> 8) & 0xff;
                                    if (files[handle] == null)
                                    {
                                        writer.Write(BYTE_SPECIAL);
                                        writer.Write(-38);
                                        writer.Write(0);
                                        break;
                                    }
                                    FileStream f = files[handle];

                                    FSStat stat = new FSStat();

                                    stat.flags = FSStatFlag.None;
                                    stat.permission = 0x400;
                                    stat.owner = ids[1];
                                    stat.group = 0x101e;
                                    stat.file_size = (uint) f.Length;

                                    writer.Write(BYTE_SPECIAL);
                                    writer.Write(0);
                                    writer.Write(Marshal.SizeOf(stat));
                                    writer.Write(stat);
                                }
                                else
                                {
                                    writer.Write(BYTE_NORMAL);
                                }
                                break;
                            }
                            case BYTE_EOF:
                            {
                                int fd = reader.ReadInt32();
                                if ((fd & 0x0fff00ff) == 0x0fff00ff)
                                {
                                    int handle = (fd >> 8) & 0xff;
                                    if (files[handle] == null)
                                    {
                                        writer.Write(BYTE_SPECIAL);
                                        writer.Write(-38);
                                        break;
                                    }
                                    FileStream f = files[handle];

                                    writer.Write(BYTE_SPECIAL);
                                    writer.Write(f.Position == f.Length ? -5 : 0);
                                }
                                else
                                {
                                    writer.Write(BYTE_NORMAL);
                                }
                                break;
                            }
                            case BYTE_GETPOS:
                            {
                                int fd = reader.ReadInt32();
                                if ((fd & 0x0fff00ff) == 0x0fff00ff)
                                {
                                    int handle = (fd >> 8) & 0xff;
                                    if (files[handle] == null)
                                    {
                                        writer.Write(BYTE_SPECIAL);
                                        writer.Write(-38);
                                        writer.Write(0);
                                        break;
                                    }
                                    FileStream f = files[handle];

                                    writer.Write(BYTE_SPECIAL);
                                    writer.Write(0);
                                    writer.Write((int) f.Position);
                                }
                                else
                                {
                                    writer.Write(BYTE_NORMAL);
                                }
                                break;
                            }
                            case BYTE_PING:
                            {
                                int val1 = reader.ReadInt32();
                                int val2 = reader.ReadInt32();

                                break;
                            }
                            default:
                                throw new InvalidDataException();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(name + " " + e.Message);
            }
            finally
            {
                foreach (var item in files)
                {
                    if (item != null)
                        item.Close();
                }
                foreach (var item in filesRequest)
                {
                    if (item.Value != null)
                        item.Value.Close();
                }
            }
            Console.WriteLine(name + " Exit");
        }
    }
}