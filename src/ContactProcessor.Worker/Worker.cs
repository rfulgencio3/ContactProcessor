namespace ContactProcessor.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker execution started at: {time}", DateTimeOffset.Now);
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running and waiting for messages at: {time}", DateTimeOffset.Now);
            await Task.Delay(10000, stoppingToken); // Reduzindo a frequência para cada 10 segundos
        }
        _logger.LogInformation("Worker execution stopped at: {time}", DateTimeOffset.Now);
    }

}
