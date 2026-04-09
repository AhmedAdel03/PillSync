using System;
using System.Security.Cryptography;
using System.Text;
using PillSync.Data.Repo;
using PillSync.DTOs;
using PillSync.Entites;
using PillSync.Extention;
using PillSync.Services.Interface;

namespace PillSync.Services;

public class AccountService(IMemberRepo memberRepo, ITokenService tokenService, IOTP otpService) : IAccountService
{
    public async Task<bool> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
    {
        var user = await memberRepo.GetByEmail(forgotPasswordDTO.EmailAddress);
        if (user == null || user.Member == null)
        {
            return false;
        }

        var otpCodeDto = new OTPCodeDTO
        {
            Code = forgotPasswordDTO.Code
        };

        var isOtpValid = await otpService.VerifyPasswordResetOTP(otpCodeDto, user.Member.Id);
        if (!isOtpValid)
        {
            return false;
        }

        var hmac = new HMACSHA3_512();
        user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(forgotPasswordDTO.NewPassword));
        user.PasswordKey = hmac.Key;
        await memberRepo.SaveChanges();
        return true;
    }

    public async Task<UserDTOs> EditProfile(EditProfileDTO editProfileDTO,string memberId)
    {
       var CurrentUserData= await memberRepo.GetByID(memberId);
       if(CurrentUserData==null)
        {
            return null;
        }
        CurrentUserData.EmailAddress=editProfileDTO.EmailAddress;
        CurrentUserData.FullName=editProfileDTO.FullName;
        CurrentUserData.DateOfBirth=editProfileDTO.DateOfBirth;
        CurrentUserData.PhoneNumber=editProfileDTO.PhoneNumber;
        await memberRepo.SaveChanges();
        return UserExtension.ToUserDTOs(CurrentUserData,tokenService);

    }

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
           DateOfBirth=registerDTOs.DateOfBirth,
           PhoneNumber=registerDTOs.PhoneNumber,
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

    public async Task<bool> RequestPasswordReset(string emailAddress)
    {
        var user = await memberRepo.GetByEmail(emailAddress);
        if (user == null)
        {
            return false;
        }

        await otpService.SendOTP(emailAddress);
        return true;
    }
}
