using OzonParserService.Application.ParsingTasks.Persistence;
using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence;

public class ParsingTaskRepository(
    ILogger<ParsingTaskRepository> logger,
    OzonDbContext context) : IParsingTaskRepository
{
    public async Task<IEnumerable<ParsingTask>> GetDueTasksAsync(
        DateTime currentTime,
        CancellationToken cancellationToken) =>
        await context.ParsingTasks
            .AsNoTracking()
            .Where(t => t.NextRun <= currentTime && (t.Status == ParserTaskStatus.Scheduled))
            .OrderBy(t => t.NextRun)
            .ToListAsync(cancellationToken: cancellationToken);

    public async Task<ParsingTask> AddAsync(
        ParsingTask task,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding new parsing task");
        var addedEntity = await context.ParsingTasks.AddAsync(task, cancellationToken);
        return addedEntity.Entity;
    }

    public async Task<ParsingTask?> GetByIdAsync(
        ParsingTaskId id,
        CancellationToken cancellationToken = default)
    {
        var parsingTaskById = await context.ParsingTasks
            .FindAsync([id, cancellationToken], cancellationToken);

        if (parsingTaskById == null)
            logger.LogError($"Parsing task not found with this {id} was not found");

        logger.LogInformation($"Parsing task found with this {id}");
        return parsingTaskById;
    }

    public async Task<ParsingTask> UpdateByIdAsync(
        ParsingTask task,
        ParsingTaskId id,
        CancellationToken cancellationToken)
    {
        var parsingTask = await GetByIdAsync(id, cancellationToken);

        context.Entry(parsingTask!).CurrentValues.SetValues(task);
        logger.LogInformation($"Parsing task with this {id} was updated");

        return parsingTask!;
    }

    public async Task DeleteByIdAsync(
        ParsingTaskId id,
        CancellationToken cancellationToken)
    {
        var parsingTask = await GetByIdAsync(id, cancellationToken);

        context.ParsingTasks.Remove(parsingTask!);
        logger.LogInformation($"Parsing task with this {id} was deleted");
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
