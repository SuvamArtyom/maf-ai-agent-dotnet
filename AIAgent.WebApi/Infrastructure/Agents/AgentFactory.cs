using AIAgent.WebApi.Infrastructure.ToolsAgent;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AIAgent.WebApi.Infrastructure.Agents;

/// <summary>
/// Implementation of <see cref="IAgentFactory"/> to assemble Microsoft Agentic Framework (MAF) agents.
/// </summary>
public class AgentFactory : IAgentFactory
{
    private readonly ILogger<AgentFactory> _logger;
    private readonly IToolsAgent _tools;
    private readonly string _knowledgeAssistantInstructions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AgentFactory"/> class.
    /// Loads system instructions dynamically from markdown files (Prompt Engineering as Code).
    /// </summary>
    /// <param name="logger">Structured logger instance.</param>
    /// <param name="tools">Tools agent providing callable functions for the agent.</param>
    public AgentFactory(ILogger<AgentFactory> logger, IToolsAgent tools)
    {
        _logger = logger;
        _tools = tools;
        _knowledgeAssistantInstructions = LoadInstructions("KnowledgeAssistant.md");
    }

    /// <inheritdoc />
    public ChatClientAgent CreateKnowledgeAssistantAgent(IChatClient client, ChatResponseFormat? chatResponseFormat = null)
    {
        _logger.LogInformation("Assembling KnowledgeAssistant agent with markdown system instructions and tool definitions.");

        return client.AsAIAgent(new ChatClientAgentOptions
        {
            Name = "KnowledgeAssistant",
            Description = "Enterprise assistant that answers user questions using enterprise knowledge and guidelines.",
            ChatOptions = new ChatOptions
            {
                Instructions = _knowledgeAssistantInstructions,
                ResponseFormat = chatResponseFormat,
                Tools =
                [
                    AIFunctionFactory.Create(
                        _tools.SearchDocumentsAsync,
                        nameof(_tools.SearchDocumentsAsync),
                        _tools.Descriptions(nameof(IToolsAgent.SearchDocumentsAsync))),
                    AIFunctionFactory.Create(
                        _tools.GetCurrentUtcDateTime,
                        nameof(_tools.GetCurrentUtcDateTime),
                        _tools.Descriptions(nameof(IToolsAgent.GetCurrentUtcDateTime)))
                ]
            }
        });
    }

    private string LoadInstructions(string fileName)
    {
        string primaryPath = Path.Combine(AppContext.BaseDirectory, "Resources", "AgentInstructions", fileName);
        if (File.Exists(primaryPath))
        {
            _logger.LogDebug("Loaded system prompt from primary path: {Path}", primaryPath);
            return File.ReadAllText(primaryPath);
        }

        string fallbackPath = Path.Combine("Resources", "AgentInstructions", fileName);
        if (File.Exists(fallbackPath))
        {
            _logger.LogDebug("Loaded system prompt from fallback path: {Path}", fallbackPath);
            return File.ReadAllText(fallbackPath);
        }

        _logger.LogWarning("System prompt file {FileName} not found. Falling back to default baseline instruction.", fileName);
        return "You are KnowledgeAssistant, an enterprise AI assistant built with Microsoft Agentic Framework in .NET 10.";
    }
}
