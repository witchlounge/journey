using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WitchLounge.Journey.Database.DatabaseUpgrader;

internal sealed class DatabaseUpgraderService(ILogger<DatabaseUpgraderService> logger, IConfiguration configuration, IHostApplicationLifetime hostApplicationLifetime) : IHostedService
{
    private readonly ILogger _logger = logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;
    private readonly IConfiguration _configuration = configuration;
    private ExitCode? _exitCode;

    [Flags]
    enum ExitCode {
        Success = 0,
        Failure = 1
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(DatabaseUpgraderService)}");
        // TODO is the line below necessary?
        // _hostApplicationLifetime.ApplicationStarted.Register(() => Task.Run(async () =>
        // {
        try
        {
            _logger.LogInformation("Hello World");
            await Task.Delay(1000, cancellationToken);
            _exitCode = ExitCode.Success;
        }
        catch (OperationCanceledException) {
            _logger.LogInformation("Database Upgrade operation canceled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            _exitCode = ExitCode.Failure;
        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Environment.ExitCode = (int?)_exitCode ?? -1;
        _logger.LogInformation($"Stopping service {nameof(DatabaseUpgraderService)}");
        return Task.CompletedTask;
    }
}