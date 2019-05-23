using System;
using System.Collections.Generic;
using Polly;

namespace PollyMoq
{
    internal class PolicyDelegateContext : Context
    {
        // Constructors
        public PolicyDelegateContext() { }
        public PolicyDelegateContext(string operationKey) { }
        public PolicyDelegateContext(string operationKey, IDictionary<string, object> contextData) { }
        public PolicyDelegateContext(string operationKey, IDictionary<string, object> contextData,
            Action<Exception> fallbackAction) : base(operationKey, contextData)
        {
            FallbackAction = fallbackAction;
        }

        public Action<Exception> FallbackAction { get; private set; }

        public PolicyDelegateContext WithFallbackAction(Action<Exception> func)
        {
            FallbackAction = func;
            return this;
        }
    }
}
