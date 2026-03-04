using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PillSync.DTOs;
using PillSync.Services;

namespace PillSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account(IAccountService accountService) : ControllerBase
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
}
    } 
