# Xtensible.Aspire.Hosting.Cassandra
[![NuGet Version](https://img.shields.io/nuget/v/Xtensible.Aspire.Hosting.Cassandra)](https://www.nuget.org/packages/Xtensible.Aspire.Hosting.Cassandra)

This project provides [Cassandra](https://cassandra.apache.org/_/index.html) and [Scylla](https://www.scylladb.com/) support for [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview). It allows you to easily add and configure Cassandra or Scylla databases to your Aspire application using the `AddCassandra` and `AddScylla` extension methods.

## Purpose

The purpose of this library is to simplify the integration of Cassandra and Scylla databases into .NET Aspire applications. It provides a set of extension methods and options that allow you to configure your Cassandra or Scylla resources with minimal code. It also provides healthchecks.

## Usage

### Adding Cassandra to your Aspire AppHost

To add a Cassandra database to your Aspire application, you can use the `AddCassandra` extension method in your AppHost project.

```csharp
// Program.cs (AppHost)
using Xtensible.Aspire.Hosting.Cassandra;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<CassandraResource> cassandra =
    builder.AddCassandra("cassandra", new()
    {
        OnResourceReadyAsync = async (r, c) =>
        {
            // optionally run some code when the resource is ready
            Console.WriteLine($"Cassandra resource {r.Name} is ready. {await r.ConnectionStringExpression.GetValueAsync(c)}");

        }
    }).PublishAsConnectionString();

builder.Build().Run();
```

This code adds a Cassandra resource named "cassandra" to your application.

### Adding Scylla to your Aspire AppHost

To add a Scylla database, which is a Cassandra-compatible database, you can use the AddScylla extension method. This uses the scylladb/scylla docker image by default

```csharp
// Program.cs (AppHost)
using Xtensible.Aspire.Hosting.Cassandra;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<CassandraResource> scylla =
    builder.AddScylla("scylla").PublishAsConnectionString();

builder.Build().Run();
```

This code adds a Scylla resource named "scylla" to your application.

### Configuring Cassandra

You can configure the Cassandra resource using the CassandraBuilderOptions class. This class allows you to set the username, password, port, and scheme for your Cassandra database.

```csharp
builder.AddCassandra("cassandra", new CassandraBuilderOptions("myuser", "mypassword")
{
    Port = 9042,
    Scheme = "tcp"
});
```

### Health Checks

This library automatically adds health checks for your Cassandra resources. These health checks can be used to monitor the health of your Cassandra databases and ensure that they are running and able to receive commands. Health Check is available as a separate Nuget package, so that you can use it in your Asp.net health checks in production.

### Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue.
