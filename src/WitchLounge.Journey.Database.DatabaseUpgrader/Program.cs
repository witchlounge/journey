using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using WitchLounge.Journey.Common;

namespace WitchLounge.Journey.Database.DatabaseUpgrader;

internal class Program
{
    private static HostApplicationBuilder ConfigureHostBuilderAsync(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        IHostEnvironment env = builder.Environment;

        builder.Configuration.Sources.Clear();
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false)
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
            .ValidateDataAnnotations();

        return builder;
    }

    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            using IHost host = ConfigureHostBuilderAsync(args).Build();
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An unhandled exception occurred during initialization.");
        }
        finally
        {
            Log.CloseAndFlush();

        }
    }
}