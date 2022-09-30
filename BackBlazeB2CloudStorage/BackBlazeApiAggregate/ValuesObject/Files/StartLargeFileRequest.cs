namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Files
{
  public class StartLargeFileRequest : HideFileRequest
  {
    public string ContentType { get; set; } = "b2/x-auto";
  }
}
