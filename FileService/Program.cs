using DotNetEnv;
using FileService;
using FileService.Service;
using Infrastructure.FileService;
using Microsoft.EntityFrameworkCore;
using Shared.Lib.Extensions;
using Shared.Logging;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication();
builder.Host.UseLogger();
builder.Services.AddDbContext<PostgresContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING")));

builder.Services.AddScoped<IFilesManager, GoogleDriveFilesManager>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();

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

app.MapFileServiceApi();
app.UseHsts();

app.Run();