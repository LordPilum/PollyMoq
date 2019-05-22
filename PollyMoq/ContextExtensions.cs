﻿using System;
using Polly;

namespace PollyMoq
{
    public static class ContextExtensions
    {
        public static Context WithFallbackLogger(this Context context, Action<Exception> func)
        {
            context.Add(PolicyContextKeys.FallbackAction, func);
            return context;
        }
    }
}
