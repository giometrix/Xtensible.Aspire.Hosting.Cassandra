using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xtensible.Cassandra.HealthChecks;

namespace Xtensible.Aspire.Hosting.Cassandra
{
    public static class CassandraBuilderExtensions
    {
        public static IResourceBuilder<CassandraResource> AddCassandra(this IDistributedApplicationBuilder builder,
            [ResourceName] string name,
            CassandraBuilderOptions? options = null, CassandraImageSettings? imageSettings = null)
        {

            options ??= new CassandraBuilderOptions();
            imageSettings ??= CassandraImageSettings.Default;

            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(options.Scheme, nameof(options.Scheme));

            var resource = new CassandraResource(name, options.Username, options.Password, imageSettings);

            builder.Services.AddHealthChecks().AddAsyncCheck(name, async cancellationToken =>
            {
                int port = int.TryParse(await resource.PrimaryEndpoint.Property(EndpointProperty.Port).GetValueAsync(cancellationToken), out int p) ? p : 9042;

                var config = new CassandraHealthCheckConfig(
                    (await resource.PrimaryEndpoint.Property(EndpointProperty.Host).GetValueAsync(cancellationToken))!,
                    (await resource.UsernameReference.GetValueAsync(CancellationToken.None))!,
                    (await resource.UsernameReference.GetValueAsync(CancellationToken.None))!, port);
                var healthCheck = new CassandraHealthCheck(config);
                return await healthCheck.CheckHealthAsync(new HealthCheckContext(), cancellationToken);
            });

            if(options.OnResourceReadyAsync is not null)
            {
                builder.Eventing.Subscribe<ResourceReadyEvent>(async (e, c) =>
                {
                    if (e.Resource == resource)
                    {
                        await options.OnResourceReadyAsync(resource, c);
                    }
                });
            }


            return builder.Build(options.Port, resource);
        }

        public static IResourceBuilder<CassandraResource> AddScylla(this IDistributedApplicationBuilder builder,
            [ResourceName] string name,
            CassandraBuilderOptions? options = null, ScyllaImageSettings? imageSettings = null)
        {
            imageSettings ??= ScyllaImageSettings.Default;

            return AddCassandra(builder, name, options, imageSettings);
        }

        private static IResourceBuilder<CassandraResource> Build(this IDistributedApplicationBuilder builder, int? port,
            CassandraResource cassandra)
        {
            IResourceBuilder<CassandraResource> result = builder.AddResource(cassandra)
                .WithImage(cassandra.ImageSettings.Image, cassandra.ImageSettings.Tag)
                .WithImageRegistry(cassandra.ImageSettings.Registry)
                .WithEndpoint(port, 9042, name: CassandraResource.PrimaryEndpointName,
                    scheme: CassandraResource.PrimaryEndpointName)
                .WithHealthCheck(cassandra.Name);
            return result;
        }
    }
}