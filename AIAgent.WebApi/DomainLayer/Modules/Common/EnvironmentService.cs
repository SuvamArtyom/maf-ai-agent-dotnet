using System.Reflection;

namespace AIAgent.WebApi.DomainLayer.Modules.Common
{
    /// <summary>
    /// Implementation of <see cref="IEnvironmentService"/> using <see cref="IConfiguration"/> and process environment variables.
    /// </summary>
    public class EnvironmentService : IEnvironmentService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<EnvironmentService> logger;

        /// <summary>
        /// Initializes a new instance of <see cref="EnvironmentService"/>.
        /// </summary>
        public EnvironmentService(IConfiguration configuration, ILogger<EnvironmentService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <inheritdoc />
        public string GetValue(string key, string defaultValue = "")
        {
            logger.LogDebug($"Started: {this.GetType().Name},{MethodBase.GetCurrentMethod()?.Name} for key: {key}");

            // 1. Check direct environment variables (e.g. Docker, Cloud Run, App Engine)
            string? envValue = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrWhiteSpace(envValue))
            {
                return envValue;
            }

            // 2. Check IConfiguration (appsettings.json, User Secrets)
            string? configValue = configuration[key];
            if (!string.IsNullOrWhiteSpace(configValue))
            {
                return configValue;
            }

            // 3. Check within nested section "GCP_Configuration:{key}"
            string? nestedValue = configuration[$"GCP_Configuration:{key}"];
            if (!string.IsNullOrWhiteSpace(nestedValue))
            {
                return nestedValue;
            }

            return defaultValue;
        }
    }
}

