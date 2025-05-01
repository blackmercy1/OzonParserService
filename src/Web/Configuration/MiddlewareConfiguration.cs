using OzonParserService.Application.Outbox.Jobs;
using OzonParserService.Application.ParsingTasks.Jobs;

namespace OzonParserService.Web.Configuration;

public static class MiddlewareConfiguration
{
    public static WebApplication Configure(this WebApplication app)
    {
        app
            .UseSerilogRequestLogging()
            .UseExceptionHandler(opt => { });


        if (app.Environment.IsDevelopment())
            app.ConfigureSwaggerGen(app);
        else
            app.UseHsts();

        app
            .UseHttpsRedirection()
            .UseRouting()
            .UseCors("AllowAll")
            .UseHangfire();

        app.MapControllers();

        return app;
    }

    private static IApplicationBuilder UseHangfire(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard();

        RecurringJob.AddOrUpdate<IParsingTaskJob>(
            "process-tasks",
            job => job.ExecuteAsync(CancellationToken.None),
            Cron.Minutely);

        RecurringJob.AddOrUpdate<IProcessOutboxJobMessagesJob>(
            "outbox-processes",
            job => job.ProcessOutboxMessages(CancellationToken.None),
            Cron.Minutely);

        return app;
    }

    private static IApplicationBuilder ConfigureSwaggerGen(
        this IApplicationBuilder builder,
        WebApplication app)
    {
        app
            .UseSwagger()
            .UseSwaggerUI(c =>
            {
                var url = "/swagger/v1/swagger.json";
                var appName = $"{app.Environment.ApplicationName} v1";

                c.SwaggerEndpoint(
                    url,
                    appName);
            });

        return builder;
    }
}
