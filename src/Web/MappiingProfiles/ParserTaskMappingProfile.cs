using AutoMapper;
using OzonParserService.Application.ParsingTasks.Commands;
using OzonParserService.Contracts.Tasks;

namespace OzonParserService.Web.MappiingProfiles;

public class ParserTaskMappingProfile : Profile
{
    public ParserTaskMappingProfile()
    {
        CreateMap<CreateParserTaskRequest, CreateParserTaskCommand>();
    }
}
