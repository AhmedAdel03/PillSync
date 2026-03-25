using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
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

    private async Task SendToEmailServer(string email, string body)
{
    using var httpClient = new HttpClient();

    var url = "https://api.turbo-smtp.com/api/v2/mail/send";

    // Set required headers
    httpClient.DefaultRequestHeaders.Add("ConsumerKey", config["TurboSetting:Consumer-Key"]);
    httpClient.DefaultRequestHeaders.Add("ConsumerSecret", config["TurboSetting:Consumer-Secret"]);

    var payload = new
    {
        from = "pillsync@domain.com",
        to =email,
        subject = "PillSync Verification Code",
        content = body
    };

    var json = System.Text.Json.JsonSerializer.Serialize(payload);

    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await httpClient.PostAsync(url, content);

    var responseBody = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception($"TurboSMTP Error: {response.StatusCode} - {responseBody}");
    }
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
