using OzonParserService.Domain.ParserTaskAggregate.ValueObject;

namespace OzonParserService.Domain.ParserTaskAggregate.DomainEvents;

public record ParserTaskCreatedEvent(ParsingTaskId TaskId) : IDomainEvent;