using System;
using Autofac;
using Polly;
using Polly.Registry;

namespace PollyMoq
{
    public class Application : IStartable
    {
        private const int NumberOfFailures = 5;
        private static int _tries = 0;
        private IReadOnlyPolicyRegistry<string> _policyRegistry;

        public Application(IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _policyRegistry = policyRegistry;
        }

        public void Start()
        {
            Console.WriteLine("Executing application!");

            var policy = _policyRegistry.Get<Policy>(PolicyRegistryKeys.Default);
            policy.Execute((ctx) => RetryMe(),
                new PolicyDelegateContext("ApplicationContext")
                    .WithFallbackAction((ex) =>
                    {
                        Console.WriteLine($"Invoking fallback! {ex.Message}");
                    })
            );

            Console.ReadKey();
        }

        public static void RetryMe()
        {
            if (_tries++ <= NumberOfFailures)
            {
                throw new InvalidOperationException($"Derp on the {_tries}. try.");
            }
        }
    }
}
