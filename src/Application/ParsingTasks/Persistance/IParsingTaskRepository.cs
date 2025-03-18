using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Application.ParsingTasks.Persistance;

public interface IParsingTaskRepository
{
    Task<ParsingTask?> GetByIdAsync(
        ParsingTaskId id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<ParsingTask>> GetDueTasksAsync(
        DateTime currentTime,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        ParsingTask task,
        CancellationToken cancellationToken = default);

    Task UpdateByIdAsync(
        ParsingTask task,
        ParsingTaskId id,
        CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(
        ParsingTaskId id,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
