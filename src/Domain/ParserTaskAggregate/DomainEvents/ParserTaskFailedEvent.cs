using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Domain.ParserTaskAggregate.DomainEvents;

public record ParserTaskFailedEvent(ParsingTaskId TaskId, string Error) : IDomainEvent;