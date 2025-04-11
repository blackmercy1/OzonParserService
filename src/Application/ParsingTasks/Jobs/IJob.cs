namespace OzonParserService.Application.ParsingTasks.Jobs;

public interface IJob
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
