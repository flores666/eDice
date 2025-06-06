using AuthorizationService;
using Shared.Lib.Extensions;
using Shared.Logging;
using Shared.MessageBus.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorizationServices();
builder.Services.AddKafka();
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

app.MapDefaultEndpoints();
app.MapAuthorizationApi();
app.UseHsts();

app.Run();