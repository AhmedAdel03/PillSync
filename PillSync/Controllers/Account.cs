using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PillSync.DTOs;
using PillSync.Services;
using PillSync.Services.Interface;

namespace PillSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account(IAccountService accountService,IOTP oTPservice) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTOs>> Register(RegisterDTOs registerDTOs)
        {
            var result = await accountService.Register(registerDTOs);
            if (result == null) return BadRequest("Email Already exist");
            else
            {
                return Ok(result);
            }

        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTOs>> Login(LoginDTOs loginDTOs)
        {

            var result = await accountService.Login(loginDTOs);

            if (result == null)
                return Unauthorized("Invalid email or password");

            return Ok(result);


        }
        [HttpPost("Forgot-password/request")]
        public async Task<ActionResult> RequestForgotPassword(ForgotPasswordRequestDTO requestDTO)
        {
            var isSent = await accountService.RequestPasswordReset(requestDTO.EmailAddress);
            if (!isSent)
            {
                return BadRequest("Email is not registered");
            }

            return Ok("Reset code sent to your email");
        }

        [HttpPost("Forgot-password/reset")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            var isReset = await accountService.ForgotPassword(forgotPasswordDTO);
            if (!isReset)
            {
                return BadRequest("Invalid email or code");
            }

            return Ok("Password reset successfully");
        }
        [Authorize]
        [HttpPost("Send-otp")]
       public async Task<ActionResult> SendOtp()
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            if(email==null) return Unauthorized();
            await oTPservice.SendOTP(email);
            return Ok();
        }
        [Authorize]
        [HttpPost("Verify-otp")]
       public async Task<ActionResult> VerifyOtp(OTPCodeDTO code)
        {
            var memberId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(memberId==null) return BadRequest();
            var verification= await oTPservice.VerifyOTP(code,memberId);
             if(verification==false)
             return BadRequest("Wrong code");
             else 
             return Ok("member Is verifed");
        }
        [Authorize]
        [HttpPost("EditProfile")]
    public async Task<ActionResult<UserDTOs>>EditProfile(EditProfileDTO editProfileDTO)
        {
          var memberId=User.FindFirstValue(ClaimTypes.NameIdentifier);
          var result= await accountService.EditProfile(editProfileDTO,memberId);
         return result;
 
        }
    }
   

}
