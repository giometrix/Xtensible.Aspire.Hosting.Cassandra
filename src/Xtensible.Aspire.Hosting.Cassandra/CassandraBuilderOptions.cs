using Aspire.Hosting.ApplicationModel;

namespace Xtensible.Aspire.Hosting.Cassandra;

public class CassandraBuilderOptions
{
    public IResourceBuilder<ParameterResource>? Username { get; set; } = null;
    public IResourceBuilder<ParameterResource>? Password { get; set; } = null;
    public int? Port { get; set; } = null;
    public string Scheme { get; set; } = "tcp";

    public Func<CassandraResource, CancellationToken, Task>? OnResourceReadyAsync { get; set; }
}