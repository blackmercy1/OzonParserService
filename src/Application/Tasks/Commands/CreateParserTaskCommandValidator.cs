using FluentValidation;
using JetBrains.Annotations;

namespace OzonParserService.Application.Tasks.Commands;

[UsedImplicitly]
public class CreateParserTaskCommandValidator : AbstractValidator<CreateParserTaskCommand>
{
    public CreateParserTaskCommandValidator()
    {
        RuleFor(x => x.ProductUrl)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.IntervalHours)
            .NotEmpty()
            .GreaterThan(0);
    }
}
