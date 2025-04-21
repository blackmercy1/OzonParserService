using OzonParserService.Application.ParsingTasks.Persistence;
using OzonParserService.Application.ProductParsers.Services;
using OzonParserService.Application.Publish;
using OzonParserService.Domain.ParserTaskAggregate;
using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Application.ParsingTasks.Services;

public class ParsingTaskService(
    IParsingTaskRepository parsingTaskRepository,
    IProductParserService productParserService,
    IProductDataPublisher productDataPublisher,
    ILogger<ParsingTaskService> logger)
    : IParsingTaskService
{
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

            var parsingTask = await parsingTaskRepository.AddAsync(task, cancellationToken); 
            await parsingTaskRepository.SaveChangesAsync(cancellationToken);
            
            logger.LogInformation("Task is scheduled.");
            
            return parsingTask;
        }

        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: "failed to schedule task");
        }
    }

    public async Task<ErrorOr<Success>> ExecuteTaskAsync(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        var parsingTaskId = ParsingTaskId.Create(taskId);
        var task = await parsingTaskRepository.GetByIdAsync(parsingTaskId, cancellationToken);
        
        if (task == null)
        {
            logger.LogError($"Task with id {taskId} not found.");
            return Error.Failure(description: "task not found");
        }

        task.Start();
        
        var productDataResult = await productParserService.ParserAsync(task.ProductUrl);
        if (productDataResult.IsError)
        {
            var errors = productDataResult.Errors;
            logger.LogError(errors.ToString());
            return errors;
        }
        
        task.Complete();
        
        await parsingTaskRepository.UpdateByIdAsync(task, parsingTaskId, cancellationToken);
        await parsingTaskRepository.SaveChangesAsync(cancellationToken);
        
        await productDataPublisher.PublishProductDataAsync(productDataResult.Value);
        
        return Result.Success;
    }

    public Task<ErrorOr<Success>> CancelTaskAsync(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
