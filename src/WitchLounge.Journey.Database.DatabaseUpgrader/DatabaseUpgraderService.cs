using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WitchLounge.Journey.Database.DatabaseUpgrader;

internal sealed class DatabaseUpgraderService(ILogger<DatabaseUpgraderService> logger, IConfiguration configuration, IHostApplicationLifetime hostApplicationLifetime) : IHostedService {
  private readonly ILogger _logger = logger; 
  private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;
  private readonly IConfiguration _configuration = configuration;

    public Task StartAsync(CancellationToken cancellationToken){    
    _hostApplicationLifetime.ApplicationStarted.Register(() => Task.Run(async () =>
    {
        try
        {
            _logger.LogInformation("Hello World");
            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }));
    return Task.CompletedTask;    
  }

  public Task StopAsync(CancellationToken cancellationToken) {
    return Task.CompletedTask;
  }
}