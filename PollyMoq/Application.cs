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

            /*var policy = _policyRegistry.Get<Policy>(PolicyRegistryKeys.Default);
            policy.Execute((ctx) => RetryMeVoid(),
                new PolicyDelegateContext("ApplicationContext")
                    .WithFallbackAction((ex) =>
                    {
                        Console.WriteLine($"Invoking fallback! {ex.Message}");
                    })
            );*/

            var policyStr = _policyRegistry.Get<Policy>(PolicyRegistryKeys.String);
            policyStr.Execute((ctx) => RetryMeStr(),
                new PolicyDelegateContext("ApplicationContext")
                    .WithFallbackAction((ex) =>
                    {
                        Console.WriteLine($"Invoking fallback! {ex.Message}");
                    })
            );

            Console.ReadKey();
        }

        public static void RetryMeVoid()
        {
            if (_tries++ <= NumberOfFailures)
            {
                throw new InvalidOperationException($"Fallback(void) entered on the {_tries}. try.");
            }
        }

        public static string RetryMeStr()
        {
            if (_tries++ <= NumberOfFailures)
            {
                throw new InvalidOperationException($"Fallback(string) entered on the {_tries}. try.");
            }

            return "Does not compute!";
        }

        public static int RetryMeInt()
        {
            if (_tries++ <= NumberOfFailures)
            {
                throw new InvalidOperationException($"Fallback(int) entered on the {_tries}. try.");
            }

            return 17;
        }

        public static decimal RetryMeDec()
        {
            if (_tries++ <= NumberOfFailures)
            {
                throw new InvalidOperationException($"Fallback(decimal) entered on the {_tries}. try.");
            }

            return 1.28M;
        }
    }
}
