using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AIAgent.WebApi.Infrastructure.Agents;

/// <summary>
/// Implementation of <see cref="IAgentFactory"/> to assemble Microsoft Agentic Framework (MAF) agents.
/// </summary>
public class AgentFactory : IAgentFactory
{
    private readonly ILogger<AgentFactory> _logger;
    private readonly string _knowledgeAssistantInstructions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AgentFactory"/> class.
    /// Loads system instructions dynamically from markdown files (Prompt Engineering as Code).
    /// </summary>
    /// <param name="logger">Structured logger instance.</param>
    public AgentFactory(ILogger<AgentFactory> logger)
    {
        _logger = logger;
        _knowledgeAssistantInstructions = LoadInstructions("KnowledgeAssistant.md");
    }

    /// <inheritdoc />
    public ChatClientAgent CreateKnowledgeAssistantAgent(IChatClient client, ChatResponseFormat? chatResponseFormat = null)
    {
        _logger.LogInformation("Assembling KnowledgeAssistant agent with markdown system instructions.");

        return client.AsAIAgent(new ChatClientAgentOptions
        {
            Name = "KnowledgeAssistant",
            Description = "Enterprise assistant that answers user questions using enterprise knowledge and guidelines.",
            ChatOptions = new ChatOptions
            {
                Instructions = _knowledgeAssistantInstructions,
                ResponseFormat = chatResponseFormat
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
