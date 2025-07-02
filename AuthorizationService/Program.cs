using AuthorizationService;
using DotNetEnv;
using Shared.Lib.Auth;
using Shared.Lib.Extensions;
using Shared.Logging;
using Shared.MessageBus.Kafka;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorizationServices();
builder.Services.AddKafkaProducer();

builder.Services.AddDenyAuthenticatedHandler();
builder.Services.AddAuthorization(options =>
{
    options.AddDenyAuthenticatedPolicy();
});

builder.Services.AddJwtAuthentication();

builder.AddDefaultHealthChecks();
builder.Host.UseLogger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseStatusCodePages();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapAuthorizationApi();
app.UseHsts();

app.Run();