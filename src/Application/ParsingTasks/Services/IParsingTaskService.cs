using OzonParserService.Domain.ParserTaskAggregate;

namespace OzonParserService.Application.ParsingTasks.Services;

public interface IParsingTaskService
{
    Task<ErrorOr<ParsingTask>> ScheduleTaskAsync(string url, TimeSpan interval, CancellationToken cancellationToken);

    Task<ErrorOr<Success>> ExecuteTaskAsync(Guid taskId, CancellationToken cancellationToken);

    Task<ErrorOr<Success>> CancelTaskAsync(Guid taskId, CancellationToken cancellationToken);
}
