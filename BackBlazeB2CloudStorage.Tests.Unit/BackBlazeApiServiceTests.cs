using BackBlazeB2CloudStorage.AuthorizationAggregate;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Files;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Bucket;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Files;
using Microsoft.Extensions.Logging;
using Moq;

namespace BackBlazeB2CloudStorage.Tests.Unit
{
  public class BackBlazeApiServiceTests
  {
    private readonly BackBlazeApiService _backBlazeApiService;

    public BackBlazeApiServiceTests()
    {
      _backBlazeApiService = new BackBlazeApiService(new BackBlazeAuthorizationService("0045d88ceb180d80000000003", "K004QQfvmYM6a0863V61GNCbsPjsFHI"), new Mock<ILogger<BackBlazeApiService>>().Object);
    }

    [Fact]
    public async Task Should_create_and_link_key_to_bucket_When_request_exist()
    {
      // Arrange
      const string keyName = "files-manager";
      string bucketId = "05edb8680cbe4bb188300d18";
      string[] capabilities = new[] { "listFiles", "readFiles", "shareFiles", "writeFiles", "deleteFiles" };

      // Act
      var result = await _backBlazeApiService.CreateAndLinkKeyToBucketAsync(bucketId, capabilities, keyName);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(keyName, result.KeyName);
      Assert.NotNull(result.AccountId);
      Assert.NotNull(result.ApplicationKey);
      Assert.NotNull(result.ApplicationKeyId);
      Assert.NotNull(result.BucketId);
      Assert.Equal(bucketId, result.BucketId);
      Assert.NotEmpty(result.Capabilities);
    }

    [Fact]
    public async Task Should_create_bucket_When_required_informations_request_are_valid()
    {
      // Arrange
      const string bucketName = "demo-09ab-cf-test-001";

      // Act
      var bucketsResponse = await _backBlazeApiService.GetBucketsAsync(bucketName, null);
      var bucket = bucketsResponse.Buckets.FirstOrDefault();
      bucket ??= await _backBlazeApiService.CreateBucketAsync(bucketName, BucketType.AllPrivate);

      // Assert
      Assert.NotNull(bucket);
      Assert.Equal(bucketName, bucket.BucketName);
      Assert.Equal(BucketType.AllPrivate, bucket.BucketType);
    }

    [Fact]
    public async Task Should_create_folder_with_empty_file_When_upload_is_authorized()
    {
      // Arrange
      const string bucketId = "05edb8680cbe4bb188300d18";
      string folderName = $"testEmptyFile{Guid.NewGuid()}";

      // Act
      var fileInformation = await _backBlazeApiService.CreateEmptyFolderAsync(bucketId, folderName);

      // Assert
      Assert.NotNull(fileInformation);
      Assert.Equal(bucketId, fileInformation.BucketId);
      Assert.Equal($"{folderName}/.bzEmpty", fileInformation.FileName);
      Assert.Equal(FileAction.Upload, fileInformation.Action);
      Assert.Equal("text/plain", fileInformation.ContentType);
      Assert.Equal(0, fileInformation.ContentLength);
      Assert.NotNull(fileInformation.FileId);
    }

    [Fact]
    public async Task Should_create_key_When_request_exist()
    {
      // Arrange
      const string keyName = "bucket-manager";
      string[] capabilities = new[] { "listAllBucketNames", "listBuckets", "readBuckets", "writeBuckets", "deleteBuckets" };

      // Act
      var result = await _backBlazeApiService.CreateKeyAsync(capabilities, keyName);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(keyName, result.KeyName);
      Assert.NotNull(result.AccountId);
      Assert.NotNull(result.ApplicationKey);
      Assert.NotNull(result.ApplicationKeyId);
      Assert.Null(result.BucketId);
      Assert.NotEmpty(result.Capabilities);
    }

    [Fact]
    public async Task Should_delete_bucket_When_bucket_exist()
    {
      // Arrange
      const string bucketName = "demo-09ab-cf-test-001";
      string bucketId;

      var bucketsResponse = await _backBlazeApiService.GetBucketsAsync(bucketName);
      if (bucketsResponse?.Buckets == null || !bucketsResponse.Buckets.Any())
      {
        var bucket = await _backBlazeApiService.CreateBucketAsync(bucketName, BucketType.AllPrivate);
        bucketId = bucket.BucketId;
      }
      else
      {
        bucketId = bucketsResponse.Buckets.First().BucketId;
      }

      // Act
      var result = await _backBlazeApiService.DeleteBucketAsync(bucketId);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(bucketName, result.BucketName);
      Assert.Equal(bucketId, result.BucketId);
      Assert.Equal(BucketType.AllPrivate, result.BucketType);
    }

    [Fact]
    public async Task Should_delete_file_version_When_file_exist()
    {
      // Arrange
      const string bucketId = "05edb8680cbe4bb188300d18";
      const string prefix = "testFolder/";

      (string fileName, string fileId) = await GetFileIdAndFileNameRandomlyAsync(bucketId, prefix);

      // Act
      var result = await _backBlazeApiService.DeleteFileVersionAsync(fileId, fileName);

      // Assert
      Assert.True(result);
    }

