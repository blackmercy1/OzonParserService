using Hangfire;
using OzonParserService.Infrastructure.ParsingTaskPersistence.Jobs;

namespace OzonParserService.Web.Configuration;

public static class MiddlewareConfiguration
{
    public static WebApplication Configure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app
                .UseDeveloperExceptionPage()
                .ConfigureSwaggerGen(app);
        }
        else
            app
                .UseExceptionHandler(_ => { })
                .UseHsts();

        app
            .UseHttpsRedirection()
            .UseRouting()
            .UseHangfire();
        
        app.MapControllers();

        return app;
    }
    
    private static IApplicationBuilder UseHangfire(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard();
        
        RecurringJob.AddOrUpdate<ParsingTaskJob>(
            "process-tasks", 
            job => job.ExecuteAsync(CancellationToken.None), 
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

            c.SwaggerEndpoint(url, appName);
        });

        return builder;
    }
}
