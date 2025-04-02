using OzonParserService.Application;
using OzonParserService.Infrastructure.Common.DI;
using OzonParserService.Web.Common.DI;
using OzonParserService.Web.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWeb(builder)
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Host.UseSerilog();

var app = builder.Build();

app.Configure();

app.Run();
