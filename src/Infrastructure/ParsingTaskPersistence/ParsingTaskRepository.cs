using OzonParserService.Application.ParsingTasks.Persistence;
using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Infrastructure.ParsingTaskPersistence;

public class ParsingTaskRepository : IParsingTaskRepository
{
    private readonly ILogger<ParsingTaskRepository> _logger;
    private readonly OzonDbContext _context;

    public ParsingTaskRepository(
        ILogger<ParsingTaskRepository> logger,
        OzonDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IEnumerable<ParsingTask>> GetDueTasksAsync(
        DateTime currentTime,
        CancellationToken cancellationToken) =>
        await _context.ParsingTasks
            .AsNoTracking()
            .Where(t => t.NextRun <= currentTime && (t.Status == ParserTaskStatus.Scheduled))
            .OrderBy(t => t.NextRun)
            .ToListAsync(cancellationToken: cancellationToken);

    public async Task<ParsingTask> AddAsync(
        ParsingTask task,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new parsing task");
        var addedEntity = await _context.ParsingTasks.AddAsync(task, cancellationToken);
        return addedEntity.Entity;
    }

    public async Task<ParsingTask?> GetByIdAsync(
        ParsingTaskId id,
        CancellationToken cancellationToken = default)
    {
        var parsingTaskById = await _context.ParsingTasks
            .FindAsync([id, cancellationToken], cancellationToken);

        if (parsingTaskById == null)
            _logger.LogError($"Parsing task not found with this {id} was not found");

        _logger.LogInformation($"Parsing task found with this {id}");
        return parsingTaskById;
    }

    public async Task<ParsingTask> UpdateByIdAsync(
        ParsingTask task,
        ParsingTaskId id,
        CancellationToken cancellationToken)
    {
        var parsingTask = await GetByIdAsync(id, cancellationToken);

        _context.Entry(parsingTask!).CurrentValues.SetValues(task);
        _logger.LogInformation($"Parsing task with this {id} was updated");

        return parsingTask!;
    }

    public async Task DeleteByIdAsync(
        ParsingTaskId id,
        CancellationToken cancellationToken)
    {
        var parsingTask = await GetByIdAsync(id, cancellationToken);

        _context.ParsingTasks.Remove(parsingTask!);
        _logger.LogInformation($"Parsing task with this {id} was deleted");
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
