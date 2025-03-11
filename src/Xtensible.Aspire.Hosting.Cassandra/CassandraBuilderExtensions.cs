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
            IResourceBuilder<ParameterResource>? userName = null,
            IResourceBuilder<ParameterResource>? password = null,
            int? port = null,
            string scheme = "tcp")
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(scheme, nameof(scheme));

            var resource = new CassandraResource(name, userName?.Resource, password?.Resource);

            builder.Services.AddHealthChecks().AddAsyncCheck(name, async cancellationToken =>
            {
                var config = new CassandraHealthCheckConfig(
                    (await resource.PrimaryEndpoint.Property(EndpointProperty.Host).GetValueAsync(cancellationToken))!,
                    (await resource.UsernameReference.GetValueAsync(CancellationToken.None))!,
                    (await resource.UsernameReference.GetValueAsync(CancellationToken.None))!, 9042);
                var healthCheck = new CassandraHealthCheck(config);
                return await healthCheck.CheckHealthAsync(new HealthCheckContext(), cancellationToken);
            });


            return builder.Build(port, resource);
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