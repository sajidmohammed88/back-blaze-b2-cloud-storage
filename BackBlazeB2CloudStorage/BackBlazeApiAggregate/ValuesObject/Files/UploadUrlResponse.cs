using System.Text.Json.Serialization;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Files
{
  public class UploadUrlResponse
  {
    public string AuthorizationToken { get; set; }
    public string BucketId { get; set; }
    [JsonIgnore]
    public DateTime ExpirationDate { get; } = DateTime.UtcNow.AddHours(23);
    public string UploadUrl { get; set; }
  }
}
