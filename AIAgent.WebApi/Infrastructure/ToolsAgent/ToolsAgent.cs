using System.ComponentModel;
using AIAgent.WebApi.DomainLayer.Modules.Agents;
using AIAgent.WebApi.DomainLayer.Modules.Common;
using Microsoft.Extensions.Options;

namespace AIAgent.WebApi.Infrastructure.ToolsAgent;

/// <summary>
/// Implementation of <see cref="IToolsAgent"/> providing tools and external capabilities to the MAF agent.
/// </summary>
public class ToolsAgent : IToolsAgent
{
    private readonly ILogger<ToolsAgent> _logger;
    private readonly IChunkLayoutFormatter _chunkFormatter;
    private readonly DatastoreOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToolsAgent"/> class.
    /// </summary>
    /// <param name="logger">Structured logger instance.</param>
    /// <param name="chunkFormatter">Layout chunk formatter service for assembling grounded context.</param>
    /// <param name="options">Datastore configuration options.</param>
    public ToolsAgent(
        ILogger<ToolsAgent> logger,
        IChunkLayoutFormatter chunkFormatter,
        IOptions<DatastoreOptions> options)
    {
        _logger = logger;
        _chunkFormatter = chunkFormatter;
        _options = options.Value;
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

        // Layout chunks with ancestor headings and sliding context (pre-Discovery Engine SDK in Episode 7)
        List<LayoutChunk> sampleChunks =
        [
            new LayoutChunk
            {
                ChunkId = "chunk-arch-001",
                DocumentId = "Enterprise_Architecture_Guidelines.md",
                DocumentTitle = "Enterprise Architecture Standards",
                DocumentUri = "gs://enterprise-knowledge-vault/Enterprise_Architecture_Guidelines.md",
                AncestorHeadings = ["Enterprise Architecture Standards", "AI Agent Framework Requirements"],
                Content = """
                    1. Agents must be built on .NET 10 using Microsoft Agentic Framework (MAF) and Google Gemini Enterprise.
                    2. Prompts as Code: All system prompts, personas, and guardrails must be maintained in version-controlled Markdown files.
                    3. Native Function Calling: Dynamic agent capabilities must use AIFunctionFactory.Create and .UseFunctionInvocation().
                    4. Retrieval-Augmented Generation (RAG): Must operate on pure layout-based chunks (500 tokens) with sliding context window expansion.
                    """,
                PreviousChunks = ["# Document Overview: Enterprise Architecture Standards v1.0"],
                NextChunks = ["Architecture Layer Matrix table defining Controllers, ServiceLayer, DomainLayer, and Infrastructure."],
                RelevanceScore = 0.95
            },
            new LayoutChunk
            {
                ChunkId = "chunk-sec-002",
                DocumentId = "Security_Compliance_Standards.md",
                DocumentTitle = "Enterprise Security & Compliance Standards",
                DocumentUri = "gs://enterprise-knowledge-vault/Security_Compliance_Standards.md",
                AncestorHeadings = ["Enterprise Security Standards", "Safe AI Execution Policies"],
                Content = """
                    1. Input Sanitization & Screening: Inbound user queries must be screened against enterprise guardrails.
                    2. Confidentiality & Data Perimeter: No corporate data or proprietary source code may leave the cloud perimeter.
                    3. Secret Management: Credentials and keys must never appear in LLM responses or prompt context.
                    """,
                PreviousChunks = ["# Security Policy Overview: Enterprise Data Classification"],
                NextChunks = ["Data Classification Matrix: Public, Internal Enterprise, Confidential, Restricted."],
                RelevanceScore = 0.91
            }
        ];

        // Format chunks through pure layout chunk assembler with sliding windows
        return _chunkFormatter.FormatChunks(sampleChunks, _options.MaxCharacterLimit);
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
