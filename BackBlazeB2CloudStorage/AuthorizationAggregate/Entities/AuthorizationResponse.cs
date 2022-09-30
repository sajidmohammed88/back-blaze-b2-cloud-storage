using BackBlazeB2CloudStorage.AuthorizationAggregate.ValuesObject;
using System.Text.Json.Serialization;

namespace BackBlazeB2CloudStorage.AuthorizationAggregate.Entities
{
  public class AuthorizationResponse
  {
    public int AbsoluteMinimumPartSize { get; set; }
    public string AccountId { get; set; }
    public AllowedCapabilities Allowed { get; set; }
    public string ApiUrl { get; set; }

    /// <summary>
    /// An authorization token to use with all calls, other than b2_authorize_account, 
    /// that need an Authorization header. This authorization token is valid for at most 24 hours.
    /// </summary>
    public string AuthorizationToken { get; set; }
    public string DownloadUrl { get; set; }

    [JsonIgnore]
    public DateTime ExpirationDate { get; } = DateTime.UtcNow.AddHours(23);
    public int RecommendedPartSize { get; set; }
    public string S3ApiUrl { get; set; }
  }
}