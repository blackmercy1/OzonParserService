using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Domain.ParserTaskAggregate.DomainEvents;

public record ParserTaskCompletedEvent(ParsingTaskId TaskId) : IDomainEvent;
