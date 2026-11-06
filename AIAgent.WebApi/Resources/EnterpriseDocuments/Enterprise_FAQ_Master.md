# Enterprise FAQ Masterdata

## Frequently Asked Questions

### Q1: What is the official AI Agent Framework for .NET 10?
**Answer**: The official framework is Microsoft Agentic Framework (MAF) (`Microsoft.Agents.AI`) built in combination with Google Gemini Enterprise via `Google.Cloud.VertexAI.Extensions`.

### Q2: Why does the platform use layout-based chunks instead of search snippets?
**Answer**: Traditional search snippets return fragmented, out-of-context text excerpts that cut off tables and split critical sentences. Layout-based chunks preserve semantic boundaries, ancestor headings (`# Document > ## Section`), markdown tables, and multi-sheet layouts, providing accurate citations and zero hallucination.

### Q3: How do agents authenticate against cloud services without hardcoded API keys?
**Answer**: Authentication is handled via Google Application Default Credentials (ADC). When running locally, developers authenticate using `gcloud auth application-default login`. In cloud environments, the application uses Managed Service Identity (MSI) or Workload Identity.

