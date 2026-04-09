using System;
using PillSync.DTOs;
using PillSync.Entites;

namespace PillSync.Services.Interface;

public interface IOTP
{
public Task SendOTP(string memberEmail);
public Task<bool> VerifyOTP(OTPCodeDTO codeDTO,string memberid);
public Task<bool> VerifyPasswordResetOTP(OTPCodeDTO codeDTO, string memberid);

}
