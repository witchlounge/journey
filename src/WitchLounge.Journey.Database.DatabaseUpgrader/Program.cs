using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using WitchLounge.Journey.Common;

namespace WitchLounge.Journey.Database.DatabaseUpgrader;

internal class Program
{
  static async Task Main(string[] args)
  {
    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
    IHostEnvironment env = builder.Environment;

    builder.Configuration.Sources.Clear();
    builder.Configuration
      .AddJsonFile("appsettings.json", true, true)
      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
      .AddEnvironmentVariables(prefix: "JOURNEY_")
      .AddCommandLine(args);

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(
      new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger());

    builder.Services
      .AddHostedService<DatabaseUpgraderService>()
      .AddOptions<DatabaseOptions>()
      .Bind(builder.Configuration.GetSection(DatabaseOptions.Key))
      .ValidateDataAnnotations();  // TODO is this working?

    using IHost host = builder.Build();
    await host.RunAsync();
  }
}