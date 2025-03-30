using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;

using OzonParserService.Application.ParsingTasks.Commands;
using OzonParserService.Web.Common.Errors;

namespace OzonParserService.Web.Common.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddWeb(
        this IServiceCollection _,
        WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllers()
            .AddNewtonsoftJson();

        builder.Services
            .AddEndpointsApiExplorer()
            .AddSingleton<ProblemDetailsFactory, ParserProblemDetailsFactory>();

        builder.Services
            .AddSerilogServices(builder)
            .AddFluentValidation()
            .AddAutoMapper()
            .AddSwagger()
            .AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsBuilder => corsBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

        builder.Host.UseSerilog();

        return builder.Services;
    }

    private static IServiceCollection AddSerilogServices(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog();

        return services;
    }

    private static IServiceCollection AddSwagger(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo {Title = "Ozon parser API", Version = "v1"});
        });
        return services;
    }

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<CreateParserTaskCommandValidator>();

        return services;
    }

    private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}
