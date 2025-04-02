using AutoMapper;
using JetBrains.Annotations;
using OzonParserService.Application.ParsingTasks.Commands;
using OzonParserService.Contracts.ParsingTask;
using OzonParserService.Domain.ParserTaskAggregate;

namespace OzonParserService.Web.ParsingTasks.Mapping;

[UsedImplicitly]
public class ParserTaskMappingProfile : Profile
{
    public ParserTaskMappingProfile()
    {
        CreateMap<CreateParsingTaskRequest, CreateParsingTaskCommand>();
        CreateMap<ParsingTask, ParsingTaskResponse>();
    }
}
