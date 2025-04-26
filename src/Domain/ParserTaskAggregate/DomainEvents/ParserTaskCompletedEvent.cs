using OzonParserService.Domain.ProductDataAggregate;

namespace OzonParserService.Domain.ParserTaskAggregate.DomainEvents;

public record ParserTaskCompletedEvent(
    ParsingTask ParsingTask,
    ProductData ProductData) : IDomainEvent;
