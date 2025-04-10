namespace OzonParserService.Application.Common.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken();
}
