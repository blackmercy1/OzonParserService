using System.Reflection;
using FluentValidation;
using MediatR;

using Microsoft.Extensions.DependencyInjection;

using OzonParserService.Application.Common.Authentication;
using OzonParserService.Application.Common.Behaviours;
using OzonParserService.Application.ParsingTasks.Services;
using OzonParserService.Application.ProductParsers.Services;

namespace OzonParserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services
            .AddScoped<IParsingTaskService, ParsingTaskService>()
            .AddScoped<IProductParserService, ProductParserService>()
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}
