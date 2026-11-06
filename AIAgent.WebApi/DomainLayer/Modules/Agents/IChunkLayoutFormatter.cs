namespace AIAgent.WebApi.DomainLayer.Modules.Agents;

/// <summary>
/// Defines the contract for formatting layout-based chunks into structured, grounded LLM context.
/// Completely eliminates legacy snippet fallback logic in favor of pure chunk layout assembly.
/// </summary>
public interface IChunkLayoutFormatter
{
    /// <summary>
    /// Assembles a collection of layout chunks into a formatted context string for LLM prompting.
    /// Preserves document identifiers, ancestor headings, markdown tables, and sliding window boundaries.
    /// </summary>
    /// <param name="chunks">The retrieved layout chunks to format.</param>
    /// <param name="maxCharacterLimit">The maximum character ceiling before truncating context.</param>
    /// <returns>A formatted context string containing document citations and structured chunk evidence.</returns>
    string FormatChunks(IEnumerable<LayoutChunk> chunks, int maxCharacterLimit = 120_000);
}

