using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Polly;
using Polly.Registry;

namespace PollyMoq.Moq
{
	[TestFixture]
    public class RetryAndFallbackMoq
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var policyRegistry = new PolicyRegistry {{PolicyRegistryKeys.Default, Policy.NoOpAsync()}};

            var app = new Mock<Application>(policyRegistry);
            app.Object.Start();
        }
    }
}
