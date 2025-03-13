namespace Xtensible.Aspire.Hosting.Cassandra
{
    public class ScyllaImageSettings : CassandraImageSettings
    {
        public static new ScyllaImageSettings Default { get; } = new();

        public ScyllaImageSettings()
        {
            Image = "scylladb/scylla";
        }
    }
}