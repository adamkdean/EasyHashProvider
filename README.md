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

## License

Copyright (c) 2013, Adam K Dean. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

- Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
- Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.