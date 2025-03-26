using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc.Infrastructure;

using OzonParserService.Application.ParsingTasks.Commands;
using OzonParserService.Web.Common.Errors;

namespace OzonParserService.Web.Common.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddWeb(
        this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddNewtonsoftJson();

        services.AddEndpointsApiExplorer();

        services.AddSingleton<ProblemDetailsFactory, ParserProblemDetailsFactory>();

        services
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

        return services;
    }

    private static IServiceCollection AddSwagger(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new() {Title = "Ozon parser API", Version = "v1"});
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
