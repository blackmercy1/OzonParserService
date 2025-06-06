﻿using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using ProductDataContract;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using OzonParserService.Application.Common.DateTimeProvider;
using OzonParserService.Application.ParsingTasks.Jobs;
using OzonParserService.Application.ParsingTasks.Persistance;
using OzonParserService.Application.Publish;

using OzonParserService.Infrastructure.Common.DateTime;
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
        services.AddPersistence(configuration);
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services,
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
            .AddScoped<IJob, ParsingTaskJob>()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>();

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
                cfg.Host(
                    new Uri(configuration["MessageBroker:Host"]!),
                    c =>
                    {
                        c.Username(configuration["MessageBroker:UserName"]!);
                        c.Password(configuration["MessageBroker:Password"]!);
                    });

                cfg.UseMessageRetry(r =>
                {
                    r.Exponential(
                        retryLimit: 4,
                        minInterval: TimeSpan.FromSeconds(1),
                        maxInterval: TimeSpan.FromSeconds(30),
                        intervalDelta: TimeSpan.FromSeconds(2));
                    r.Ignore<SecurityTokenExpiredException>();
                    r.Ignore<SecurityTokenException>();
                });

                cfg.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                    cb.TripThreshold = 15;
                    cb.ActiveThreshold = 10;
                    cb.ResetInterval = TimeSpan.FromMinutes(5);
                });

                cfg.Publish<ProductDataMessage>();

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
