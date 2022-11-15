// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;

namespace Standardly.Core.Models.Configurations.Retries
{
    public class RetryConfig : IRetryConfig
    {
        public RetryConfig()
        {
            MaxRetryAttempts = 5;
            PauseBetweenFailures = TimeSpan.FromSeconds(3);
        }

        public RetryConfig(int maxRetryAttempts, TimeSpan pauseBetweenFailures)
        {
            MaxRetryAttempts = maxRetryAttempts;
            PauseBetweenFailures = pauseBetweenFailures;
        }

        public int MaxRetryAttempts { get; private set; }
        public TimeSpan PauseBetweenFailures { get; private set; }
    }
}
