using Authorization.API;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddDefaultHealthChecks();
builder.Host.UseLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseStatusCodePages();

app.MapDefaultEndpoints();
app.MapAuthorizationApi();
app.UseHsts();

app.Run();