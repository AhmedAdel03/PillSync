using System;
using PillSync.DTOs;
using PillSync.Entites;
using PillSync.Services.Interface;

namespace PillSync.Extention;

public static class UserExtension
{
  public static UserDTOs ToUserDTOs(this User user,ITokenService tokenService) 
    {
        var newUser=new UserDTOs()
        {
            UserId=user.Id,
            FullName=user.FullName,
            EmailAddress=user.EmailAddress,
            IsVerifed=user.Member.IsVerifed,
            Token=tokenService.CreateToken(user)

        };
        return newUser;
        
    }
    
}
