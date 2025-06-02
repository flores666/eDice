using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddOcelot();
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();
await app.RunAsync();
