using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OzonParserService.Application.Common.Behaviours;
using OzonParserService.Application.ParsingTasks.Services;
using OzonParserService.Application.ProductParsers.Services;

namespace OzonParserService.Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IParsingTaskService, ParsingTaskService>()
            .AddScoped<IProductParserService, ProductParserService>()
            .AddHostedService<TaskExecutionBackgroundService>();
        
        builder.Services
            .AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));
        
        builder.Services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));
        
        builder.Services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return builder;
    }
}
