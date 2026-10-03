using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AIAgent.WebApi.Infrastructure.Agents;

/// <summary>
/// Factory contract for constructing Microsoft Agentic Framework (MAF) agents.
/// </summary>
public interface IAgentFactory
{
    /// <summary>
    /// Constructs an instance of the KnowledgeAssistant agent.
    /// </summary>
    /// <param name="client">The underlying IChatClient (e.g., Vertex AI Gemini).</param>
    /// <param name="chatResponseFormat">Optional response formatting contract for structured responses.</param>
    /// <returns>A configured ChatClientAgent ready to process user interactions.</returns>
    ChatClientAgent CreateKnowledgeAssistantAgent(IChatClient client, ChatResponseFormat? chatResponseFormat = null);
}
