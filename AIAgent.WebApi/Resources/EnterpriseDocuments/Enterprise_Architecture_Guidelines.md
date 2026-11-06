# Enterprise Architecture Standards

## AI Agent Framework Requirements
All enterprise artificial intelligence agents deployed in production must adhere strictly to the following architectural guidelines:

1. **Framework & Runtime**: Agents must be built on **.NET 10** using the **Microsoft Agentic Framework (MAF)** (`Microsoft.Agents.AI`) and **Google Gemini Enterprise** via `Google.Cloud.VertexAI.Extensions`.
2. **Prompts as Code**: All system prompts, personas, and guardrails must be maintained in version-controlled Markdown files rather than hardcoded C# strings.
3. **Native Function Calling**: Dynamic agent capabilities must use `AIFunctionFactory.Create` and the `.UseFunctionInvocation()` pipeline.
4. **Retrieval-Augmented Generation (RAG)**: The system must operate on pure layout-based chunks (500 tokens) with sliding context window expansion (`NumPreviousChunks = 1`, `NumNextChunks = 2`). Legacy keyword snippet fallback is strictly prohibited.

## Architecture Layer Matrix

| Layer | Responsibility | Primary Technology | Architectural Pattern |
|---|---|---|---|
| **Controllers** | REST API endpoints, OpenAPI documentation, rate limiting | ASP.NET Core Minimal/Controllers | Clean Architecture API Gateway |
| **Service Layer** | Business logic orchestration, agent session management | `IKnowledgeAgentService` | Application Service Pattern |
| **Domain Layer** | ViewModels, domain models, chunk layout formatters | `LayoutChunk`, `IChunkLayoutFormatter` | Domain-Driven Design (DDD) Primitives |
| **Infrastructure** | Agent factory, external tool dispatch, Discovery Engine | MAF `ChatClientAgent`, `AIFunctionFactory` | Hexagonal / Ports & Adapters |
| **Cloud RAG Engine** | Vector search, layout chunking, parent document resolution | Google Cloud Discovery Engine | 500-Token Layout Chunk RAG |

