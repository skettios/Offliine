﻿using System;
using System.Globalization;
using Java.IO;
using Java.Lang;

namespace Offliine.Injection
{
    public class Util
    {
        public static void WriteU32(int value, OutputStream output)
        {
            output.Write(value >> 24 & 0xff);
            output.Write(value >> 16 & 0xff);
            output.Write(value >> 8 & 0xff);
            output.Write(value >> 0 & 0xff);
        }

        public static byte[] ToArray(string value)
        {
            var data = new byte[value.Length / 2];

            for (var i = 0; i < data.Length; i++)
            {
                var val = value.Substring(i*2, 2);
                data[i] = (byte) int.Parse(val, NumberStyles.HexNumber);
            }

            return data;
        }

        public static byte[] ReadFile(File file)
        {
            var input = new FileInputStream(file);
            var output = new ByteArrayOutputStream();

            var buffer = new byte['Ѐ'];

            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, read);

            input.Close();

            return output.ToByteArray();
        }
    }
}