using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonParserService.Application.ParsingTasks.Commands;
using OzonParserService.Contracts.ParsingTask;
using OzonParserService.Web.Common.Controllers;

namespace OzonParserService.Web.ParsingTasks.Controllers;

[Produces("application/json")]
public class ParsingTaskController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public ParsingTaskController(
        IMapper mapper,
        ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ParsingTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateParsingTaskRequest request,
        CancellationToken cancellationToken)
    {
        var createTaskRequest = _mapper.Map<CreateParsingTaskCommand>(request);
        var result = await _mediator.Send(createTaskRequest, cancellationToken);

        return result.Match(
            parsingTask => Ok(_mapper.Map<ParsingTaskResponse>(parsingTask)),
            error => Problem(error));
    }
}
