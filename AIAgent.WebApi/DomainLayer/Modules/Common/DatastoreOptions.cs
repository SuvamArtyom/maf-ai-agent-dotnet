namespace AIAgent.WebApi.DomainLayer.Modules.Common;

/// <summary>
/// Configuration options for Google Cloud Discovery Engine Datastore and Layout Chunk Retrieval.
/// </summary>
public class DatastoreOptions
{
    /// <summary>
    /// Configuration section name in appsettings.json.
    /// </summary>
    public const string SectionName = "GCP_Configuration";

    /// <summary>
    /// Gets or sets the Google Cloud Project ID hosting Discovery Engine.
    /// </summary>
    public string GCP_PROJECT_ID { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Google Cloud region for Discovery Engine and Vertex AI (e.g., us-central1 or global).
    /// </summary>
    public string GCP_LOCATION { get; set; } = "us-central1";

    /// <summary>
    /// Gets or sets the Discovery Engine Datastore identifier containing enterprise document layout chunks.
    /// </summary>
    public string GCP_DATASTORE_ID { get; set; } = "knowledge-datastore";

    /// <summary>
    /// Gets or sets the model publisher ID (e.g., google).
    /// </summary>
    public string MODEL_PUBLISHERID { get; set; } = "google";

    /// <summary>
    /// Gets or sets the Gemini model name (e.g., gemini-3.8-flash).
    /// </summary>
    public string MODEL_LLM { get; set; } = "gemini-3.8-flash";

    /// <summary>
    /// Gets or sets the maximum number of chunks to retrieve per search query.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the maximum number of candidate documents to evaluate during vector search.
    /// </summary>
    public int CandidateLimit { get; set; } = 20;

    /// <summary>
    /// Gets or sets the maximum total characters allowed in assembled LLM context to prevent context window overflow.
    /// </summary>
    public int MaxCharacterLimit { get; set; } = 120_000;

    /// <summary>
    /// Gets or sets the number of preceding layout chunks to stitch for sliding window context expansion.
    /// </summary>
    public int NumPreviousChunks { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of succeeding layout chunks to stitch for sliding window context expansion.
    /// </summary>
    public int NumNextChunks { get; set; } = 2;
}

