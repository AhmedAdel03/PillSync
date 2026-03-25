using System;

namespace PillSync.DTOs;

public class EditProfileDTO
{
    public required string FullName { get; set; }
    public required string EmailAddress { get; set; }
    public required string PhoneNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }



}
