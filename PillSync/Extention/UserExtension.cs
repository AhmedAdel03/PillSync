using System;
using PillSync.DTOs;
using PillSync.Entites;

namespace PillSync.Extention;

public static class UserExtension
{
  public static UserDTOs ToUserDTOs(this User user) 
    {
        var newUser=new UserDTOs
        {
            UserId=user.Id,
            FullName=user.FullName,
            EmailAddress=user.EmailAddress,
            IsVerifed=user.Member.IsVerifed,

        };
        return newUser;
        
    }

}
