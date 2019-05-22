using System;
using Autofac;
using Polly;
using Polly.Registry;

namespace PollyMoq
{
    public class PollyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Creating policy registry.
            var registry = new PolicyRegistry();

            // Creating a retry policy.
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine("Foo");
                });


            if (PolicyContextKeys.FallbackAction != null)
            {
                // Creating a fallback policy to execute a function,
                // presumably a ServiceEventSource logger.
                var fallback = Policy
                    .Handle<Exception>()
                    .Fallback(
                        () => { /* Do nothing here. */ },
                        ex => { PolicyContextKeys.FallbackAction.Invoke(ex); }
                    );

                // Wrap the fallback policy with the retry policy.
                retryPolicy.Wrap(fallback);
            }

            // Adding the retry policy to the registry.
            registry.Add(PolicyRegistryKeys.Default, retryPolicy);

            // Register the policy registry with the container builder.
            builder.RegisterInstance(registry).As<IReadOnlyPolicyRegistry<string>>();
        }
    }
}