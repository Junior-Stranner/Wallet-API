
using System.Security.Claims;
using System.Text;

public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _issuer = "auth-api";
    private readonly string _audience = "auth-api-client";

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string email)
    {
        var secretKey = _configuration["Jwt:Secret"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddHours(12);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            },
            expires: expiration,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParams = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

        return tokenHandler.ValidateToken(token, validationParams, out _);
    }
}

internal class JwtSecurityToken
{
    private string issuer;
    private string audience;
    private Claim[] claims;
    private DateTime expires;
    private SigningCredentials signingCredentials;

    public JwtSecurityToken(string issuer, string audience, Claim[] claims, DateTime expires, SigningCredentials signingCredentials)
    {
        this.issuer = issuer;
        this.audience = audience;
        this.claims = claims;
        this.expires = expires;
        this.signingCredentials = signingCredentials;
    }
}

internal class SigningCredentials
{
    private SymmetricSecurityKey key;
    private object hmacSha256;

    public SigningCredentials(SymmetricSecurityKey key, object hmacSha256)
    {
        this.key = key;
        this.hmacSha256 = hmacSha256;
    }
}

internal class SymmetricSecurityKey
{
    private byte[] bytes;

    public SymmetricSecurityKey(byte[] bytes)
    {
        this.bytes = bytes;
    }
}