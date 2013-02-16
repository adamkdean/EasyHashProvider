using System;
using System.Linq;
using System.Security.Cryptography;

namespace HashProviderTest
{
    public class SHA1HashProvider : HashProvider
    {
        private HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();

        public override byte[] GetSalt()
        {
            return GetSalt(SaltLength);
        }

        public override byte[] GetSalt(int length)
        {
            var buffer = new byte[length];
            Random random = new Random(DateTime.Now.Millisecond);
            random.NextBytes(buffer);
            return buffer;
        }

        public override byte[] GetHash(byte[] data, byte[] salt)
        {
            byte[] combined = data.Concat(salt).ToArray();
            byte[] hash = hashAlgorithm.ComputeHash(combined);
            return hash;
        }

        public override string GetHash(string data, byte[] salt)
        {
            var dataBytes = GetBytes(data);
            var hashBytes = GetHash(dataBytes, salt);
            var hashString = GetString(hashBytes);
            return hashString;
        }
    }
}