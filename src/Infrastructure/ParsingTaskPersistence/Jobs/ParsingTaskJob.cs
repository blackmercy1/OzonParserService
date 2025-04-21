using OzonParserService.Application.ParsingTasks.Jobs;
using OzonParserService.Application.ParsingTasks.Persistence;
using OzonParserService.Application.ParsingTasks.Services;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence.Jobs;

public class ParsingTaskJob(
    IParsingTaskRepository repository,
    IParsingTaskService taskService,
    ILogger<ParsingTaskJob> logger)
    : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await repository.GetDueTasksAsync(DateTime.UtcNow, cancellationToken);

            foreach(var task in tasks) 
                await taskService.ExecuteTaskAsync(task.Id.Value, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception with hangfire job");
        }
    }
}
