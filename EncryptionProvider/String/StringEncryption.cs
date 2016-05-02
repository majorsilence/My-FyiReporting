using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace EncryptionProvider.String
{
    public class StringEncryption
    {
        private readonly Random random;
        private readonly byte[] key;
        private readonly RijndaelManaged rm;
        private readonly UTF8Encoding encoder;

        public StringEncryption(string pass)
        {
            //make sure that the password contains base 64 characters ONLY
            string codes = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
            StringBuilder sb = new StringBuilder();
            foreach (char c in pass)
            {
                if (codes.Contains(c))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append("+");
                }
            }

            //make sure that the length of the password is divisible by 4
            int padding = sb.Length%4;
            if (padding > 0 )
            {
                for (int i = 4; i > padding; i--)
                {
                    sb = sb.Append(Convert.ToString(i));
                }
            }

            this.random = new Random();
            this.rm = new RijndaelManaged();
            this.encoder = new UTF8Encoding();
            this.key = Convert.FromBase64String(sb.ToString());

        }

        public string Encrypt(string unencrypted)
        {
            var vector = new byte[16];
            this.random.NextBytes(vector);
            var cryptogram = vector.Concat(this.Encrypt(this.encoder.GetBytes(unencrypted), vector));
            return Convert.ToBase64String(cryptogram.ToArray());
        }

        public string Decrypt(string encrypted)
        {
            var cryptogram = Convert.FromBase64String(encrypted);
            if (cryptogram.Length < 17)
            {
                throw new ArgumentException("Not a valid encrypted string", "encrypted");
            }

            var vector = cryptogram.Take(16).ToArray();
            var buffer = cryptogram.Skip(16).ToArray();
            return this.encoder.GetString(this.Decrypt(buffer, vector));
        }

        private byte[] Encrypt(byte[] buffer, byte[] vector)
        {
            var encryptor = this.rm.CreateEncryptor(this.key, vector);
            return this.Transform(buffer, encryptor);
        }

        private byte[] Decrypt(byte[] buffer, byte[] vector)
        {
            var decryptor = this.rm.CreateDecryptor(this.key, vector);
            return this.Transform(buffer, decryptor);
        }

        private byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            var stream = new MemoryStream();
            using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }

            return stream.ToArray();
        }

    }
}
