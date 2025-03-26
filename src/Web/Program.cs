using OzonParserService.Application;
using OzonParserService.Infrastructure.Common.DI;
using OzonParserService.Web.Common.DI;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddWebServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
