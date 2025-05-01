using OzonParserService.Application.ParsingTasks.Commands;
using OzonParserService.Web.Handlers;

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
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails()
            .AddSerilogServices(builder)
            .AddFluentValidation()
            .AddEndpointsApiExplorer()
            .AddAutoMapper()
            .AddSwagger()
            .AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsBuilder => corsBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

        return builder.Services;
    }

    private static IServiceCollection AddSerilogServices(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo {Title = "Ozon parser API", Version = "v1"});
            options.CustomSchemaIds(x => x.FullName);
        });
        
        return services;
    }

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
        => services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<CreateParsingTaskCommandValidator>();

    private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
}
