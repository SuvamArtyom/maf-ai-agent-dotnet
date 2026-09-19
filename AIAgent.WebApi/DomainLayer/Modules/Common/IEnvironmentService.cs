namespace AIAgent.WebApi.DomainLayer.Modules.Common
{
    /// <summary>
    /// Service to retrieve configuration and environment variable values.
    /// </summary>
    public interface IEnvironmentService
    {
        /// <summary>
        /// Retrieves the value of the specified configuration key or environment variable.
        /// </summary>
        /// <param name="key">The configuration or environment key name.</param>
        /// <param name="defaultValue">Optional fallback value if the key is not set.</param>
        /// <returns>The resolved configuration string value.</returns>
        string GetValue(string key, string defaultValue = "");
    }
}

