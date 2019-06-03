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
                .WaitAndRetry(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount}");
                });

            var retryPolicyStr = Policy<string>
                .Handle<Exception>()
                .WaitAndRetry(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount}");
                });


            // Creating a fallback policy to execute a function,
            // presumably a ServiceEventSource logger.
            var fallback = Policy
                .Handle<Exception>()
                .Fallback(
                    (exception, context, token) => {},
                    (exception, context) =>
                    {
                        if (!(context is PolicyDelegateContext delegateContext))
                            throw exception;

                        delegateContext.FallbackAction?.Invoke(exception);
                    }
                );

            // Wrap the fallback policy with the retry policy.
            var defaultPolicy = fallback.Wrap(retryPolicy);

            // Creating a fallback policy for void,
            // and wrapping it with the retry policy.
            var stringPolicy = Policy<string>
                .Handle<Exception>()
                .Fallback(
                    (exception, context, token) => string.Empty,
                    (result, context) =>
                    {
                        if (!(context is PolicyDelegateContext delegateContext))
                            throw result.Exception;

                        delegateContext.FallbackAction?.Invoke(result.Exception);
                    }
                ).Wrap(retryPolicyStr);

            // Adding the retry policies to the registry.
            registry.Add(PolicyRegistryKeys.Default, defaultPolicy);
            registry.Add(PolicyRegistryKeys.String, stringPolicy);

            // Register the policy registry with the container builder.
            builder.RegisterInstance(registry).As<IReadOnlyPolicyRegistry<string>>();
        }
    }
}