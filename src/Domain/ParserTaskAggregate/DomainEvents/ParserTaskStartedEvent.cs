using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Domain.ParserTaskAggregate.DomainEvents;

public record ParserTaskStartedEvent(ParsingTaskId Id) : IDomainEvent;