/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-11-01                        | 
'| Use: General                                         |
' \====================================================/
*/
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;

using K2host.Core;

namespace K2host.Services
{
    public static class OHelpers
    {

        public static string ByteArrayToDataString(this byte[] data, string delimiter = "")
        {
            var hex = new StringBuilder(data.Length * 2);
            data.ForEach(b => { hex.Append($"{b:x2}" + delimiter); });
            return hex.ToString();
        }

        public static byte[] DataStringToByteArray(this string hex, bool trim = true)
        {
            if (trim)
                hex = hex.Replace(" ", string.Empty);

            int nc = hex.Length;
            byte[] bytes = new byte[nc / 2];

            for (int i = 0; i < nc; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return bytes;
        }

        public static string BitsToString(this BitArray bits)
        {
            int i = 1;
            var derp = string.Empty;
            foreach (var bit in bits)
            {
                derp += Convert.ToInt32(bit);
                if (i % 8 == 0)
                    derp += " ";
                i++;
            }
            return derp.Trim();
        }

        public static byte[] IntToBerLength(this int length)
        {

            if (length <= 127)
                return new byte[] { (byte)length }; // Short notation
            else
            {
                var intbytes = BitConverter.GetBytes(length);
                Array.Reverse(intbytes);
                byte intbyteslength = (byte)intbytes.Length;
                var lengthByte = intbyteslength + 128;
                var berBytes = new byte[1 + intbyteslength];
                berBytes[0] = (byte)lengthByte;
                Buffer.BlockCopy(intbytes, 0, berBytes, 1, intbyteslength);
                return berBytes;                    // Long notation
            }
        }

        public static int BerLengthToInt(this byte[] bytes, int offset, out int berByteCount)
        {
            return new MemoryStream(bytes, offset, bytes.Length - offset, false).BerLengthToInt(out berByteCount);
        }

        public static int BerLengthToInt(this Stream stream, out int berByteCount)
        {
            berByteCount = 1;   // The minimum length of a ber encoded length is 1 byte

            int attributeLength;
            var berByte = new byte[1];

            stream.Read(berByte, 0, 1);

            if (berByte[0] >> 7 == 1)    // Long notation, first byte tells us how many bytes are used for the length
            {
                var lengthoflengthbytes = berByte[0] & 127;
                var lengthBytes = new byte[lengthoflengthbytes];
                stream.Read(lengthBytes, 0, lengthoflengthbytes);
                Array.Reverse(lengthBytes);
                Array.Resize(ref lengthBytes, 4);   // this will of course explode if length is larger than a 32 bit integer
                attributeLength = BitConverter.ToInt32(lengthBytes, 0);
                berByteCount += lengthoflengthbytes;
            }
            else // Short notation, length contained in the first byte
                attributeLength = berByte[0] & 127;

            return attributeLength;
        }

        public static string Repeat(this string stuff, int n) => string.Concat(Enumerable.Repeat(stuff, n));

    }
}
