
using Microsoft.Extensions.Hosting;

namespace OzonParserService.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        return builder;
    }
}
