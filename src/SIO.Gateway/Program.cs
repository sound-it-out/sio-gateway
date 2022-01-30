using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using SIO.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", false, true)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", false, true);

builder.Services.AddAuthentication(builder.Configuration, builder.Environment)
    .AddCors()
    .AddOcelot();

var app = builder.Build();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()
                .WithExposedHeaders("Content-Disposition"));

await app.UseOcelot();
await app.RunAsync();
