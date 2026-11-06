namespace AIAgent.WebApi.DomainLayer.Modules.Agents;

/// <summary>
/// Represents a structured layout-based document chunk retrieved from enterprise knowledge datastores.
/// Preserves structural document hierarchy, ancestor headings, tables, and sliding context windows.
/// </summary>
public class LayoutChunk
{
    /// <summary>
    /// Gets or sets the unique chunk identifier assigned by Discovery Engine layout parser.
    /// </summary>
    public string ChunkId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parent document identifier (e.g., Enterprise_Architecture_Guidelines.md).
    /// </summary>
    public string DocumentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the human-readable document title.
    /// </summary>
    public string DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source Cloud Storage URI or origin link.
    /// </summary>
    public string DocumentUri { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the primary 500-token semantic chunk content.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hierarchical ancestor headings (e.g., ["Architecture Standards", "AI Agent Integration"]).
    /// </summary>
    public List<string> AncestorHeadings { get; set; } = [];

    /// <summary>
    /// Gets or sets the previous neighboring chunks stitched for sliding context window expansion.
    /// </summary>
    public List<string> PreviousChunks { get; set; } = [];

    /// <summary>
    /// Gets or sets the next neighboring chunks stitched for sliding context window expansion.
    /// </summary>
    public List<string> NextChunks { get; set; } = [];

    /// <summary>
    /// Gets or sets the semantic relevance score computed during vector search.
    /// </summary>
    public double RelevanceScore { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this chunk originates from an authoritative FAQ document.
    /// </summary>
    public bool IsFaq { get; set; }
}

