using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using OzonParserService.Application.ParsingTasks.Persistance;

namespace OzonParserService.Application.ParsingTasks.Services;

public class TaskExecutionBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TaskExecutionBackgroundService> _logger;

    public TaskExecutionBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<TaskExecutionBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IParsingTaskRepository>();
                    var taskService = scope.ServiceProvider.GetRequiredService<IParsingTaskService>();

                    var tasks = await repository.GetDueTasksAsync(DateTime.UtcNow);
                    
                    foreach (var task in tasks)
                    {
                        await taskService.ExecuteTaskAsync(task.Id.Value, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing tasks");
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
        }
    }
}
