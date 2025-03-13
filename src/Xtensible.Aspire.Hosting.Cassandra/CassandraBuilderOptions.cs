using Aspire.Hosting.ApplicationModel;

namespace Xtensible.Aspire.Hosting.Cassandra;

public class CassandraBuilderOptions(ParameterResource username, ParameterResource password)
{
    private const string _defaultUserName = "cassandra";
    private const string _defaultPassword = "cassandra";

    public ParameterResource Username { get; set; } = username;
    public ParameterResource Password { get; set; } = password;
    public int? Port { get; set; } = null;
    public string Scheme { get; set; } = "tcp";

    public Func<CassandraResource, CancellationToken, Task>? OnResourceReadyAsync { get; set; }


    public CassandraBuilderOptions() : 
        this(new ParameterResource("username", _ => _defaultUserName, false), 
            new ParameterResource("password", _ => _defaultPassword, true))
    {
    }

    public CassandraBuilderOptions(string username, string password): 
        this(new ParameterResource("username", _ => username, false), 
    new ParameterResource("password", _ => password, true))
    {
        
    }
}