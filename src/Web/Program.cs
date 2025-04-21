var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWeb(builder)
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Host.UseSerilog();

var app = builder.Build();

app.Configure();

app.Run();
