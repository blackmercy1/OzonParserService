

using Microsoft.Extensions.Hosting;

namespace OzonParserService.Application;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        return builder;
    }
}
