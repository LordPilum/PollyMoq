using System;
using Autofac;
using Polly;
using Polly.Fallback;
using Polly.Registry;
using Polly.Retry;

namespace PollyMoq
{
    public class Application : IStartable
    {
        const int numberOfRetries = 3;
        const int numberOfFailures = 5;
        static int tries = 0;
        private IReadOnlyPolicyRegistry<string> _policyRegistry;

        public Application(IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _policyRegistry = policyRegistry;
        }

        public void Start()
        {
            Console.WriteLine("Run, baby, run!");

            var policy = _policyRegistry.Get<Policy>(PolicyRegistryKeys.Default);
            policy.Execute((ctx) => RetryMe(),
                new Context("ApplicationContext")
                    .WithFallbackLogger((ex) =>
                    {
                        Console.WriteLine("Falling back!");
                    })
            );

            Console.ReadKey();
        }

        public static string RetryMe()
        {
            if (tries++ <= numberOfFailures)
            {
                throw new InvalidOperationException($"Derp on the {tries}. try.");
            }

            return "How did I end up here?";
        }
    }
}
