using Xtensible.Aspire.Hosting.Cassandra;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<CassandraResource> cassandra =
    builder.AddCassandra("cassandra", port: 9042).PublishAsConnectionString();

builder.Build().Run();