using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonParserService.Application.ParsingTasks.Commands;
using OzonParserService.Contracts.Tasks;

namespace OzonParserService.Web.Controllers;

public class ParserTaskController : ApiController
{
    private readonly IMapper _mapper; 
    private readonly ISender _mediator;
    
    public ParserTaskController(
        IMapper mapper,
        ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTaskAsync([FromBody] CreateParserTaskRequest request)
    {
        var createTaskRequest = _mapper.Map<CreateParserTaskCommand>(request);
        var result = await _mediator.Send(createTaskRequest);

        return Ok();
    }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetTaskAsync(string id)
    // {
    //     
    // }
}
