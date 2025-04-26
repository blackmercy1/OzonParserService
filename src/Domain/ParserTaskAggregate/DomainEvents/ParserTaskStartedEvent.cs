namespace OzonParserService.Domain.ParserTaskAggregate.DomainEvents;

public record ParserTaskStartedEvent(ParsingTask ParsingTask) : IDomainEvent;