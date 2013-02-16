using System;

namespace HashProviderTest
{
    public abstract class HashProvider
    {
        public virtual int SaltLength { get; set; }

        protected HashProvider()
        {
            SaltLength = 8;
        }

        public abstract byte[] GetSalt();
        public abstract byte[] GetSalt(int length);
        public abstract byte[] GetHash(byte[] data, byte[] salt);
        public abstract string GetHash(string data, byte[] salt);

        public virtual byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public virtual string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}