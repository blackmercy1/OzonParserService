﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OzonParserService.Application.ParsingTasks.Persistance;
using OzonParserService.Infrastructure.ParsingTaskPersistence;
using OzonParserService.Infrastructure.Persistance;

namespace OzonParserService.Infrastructure.Common.DI;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddPersistance(builder);
        return builder;
    }

    private static IServiceCollection AddPersistance(
        this IServiceCollection services,
        IHostApplicationBuilder builder)
    {
        services.AddDbContext<OzonDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddScoped<IParsingTaskRepository, ParsingTaskRepository>();
        
        return services;
    }
}
