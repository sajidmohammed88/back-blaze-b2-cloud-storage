using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Bucket;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Bucket.FileLockConfiguration;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Bucket
{
  public class Bucket : BucketBase
  {
    public string BucketName { get; set; }
    public BucketType BucketType { get; set; }
    public FileLockConfiguration FileLockConfiguration { get; set; }
    public int Revision { get; set; }
  }
}