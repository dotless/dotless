using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotless.SourceMapping{
    /// <summary>
    /// collections of utils to encode &amp; decode numeric values to vlq (variable length base64 encoded string representations)
    /// https://github.com/mozilla/source-map/blob/master/lib/source-map/base64-vlq.js#L126
    /// </summary>
    public static class Base64Vlq {
        
        private const byte VLQ_BASE_SHIFT = 5;

          // binary: 100000
        private static byte VLQ_BASE = 1 << VLQ_BASE_SHIFT;

          // binary: 011111
        private static byte VLQ_BASE_MASK = (byte) (VLQ_BASE -  1);

          // binary: 100000
        private static byte VLQ_CONTINUATION_BIT = VLQ_BASE;

        private static long toVLQSigned(long aValue) {
            return aValue < 0 ? ((-aValue) << 1) + 1 : (aValue << 1) + 0;
        }

        private static long fromVLQSigned(long aValue) {
            bool isNegative = (aValue & 1) == 1;
            long shifted = aValue >> 1;
            return isNegative ? -shifted : shifted;
        }

        /// <summary>
        /// encodes a long value to the a variable length int-base64-endoded version 
        /// </summary>
        /// <param name="aValue">the value you want to encode</param>
        /// <returns>the variable length string representation of the given value</returns>
        public static string Encode(long aValue) {
            long vlq = toVLQSigned(aValue);    
            List<byte> data = new List<byte>();

            do {
                byte digit = (byte) (vlq & VLQ_BASE_MASK);
                // take next bytes
                vlq >>= VLQ_BASE_SHIFT;
                if (vlq > 0) {
                    // There are still more digits in this value, so we must make sure the
                    // continuation bit is marked.
                    digit |= VLQ_CONTINUATION_BIT;
                }
                data.Add(digit);
            } while (vlq > 0);

            return String.Join(String.Empty, (from byte @by in data
                                              select Base64Encode(@by)).ToArray());
                                   
        }

        ///<summary>
        /// method to decode a given string chunk to it's long value
        ///</summary>
        ///<param name="chunk">the string encoding the numeric value</param>
        ///<returns>the numeric value encoded by the string</returns>
        ///<exception cref="Exception">thrown when the given string is to short to encode a number successfully</exception>
        public static long Decode(string chunk) {
            var i = 0;
            int strLen = chunk.Count();
            long result = 0;
            byte shift = 0;
            bool continuation = false;

            // get each char as byte value
            byte[] data = (from char ch in chunk
                           select Base64Decode(ch)).ToArray();

            do {
                if (i >= strLen) {
                throw new Exception("Expected more digits in base 64 VLQ value.");
                }
                // get the char 
                byte digit = data[i++];

                // shift it to extract the value & if there is still other informationen to follow
                continuation = (digit & VLQ_CONTINUATION_BIT) == VLQ_CONTINUATION_BIT;
                digit &= VLQ_BASE_MASK;
                result = result + (digit << shift);
                shift += VLQ_BASE_SHIFT;
            } while (continuation);

            // decode
            return fromVLQSigned(result);       
        }

        /// <summary>
        /// encodes a byte to a base 64 string, since the dafault .net-implementation behaves differently from the needed version
        /// </summary>
        /// <param name="by">the byte value you want to encode</param>
        /// <returns>the string representation of the given byte</returns>
        private static string Base64Encode(byte @by) {
            const string map = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            if (@by < 0 || @by > map.Length)
                // fo the basti... no with the real only one right exception for the out of range shit!
                throw new ArgumentOutOfRangeException("value out of range");

            return Char.ToString(map[@by]);
        }

        /// <summary>
        /// decodes a base64-char back to it's byte value, since the .net-default behaves differently from the needed decoding
        /// </summary>
        /// <param name="encoded">the char to decode</param>
        /// <returns>the byte value that was encoed by the given char</returns>
        private static byte Base64Decode(char encoded) {
            const string map = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            var indx = map.IndexOf(encoded);
            if (indx < Byte.MinValue || indx > Byte.MaxValue)
                // for tDo... now with the real only right exception-type for the out of range shit!
                throw new ArgumentOutOfRangeException("value out of range");

            return (byte) indx;
        }
    }
}
