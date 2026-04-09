using System;

namespace PillSync.DTOs;

public class ForgotPasswordRequestDTO
{
    public required string EmailAddress { get; set; }
}
