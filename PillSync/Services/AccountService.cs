using System;
using System.Security.Cryptography;
using System.Text;
using PillSync.Data.Repo;
using PillSync.DTOs;
using PillSync.Entites;
using PillSync.Extention;
using PillSync.Services.Interface;

namespace PillSync.Services;

public class AccountService(IMemberRepo memberRepo,ITokenService tokenService) : IAccountService
{
    public async Task<UserDTOs?> Login(LoginDTOs loginDTOs)
    {
     var UserExist= await memberRepo.GetByEmail(loginDTOs.EmailAddress);
     if (UserExist==null)
        {
             return null;
        }
     else
        {
             
            var Hmac=new HMACSHA3_512(UserExist.PasswordKey);
            var ComputeHash=Hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTOs.password));
            if(ComputeHash.SequenceEqual(UserExist.Password))
            {
                return UserExtension.ToUserDTOs(UserExist,tokenService);
                
            }
            else
            {
                return null;
            }

            
        }
         
 
    }

    public async Task<UserDTOs> Register(RegisterDTOs registerDTOs)
    {
        var User= await memberRepo.GetByEmail(registerDTOs.EmailAddress);
        if(User!=null) return null;
        else
        {
            var Hmac=new HMACSHA3_512();
         var password=Hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTOs.password));
         var passwordKey=Hmac.Key;
         var newUser=new User
         {
           FullName=registerDTOs.FullName,
           EmailAddress=registerDTOs.EmailAddress,
           Password=password,
           PasswordKey=passwordKey,
           Member=new Member
           {
               FullName=registerDTOs.FullName,
               IsVerifed=false
           }
         };
        await memberRepo.AddNewUser(newUser);
        await memberRepo.SaveChanges();
        return UserExtension.ToUserDTOs(newUser, tokenService);
 
        }
        
    }
}
