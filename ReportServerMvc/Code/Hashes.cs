using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace fyiReporting.ReportServerMvc.Code
{
    public class Hashes
    {
        /// <summary>
        /// Creates a SHA512 hash of a string.
        /// </summary>
        /// <param name="nonHashedString">
        /// The string to hash<see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A SHA512 hash <see cref="System.String"/>
        /// </returns>
        public static string GetSHA512StringHash(string nonHashedString)
        {
            string hashValue = "";
            using (SHA512 _sha512 = new SHA512Managed())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(nonHashedString);
                byte[] retVal = _sha512.ComputeHash(data);
                hashValue = BitConverter.ToString(retVal).Replace("-", ""); // hex string

            }


            return hashValue;
        }
    }
}