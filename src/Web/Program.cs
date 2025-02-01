using OzonParserService.Application;
using OzonParserService.Web;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApplicationServices()
    .AddApplicationServices()
    .AddWebServices();

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
//ToDo: make authorization
app.MapControllers();
await app.RunAsync();
