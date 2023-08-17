namespace Customer_Management.Web.Helpers
{
    public class AppSettingsHelper
    {
        private IConfiguration _configuration;

        public AppSettingsHelper()
        {
            InitializeConfiguration();
        }

        public string GetConnectionString(string key)
        {
            var connection = _configuration.GetConnectionString(key);
            if(connection == null)
            {
                throw new ArgumentNullException($"Value not defined for the key {nameof(key)}");
            }

            return connection;
        }

        private void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            _configuration = builder.Build();
        }
    }
}
