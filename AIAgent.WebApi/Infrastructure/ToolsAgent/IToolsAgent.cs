namespace AIAgent.WebApi.Infrastructure.ToolsAgent;

/// <summary>
/// Defines the contract for tool providers exposed to Microsoft Agentic Framework (MAF) agents.
/// Exposes domain API operations and utility functions that the LLM can invoke dynamically via function calling.
/// </summary>
public interface IToolsAgent
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    /// <returns>The current <see cref="DateTime"/> in UTC.</returns>
    DateTime GetCurrentUtcDateTime();

    /// <summary>
    /// Queries internal enterprise documents and guidelines to extract relevant knowledge and context.
    /// </summary>
    /// <param name="query">Semantic search query formulated by the agent.</param>
    /// <returns>Extracted evidence from relevant enterprise documents or a fallback message if no matching records are found.</returns>
    Task<string> SearchDocumentsAsync(string query);

    /// <summary>
    /// Retrieves a human-readable and model-consumable description of the specified tool's functionality.
    /// </summary>
    /// <param name="tool">The method name of the tool.</param>
    /// <returns>A description string used when registering the tool with <c>AIFunctionFactory</c>.</returns>
    string Descriptions(string tool);
}

