using System.Text.Json.Serialization;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Files
{
  public class UploadPartUrlResponse
  {
    public string AuthorizationToken { get; set; }
    [JsonIgnore]
    public DateTime ExpirationDate { get; } = DateTime.UtcNow.AddHours(23);
    public string FileId { get; set; }
    public string UploadUrl { get; set; }
  }
}
