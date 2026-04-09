using System;
using PillSync.DTOs;
using PillSync.Entites;

namespace PillSync.Services;

public interface IAccountService
{
    public Task<UserDTOs> Register(RegisterDTOs registerDTOs);
    public Task<UserDTOs?>Login(LoginDTOs loginDTOs);
    public Task<UserDTOs> EditProfile(EditProfileDTO editProfileDTO,string memberId);
    public Task<bool> RequestPasswordReset(string emailAddress);
    public Task<bool> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO);
    

}
