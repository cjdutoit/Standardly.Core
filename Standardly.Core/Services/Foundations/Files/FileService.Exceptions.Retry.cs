// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Standardly.Core.Services.Foundations.Files
{
    public partial class FileService
    {
        private readonly List<Type> retryExceptionTypes =
            new List<Type>()
            {
                typeof(IOException)
            };

        private bool WithRetry(ReturningBooleanFunction returningBooleanFunction)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    attempts++;
                    return returningBooleanFunction();
                }
                catch (Exception ex)
                {
                    if (retryExceptionTypes.Any(exception => exception == ex.GetType()))
                    {
                        if (attempts == this.retryConfig.MaxRetryAttempts)
                        {
                            throw;
                        }

                        Task.Delay(this.retryConfig.PauseBetweenFailures).Wait();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private string WithRetry(ReturningStringFunction returningStringFunction)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    attempts++;
                    return returningStringFunction();
                }
                catch (Exception ex)
                {
                    if (retryExceptionTypes.Any(exception => exception == ex.GetType()))
                    {
                        if (attempts == this.retryConfig.MaxRetryAttempts)
                        {
                            throw;
                        }

                        Task.Delay(this.retryConfig.PauseBetweenFailures).Wait();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private List<string> WithRetry(ReturningStringListFunction returningStringListFunction)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    attempts++;
                    return returningStringListFunction();
                }
                catch (Exception ex)
                {
                    if (retryExceptionTypes.Any(exception => exception == ex.GetType()))
                    {
                        if (attempts == this.retryConfig.MaxRetryAttempts)
                        {
                            throw;
                        }

                        Task.Delay(this.retryConfig.PauseBetweenFailures).Wait();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
