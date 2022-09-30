using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Bucket;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Files;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Bucket;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Files;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Keys;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Abstractions
{
  public interface IBackBlazeApiService
  {
    Task<KeyResponse> CreateAndLinkKeyToBucketAsync(string bucketId, string[] capabilities, string keyName);

    Task<Bucket> CreateBucketAsync(string bucketName, BucketType bucketType);

    Task<FileInformation> CreateEmptyFolderAsync(string bucketId, string folderName);

    Task<KeyResponse> CreateKeyAsync(string[] capabilities, string keyName);

    Task<Bucket> DeleteBucketAsync(string bucketId);

    Task<bool> DeleteFilesPermanentlyAsync(string bucketId, string filePrefix);

    Task<bool> DeleteFileVersionAsync(string fileId, string fileName);

    Task<KeyResponse> DeleteKeyAsync(string applicationKeyId);

    /// <summary>
    /// Download file asynchrOnously
    /// </summary>
    /// <param name="fileId">The file identifier, required for download a file by file id and optional for download file by name.</param>
    /// <param name="fileDestination">The file destination.</param>
    /// <param name="bucketName">The bucket name, required for download a file by name and optional for download a file by file id.</param>
    /// <param name="fileName">The file name, required for download a file by name and optional for download a file by file id.</param>
    /// <returns>True, if the file downloaded succefully.</returns>
    Task<bool> DownloadFileAsync(string fileId, string fileDestination, string bucketName, string fileName);

    Task<BucketsResponse> GetBucketsAsync(string bucketName = null, string bucketId = null);

    Task<GetKeysResponse> GetKeysAsync();

    Task<BucketContent> GetLatestFileAsync(BucketContentRequest request);

    Task<UploadUrlResponse> GetUploadUrlAsync(string bucketId);

    Task<FileInformation> HideFileAsync(string bucketId, string fileName);

    Task<bool> HideFolderFilesAsync(string bucketId, string filePrefix);

    Task<bool> IsBucketExistByBucketIdAsync(string bucketId);

    Task<bool> IsBucketExistByBucketNameAsync(string bucketName);

    Task<FileInformation> UploadFileAsync(string bucketId, string filePath, string fileName, string contentType);
  }
}
