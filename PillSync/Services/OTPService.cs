using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using PillSync.Data.Repo;
using PillSync.DTOs;
using PillSync.Entites;
using PillSync.Services.Interface;
 

namespace PillSync.Services;

public class OTPService (IMemberRepo memberRepo,IConfiguration config ): IOTP
{
    public async Task SendOTP(string memberEmail)
    {
        var member= await memberRepo.GetByEmail(memberEmail);
        var otpCode=CreateRandomCode();
        var otp=new OTP
        {
          MemberId=member.Id,
          OTPCode=otpCode,
          IsRevoked=false,
        };
        await SendToEmailServer(memberEmail,otpCode);
        await memberRepo.PostOtp(otp);
        await memberRepo.SaveChanges();
    }

    

    private string CreateRandomCode()
    {
        int randomNumber = RandomNumberGenerator.GetInt32(10000, 100000);
    return randomNumber.ToString("D5");
    }

    private async Task RevokeOTP( OTP oTP)
    {
        
           oTP.IsRevoked=true;
       
    await memberRepo.SaveChanges();         
    }

    private async Task SendToEmailServer(string email, string Body)
    {
        
      var smtpClient = new SmtpClient("pro.turbo-smtp.com")
    {
        Port = 587,
        Credentials = new NetworkCredential(
            config["TurboSetting:Consumer-Key"], 
            config["TurboSetting:Consumer-Secret"]
        ),
        EnableSsl = true
    };

    var mail = new MailMessage
    {
        From = new MailAddress("pillsync@domain.com"),
        Subject = "PillSync Verification Code",
        Body = Body,
        IsBodyHtml = true
    };

    mail.To.Add(email);

    await smtpClient.SendMailAsync(mail);
    }

    public async Task<bool> VerifyOTP(OTPCodeDTO codeDTO, string memberId)
{
    var member = await memberRepo.GetMemberOTPs(memberId);
    
    
    if (member?.OTPs == null || !member.OTPs.Any()) 
    {
        return false;
    }
    var validOtp = member.OTPs.FirstOrDefault(x => 
        x.OTPCode == codeDTO.Code && 
        (DateTime.UtcNow - x.Timecreated).TotalMinutes <= 5);

    if (validOtp == null) 
    {
        return false;
    }

    await RevokeOTP(validOtp);
     member.IsVerifed=true;
    await memberRepo.SaveChanges();
   

    return true;
}

    
}
