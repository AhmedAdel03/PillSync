using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PillSync.Enums;

namespace PillSync.Entites;

public class User
{
    public string Id { get; set; }=Guid.NewGuid().ToString();
    public required string FullName { get; set; }
    [EmailAddress]
    public required string EmailAddress { get; set; }

    public required byte[] Password { get; set; }
    public required byte[]  PasswordKey { get; set; }
     public DateOnly DateOfBirth { get; set; }

    public required string PhoneNumber { get; set; }
    [JsonIgnore]
    public  Member Member { get; set; }
 
}
