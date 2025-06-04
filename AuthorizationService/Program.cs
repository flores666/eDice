using AuthorizationService;
using Shared.Lib.Extensions;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorizationServices();
builder.AddDefaultHealthChecks();
builder.Host.UseLogger();
builder.Services.AddCors();

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
app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin());

app.MapDefaultEndpoints();
app.MapAuthorizationApi();
app.UseHsts();

app.Run();