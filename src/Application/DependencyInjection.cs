using System.Reflection;
using FluentValidation;
using MassTransit;
using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OzonParserService.Application.Common.Behaviours;
using OzonParserService.Application.ParsingTasks.Services;
using OzonParserService.Application.ProductParsers.Services;
using OzonParserService.Application.RabbitMq.Configurations;
using OzonParserService.Application.RabbitMq.Services;

namespace OzonParserService.Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplicationServices(
        this IHostApplicationBuilder builder,
        IConfiguration configuration)
    {
        builder.Services
            .AddScoped<IParsingTaskService, ParsingTaskService>()
            .AddScoped<IProductParserService, ProductParserService>()
            .AddSingleton<IParsingTaskRabbitMQProducerService, ParsingTaskRabbitMQProducerService>()
            .AddHostedService<TaskExecutionBackgroundService>();
        
        builder.Services
            .AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));
        
        builder.Services.
            Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

        builder.Services.AddMassTransitServices(configuration);
        
        builder.Services
            .AddMediatR(cfg
                => cfg.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));
        
        builder.Services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return builder;
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
