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
