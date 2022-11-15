// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

namespace Standardly
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AddBrokers(services);
            AddFoundations(services);
            AddProcessings(services);
            AddOrchestrations(services);
            services.AddLogging();

            services.AddControllers()
                .AddOData(options => options.Select().Filter().Expand().OrderBy().Count().SetMaxTop(25))
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = jsonNamingPolicy;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = jsonNamingPolicy;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddSingleton(new JsonSerializerOptions
            {
                PropertyNamingPolicy = jsonNamingPolicy,
                DictionaryKeyPolicy = jsonNamingPolicy,
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
        }

        private static void AddFoundations(IServiceCollection services)
        {
        }

        private static void AddProcessings(IServiceCollection services)
        {
        }

        private static void AddOrchestrations(IServiceCollection services)
        {
        }
    }
}