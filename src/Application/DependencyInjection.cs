using System.Reflection;
using FluentValidation;
using MassTransit;
using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OzonParserService.Application.Common.Behaviours;
using OzonParserService.Application.ParsingTasks.Services;
using OzonParserService.Application.ProductParsers.Services;
using OzonParserService.Application.RabbitMq.Configurations;
using OzonParserService.Application.RabbitMq.Services;

namespace OzonParserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddScoped<IParsingTaskService, ParsingTaskService>()
            .AddScoped<IProductParserService, ProductParserService>()
            .AddSingleton<IParsingTaskRabbitMQProducerService, ParsingTaskRabbitMQProducerService>()
            .AddHostedService<TaskExecutionBackgroundService>();
        
        services
            .AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));
        
        services.
            Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

        services.AddMassTransitServices(configuration);
        
        services
            .AddMediatR(cfg
                => cfg.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));
        
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
    
    private static IServiceCollection AddMassTransitServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(busConfiguration =>
        {
            busConfiguration.SetKebabCaseEndpointNameFormatter();
            
            busConfiguration.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                {
                    h.Username(configuration["MessageBroker:UserName"]!);
                    h.Password(configuration["MessageBroker:Password"]!);
                });

                configurator.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}
