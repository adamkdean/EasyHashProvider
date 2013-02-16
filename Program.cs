using System;
using System.Diagnostics;

namespace HashProviderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var examples = new string[] {
                "password", "test", "example", "^CrAzYPa55w0rd$"
            };

            var providers = new HashProvider[] {
                new SHA1HashProvider(),
                new SHA256HashProvider(),
                new SHA384HashProvider(),
                new SHA512HashProvider(),
                new RIPEMD160HashProvider(),
                new SHA256L100HashProvider(),
            };

            HashTest(examples, providers);
            HashComputeTest("example", providers);

            Console.Write("Done, press any key to exit...");
            Console.ReadKey();
        }

        static void HashTest(dynamic examples, dynamic providers)
        {
            Stopwatch stopwatch = new Stopwatch();

            foreach (var provider in providers)
            {
                string providerName = provider.GetType().Name;

                Console.WriteLine("Using {0}:", providerName);
                Console.WriteLine("");

                foreach (var example in examples)
                {
                    for (int i = 0, j = 5; i < j; i++)
                    {
                        Console.WriteLine("> Hashing `{0}` with {1} ({2}/{3})", example, providerName, i, j);

                        stopwatch.Reset();
                        stopwatch.Start();
                        var salt = provider.GetSalt();
                        var hash1 = provider.GetHash(example, salt);
                        stopwatch.Stop();

                        var elapsed = stopwatch.Elapsed.TotalMilliseconds;
                        var saltStr = provider.GetString(salt);
                        var hash2 = provider.GetHash(example, salt);
                        var hashMatch = hash1.Equals(hash2);

                        Console.WriteLine("Salt: `{0}`", saltStr);
                        Console.WriteLine("Hash: `{0}`", hash1);
                        Console.WriteLine("Hashes match? {0}", hashMatch);
                        Console.WriteLine("Elapsed: {0} ms", elapsed);

                        Console.WriteLine("");
                    }
                }

                Console.WriteLine("");
            }
        }

        static void HashComputeTest(dynamic str, dynamic providers)
        {
            Stopwatch stopwatch = new Stopwatch();

            foreach (var provider in providers)
            {
                string providerName = provider.GetType().Name;
                double elapsed = HashComputeTime(str, provider);
                double elapsedPC = elapsed / provider.HashLength;
                
                Console.WriteLine("Hashing `{0}` with `{1}`: ", str, providerName);
                Console.WriteLine("Salt length: {0}, Hash length: {1}", provider.SaltLength, provider.HashLength);
                Console.WriteLine("Elapsed: {0} ms, MS/Char: {1:0.0000} ms", elapsed, elapsedPC);
                Console.WriteLine("");
            }

            Console.WriteLine("");
        }

        static double HashComputeTime(string str, dynamic provider)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            var salt = provider.GetSalt();
            var hash1 = provider.GetHash(str, salt);
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}
