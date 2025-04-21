using OzonParserService.Application.ParsingTasks.Jobs;
using OzonParserService.Application.ParsingTasks.Persistence;
using OzonParserService.Application.ParsingTasks.Services;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence.Jobs;

public class ParsingTaskJob : IJob
{
    private readonly IParsingTaskRepository _repository;
    private readonly IParsingTaskService _taskService;
    private readonly ILogger<ParsingTaskJob> _logger;

    public ParsingTaskJob(
        IParsingTaskRepository repository,
        IParsingTaskService taskService,
        ILogger<ParsingTaskJob> logger)
    {
        _repository = repository;
        _taskService = taskService;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await _repository.GetDueTasksAsync(DateTime.UtcNow, cancellationToken);

            foreach(var task in tasks) 
                await _taskService.ExecuteTaskAsync(task.Id.Value, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception with hangfire job");
        }
    }
}
