using System;
using System.Text.Json.Serialization;

namespace PillSync.Entites;

public class Photo
{
    public required int Id { get; set; }
    public int  PublicId { get; set; }
    public required string URL { get; set; }
    public required string MemberId { get; set; }
    //Nav
    [JsonIgnore]
    public Member Member { get; set; }

}
