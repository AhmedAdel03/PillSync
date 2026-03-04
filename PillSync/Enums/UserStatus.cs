using System.Text.Json.Serialization;

namespace PillSync.Enums;

 [JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserStatus
{
Offline = 0,
Online = 1
}
