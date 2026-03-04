using System;

namespace PillSync.DTOs;

public class RegisterDTOs
{
    public required string EmailAddress { get; set; }
    public required string FullName { get; set; }
    public required string password { get; set; }


}
