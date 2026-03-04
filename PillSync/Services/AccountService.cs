using System;
using System.Security.Cryptography;
using System.Text;
using PillSync.Data.Repo;
using PillSync.DTOs;
using PillSync.Entites;
using PillSync.Extention;

namespace PillSync.Services;

public class AccountService(IMemberRepo memberRepo) : IAccountService
{
    public async Task<UserDTOs> Login(LoginDTOs loginDTOs)
    {
         
return null;

    }

    public async Task<UserDTOs> Register(RegisterDTOs registerDTOs)
    {
        var User= await memberRepo.GetByEmail(registerDTOs.EmailAddress);
        if(User!=null) return null;
        else
        {
            var Hmac=new HMACSHA3_512();
         var password=Hmac.ComputeHash(Encoding.UTF32.GetBytes(registerDTOs.password));
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
        return UserExtension.ToUserDTOs(newUser);
 
        }
        
    }
}
