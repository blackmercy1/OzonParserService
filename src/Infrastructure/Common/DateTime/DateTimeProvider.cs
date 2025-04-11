using OzonParserService.Application.Common.DateTimeProvider;

namespace OzonParserService.Infrastructure.Common.DateTime;

public class DateTimeProvider : IDateTimeProvider
{
    public System.DateTime UtcNow => System.DateTime.UtcNow;
}
