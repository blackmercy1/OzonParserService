using AutoMapper;
using Contracts.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonParserService.Application.Tasks.Commands;

namespace OzonParserService.Web.Controllers;

[ApiController]
[Route("api/tasks")]
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
        
        
    }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetTaskAsync(string id)
    // {
    //     
    // }
}
