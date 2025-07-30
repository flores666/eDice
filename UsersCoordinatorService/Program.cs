using DotNetEnv;
using Shared.Lib.Extensions;
using Shared.Logging;
using Shared.MessageBus.Kafka;
using UsersCoordinatorService;
using UsersCoordinatorService.Handlers.EmailConfirm;
using UsersCoordinatorService.Handlers.PasswordReset;
using UsersCoordinatorService.Handlers.UserRegistration;
using UsersCoordinatorService.MessageModels;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.Configure<KafkaConsumeTopics>(
    builder.Configuration.GetSection("KafkaConsumerOptions")
);

builder.Services.Configure<KafkaProduceTopics>(
    builder.Configuration.GetSection("KafkaProducerOptions")
);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddKafkaProducer();

builder.Services.AddKafkaConsumer<UserMessage, UserRegisteredHandler>(builder.Configuration.GetSection("KafkaConsumerOptions:UserRegistered"));
builder.Services.AddKafkaConsumer<UserMessage, PasswordResetHandler>(builder.Configuration.GetSection("KafkaConsumerOptions:PasswordResetRequested"));
builder.Services.AddKafkaConsumer<UserMessage, EmailConfirmHandler>(builder.Configuration.GetSection("KafkaConsumerOptions:EmailConfirmRequested"));

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