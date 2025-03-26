using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OzonParserService.Application.ParsingTasks.Persistance;
using OzonParserService.Infrastructure.ParsingTaskPersistence;
using OzonParserService.Infrastructure.Persistance;

namespace OzonParserService.Infrastructure.Common.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddPersistance(configurationManager);
        return services;
    }

    private static IServiceCollection AddPersistance(
        this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        configurationManager.AddUserSecrets(Assembly.GetExecutingAssembly());
        
        services.AddDbContext<OzonDbContext>(options =>
        {
            options.UseNpgsql(configurationManager["ConnectionStrings:ParserTasks"]);
        });
        
        services.AddScoped<IParsingTaskRepository, ParsingTaskRepository>();
        
        return services;
    }
}
