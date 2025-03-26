using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

using OzonParserService.Application.RabbitMq.Configurations;

namespace OzonParserService.Application.RabbitMq.Services;

public class ParsingTaskRabbitMQProducerService : IParsingTaskRabbitMQProducerService, IAsyncDisposable
{
    private readonly string _exchangeName;
    private readonly string _routingKey;
    
    private IConnection? _connection;
    private IChannel? _channel;

    public ParsingTaskRabbitMQProducerService(IOptions<RabbitMQSettings> settings)
    {
        _exchangeName = settings.Value.ExchangeName;
        _routingKey = settings.Value.RoutingKey;
        
        Task.Run(async () => await OpenConnect(settings));
    }
    
    private async Task OpenConnect(IOptions<RabbitMQSettings> settings)
    {
        var factory = new ConnectionFactory
        {
            HostName = settings.Value.HostName,
            Port = settings.Value.Port,
            UserName = settings.Value.UserName,
            Password = settings.Value.Password
        };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public async Task SendMessage<T>(T obj)
    {
        var serializedObj = JsonConvert.SerializeObject(obj);
        var body = Encoding.UTF8.GetBytes(serializedObj);
        
        await _channel!.BasicPublishAsync(
            exchange: _exchangeName,
            routingKey: _routingKey,
            body: body);
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_connection != null) 
            await _connection.DisposeAsync();

        if (_channel != null) 
            await _channel.DisposeAsync();
        
        GC.SuppressFinalize(this);
    }
}
