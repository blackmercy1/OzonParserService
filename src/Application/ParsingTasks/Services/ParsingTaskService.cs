using ErrorOr;
using Microsoft.Extensions.Logging;

using OzonParserService.Application.ParsingTasks.Persistance;
using OzonParserService.Application.ProductParsers.Services;
using OzonParserService.Application.Publish;
using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Application.ParsingTasks.Services;

public class ParsingTaskService : IParsingTaskService
{
    private readonly IParsingTaskRepository _parsingTaskRepository;
    private readonly IProductParserService _productParserService;
    private readonly IProductDataPublisher _productDataPublisher;

    private readonly ILogger<ParsingTaskService> _logger;

    public ParsingTaskService(
        IParsingTaskRepository parsingTaskRepository,
        IProductParserService productParserService,
        IProductDataPublisher productDataPublisher,
        ILogger<ParsingTaskService> logger)
    {
        _parsingTaskRepository = parsingTaskRepository;
        _productParserService = productParserService;
        _productDataPublisher = productDataPublisher;
        _logger = logger;
    }

    public async Task<ErrorOr<ParsingTask>> ScheduleTaskAsync(
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

            var parsingTask = await _parsingTaskRepository.AddAsync(task, cancellationToken); 
            await _parsingTaskRepository.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Task is scheduled.");
            
            return parsingTask;
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
        
        var productDataResult = await _productParserService.ParserAsync(task.ProductUrl);
        if (productDataResult.IsError)
        {
            var errors = productDataResult.Errors;
            _logger.LogError(errors.ToString());
            return errors;
        }
        
        task.Complete();
        
        await _parsingTaskRepository.UpdateByIdAsync(task, parsingTaskId, cancellationToken);
        await _parsingTaskRepository.SaveChangesAsync(cancellationToken);
        
        await _productDataPublisher.PublishProductDataAsync(productDataResult.Value);
        
        return Result.Success;
    }

    public Task<ErrorOr<Success>> CancelTaskAsync(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
