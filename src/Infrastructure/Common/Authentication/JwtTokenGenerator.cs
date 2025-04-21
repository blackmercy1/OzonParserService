namespace OzonParserService.Infrastructure.Common.Authentication;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly SymmetricSecurityKey _securityKey;
    private readonly IDateTimeProvider _timeProvider;
    
    public JwtTokenGenerator(
        IDateTimeProvider timeProvider,
        IConfiguration configuration)
    {
        _timeProvider = timeProvider;

        var secretKey = configuration["Ozon:SecurityKey"]!;
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    }

    public string GenerateToken()
    {
        var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "parser-service"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, "parser-service"),
            new Claim(JwtRegisteredClaimNames.Aud, "storage-service")
        };

        var token = new JwtSecurityToken(
            issuer: "parser-service",
            audience: "storage-service",
            claims: claims,
            expires: _timeProvider.UtcNow.AddMinutes(60),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
