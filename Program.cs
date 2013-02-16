using System;

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
                new MD5HashProvider(), new SHA1HashProvider()                
            };


            foreach (var provider in providers)
            {
                Console.WriteLine("Using {0}:", provider.GetType().Name);
                foreach (var example in examples)
                {
                    Console.WriteLine("> Hashing {0}", example);

                    var salt = provider.GetSalt();
                    var saltStr = provider.GetString(salt);
                    var hash = provider.GetHash(example, salt);

                    Console.WriteLine("Salt: {0}", saltStr);
                    Console.WriteLine("Hash: {0}", hash);
                    Console.WriteLine("");
                }
                Console.WriteLine("");
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
