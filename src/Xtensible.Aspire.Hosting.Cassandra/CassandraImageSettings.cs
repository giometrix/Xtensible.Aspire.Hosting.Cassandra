namespace Xtensible.Aspire.Hosting.Cassandra;

public class CassandraImageSettings
{
    public static CassandraImageSettings Default { get; } = new();  

    public string Registry => "docker.io";
    public string Image { get; set; } = "cassandra";
    public string Tag { get; set; } = "latest";

}