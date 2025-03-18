using ErrorOr;
using Microsoft.Extensions.Logging;

using OzonParserService.Application.ParsingTasks.Persistance;
using OzonParserService.Application.ProductParsers.Services;
using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Application.ParsingTasks.Services;

public class ParsingTaskService : IParsingTaskService
{
    private readonly IParsingTaskRepository _parsingTaskRepository;
    private readonly IProductParserService _productParserService;
    private readonly ILogger<ParsingTaskService> _logger;

    public ParsingTaskService(
        IParsingTaskRepository parsingTaskRepository,
        IProductParserService productParserService,
        ILogger<ParsingTaskService> logger)
    {
        _parsingTaskRepository = parsingTaskRepository;
        _productParserService = productParserService;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> ScheduleTaskAsync(
        string url,
        TimeSpan interval,
        CancellationToken cancellationToken)
    {
        try
        {
            var task = ParsingTask.Create(
                productUrl: url,
                checkInterval: interval
            );

            await _parsingTaskRepository.AddAsync(task, cancellationToken);
            await _parsingTaskRepository.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Task is scheduled.");
            
            return Result.Success;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Failure(description: "failed to schedule task");
        }
    }

    public async Task<ErrorOr<Success>> ExecuteTaskAsync(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        var parsingTaskId = ParsingTaskId.Create(taskId);
        var task = await _parsingTaskRepository.GetByIdAsync(parsingTaskId, cancellationToken);
        
        if (task == null)
        {
            _logger.LogError($"Task with id {taskId} not found.");
            return Error.Failure(description: "task not found");
        }

        task.Start();
        
        await _parsingTaskRepository.UpdateByIdAsync(task, parsingTaskId, cancellationToken);
        
        var productData = await _productParserService.ParserAsync(task.ProductUrl);
        //TODO: посылаем дату в другой микрач с помощью броккера

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
