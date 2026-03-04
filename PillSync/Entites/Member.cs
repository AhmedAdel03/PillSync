using System;
using System.Text.Json.Serialization;
using PillSync.Enums;

namespace PillSync.Entites;

public class Member
{
    public string Id { get; set; }
    public required string FullName { get; set; }
    public  bool IsVerifed { get; set; }=false;
     public UserStatus UserStatus { get; set; }=UserStatus.Offline;
        

      [JsonIgnore]
      public User User { get; set; }
      public List<Photo> Photos { get; set; }=[];

}
