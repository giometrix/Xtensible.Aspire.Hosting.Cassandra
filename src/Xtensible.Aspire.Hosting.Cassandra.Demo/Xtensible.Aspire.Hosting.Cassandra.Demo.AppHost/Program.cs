using Xtensible.Aspire.Hosting.Cassandra;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<CassandraResource> cassandra =
    builder.AddCassandra("cassandra", new()
    {
        Port = 9042,
        OnResourceReadyAsync = async (r, c) =>
        {
            Console.WriteLine($"Cassandra resource {r.Name} is ready. {await r.ConnectionStringExpression.GetValueAsync(c)}");

        }
    }).PublishAsConnectionString();

// Consider using WithLifetime(ContainerLifetime.Persistent) to keep the container running after the application exits, Cassandra takes a minute to start up. 
//.WithLifetime(ContainerLifetime.Persistent);


builder.Build().Run();