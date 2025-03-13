namespace Xtensible.Aspire.Hosting.Cassandra
{
    public class CassandraImageSettings
    {
        public static CassandraImageSettings Default { get; } = new();

        public static CassandraImageSettings ScyllaDefault { get; } = new()
        {
            Image = "scylladb/scylla", Tag = "latest"
        };

        public string Registry => "docker.io";
        public virtual string Image { get; set; } = "cassandra";
        public string Tag { get; set; } = "latest";
    }
}