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


builder.Build().Run();