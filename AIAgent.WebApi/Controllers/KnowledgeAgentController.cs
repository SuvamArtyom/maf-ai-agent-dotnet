using AIAgent.WebApi.DomainLayer.Modules.Common;
using AIAgent.WebApi.DomainLayer.ViewModel;
using AIAgent.WebApi.Infrastructure.Agents;
using Google.Cloud.AIPlatform.V1;
using Google.Cloud.VertexAI.Extensions;
using Microsoft.Agents.AI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Swashbuckle.AspNetCore.Annotations;

namespace AIAgent.WebApi.Controllers;

/// <summary>
/// Controller for interacting with the KnowledgeAgent using Microsoft Agentic Framework (MAF).
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class KnowledgeAgentController : ControllerBase
{
    private readonly IAgentFactory _agentFactory;
    private readonly IEnvironmentService _environmentService;
    private readonly ILogger<KnowledgeAgentController> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="KnowledgeAgentController"/>.
    /// </summary>
    /// <param name="agentFactory">Agent factory used to construct MAF agents.</param>
    /// <param name="environmentService">Environment and configuration retrieval service.</param>
    /// <param name="logger">Structured logger instance.</param>
    public KnowledgeAgentController(
        IAgentFactory agentFactory,
        IEnvironmentService environmentService,
        ILogger<KnowledgeAgentController> logger)
    {
        _agentFactory = agentFactory;
        _environmentService = environmentService;
        _logger = logger;
    }

    /// <summary>
    /// Sends a query to the KnowledgeAssistant agent with markdown-defined instructions and enterprise guardrails.
    /// </summary>
    /// <param name="request">The chat request containing the user's message and optional session identifier.</param>
    /// <returns>The response produced by the agent via Gemini Enterprise.</returns>
    /// <response code="200">The agent successfully processed the user prompt.</response>
    /// <response code="400">The query is empty or required cloud configuration is missing.</response>
    /// <response code="500">An unexpected error occurred during agent execution.</response>
    [HttpPost("ask")]
    [SwaggerOperation(
        Summary = "Execute a conversation turn with KnowledgeAssistant",
        Description = "Sends user prompt to KnowledgeAssistant via Microsoft Agentic Framework (MAF) with system prompt instructions and enterprise guardrails."
    )]
    [ProducesResponseType(typeof(KnowledgeResponseVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AskAsync([FromBody] RequestChatVM request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new { error = "Message cannot be empty." });
        }

        string projectId = _environmentService.GetValue("GCP_PROJECT_ID");
        string location = _environmentService.GetValue("GCP_LOCATION", "us-central1");
        string publisherId = _environmentService.GetValue("MODEL_PUBLISHERID", "google");
        string modelName = _environmentService.GetValue("MODEL_LLM", "gemini-3.8-flash");

        if (string.IsNullOrWhiteSpace(projectId) || projectId.Equals("your-gcp-project-id", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Execution rejected: GCP_PROJECT_ID is not configured.");
            return BadRequest(new
            {
                error = "GCP_PROJECT_ID is not configured.",
                hint = "Please set 'GCP_PROJECT_ID' in appsettings.json or via environment variables to your Google Cloud Project ID."
            });
        }

        string sessionId = string.IsNullOrWhiteSpace(request.SessionId)
            ? Guid.NewGuid().ToString()
            : request.SessionId;

        _logger.LogInformation("Invoking KnowledgeAssistant agent for session {SessionId} with model {Model} in region {Location}",
            sessionId, modelName, location);

        try
        {
            string endpoint = EndpointName.FormatProjectLocationPublisherModel(
                projectId,
                location,
                publisherId,
                modelName);

            IChatClient baseClient = await new PredictionServiceClientBuilder()
                .BuildIChatClientAsync(endpoint);

            IChatClient chatClient = new ChatClientBuilder(baseClient)
                .Build();

            ChatClientAgent agent = _agentFactory.CreateKnowledgeAssistantAgent(chatClient);

            List<ChatMessage> messages =
            [
                new ChatMessage(ChatRole.User, request.Message.Trim())
            ];

            AgentResponse agentResponse = await agent.RunAsync(messages);

            KnowledgeResponseVM response = new()
            {
                Query = request.Message.Trim(),
                Response = agentResponse.Text ?? string.Empty,
                SessionId = sessionId,
                Model = modelName,
                TimestampUtc = DateTime.UtcNow
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute agent request: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                error = "An error occurred while communicating with the agent.",
                details = ex.Message
            });
        }
    }
}
