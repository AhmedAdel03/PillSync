using System;
using PillSync.Entites;

namespace PillSync.Services.Interface;

public interface IOTP
{
public Task SendVerifyOTP(string memberEmail);
}
