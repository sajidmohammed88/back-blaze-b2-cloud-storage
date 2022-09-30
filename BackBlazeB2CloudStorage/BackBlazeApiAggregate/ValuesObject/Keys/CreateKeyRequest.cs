namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Keys
{
  public class CreateKeyRequest
  {
    public string AccountId { get; set; }
    public string BucketId { get; set; }
    public string[] Capabilities { get; set; }
    public string KeyName { get; set; }
  }
}
