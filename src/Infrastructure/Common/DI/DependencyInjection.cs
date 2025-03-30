using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OzonParserService.Application.ParsingTasks.Jobs;
using OzonParserService.Application.ParsingTasks.Persistance;
using OzonParserService.Application.Publish;
using OzonParserService.Infrastructure.ParsingTaskPersistence;
using OzonParserService.Infrastructure.ParsingTaskPersistence.Jobs;
using OzonParserService.Infrastructure.Persistance;
using OzonParserService.Infrastructure.ProductDataPersistance.Publisher;

namespace OzonParserService.Infrastructure.Common.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistance(configuration);
        return services;
    }

    private static IServiceCollection AddPersistance(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddMassTransitServices(configuration)
            .AddDbContext<OzonDbContext>(options =>
        {
            options.UseNpgsql(configuration["ConnectionStrings:DefaultConnection"]);
        });

        services
            .AddScoped<IParsingTaskRepository, ParsingTaskRepository>()
            .AddScoped<IProductDataPublisher, ProductDataPublisher>()
            .AddScoped<IJob, ParsingTaskJob>();
        
        services.AddHangfire(cfg =>
            cfg.UsePostgreSqlStorage(options 
                => options.UseNpgsqlConnection(configuration["ConnectionStrings:HangfireConnection"])));
        services.AddHangfireServer();
        
        return services;
    }
    
    private static IServiceCollection AddMassTransitServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(busConfiguration =>
        {
            busConfiguration.SetKebabCaseEndpointNameFormatter();
            busConfiguration.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["MessageBroker:Host"]!), c =>
                {
                    c.Username(configuration["MessageBroker:UserName"]!);
                    c.Password(configuration["MessageBroker:Password"]!);
                });

                cfg.UseMessageRetry(retryConfigurator =>
                {
                    retryConfigurator.Interval(5, TimeSpan.FromSeconds(2));
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}
