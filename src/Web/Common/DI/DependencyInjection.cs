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
        var requstUri = "http://localhost:5046";
        
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithProperty("microservice_name", "ozon_parser_service") 
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.DurableHttpUsingFileSizeRolledBuffers(requestUri: requstUri)
            .CreateLogger();

        builder.Host.UseSerilog();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
        => services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo {Title = "Ozon parser API", Version = "v1"});
        });

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        => services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<CreateParserTaskCommandValidator>();

    private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}
