using Xtensible.Aspire.Hosting.Cassandra;

var builder = DistributedApplication.CreateBuilder(args);

var cassandra = builder.AddCassandra("cassandra", port: 9042).PublishAsConnectionString();

builder.Build().Run();
