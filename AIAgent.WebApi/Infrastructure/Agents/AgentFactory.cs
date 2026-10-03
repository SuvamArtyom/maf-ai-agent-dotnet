using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AIAgent.WebApi.Infrastructure.Agents;

/// <summary>
/// Implementation of <see cref="IAgentFactory"/> to assemble Microsoft Agentic Framework (MAF) agents.
/// </summary>
public class AgentFactory : IAgentFactory
{
    private readonly ILogger<AgentFactory> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AgentFactory"/> class.
    /// </summary>
    /// <param name="logger">Structured logger instance.</param>
    public AgentFactory(ILogger<AgentFactory> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public ChatClientAgent CreateKnowledgeAssistantAgent(IChatClient client, ChatResponseFormat? chatResponseFormat = null)
    {
        _logger.LogInformation("Assembling KnowledgeAssistant agent instance");

        return client.AsAIAgent(new ChatClientAgentOptions
        {
            Name = "KnowledgeAssistant",
            Description = "Enterprise assistant that answers user questions using enterprise knowledge and guidelines.",
            ChatOptions = new ChatOptions
            {
                Instructions = "You are KnowledgeAssistant, an enterprise AI assistant built with Microsoft Agentic Framework (MAF) in .NET 10. Answer user queries clearly, professionally, and concisely.",
                ResponseFormat = chatResponseFormat
            }
        });
    }
}
