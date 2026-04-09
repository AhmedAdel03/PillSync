using System;

namespace PillSync.DTOs;

public class ForgotPasswordDTO
{
    public required string EmailAddress { get; set; }
    public required string Code { get; set; }
    public required string NewPassword { get; set; }
}
