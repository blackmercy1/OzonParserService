using OzonParserService.Domain.ParserTaskAggregate.ValueObject;
using OzonParserService.Domain.ProductDataAggregate;

namespace OzonParserService.Domain.ParserTaskAggregate.DomainEvents;

public record ParserTaskCompletedEvent(
    ParsingTaskId ParsingTaskId,
    ProductData ProductData) : IDomainEvent;
