namespace BackBlazeB2CloudStorage.AuthorizationAggregate.ValuesObject
{
  public class AllowedCapabilities
  {
    public string BucketId { get; set; }
    public string BucketName { get; set; }
    public List<string> Capabilities { get; set; }
    public string NamePrefix { get; set; }
  }
}
