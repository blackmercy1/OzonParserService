using OzonParserService.Application.Common.DateTimeProvider;
using OzonParserService.Application.ParsingTasks.Persistence;
using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Application.ParsingTasks.Services;

public class ParsingTaskService(
    IParsingTaskRepository parsingTaskRepository,
    IDateTimeProvider dateTimeProvider,
    ILogger<ParsingTaskService> logger)
    : IParsingTaskService
{
    public async Task<ErrorOr<ParsingTask>> ScheduleTaskAsync(
        string url,
        TimeSpan interval,
        CancellationToken cancellationToken)
    {
        var task = ParsingTask.Create(
            productUrl: url,
            checkInterval: interval,
            utcNow: dateTimeProvider.UtcNow
        );

        var parsingTask = await parsingTaskRepository.AddAsync(
            task,
            cancellationToken);
        
        await parsingTaskRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Task is scheduled.");

        return parsingTask;
    }

    public async Task<ErrorOr<Success>> ExecuteTaskAsync(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        var parsingTaskId = ParsingTaskId.Create(taskId);
        var task = await parsingTaskRepository.GetByIdAsync(
            parsingTaskId,
            cancellationToken);

        if (task is null)
        {
            logger.LogError($"Task with id {taskId} not found.");
            return Error.Failure(description: "task not found");
        }

        var result = task.Start();
        if (result.IsError)
        {
            logger.LogError(result.Errors.ToString());
            return result.Errors;
        }
        
        await parsingTaskRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }

    public Task<ErrorOr<Success>> CancelTaskAsync(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
