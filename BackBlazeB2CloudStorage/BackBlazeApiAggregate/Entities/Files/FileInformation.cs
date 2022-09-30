using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Files;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Files
{
  public class FileInformation
  {
    public string AccountId { get; set; }
    public FileAction Action { get; set; }
    public string BucketId { get; set; }
    public int ContentLength { get; set; }
    public string ContentType { get; set; }
    public string FileId { get; set; }
    public string FileName { get; set; }
    public long UploadTimestamp { get; set; }
  }
}
