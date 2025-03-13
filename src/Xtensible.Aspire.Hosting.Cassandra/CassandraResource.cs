using Aspire.Hosting.ApplicationModel;

namespace Xtensible.Aspire.Hosting.Cassandra
{
    /// <summary>
    ///     A resource that represents a Cassandra resource
    /// </summary>
    /// <param name="name">The name of the resource</param>
    /// <param name="userName">Server username. Use null for default</param>
    /// <param name="password">Server password. Use null for default</param>
    /// <param name="imageSettings">The image settings for the Cassandra container resource. Use null for default</param>
    public class CassandraResource(
        string name,
        ParameterResource userName,
        ParameterResource password,
        CassandraImageSettings? imageSettings = null) : ContainerResource(name), IResourceWithConnectionString
    {
        internal const string PrimaryEndpointName = "tcp";
       

        private EndpointReference? _primaryEndpoint;

        /// <summary>
        ///     The primary endpoint for Cassandra.
        /// </summary>
        public EndpointReference PrimaryEndpoint =>
            _primaryEndpoint ??= new EndpointReference(this, PrimaryEndpointName);


        /// <summary>
        ///     The username parameter for the Cassandra resource.
        /// </summary>
        public ParameterResource? UsernameParameter { get; } = userName;

        /// <summary>
        ///     The password parameter for the Cassandra resource.
        /// </summary>
        public ParameterResource? PasswordParameter { get; } = password;


        /// <summary>
        ///     The image settings for the Cassandra container resource.
        /// </summary>
        public CassandraImageSettings ImageSettings { get; } = imageSettings ?? CassandraImageSettings.Default;


        internal ReferenceExpression UsernameReference => ReferenceExpression.Create($"{UsernameParameter}");


        internal ReferenceExpression PasswordReference => ReferenceExpression.Create($"{PasswordParameter}");


        /// <inheritdoc />
        public ReferenceExpression ConnectionStringExpression => ReferenceExpression.Create(
            $"Contact Points={PrimaryEndpoint.Property(EndpointProperty.Host)}:{PrimaryEndpoint.Property(EndpointProperty.Port)};Username={UsernameReference};Password={PasswordReference}");
    }
}