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

        private async ValueTask<bool> WithRetry(ReturningBooleanFunction returningBooleanFunction)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    attempts++;
                    return await returningBooleanFunction();
                }
                catch (Exception ex)
                {
                    if (retryExceptionTypes.Any(exception => exception == ex.GetType()))
                    {
                        this.loggingBroker
                            .LogInformation(
                                $"Error found. Retry attempt {attempts}/{this.retryConfig.MaxRetryAttempts}. " +
                                    $"Exception: {ex.Message}");

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

        private async ValueTask<string> WithRetry(ReturningStringFunction returningStringFunction)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    attempts++;
                    return await returningStringFunction();
                }
                catch (Exception ex)
                {
                    if (retryExceptionTypes.Any(exception => exception == ex.GetType()))
                    {
                        this.loggingBroker
                            .LogInformation(
                                $"Error found. Retry attempt {attempts}/{this.retryConfig.MaxRetryAttempts}. " +
                                    $"Exception: {ex.Message}");

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

        private async ValueTask<List<string>> WithRetry(ReturningStringListFunction returningStringListFunction)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    attempts++;
                    return await returningStringListFunction();
                }
                catch (Exception ex)
                {
                    if (retryExceptionTypes.Any(exception => exception == ex.GetType()))
                    {
                        this.loggingBroker
                            .LogInformation(
                                $"Error found. Retry attempt {attempts}/{this.retryConfig.MaxRetryAttempts}. " +
                                    $"Exception: {ex.Message}");

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

        private async ValueTask WithRetry(ReturningNothingFunction returningNothingFunction)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    attempts++;
                    await returningNothingFunction();
                }
                catch (Exception ex)
                {
                    if (retryExceptionTypes.Any(exception => exception == ex.GetType()))
                    {
                        this.loggingBroker
                            .LogInformation(
                                $"Error found. Retry attempt {attempts}/{this.retryConfig.MaxRetryAttempts}. " +
                                    $"Exception: {ex.Message}");

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
