using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using SkillForge.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SkillForge.Auth.Services;

public class JwtGenService(IConfiguration configuration)
{
    private string _key => configuration["Jwt:SecretKey"];
    private string _issuer => configuration["Jwt:Issuer"];
    private string _audience => configuration["Jwt:Audience"];

    public string GenerateToken(User user)
    {
        var claims = GetClaimList(user);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            signingCredentials: creds,
            expires: DateTime.UtcNow.AddHours(1)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private List<Claim> GetClaimList(User user)
    {
        return [
            new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        ];
    }
}