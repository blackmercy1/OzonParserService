using FluentValidation;
using JetBrains.Annotations;

namespace OzonParserService.Application.ParsingTasks.Commands;

[UsedImplicitly]
public class CreateParserTaskCommandValidator : AbstractValidator<CreateParserTaskCommand>
{
    public CreateParserTaskCommandValidator()
    {
        RuleFor(x => x.ProductUrl)
            .NotEmpty();

        RuleFor(x => x.IntervalHours)
            .NotEmpty();
    }
}
