namespace OzonParserService.Application.RabbitMq.Configurations;

public class RabbitMQSettings
{
    public required string HostName { get; set; }
    public required int Port { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string ExchangeName { get; set; }
    public required string RoutingKey { get; set; }
}
