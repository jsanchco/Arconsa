namespace SGDE.Domain.Helpers
{
    public class InfrastructureAppSettings
    {
        public string Type { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public class JwtAppSettings
    {
        public string SecretKey { get; set; }
    }
}
