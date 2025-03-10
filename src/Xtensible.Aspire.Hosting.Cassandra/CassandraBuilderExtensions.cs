using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Xtensible.Aspire.Hosting.Cassandra;

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

        return builder.Build(port, resource);
    }

    private static IResourceBuilder<CassandraResource> Build(this IDistributedApplicationBuilder builder, int? port, CassandraResource cassandra)
    {
        IResourceBuilder<CassandraResource> result = builder.AddResource(cassandra)
            .WithImage(cassandra.ImageSettings.Image, cassandra.ImageSettings.Tag)
            .WithImageRegistry(cassandra.ImageSettings.Registry)
            .WithEndpoint(port: port, targetPort: 9042, name: CassandraResource.PrimaryEndpointName, scheme: CassandraResource.PrimaryEndpointName);
        return result; // todo: add health check
    }
}