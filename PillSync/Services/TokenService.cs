using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PillSync.Entites;
using PillSync.Services.Interface;

namespace PillSync.Services;

public class TokenService(IConfiguration Config) : ITokenService
{
    public string CreateToken(User user)
    {
        var TokenKey=Config["TokenKey"]?? throw new Exception("no TokenKey");
        var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey));
        var Claims=new List<Claim>
        {
            new (ClaimTypes.Email,user.EmailAddress),
            new (ClaimTypes.NameIdentifier,user.Id)
        };
        var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512);
        var tokenDescriptor=new SecurityTokenDescriptor
        {
            Subject=new ClaimsIdentity(Claims),
            Expires=DateTime.UtcNow.AddDays(30),
            SigningCredentials=creds
        };
        var tokenhandler=new JwtSecurityTokenHandler();
       var token= tokenhandler.CreateToken(tokenDescriptor);
        return tokenhandler.WriteToken(token);

         
    }
}
