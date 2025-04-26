using System.Reflection;

using OzonParserService.Application.Common.Behaviours;
using OzonParserService.Application.ParsingTasks.Services;
using OzonParserService.Application.ProductParsers;

namespace OzonParserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services
            .AddScoped<IParsingTaskService, ParsingTaskService>()
            .AddScoped<IBrowserProductDataParser, BrowserProductDataParser>()
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
