using ChallengeYardFlow.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Entity Framework
builder.Services.AddDbContext<LocadoraContext>(opts =>
    opts.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração dos Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Versionamento da API
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "VVV"; // v1, v2
    options.SubstituteApiVersionInUrl = true;
});

// Health Checks
builder.Services.AddHealthChecks();

// Serviços de ML
builder.Services.AddSingleton<ChallengeYardFlow.Services.SentimentService>();

// Serviços de domínio
builder.Services.AddSingleton<ChallengeYardFlow.Services.PricingService>();

// Autenticação JWT e API KEY
var jwtKey = builder.Configuration["Jwt:Key"] ?? string.Empty;
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "ApiKeyOrBearer";
        options.DefaultChallengeScheme = "ApiKeyOrBearer";
        options.DefaultScheme = "ApiKeyOrBearer";
    })
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    })
    .AddScheme<AuthenticationSchemeOptions, ChallengeYardFlow.Services.ApiKeyAuthenticationHandler>("ApiKey", options => { })
    .AddPolicyScheme("ApiKeyOrBearer", "ApiKeyOrBearer", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            string? authorization = context.Request.Headers.Authorization;
            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return "Bearer";
            }
            return "ApiKey";
        };
    });

builder.Services.AddAuthorization();

// Configuração do Swagger 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ChallengeYardFlow API",
        Description = "API para gerenciar motos de uma locadora"
    });

    var xmlFile = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    // Definição de segurança Bearer (JWT) no Swagger
    var bearerScheme = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    options.AddSecurityDefinition("Bearer", bearerScheme);

    // Definição de segurança API KEY no Swagger
    var apiKeyScheme = new OpenApiSecurityScheme
    {
        Description = "API Key header. Example: 'X-API-Key: {your-api-key}'",
        Name = "X-API-Key",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
    };
    options.AddSecurityDefinition("ApiKey", apiKeyScheme);

    // Adicionar ambos os esquemas como opcionais (o usuário pode usar JWT ou API KEY)
    // Não é necessário adicionar como requisito global - os endpoints que precisam de autenticação já têm [Authorize]
});

var app = builder.Build();

// Swagger sempre habilitado
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChallengeYardFlow API v1");
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Endpoint de Health Checks
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }