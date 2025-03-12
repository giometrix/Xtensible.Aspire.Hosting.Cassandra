using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Xtensible.Aspire.Hosting.Cassandra.Tests
{
    public class CassandraTestAppHostFixture() : DistributedApplicationFactory(typeof(Projects.Xtensible_Aspire_Hosting_Cassandra_Demo_AppHost)), IAsyncLifetime
    {
        public DistributedApplication App { get; private set; } = null!;

        protected override void OnBuilt(DistributedApplication application)
        {
            App = application;
            base.OnBuilt(application);
        }

        protected override void OnBuilderCreated(DistributedApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Services.AddLogging(builder =>
                {
                    if (Environment.GetEnvironmentVariable("RUNNER_DEBUG") is not null or "1")
                        builder.SetMinimumLevel(LogLevel.Trace);
                    else
                        builder.SetMinimumLevel(LogLevel.Information);
                })
                .ConfigureHttpClientDefaults(clientBuilder => clientBuilder.AddStandardResilienceHandler());

            base.OnBuilderCreated(applicationBuilder);
        }


        public Task InitializeAsync() => StartAsync().WaitAsync(TimeSpan.FromMinutes(10));

        public async Task DisposeAsync()
        {
            try
            {
                await base.DisposeAsync();
            }
            catch (Exception)
            {
                // Ignore exceptions during disposal
            }
        }
    }
}