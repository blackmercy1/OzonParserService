using OzonParserService.Application.ParsingTasks.Commands;
using OzonParserService.Contracts.ParsingTask;

namespace OzonParserService.Web.ParsingTasks.Controllers;

[Produces("application/json")]
public class ParsingTaskController(
    IMapper mapper,
    ISender mediator) : ApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(ParsingTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateParsingTaskRequest request,
        CancellationToken cancellationToken)
    {
        var createTaskRequest = mapper.Map<CreateParsingTaskCommand>(request);
        var result = await mediator.Send(createTaskRequest, cancellationToken);

        return result.Match(
            parsingTask => Ok(mapper.Map<ParsingTaskResponse>(parsingTask)),
            error => Problem(error));
    }
}
