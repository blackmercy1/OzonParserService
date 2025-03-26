namespace OzonParserService.Application.RabbitMq.Services;

public interface IParsingTaskRabbitMQProducerService
{
    Task SendMessage<T>(T obj);
}
