namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Files
{
  public class BucketContentRequest
  {
    public string BucketId { get; set; }
    public int MaxFileCount { get; set; }
    public string Prefix { get; set; }
    public string StartFileName { get; set; }
  }
}
