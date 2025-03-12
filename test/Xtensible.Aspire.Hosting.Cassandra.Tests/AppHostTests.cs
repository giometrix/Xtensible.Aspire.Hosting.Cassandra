
using Aspire.Hosting.Testing;


namespace Xtensible.Aspire.Hosting.Cassandra.Tests;

public class AppHostTests(CassandraTestAppHostFixture fixture) : IClassFixture<CassandraTestAppHostFixture>
{
   
    [Fact]
    public async Task get_connection_string_from_running_container()
    {
        var connstring = await fixture.GetConnectionString("cassandra");
        Assert.StartsWith("Contact Points", connstring);

    }

}