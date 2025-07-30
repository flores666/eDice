using DotNetEnv;
using Shared.Lib.Extensions;
using Shared.Logging;
using Shared.MessageBus.Kafka;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddKafkaProducer();

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

// app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.MapHealthChecks();
app.UseHsts();

app.Run();