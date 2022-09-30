using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Bucket;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Files;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Bucket;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Files;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Keys;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Abstractions
{
  public interface IBackBlazeApi
  {
    [Post("/b2_create_bucket")]
    public Task<Bucket> CreateBucketAsync(BucketRequest request);

    [Post("/b2_create_key")]
    public Task<KeyResponse> CreateKeyAsync(CreateKeyRequest request);

    [Post("/b2_delete_bucket")]
    public Task<Bucket> DeleteBucketAsync(BucketBase request);

    [Post("/b2_delete_file_version")]
    public Task<DeleteFileVersionBase> DeleteFileVersionAsync(DeleteFileVersionBase request);

    [Post("/b2_delete_key")]
    public Task<KeyResponse> DeleteKeyAsync(DeleteKeyRequest request);

    [Post("/b2_finish_large_file")]
    public Task<FileInformation> FinishLargeFileAsync(FinishLargeFileRequest request);

    [Post("/b2_list_buckets")]
    public Task<BucketsResponse> GetBucketsAsync(BucketsRequest request);
    [Post("/b2_list_file_versions")]
    public Task<BucketContent> GetFilesVersionsAsync(BucketContentRequest request);

    [Post("/b2_list_keys")]
    public Task<GetKeysResponse> GetKeysAsync(GetKeysRequest request);

    [Post("/b2_list_file_names")]
    public Task<BucketContent> GetLatestFileAsync(BucketContentRequest request);

    [Post("/b2_get_upload_part_url")]
    public Task<UploadPartUrlResponse> GetUploadPartUrlAsync(UploadPartUrlRequest request);

    [Post("/b2_get_upload_url")]
    public Task<UploadUrlResponse> GetUploadUrlAsync(UploadUrlRequest request);

    [Post("/b2_hide_file")]
    public Task<FileInformation> HideFileAsync(HideFileRequest request);

    [Post("/b2_start_large_file")]
    public Task<FileInformation> StartLargeFileAsync(StartLargeFileRequest request);
  }
}
