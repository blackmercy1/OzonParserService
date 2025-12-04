using OzonParserService.Application.Common.DateTimeProvider;
using OzonParserService.Application.ParsingTasks.Persistence;
using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Application.ParsingTasks.Services;

public class ParsingTaskService : IParsingTaskService
{
    private readonly IParsingTaskRepository _parsingTaskRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<ParsingTaskService> _logger;

    public ParsingTaskService(
        IParsingTaskRepository parsingTaskRepository,
        IDateTimeProvider dateTimeProvider,
        ILogger<ParsingTaskService> logger)
    {
        _parsingTaskRepository = parsingTaskRepository;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<ErrorOr<ParsingTask>> ScheduleTaskAsync(
        string url,
        TimeSpan interval,
        CancellationToken cancellationToken)
    {
        var task = ParsingTask.Create(
            productUrl: url,
            checkInterval: interval,
            utcNow: _dateTimeProvider.UtcNow
        );

        var parsingTask = await _parsingTaskRepository.AddAsync(
            task,
            cancellationToken);

        await _parsingTaskRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task is scheduled.");

        return parsingTask;
    }

    public async Task<ErrorOr<Success>> ExecuteTaskAsync(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        var parsingTaskId = ParsingTaskId.Create(taskId);

        var task = await _parsingTaskRepository.GetByIdAsync(
            parsingTaskId,
            cancellationToken);

        if (task is null)
        {
            _logger.LogError($"Task with id {taskId} not found.");

            return Error.Failure(description: "task not found");
        }

        var result = task.Start();

        if (result.IsError)
        {
            _logger.LogError(result.Errors.ToString());

            return result.Errors;
        }

        await _parsingTaskRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }

    public Task<ErrorOr<Success>> CancelTaskAsync(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}