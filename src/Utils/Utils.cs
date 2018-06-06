using System;
using System.Security.Cryptography;
using System.Text;

namespace FroalaEditor
{
    /// <summary>
    /// Basic utility functionality.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        public static string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generate an unique string based on current timestamp.
        /// </summary>
        /// <returns>Unique sha1 string</returns>
        public static string GenerateUniqueString()
        {
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            return SHA1HashStringForUTF8String(milliseconds.ToString());
        }

        /// <summary>
        /// Get file extension.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <returns>Extension without the dot.</returns>
        public static string GetFileExtension(string filename)
        {
            return filename.Substring(filename.LastIndexOf('.') + 1);
        }

        /// <summary>
        /// Convert plain text string to a base 64 encoded string.
        /// </summary>
        /// <param name="plainText">String to convert.</param>
        /// <returns>Base 64 converted string.</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Convert a string to a byte array.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>Converted bye array.</returns>
        public static byte[] ToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str.ToCharArray());
        }

        /// <summary>
        /// Convert byte array to a hex string.
        /// </summary>
        /// <param name="bytes">Byte array.</param>
        /// <returns>Hex string.</returns>
        public static string HexEncode(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }

        /// <summary>
        /// Generate HMAC256 hash.
        /// </summary>
        /// <param name="key">Byte array key.</param>
        /// <param name="value">String value to hash.</param>
        /// <returns>Byte array hash.</returns>
        public static byte[] HMAC256(byte[] key, string value)
        {
            HMACSHA256 hmac = new HMACSHA256(key);
            return hmac.ComputeHash(ToBytes(value));
        }
    }
}
