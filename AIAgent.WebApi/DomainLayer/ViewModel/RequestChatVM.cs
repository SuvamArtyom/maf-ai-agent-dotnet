using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AIAgent.WebApi.DomainLayer.ViewModel;

/// <summary>
/// Request view model for interacting with the KnowledgeAgent.
/// </summary>
[ExcludeFromCodeCoverage]
public class RequestChatVM
{
    /// <summary>
    /// Gets or sets the message or query sent by the user to the agent.
    /// </summary>
    [Required(ErrorMessage = "Message is required and cannot be empty.")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional session identifier for tracking conversational context across turns.
    /// </summary>
    public string? SessionId { get; set; }
}

