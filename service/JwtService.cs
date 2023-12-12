using System.IdentityModel.Tokens.Jwt;
using infrastructure.datamodels;
using Microsoft.IdentityModel.Tokens;

namespace service;

/**
 * This service makes sure, creates the JWT and validates them.
 */
public class JwtService
{
    private readonly JwtOptions _options;

    public JwtService(JwtOptions options)
    {
        _options = options;
    }

    private const string SignatureAlgorithm = SecurityAlgorithms.HmacSha512;

    public string IssueToken(SessionData data)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.CreateEncodedJwt(new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_options.secret),
                SignatureAlgorithm
            ),
            Issuer = _options.address,
            Audience = _options.address,
            Expires = DateTime.UtcNow.Add(_options.lifetime),
            Claims = data.ToDictionary()
        });
        return token;
    }

    /**
     * Validating the token and decodes it, to get the information out of it
     */
    public SessionData ValidateAndDecodeToken(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var principal = jwtHandler.ValidateToken(token, new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(_options.secret),
            ValidAlgorithms = new[] { SignatureAlgorithm },

            // Default value is true already.
            // They are just set here to emphasise the importance.
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,

            ValidAudience = _options.address,
            ValidIssuer = _options.address,

            // Set to 0 when validating on the same system that created the token
            ClockSkew = TimeSpan.FromSeconds(0)
        }, out var securityToken);
        return SessionData.FromDictionary(new JwtPayload(principal.Claims));
    }
}