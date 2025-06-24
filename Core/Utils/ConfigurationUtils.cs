using Microsoft.Extensions.Configuration;

namespace Core.Utils
{
    public static class ConfigurationUtils
    {
        private static IConfiguration _config;
        public static IConfiguration ReadConfiguration(string path)
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path)
                .Build();
            return _config;
        }
        public static string GetConfigurationByKey(string key, IConfiguration? config = null)
        {
            var value = config == null ? _config[key] : config[key];

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            throw new InvalidDataException($"Attribute [{key}] has not been set in appsettings.json");
        }
        
    }
}