    [Fact]
    public async Task Should_delete_files_versions_permanently_When_files_exist()
    {
      // Arrange
      const string bucketId = "05edb8680cbe4bb188300d18";
      const string prefix = "testFolder/";

      // Act
      var result = await _backBlazeApiService.DeleteFilesPermanentlyAsync(bucketId, prefix);

      // Assert
      Assert.True(result);
    }

    [Fact]
    public async Task Should_delete_key_When_request_exist()
    {
      // Arrange
      const string applicationKeyId = "0045d88ceb180d80000000005";

      // Act
      var result = await _backBlazeApiService.DeleteKeyAsync(applicationKeyId);

      // Assert
      Assert.NotNull(result);
      Assert.NotNull(result.KeyName);
      Assert.NotNull(result.AccountId);
      Assert.Equal(applicationKeyId, result.ApplicationKeyId);
      Assert.Null(result.ApplicationKey);
      Assert.Null(result.BucketId);
      Assert.NotEmpty(result.Capabilities);
    }

    [Fact]
    public async Task Should_download_file_by_id_When_download_is_authorized()
    {
      // Arrange
      const string prefix = "testFolder/";
      const string bucketId = "05edb8680cbe4bb188300d18";

      string fileDestination = Path.GetTempPath();
      string fileId = (await GetFileIdAndFileNameRandomlyAsync(bucketId, prefix)).FileId;

      // Act
      var isFileDownloaded = await _backBlazeApiService.DownloadFileAsync(fileId, fileDestination, null, null);

      // Assert
      Assert.True(isFileDownloaded);
    }

    [Fact]
    public async Task Should_download_file_by_name_When_download_is_authorized()
    {
      // Arrange
      const string bucketName = "demo-09ab-cf";
      const string prefix = "testFolder/";
      const string bucketId = "05edb8680cbe4bb188300d18";

      string fileDestination = Path.GetTempPath();
      string fileName = (await GetFileIdAndFileNameRandomlyAsync(bucketId, prefix)).FileName;

      // Act
      var isFileDownloaded = await _backBlazeApiService.DownloadFileAsync(null, fileDestination, bucketName, fileName);

      // Assert
      Assert.True(isFileDownloaded);
    }

    [Fact]
    public async Task Should_hide_file_When_file_exist()
    {
      // Arrange
      const string bucketId = "05edb8680cbe4bb188300d18";
      const string prefix = "testFolder/";

      string fileName = (await GetFileIdAndFileNameRandomlyAsync(bucketId, prefix)).FileName;

      // Act
      var result = await _backBlazeApiService.HideFileAsync(bucketId, fileName);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(fileName, result.FileName);
      Assert.Equal(bucketId, result.BucketId);
    }

    [Fact]
    public async Task Should_hide_files_under_folder_When_files_exist()
    {
      // Arrange
      const string bucketId = "05edb8680cbe4bb188300d18";
      const string prefix = "testFolder/";

      // Act
      var result = await _backBlazeApiService.HideFolderFilesAsync(bucketId, prefix);

      // Assert
      Assert.True(result);
    }

    [Fact]
    public async Task Should_retreive_keys_When_keys_exist()
    {
      // Act
      var result = await _backBlazeApiService.GetKeysAsync();

      // Assert
      Assert.NotNull(result);
      Assert.NotEmpty(result.Keys);
    }

    [Fact]
    public async Task Should_return_buckets_When_api_call_succeeded()
    {
      // Act
      var result = await _backBlazeApiService.GetBucketsAsync();

      // Assert
      Assert.NotNull(result);
      Assert.NotEmpty(result.Buckets);
    }

    [Fact]
    public async Task Should_return_expected_files_When_prefix_is_not_empty()
    {
      // Arrange
      const string bucketId = "05edb8680cbe4bb188300d18";
      const string prefix = "testFolder/";
      await GetFileIdAndFileNameRandomlyAsync(bucketId, prefix);
      var request = new BucketContentRequest
      {
        BucketId = bucketId,
        Prefix = prefix
      };

      // Act
      var result = await _backBlazeApiService.GetLatestFileAsync(request);

      // Assert
      Assert.NotEmpty(result.Files);
      Assert.NotEmpty(result.Files);
      Assert.All(result.Files, file => Assert.StartsWith(prefix, file.FileName));
    }

    [Fact]
    public async Task Should_return_false_When_the_bucket_not_exist()
    {
      // Arrange
      const string bucketName = "demo-09ab-cf1";

      // Act
      var result = await _backBlazeApiService.IsBucketExistByBucketNameAsync(bucketName);

      // Assert
      Assert.False(result);
    }

    [Fact]
    public async Task Should_return_files_informations_When_files_exist()
    {
      // Arrange
      var request = new BucketContentRequest
      {
        BucketId = "05edb8680cbe4bb188300d18"
      };

      // Act
      var result = await _backBlazeApiService.GetLatestFileAsync(request);

      // Assert
      Assert.NotEmpty(result.Files);
      Assert.NotEmpty(result.Files);
    }

