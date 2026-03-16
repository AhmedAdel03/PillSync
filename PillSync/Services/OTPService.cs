using System;
using System.Security.Cryptography;
using PillSync.Data.Repo;
using PillSync.Entites;
using PillSync.Services.Interface;
using TurboSMTP;
using TurboSMTP.Domain;

namespace PillSync.Services;

public class OTPService (IMemberRepo memberRepo, TurboSMTPClient turboSMTPClient): IOTP
{
    public async Task SendVerifyOTP(string memberEmail)
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

    private async Task RevokeOTP(string MemberId)
    {
     var memberotp=await memberRepo.GetMemberOTPs(MemberId);
     if(memberotp==null||memberotp.OTPs==null) return;
     var activeOTP=memberotp.OTPs.Where(x=>x.IsRevoked==false&& (DateTime.UtcNow - x.Timecreated).TotalMinutes <= 5);
     foreach (var otp in activeOTP)
    {
        otp.IsRevoked = true;
    }
    await memberRepo.SaveChanges();         
    }

    private async Task SendToEmailServer(string email, string Code)
    {
        
        var emailMessage = new EmailMessage.Builder()
                .SetFrom("Pillsync@yourdomain.com")
                .AddTo(email)
                .SetSubject("PillSync verification Mail")
                .SetHtmlContent($"Your Verfiy Code is <b>{Code}</b>.")
                .Build();

            
            //Send your Email Message
            var result = await turboSMTPClient.Emails.SendAsync(emailMessage);
    }

}
