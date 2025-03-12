
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


    [Fact]
    public async Task cassandra_ready_for_requests()
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            var resource = await fixture.App.ResourceNotifications.WaitForResourceHealthyAsync("cassandra", WaitBehavior.StopOnResourceUnavailable, cts.Token);
        }
        catch (OperationCanceledException e)
        {
            Assert.Fail("Cassandra did not become healthy within 10 minutes");
        }
       
       
    }

}