    [Fact]
    public async Task Should_return_one_file_When_max_file_count_is_1()
    {
      // Arrange
      const string bucketId = "05edb8680cbe4bb188300d18";
      const string prefix = "testFolder/";
      await GetFileIdAndFileNameRandomlyAsync(bucketId, prefix);
      var request = new BucketContentRequest
      {
        BucketId = bucketId,
        MaxFileCount = 1
      };

      // Act
      var result = await _backBlazeApiService.GetLatestFileAsync(request);

      // Assert
      Assert.NotNull(result);
      Assert.Single(result.Files);
    }

    [Fact]
    public async Task Should_return_true_When_the_bucket_id_exist()
    {
      // Arrange
      string bucketId = "05edb8680cbe4bb188300d18";

      // Act
      var result = await _backBlazeApiService.IsBucketExistByBucketIdAsync(bucketId);

      // Assert
      Assert.True(result);
    }

    [Fact]
    public async Task Should_return_true_When_the_bucket_name_exist()
    {
      // Arrange
      string bucketName = "demo-09ab-cf";

      // Act
      var result = await _backBlazeApiService.IsBucketExistByBucketNameAsync(bucketName);

      // Assert
      Assert.True(result);
    }

    [Fact]
    public async Task Should_return_upload_url_and_token_When_bucket_id_is_valid()
    {
      // Arrange
      const string bucketId = "05edb8680cbe4bb188300d18";

      // Act
      var result = await _backBlazeApiService.GetUploadUrlAsync(bucketId);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(bucketId, result.BucketId);
      Assert.NotEmpty(result.UploadUrl);
      Assert.NotEmpty(result.AuthorizationToken);
    }

    [Fact]
    public async Task Should_upload_file_When_upload_is_authorized()
    {
      // Arrange
      const string bucketId = "05edb8680cbe4bb188300d18";
      const string contentType = "text/plain";
      (string fileName, string filePath) = GenerateContentOfFile("testFolder/");

      // Act
      var fileInformation = await _backBlazeApiService.UploadFileAsync(bucketId, filePath, fileName, contentType);

      // Assert
      Assert.NotNull(fileInformation);
      Assert.Equal(bucketId, fileInformation.BucketId);
      Assert.Equal(fileName, fileInformation.FileName);
      Assert.Equal(FileAction.Upload, fileInformation.Action);
      Assert.Equal("text/plain", fileInformation.ContentType);
      Assert.True(fileInformation.ContentLength > 0);
      Assert.NotNull(fileInformation.FileId);
    }

    [Fact]
    public async Task Should_upload_large_file_When_upload_is_authorized()
    {
      // Arrange
      const string bucketId = "05edb8680cbe4bb188300d18";
      const string contentType = "text/plain";
      string filePath = Path.ChangeExtension(Path.GetTempFileName(), ".txt");
      string fileName = $"testFolder/{Path.GetFileName(filePath)}";
      byte[] data = new byte[6 * 1024 * 1024];
      Random rng = new();
      rng.NextBytes(data);
      File.WriteAllBytes(filePath, data);

      // Act
      var fileInformation = await _backBlazeApiService.UploadFileAsync(bucketId, filePath, fileName, contentType);

      // Assert
      Assert.NotNull(fileInformation);
      Assert.Equal(bucketId, fileInformation.BucketId);
      Assert.Equal(fileName, fileInformation.FileName);
      Assert.Equal(FileAction.Upload, fileInformation.Action);
      Assert.Equal("text/plain", fileInformation.ContentType);
      Assert.True(fileInformation.ContentLength > 0);
      Assert.NotNull(fileInformation.FileId);
    }

    private static (string FileName, string FilePath) GenerateContentOfFile(string prefix)
    {
      string filePath = Path.ChangeExtension(Path.GetTempFileName(), ".txt");
      using FileStream savedFile = new(filePath, FileMode.Create);
      savedFile.Write(Guid.NewGuid().ToByteArray());
      savedFile.Close();

      return ($"{prefix}{Path.GetFileName(filePath)}", filePath);
    }

    private async Task<(string FileName, string FileId)> GetFileIdAndFileNameRandomlyAsync(string bucketId, string prefix)
    {
      const string contentType = "text/plain";

      var result = await _backBlazeApiService.GetLatestFileAsync(new BucketContentRequest
      {
        BucketId = bucketId,
        Prefix = prefix
      });
      if (result?.Files == null || !result.Files.Any())
      {
        //upload new file
        (string fileName, string filePath) = GenerateContentOfFile(prefix);
        var fileInformation = await _backBlazeApiService.UploadFileAsync(bucketId, filePath, fileName, contentType);

        return (fileInformation.FileName, fileInformation.FileId);
      }

      return (result.Files.First().FileName, result.Files.First().FileId);
    }
  }
}
