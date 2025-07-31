using DotNetEnv;
using Shared.Lib.Extensions;
using Shared.Logging;
using Shared.MessageBus.Kafka;
using UsersCoordinatorService;
using UsersCoordinatorService.Handlers.EmailConfirm;
using UsersCoordinatorService.Handlers.PasswordReset;
using UsersCoordinatorService.Handlers.UserRegistration;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.Configure<KafkaProduceTopics>(
    builder.Configuration.GetSection("KafkaProducerOptions")
);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddKafkaProducer();

builder.Services.AddKafkaConsumer<UserRegisteredMessage, UserRegisteredHandler>(builder.Configuration.GetSection("KafkaConsumerOptions:UserRegisteredMessage"));
builder.Services.AddKafkaConsumer<PasswordResetMessage, PasswordResetHandler>(builder.Configuration.GetSection("KafkaConsumerOptions:PasswordResetMessage"));
builder.Services.AddKafkaConsumer<EmailConfirmMessage, EmailConfirmHandler>(builder.Configuration.GetSection("KafkaConsumerOptions:EmailConfirmMessage"));

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