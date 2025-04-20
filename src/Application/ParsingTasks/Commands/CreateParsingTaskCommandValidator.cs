namespace OzonParserService.Application.ParsingTasks.Commands;

[UsedImplicitly]
public class CreateParsingTaskCommandValidator : AbstractValidator<CreateParsingTaskCommand>
{
    public CreateParsingTaskCommandValidator()
    {
        RuleFor(x => x.ProductUrl)
            .NotEmpty();

        RuleFor(x => x.IntervalHours)
            .NotEmpty();
    }
}
