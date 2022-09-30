using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Files;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Files
{
  public class BucketContent
  {
    public List<FileInformation> Files { get; set; }
    public string NextFileName { get; set; }
  }
}
