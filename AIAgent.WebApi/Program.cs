using AIAgent.WebApi.DomainLayer.Modules.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// 1. Serilog Structured Logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Add MVC Controllers
builder.Services.AddControllers();

// 3. CORS Configuration
const string AllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowSpecificOrigins, policy =>
    {
        string? allowedOrigins = builder.Configuration["AllowedOrigins"];
        if (!string.IsNullOrWhiteSpace(allowedOrigins))
        {
            policy.WithOrigins(allowedOrigins.Split(","))
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

// 4. Rate Limiting (Protects downstream LLM quota)
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("AgentRateLimit", context =>
    {
        string clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
        return RateLimitPartition.GetTokenBucketLimiter(clientIp, _ => new TokenBucketRateLimiterOptions
        {
            TokenLimit = 30,
            TokensPerPeriod = 30,
            ReplenishmentPeriod = TimeSpan.FromSeconds(60),
            AutoReplenishment = true,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 2
        });
    });
});

// 5. Swagger / OpenAPI Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KnowledgeAgent Web API",
        Version = "v1",
        Description = "Production-grade Microsoft Agentic Framework (MAF) RAG Agent API."
    });

    options.EnableAnnotations();

    // Include XML comments from build output
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// 6. Dependency Injection Registration
ConfigureDependencyInjection(builder.Services);

var app = builder.Build();

// 7. Configure HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KnowledgeAgent API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors(AllowSpecificOrigins);
app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers().RequireRateLimiting("AgentRateLimit");

app.Run();

// Dependency Injection Setup Partitioned by Clean Architecture Layers
static void ConfigureDependencyInjection(IServiceCollection services)
{
    // Common Primitives
    services.AddSingleton<IEnvironmentService, EnvironmentService>();

    // Layer Placeholders (Populated progressively in upcoming episodes)
    // - Infrastructure (AgentFactory, DiscoveryEngineSearchService, GcpDataStoreRetriever) -> Episodes 3, 5, 7, 9
    // - DomainLayer (ChatSessionContext, ChatHistoryProvider, Mappers) -> Episodes 10, 11
    // - ServiceLayer (KnowledgeAgentService) -> Episode 12
}
