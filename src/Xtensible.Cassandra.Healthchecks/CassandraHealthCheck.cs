using Cassandra;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Xtensible.Cassandra.HealthChecks
{

    public record  CassandraHealthCheckConfig(string ContactPoint, string Username, string Password, int Port);

    public class CassandraHealthCheck : IHealthCheck
    {
        private readonly CassandraHealthCheckConfig _config;
        private Cluster? _cluster;

        public CassandraHealthCheck(CassandraHealthCheckConfig config)
        {
            _config = config;
            ArgumentNullException.ThrowIfNull(config, nameof(config));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (_cluster is null)
                {
                    
                    Builder? builder = Cluster.Builder().AddContactPoint(_config.ContactPoint).WithPort(_config.Port);

                    if (!string.IsNullOrEmpty(_config.Username) && !string.IsNullOrEmpty(_config.Password))
                    {
                        builder = builder.WithCredentials(_config.Username, _config.Password);
                    }

                    _cluster = builder.Build();
                }

                using ISession? session = await _cluster.ConnectAsync();
                using var rs = await session.ExecuteAsync(new SimpleStatement("SELECT release_version FROM system.local"));

                var row = rs.FirstOrDefault();
                if (row is not null)
                {
                    return HealthCheckResult.Healthy($"Cassandra is up. Version: {row["release_version"]}");
                }

                return HealthCheckResult.Unhealthy("Cassandra connection established, but query returned no results.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Cassandra health check failed: {ex.Message}");
            }
        }
    }
}