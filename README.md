# EasyHashProvider

Secure, easy hash providers for password hashing/salting.

## Install via NuGet

EasyHashProvider is available as a NuGet package.  

To install EasyHashProvider, run the following command in the Package Manager Console

    PM> Install-Package EasyHashProvider

## How to use

Use one of the built in hash providers:

    var provider = new SHA1HashProvider();
    var salt = provider.GetSalt(); // save this, you'll need it
    var hash = provider.GetHash("YourPassword", salt);

You can also set the `SaltLength`, default is set to 8 characters, you can either change the value for all salts or just for a single salt.

    provider.SaltLength = 16; 
    var salt = provider.GetSalt(16);

To get the length of the hash:

    var hashLength = provider.HashLength;

## HashProviders

There are four Secure Hash Algorithms (SHA) implemented; SHA1, SHA256, SHA384, and SHA512.

    SHA1HashProvider()
    SHA256HashProvider()
    SHA384HashProvider()
    SHA512HashProvider()

RIPEMD-160 (RACE Integrity Primitives Evaluation Message Digest) 160-bit MD algorithm.

    RIPEMD160HashProvider()

And a custom SHA256 hash that loops 100 times for extra saltiness!

    SHA256L100HashProvider()

## Custom HashProviders

It's easy to write your own custom hash provider, let us take a look at `SHA1HashProvider`.

We extend `HashProvider`, which gives us a property and four methods that we have to implement.

`HashLength` should return the length of the hash when computed.

`GetSalt()` should return a salt as a `string`, the default length is set in the property `SaltLength`.

`GetSalt(int length)` should return a salt of the length provided.

`GetHash(string data, string salt)` should return a hash in the format of a `string`.


    public class SHA1HashProvider : HashProvider
    {
        private SHA1Managed hashAlgorithm = new SHA1Managed();

        public override int HashLength
        {
            get { return hashAlgorithm.HashSize; }
        }

        public override string GetSalt()
        {
            return GetSalt(SaltLength);
        }

        public override string GetSalt(int length)
        {
            var buffer = new byte[length];
            Random random = new Random(DateTime.Now.Millisecond);
            random.NextBytes(buffer);
            return BitConverter.ToString(buffer).Replace("-", "").ToLowerInvariant();
        }

        public override string GetHash(string data, string salt)
        {
            var combined = string.Format("{0}{1}", data, salt);
            var bytes = Encoding.UTF8.GetBytes(combined);
            var hash = hashAlgorithm.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }