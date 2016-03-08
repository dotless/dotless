using System;
using System.Collections.Generic;
using System.Linq;

namespace dotless.Core.Utils
{
    /// <summary>
    /// collections of utils to encode &amp; decode numeric values to vlq (variable length base64 encoded string representations)
    /// https://github.com/mozilla/source-map/blob/master/lib/source-map/base64-vlq.js#L126
    /// </summary>
    public static class Base64Vlq
    {

        private const byte VlqBaseShift = 5;

        // binary: 100000
        private static readonly byte VlqBase = 1 << VlqBaseShift;

        // binary: 011111
        private static readonly byte VlqBaseMask = (byte)(VlqBase - 1);

        // binary: 100000
        private static readonly byte VlqContinuationBit = VlqBase;

        private static long ToVlqSigned(long aValue)
        {
            return aValue < 0 ? ((-aValue) << 1) + 1 : (aValue << 1) + 0;
        }

        private static long FromVlqSigned(long aValue)
        {
            bool isNegative = (aValue & 1) == 1;
            long shifted = aValue >> 1;
            return isNegative ? -shifted : shifted;
        }

        /// <summary>
        /// encodes a long value to the a variable length int-base64-endoded version 
        /// </summary>
        /// <param name="aValue">the value you want to encode</param>
        /// <returns>the variable length string representation of the given value</returns>
        public static string Encode(long aValue)
        {
            long vlq = ToVlqSigned(aValue);
            List<byte> data = new List<byte>();

            do
            {
                byte digit = (byte)(vlq & VlqBaseMask);
                // take next bytes
                vlq >>= VlqBaseShift;
                if (vlq > 0)
                {
                    // There are still more digits in this value, so we must make sure the
                    // continuation bit is marked.
                    digit |= VlqContinuationBit;
                }
                data.Add(digit);
            } while (vlq > 0);

            return string.Join(string.Empty, data.Select(Base64Encode).ToArray());

        }

        ///<summary>
        /// method to decode a given string chunk to it's long value
        ///</summary>
        ///<param name="chunk">the string encoding the numeric value</param>
        ///<returns>the numeric value encoded by the string</returns>
        ///<exception cref="Exception">thrown when the given string is to short to encode a number successfully</exception>
        public static long Decode(string chunk)
        {
            var i = 0;
            int strLen = chunk.Length;
            long result = 0;
            byte shift = 0;
            bool continuation;

            // get each char as byte value
            byte[] data = chunk.Select(Base64Decode).ToArray();

            do
            {
                if (i >= strLen)
                {
                    throw new Exception("Expected more digits in base 64 VLQ value.");
                }
                // get the char 
                byte digit = data[i++];

                // shift it to extract the value & if there is still other informationen to follow
                continuation = (digit & VlqContinuationBit) == VlqContinuationBit;
                digit &= VlqBaseMask;
                result = result + (digit << shift);
                shift += VlqBaseShift;
            } while (continuation);

            // decode
            return FromVlqSigned(result);
        }

        const string Base64Map = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        /// <summary>
        /// encodes a byte to a base 64 string, since the dafault .net-implementation behaves differently from the needed version
        /// </summary>
        /// <param name="value">the byte value you want to encode</param>
        /// <returns>the string representation of the given byte</returns>
        private static string Base64Encode(byte value)
        {
            if (value > Base64Map.Length) {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return char.ToString(Base64Map[value]);
        }

        /// <summary>
        /// decodes a base64-char back to its byte value, since the .net-default behaves differently from the needed decoding
        /// </summary>
        /// <param name="encoded">the char to decode</param>
        /// <returns>the byte value that was encoed by the given char</returns>
        private static byte Base64Decode(char encoded)
        {
            var indx = Base64Map.IndexOf(encoded);
            if (indx == -1) {
                throw new ArgumentOutOfRangeException(nameof(encoded));
            }

            return (byte)indx;
        }
    }
}
