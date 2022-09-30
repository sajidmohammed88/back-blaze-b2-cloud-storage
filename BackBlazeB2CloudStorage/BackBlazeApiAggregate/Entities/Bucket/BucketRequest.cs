using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Bucket;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Bucket
{
  public class BucketRequest : BucketRequestBase
  {
    public BucketType BucketType { get; set; }
  }
}
