# KnowledgeAgent — Enterprise AI Agent in .NET with Microsoft Agentic Framework (MAF)

Welcome to the companion repository for the YouTube course:  
**"Building Enterprise AI Agents in .NET with Microsoft Agentic Framework (MAF)"**

This repository demonstrates how to build a production-grade Web API in .NET 10 that implements an autonomous retrieval-augmented generation (RAG) agent using **`Microsoft.Agents.AI`** and **pure layout-based document chunking** (zero legacy snippets).

---

## 📺 Course Episodes & Git Checkpoint Matrix

Each episode in the course corresponds to a dedicated git tag/branch in this repository. You can switch to the exact code for any episode using `git checkout <tag>`:

| Episode | Title & Topic | YouTube Video | Git Tag / Branch Link | Status |
|:---:|---|:---:|:---:|:---:|
| **01** | Architecture Blueprint & Course Overview | [▶️ Watch Video](https://youtu.be/TD_rMSwciDg?si=xRUqrbn5_dpsdik4) | — *(Architecture)* | 🎬 Released |
| **02** | Baseline Solution Setup & Clean Architecture | [▶️ Watch Video](https://youtu.be/LIFi0ktOtVw?si=lm9o1XglKL58-aRN) | [`episode-02-base-setup`](https://github.com/SuvamArtyom/maf-ai-agent-dotnet/tree/episode-02-base-setup) | 🎬 Released |
| **03** | First MAF Agent in C# with `IChatClient` | 🔜 Coming Soon | [`episode-03-hello-agent`](https://github.com/SuvamArtyom/maf-ai-agent-dotnet/tree/episode-03-hello-agent) | 🎬 Released |
| **04** | Prompt Engineering as Code: Personas & Input Guardrails | — | `episode-04-prompt-and-guardrails` | 🔜 Upcoming |
| **05** | Agent Tooling & Function Calling (`AIFunctionFactory`) | — | `episode-05-agent-tooling` | 🔜 Upcoming |
| **06** | Enterprise Document RAG: Why Chunks Beat Snippets | — | `episode-06-rag-theory` | 🔜 Upcoming |
| **07** | Vector & Layout Chunk Search via Discovery Engine | — | `episode-07-chunk-search-service` | 🔜 Upcoming |
| **08** | Document Metadata Resolution & Performance Caching | — | `episode-08-metadata-enrichment` | 🔜 Upcoming |
| **09** | Layout Chunk Retriever: Assembling Structured Context | — | `episode-09-chunk-retriever` | 🔜 Upcoming |
| **10** | Master FAQ Priority Protocol & Strict Grounding | — | `episode-10-faq-prioritization` | 🔜 Upcoming |
| **11** | Stateful Conversations: Custom `ChatHistoryProvider` | — | `episode-11-chat-history` | 🔜 Upcoming |
| **12** | Core Service Layer & Agent Response Mapping | — | `episode-12-service-layer` | 🔜 Upcoming |
| **13** | Exposing the Web API, Swagger UI & Rate Limiting | — | `episode-13-controller-and-api` | 🔜 Upcoming |
| **14** | Automated Cloud Document Ingestion Pipeline | — | `episode-14-document-indexing` | 🔜 Upcoming |
| **15** | Unit Testing AI Agents (xUnit & Moq) & Wrap-Up | — | `episode-15-complete-system` | 🔜 Upcoming |

> 💡 **Tip for Viewers**: If you clone the `main` branch, you will see the latest state of the codebase. To view the exact code for any episode, checkout its tag:
> ```bash
> git checkout episode-03-hello-agent
> ```

---

## 🏛 Clean Architecture Layers

```
AIAgent_YT/
├── AIAgent.sln
└── AIAgent.WebApi/
    ├── Controllers/               # Thin REST API endpoints
    ├── ServiceLayer/              # Business orchestration & agent execution
    ├── DomainLayer/               # DTOs, ViewModels, session context & validations
    │   ├── ViewModel/
    │   └── Modules/
    ├── Infrastructure/            # MAF AgentFactory, Tools, and Chunk Retriever
    │   ├── Agents/
    │   └── ToolsAgent/
    └── Resources/                 # System prompt markdown instructions (.md)
        └── AgentInstructions/
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- Visual Studio 2022 (v17.12+) / VS Code / JetBrains Rider

### Build & Run Locally
```bash
# Clone the repository
git clone https://github.com/SuvamArtyom/maf-ai-agent-dotnet.git
cd maf-ai-agent-dotnet

# Build the solution
dotnet build

# Run the Web API
cd AIAgent.WebApi
dotnet run
```

Navigate to: `https://localhost:7001/swagger` to explore the interactive OpenAPI documentation.
