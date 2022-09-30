namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Files
{
  public class FilePartInformation
  {
    public string FileId { get; set; }
    public int PartNumber { get; set; }
    public long ContentLength { get; set; }
    public string ContentSha1 { get; set; }
    public long UploadTimestamp { get; set; }
  }
}
