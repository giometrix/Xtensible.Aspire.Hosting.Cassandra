using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;

namespace Xtensible.Aspire.Hosting.Cassandra.Tests
{
    public class ContainerResourceCreationTests
    {
        [Fact]
        public void add_cassandra_to_builder_should_not_be_null()
        {
            IDistributedApplicationBuilder builder = null!;
            Assert.Throws<ArgumentNullException>(() => builder.AddCassandra("cassandra"));
        }

        [Fact]
        public void add_cassandra_to_builder_should_not_have_null_or_whitespace_name()
        {
            IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder();
            Assert.Throws<ArgumentNullException>(() => builder.AddCassandra(null!));
        }

        [Fact]
        public void add_cassandra_to_builder_should_not_have_null_or_whitespace_scheme()
        {
            IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder();
            Assert.Throws<ArgumentNullException>(() => builder.AddCassandra("cassandra", scheme: null!));
        }

        [Fact]
        public void add_cassandra_builder_container_details_set_on_resource()
        {
            IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder();

            builder.AddCassandra("cassandra", builder.AddParameter("username", "testuser"),
                builder.AddParameter("password", "testpass"));

            DistributedApplication app = builder.Build();
            var appModel = app.Services.GetRequiredService<DistributedApplicationModel>();

            CassandraResource resource = appModel.Resources.OfType<CassandraResource>().Single();

            Assert.Equal("cassandra", resource.Name);
            Assert.Equal("testuser", resource.UsernameParameter!.Value);
            Assert.Equal("testpass", resource.PasswordParameter!.Value);


            Assert.True(resource.TryGetLastAnnotation(out ContainerImageAnnotation? imageAnnotations));

            Assert.Equal("cassandra", imageAnnotations!.Image);
            Assert.Equal("latest", imageAnnotations!.Tag);
            Assert.Equal("docker.io", imageAnnotations!.Registry);
        }
    }
}