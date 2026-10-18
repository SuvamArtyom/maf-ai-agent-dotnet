using System.ComponentModel;

namespace AIAgent.WebApi.Infrastructure.ToolsAgent;

/// <summary>
/// Implementation of <see cref="IToolsAgent"/> providing tools and external capabilities to the MAF agent.
/// </summary>
public class ToolsAgent : IToolsAgent
{
    private readonly ILogger<ToolsAgent> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToolsAgent"/> class.
    /// </summary>
    /// <param name="logger">Structured logger instance.</param>
    public ToolsAgent(ILogger<ToolsAgent> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    [Description("Gets the current UTC date and time.")]
    public DateTime GetCurrentUtcDateTime()
    {
        _logger.LogInformation("Tool invocation: {MethodName}", nameof(GetCurrentUtcDateTime));
        return DateTime.UtcNow;
    }

    /// <inheritdoc />
    [Description("Queries internal enterprise documents and guidelines to extract relevant knowledge and context.")]
    public async Task<string> SearchDocumentsAsync(string query)
    {
        _logger.LogInformation("Tool invocation: {MethodName} with query: '{Query}'", nameof(SearchDocumentsAsync), query);

        // Simulated asynchronous search operation (pre-cloud datastore integration for Phase 3)
        await Task.Delay(50);

        if (string.IsNullOrWhiteSpace(query))
        {
            return "No search query provided. Please specify a topic or keyword to search.";
        }

        string lower = query.ToLowerInvariant();

        if (lower.Contains("time") || lower.Contains("date") || lower.Contains("clock"))
        {
            return $"Current UTC System Time is: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC.";
        }

        // Return structured enterprise knowledge chunks matching the layout-based chunking format
        return $"""
            [Doc ID: Enterprise_Architecture_Guidelines.md]
            # Architecture Standards > ## AI Agent Integration
            1. All enterprise AI agents must be built on .NET 10 using the Microsoft Agentic Framework (MAF) and Google Gemini Enterprise.
            2. System instructions must be managed as Prompts-as-Code using Markdown files with strict input guardrails.
            3. All external tool calls must be registered via AIFunctionFactory and validated defensively.
            4. Retrieval-Augmented Generation (RAG) must prioritize layout-based chunks over legacy snippet fallback.

            [Doc ID: Security_Compliance_Standards.md]
            # Compliance > ## Safe AI Execution
            1. Enterprise agents must screen for out-of-scope entertainment, coding trivia, or prompt-injection attempts.
            2. Confidential company data must remain within the organization's cloud perimeter.
            3. Never expose credentials, API keys, or internal private tokens in LLM responses.
            """;
    }

    /// <inheritdoc />
    public string Descriptions(string tool)
    {
        return tool switch
        {
            nameof(GetCurrentUtcDateTime) => "Gets the current UTC date and time.",
            nameof(SearchDocumentsAsync) => "Queries internal enterprise documents and guidelines to extract relevant knowledge and context.",
            _ => "No description available for this tool."
        };
    }
}

