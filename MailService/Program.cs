using DotNetEnv;
using MailService;
using MailService.Models;
using Shared.Logging;
using Shared.MessageBus.Kafka;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings")
);

builder.Services.AddKafkaConsumer<EmailRequest, EmailEventHandler>(builder.Configuration.GetSection("KafkaConsumerOptions:Emails"));

builder.Services.AddTransient<ISmtpService, SmtpService>();
builder.Host.UseLoggerMinimalApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapServiceBuilder();

app.Run();