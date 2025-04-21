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
        CreateMap<ParsingTask, ParsingTaskResponse>()
            .ForMember(dest => dest.Status, 
                opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
