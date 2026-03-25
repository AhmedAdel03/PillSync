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
            Token=tokenService.CreateToken(user),
            Age=CalculateAge(user.DateOfBirth),
            PhoneNumber=user.PhoneNumber

        };
        return newUser;
        
    }


    public static int CalculateAge(DateOnly dateOfBirth)
{
    var today = DateOnly.FromDateTime(DateTime.Today);
    var age = today.Year - dateOfBirth.Year;

    if (dateOfBirth > today.AddYears(-age)) 
    {
        age--;
    }

    return age;
}
    
}
