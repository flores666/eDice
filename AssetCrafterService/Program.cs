using AssetCrafterService.Services;
using DotNetEnv;
using Infrastructure.AssetCrafterService;
using Microsoft.EntityFrameworkCore;
using Shared.Lib.Extensions;
using Shared.Logging;
using Shared.MessageBus.Kafka;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication();
builder.Services.AddControllers();
builder.Host.UseLogger();
builder.Services.AddDbContext<PostgresContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING")));

builder.Services.AddScoped<ITokensService, TokensService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddKafkaProducer();

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

app.MapControllers();
// app.UseHttpsRedirection();
app.UseStatusCodePages();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseHsts();

app.Run();