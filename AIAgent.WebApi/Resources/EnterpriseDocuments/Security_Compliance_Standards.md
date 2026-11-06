# Enterprise Security & Compliance Standards

## Safe AI Execution Policies
Enterprise agents operating within corporate environments must satisfy stringent compliance guardrails:

1. **Input Sanitization & Screening**: All inbound user queries must be screened against enterprise guardrails. Malicious prompt injection, offensive queries, and out-of-scope trivia must be rejected before tool invocation.
2. **Confidentiality & Data Perimeter**: No corporate data, customer records, or proprietary source code may leave the organization's cloud perimeter.
3. **Secret Management**: API keys, credentials, database connection strings, and service tokens must never appear in LLM responses or prompt context. Authentication must rely on Google Application Default Credentials (ADC).

## Data Classification Standards

| Classification Level | Sensitivity | Storage Location | Permitted in RAG Context? |
|---|---|---|---|
| **Public** | Low | Public Documentation | Yes |
| **Internal Enterprise** | Medium | Secure Cloud Storage (`gs://enterprise-vault`) | Yes (Authenticated Users) |
| **Confidential / PII** | High | Encrypted Datastore with DLP redaction | Redacted Only |
| **Restricted / Secrets** | Critical | Cloud Key Management / Secret Manager | Strictly Prohibited |

