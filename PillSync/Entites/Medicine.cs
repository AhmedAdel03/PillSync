using System;
using System.Text.Json.Serialization;

namespace PillSync.Entites;

public class Medicine
{
    public string ID { get; set; }=Guid.NewGuid().ToString();
    public required string MedicineName { get; set; }
    
 public required string Dosage { get; set; }
 public required string TypeOfDrug { get; set; }
 public required string Frequency { get; set; }
 public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
 public required string TimeTotake { get; set; }
 public  string Instructions { get; set; }=string.Empty;


      [JsonIgnore]
 public Member? member { get; set; }
public string? MemberId { get; set; }
public bool IsDeleted { get; set; }=false;

}
