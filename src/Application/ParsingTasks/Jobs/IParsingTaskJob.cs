namespace OzonParserService.Application.ParsingTasks.Jobs;

public interface IParsingTaskJob
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
