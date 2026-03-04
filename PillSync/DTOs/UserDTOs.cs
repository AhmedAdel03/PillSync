using System;

namespace PillSync.DTOs;

public class UserDTOs
{
  public required string UserId { get; set; }
    public required string EmailAddress { get; set; }

    public required string FullName { get; set; }
    public string? ImageURl { get; set; }
    public bool IsVerifed { get; set; }
   // public required string Token { get; set; }
}
