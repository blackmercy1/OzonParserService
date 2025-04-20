using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Application.ParsingTasks.Persistence;

public interface IParsingTaskRepository
{
    Task<ParsingTask?> GetByIdAsync(
        ParsingTaskId id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<ParsingTask>> GetDueTasksAsync(
        DateTime currentTime,
        CancellationToken cancellationToken = default);

    Task<ParsingTask> AddAsync(
        ParsingTask task,
        CancellationToken cancellationToken = default);

    Task<ParsingTask> UpdateByIdAsync(
        ParsingTask task,
        ParsingTaskId id,
        CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(
        ParsingTaskId id,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
