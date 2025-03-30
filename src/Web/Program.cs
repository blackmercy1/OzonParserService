using OzonParserService.Application;
using OzonParserService.Infrastructure.Common.DI;
using OzonParserService.Web.Common.DI;
using OzonParserService.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWeb()
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

app.Configure();

app.Run();
