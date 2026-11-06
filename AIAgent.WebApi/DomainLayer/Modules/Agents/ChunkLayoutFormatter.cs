using System.Text;

namespace AIAgent.WebApi.DomainLayer.Modules.Agents;

/// <summary>
/// Implementation of <see cref="IChunkLayoutFormatter"/> using pure layout-based chunk parsing.
/// Completely eliminates legacy snippet fallback logic and stitches sliding context windows.
/// </summary>
public class ChunkLayoutFormatter : IChunkLayoutFormatter
{
    private readonly ILogger<ChunkLayoutFormatter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChunkLayoutFormatter"/> class.
    /// </summary>
    /// <param name="logger">Structured logger instance.</param>
    public ChunkLayoutFormatter(ILogger<ChunkLayoutFormatter> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public string FormatChunks(IEnumerable<LayoutChunk> chunks, int maxCharacterLimit = 120_000)
    {
        if (chunks == null || !chunks.Any())
        {
            _logger.LogInformation("No chunks provided to ChunkLayoutFormatter. Returning empty datastore fallback.");
            return "No relevant technical documentation or matching records found in the datastore.";
        }

        StringBuilder contextBuilder = new();
        int totalCharacters = 0;
        int chunkCount = 0;

        foreach (LayoutChunk chunk in chunks)
        {
            if (string.IsNullOrWhiteSpace(chunk.Content))
            {
                continue;
            }

            StringBuilder chunkBuilder = new();

            // 1. Document Identifier Header
            string docIdentifier = !string.IsNullOrWhiteSpace(chunk.DocumentId)
                ? chunk.DocumentId
                : (!string.IsNullOrWhiteSpace(chunk.DocumentTitle) ? chunk.DocumentTitle : "Enterprise_Document");

            chunkBuilder.AppendLine($"[Doc ID: {docIdentifier}]");

            // 2. Ancestor Headings (Preserves layout hierarchy: # Doc > ## Section > ### Subsection)
            if (chunk.AncestorHeadings != null && chunk.AncestorHeadings.Count > 0)
            {
                chunkBuilder.AppendLine($"# {string.Join(" > ## ", chunk.AncestorHeadings)}");
            }

            // 3. Sliding Context Window: Preceding Neighbor Chunks
            if (chunk.PreviousChunks != null && chunk.PreviousChunks.Count > 0)
            {
                foreach (string prev in chunk.PreviousChunks)
                {
                    if (!string.IsNullOrWhiteSpace(prev))
                    {
                        chunkBuilder.AppendLine(prev.Trim());
                    }
                }
            }

            // 4. Primary Chunk Content (Preserves complete Markdown tables and semantic structure)
            chunkBuilder.AppendLine(chunk.Content.Trim());

            // 5. Sliding Context Window: Succeeding Neighbor Chunks
            if (chunk.NextChunks != null && chunk.NextChunks.Count > 0)
            {
                foreach (string next in chunk.NextChunks)
                {
                    if (!string.IsNullOrWhiteSpace(next))
                    {
                        chunkBuilder.AppendLine(next.Trim());
                    }
                }
            }

            chunkBuilder.AppendLine();

            string formattedChunk = chunkBuilder.ToString();

            // Enforce character ceiling limit
            if (totalCharacters + formattedChunk.Length > maxCharacterLimit)
            {
                _logger.LogWarning(
                    "Context character limit reached ({CurrentTotal} + {ChunkLength} > {Limit}). Terminating chunk assembly.",
                    totalCharacters, formattedChunk.Length, maxCharacterLimit);
                break;
            }

            contextBuilder.Append(formattedChunk);
            totalCharacters += formattedChunk.Length;
            chunkCount++;
        }

        _logger.LogInformation("Assembled {ChunkCount} layout chunks into {TotalCharacters} characters of grounded context.",
            chunkCount, totalCharacters);

        return contextBuilder.ToString().TrimEnd();
    }
}

