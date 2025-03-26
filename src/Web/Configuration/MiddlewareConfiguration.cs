namespace OzonParserService.Web.Configuration;

public static class MiddlewareConfiguration
{
    public static WebApplication Configure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            ConfigureSwaggerGen(app);
        }
        else
        {
            app.UseExceptionHandler(_ => { });
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.MapControllers();

        return app;
    }
    
    private static void ConfigureSwaggerGen(
        WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            var url = "/swagger/v1/swagger.json";
            var appName = $"{app.Environment.ApplicationName} v1";

            c.SwaggerEndpoint(url, appName);
        });
    }
}
