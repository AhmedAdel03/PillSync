using System;
using System.Text.Json.Serialization;

namespace PillSync.Entites;

public class OTP
{
 public string OtpId { get; set; }=Guid.NewGuid().ToString();
public required string MemberId { get; set; }
public required string  OTPCode { get; set; }
public bool IsRevoked {get;set;}
public DateTime Timecreated { get; set; }=DateTime.UtcNow;
// nav
[JsonIgnore]
public Member member { get; set; }
}
