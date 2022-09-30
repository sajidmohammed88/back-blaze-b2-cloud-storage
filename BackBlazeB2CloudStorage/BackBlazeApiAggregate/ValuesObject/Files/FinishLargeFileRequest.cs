namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Files
{
  public class FinishLargeFileRequest
  {
    public string FileId { get; set; }
    public string[] PartSha1Array { get; set; }
  }
}
