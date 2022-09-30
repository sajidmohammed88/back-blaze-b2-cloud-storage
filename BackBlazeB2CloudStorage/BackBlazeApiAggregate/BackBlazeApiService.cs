using BackBlazeB2CloudStorage.AuthorizationAggregate;
using BackBlazeB2CloudStorage.AuthorizationAggregate.Abstractions;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Abstractions;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Bucket;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Files;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Bucket;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Files;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.ValuesObject.Keys;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate
{
  public class BackBlazeApiService : BackBlazeApiBaseService, IBackBlazeApiService
  {
    readonly ILogger<BackBlazeApiService> _logger;
    public BackBlazeApiService(IBackBlazeAuthorizationService backBlazeAuthorizationService, ILogger<BackBlazeApiService> logger)
      : base(backBlazeAuthorizationService)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
    }

    public async Task<KeyResponse> CreateAndLinkKeyToBucketAsync(string bucketId, string[] capabilities, string keyName)
    {
      try
      {
        await Initialize();
        return await BackBlazeApi.CreateKeyAsync(new CreateKeyRequest
        {
          AccountId = AccountId,
          Capabilities = capabilities,
          KeyName = keyName,
          BucketId = bucketId
        });
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<Bucket> CreateBucketAsync(string bucketName, BucketType bucketType)
    {
      try
      {
        await Initialize();
        return await BackBlazeApi.CreateBucketAsync(new BucketRequest
        {
          AccountId = AccountId,
          BucketName = bucketName,
          BucketType = bucketType
        });
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<FileInformation> CreateEmptyFolderAsync(string bucketId, string folderName)
    {
      try
      {
        await Initialize();

        (IBackBlazeUploadApi backBlazeUploadApi, string authorizationToken, string uploadDestination) = await GetBackBlazeUploadApiInformationsAsync(bucketId);

        byte[] bytes = Array.Empty<byte>();

        return await backBlazeUploadApi.UploadFileAsync(bucketId, uploadDestination, new ByteArrayContent(bytes), new Dictionary<string, string>
        {
          {"Authorization", authorizationToken },
          {"Content-Type", "text/plain"},
          {"X-Bz-File-Name", $"{folderName}/.bzEmpty"},
          {"X-Bz-Content-Sha1", GenerateContentSha1(bytes) }
        });
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<KeyResponse> CreateKeyAsync(string[] capabilities, string keyName)
    {
      try
      {
        await Initialize();
        return await BackBlazeApi.CreateKeyAsync(new CreateKeyRequest
        {
          AccountId = AccountId,
          Capabilities = capabilities,
          KeyName = keyName
        });
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<Bucket> DeleteBucketAsync(string bucketId)
    {
      try
      {
        await Initialize();
        return await BackBlazeApi.DeleteBucketAsync(new BucketBase
        {
          AccountId = AccountId,
          BucketId = bucketId
        });
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<bool> DeleteFilesPermanentlyAsync(string bucketId, string filePrefix)
    {
      try
      {
        await Initialize();
        var filesVersionsResponse = await BackBlazeApi.GetFilesVersionsAsync(new BucketContentRequest
        {
          BucketId = bucketId,
          Prefix = filePrefix
        });

        foreach (var file in filesVersionsResponse.Files)
        {
          await BackBlazeApi.DeleteFileVersionAsync(new DeleteFileVersionBase
          {
            FileId = file.FileId,
            FileName = file.FileName
          });
        }

        return true;
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<bool> DeleteFileVersionAsync(string fileId, string fileName)
    {
      try
      {
        await Initialize();
        await BackBlazeApi.DeleteFileVersionAsync(new DeleteFileVersionBase
        {
          FileId = fileId,
          FileName = fileName
        });

        return true;
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<KeyResponse> DeleteKeyAsync(string applicationKeyId)
    {
      try
      {
        await Initialize();
        return await BackBlazeApi.DeleteKeyAsync(new DeleteKeyRequest
        {
          ApplicationKeyId = applicationKeyId
        });
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    /// <inheritdoc />
    public async Task<bool> DownloadFileAsync(string fileId, string fileDestination, string bucketName, string fileName)
    {
      try
      {
        ApiResponse<Stream> apiResponse;
        await Initialize();

        if (!string.IsNullOrEmpty(fileId))
        {
          apiResponse = await BackBlazeDownloadApi.DownloadFileByIdAsync(fileId, DownloadAuthorizationToken);
        }
        else if (!string.IsNullOrEmpty(bucketName) && !string.IsNullOrEmpty(fileName))
        {
          apiResponse = await BackBlazeDownloadApi.DownloadFileByNameAsync(bucketName, fileName, DownloadAuthorizationToken);
        }
        else
        {
          throw new ArgumentException($"The input {nameof(File)}, {nameof(bucketName)} or {nameof(fileId)} are not valid.");
        }

        await DownloadStreamAsync(fileDestination, apiResponse);

        return true;
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<BucketsResponse> GetBucketsAsync(string bucketName = null, string bucketId = null)
    {
      try
      {
        await Initialize();
        return await BackBlazeApi.GetBucketsAsync(new BucketsRequest
        {
          AccountId = AccountId,
          BucketId = bucketId,
          BucketName = bucketName
        });
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<GetKeysResponse> GetKeysAsync()
    {
      try
      {
        await Initialize();
        return await BackBlazeApi.GetKeysAsync(new GetKeysRequest
        {
          AccountId = AccountId
        });
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<BucketContent> GetLatestFileAsync(BucketContentRequest request)
    {
      try
      {
        await Initialize();
        return await BackBlazeApi.GetLatestFileAsync(request);
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<UploadUrlResponse> GetUploadUrlAsync(string bucketId)
    {
      try
      {
        await Initialize();
        return await BackBlazeApi.GetUploadUrlAsync(new UploadUrlRequest { BucketId = bucketId });
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<FileInformation> HideFileAsync(string bucketId, string fileName)
    {
      try
      {
        await Initialize();
        return await BackBlazeApi.HideFileAsync(new HideFileRequest { BucketId = bucketId, FileName = fileName });
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<bool> HideFolderFilesAsync(string bucketId, string filePrefix)
    {
      try
      {
        await Initialize();

        var result = await BackBlazeApi.GetLatestFileAsync(new BucketContentRequest
        {
          BucketId = bucketId,
          Prefix = filePrefix
        });

        foreach (var file in result.Files)
        {
          await BackBlazeApi.HideFileAsync(new HideFileRequest
          {
            BucketId = file.BucketId,
            FileName = file.FileName
          });
        }
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }

      return true;
    }

    public async Task<bool> IsBucketExistByBucketIdAsync(string bucketId)
    {
      try
      {
        await Initialize();
        BucketsResponse bucketsResponse = await BackBlazeApi.GetBucketsAsync(new BucketsRequest
        {
          AccountId = AccountId,
          BucketId = bucketId
        });

        return bucketsResponse.Buckets.Any();
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<bool> IsBucketExistByBucketNameAsync(string bucketName)
    {
      try
      {
        await Initialize();
        BucketsResponse bucketsResponse = await BackBlazeApi.GetBucketsAsync(new BucketsRequest
        {
          AccountId = AccountId,
          BucketName = bucketName
        });

        return bucketsResponse.Buckets.Any();
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    public async Task<FileInformation> UploadFileAsync(string bucketId, string filePath, string fileName, string contentType)
    {
      try
      {
        await Initialize();

        (List<byte[]> parts, List<string> partsSha1, long fileSize) = await GeneratePartsAndPartsSha1Async(filePath);

        if (fileSize < AbsoluteMinimumPartSize)
        {
          (IBackBlazeUploadApi backBlazeUploadApi, string authorizationToken, string uploadDestination) = await GetBackBlazeUploadApiInformationsAsync(bucketId);

          return await backBlazeUploadApi.UploadFileAsync(bucketId, uploadDestination, new ByteArrayContent(parts.First()), new Dictionary<string, string>
          {
            {"Authorization", authorizationToken },
            {"Content-Type", contentType},
            {"X-Bz-File-Name", fileName},
            {"X-Bz-Content-Sha1", partsSha1.First() }
          });
        }

        return await UploadLargeFileAsync(bucketId, filePath, fileName, contentType);
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    private async Task<FileInformation> UploadLargeFileAsync(string bucketId, string filePath, string fileName, string contentType)
    {
      try
      {
        (List<byte[]> parts, List<string> partsSha1, long fileSize) = await GeneratePartsAndPartsSha1Async(filePath);

        FileInformation startFileInformation = await BackBlazeApi.StartLargeFileAsync(new StartLargeFileRequest
        {
          BucketId = bucketId,
          FileName = fileName,
          ContentType = contentType
        });

        (IBackBlazeLargeUploadApi BackBlazeLargeUploadApi, string AuthorizationToken, string bucketIdAndCvt, string secondPart) =
          await GetBackBlazeUploadPartApiInformationsAsync(startFileInformation.FileId);

        for (int i = 0; i < parts.Count; i++)
        {
          await BackBlazeLargeUploadApi.UploadLargeFileAsync(bucketIdAndCvt, secondPart, new ByteArrayContent(parts[i]), new Dictionary<string, string>
          {
            {"Authorization", AuthorizationToken },
            {"X-Bz-Part-Number", (i+1).ToString() },
            {"X-Bz-Content-Sha1", partsSha1[i] },
            {"Content-Length", parts[i].Length.ToString() }
          });
        }

        FileInformation finishFileInformation = await BackBlazeApi.FinishLargeFileAsync(new FinishLargeFileRequest
        {
          FileId = startFileInformation.FileId,
          PartSha1Array = partsSha1.ToArray()
        });

        return finishFileInformation;
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex.Message);

        throw;
      }
    }

    private async Task DownloadStreamAsync(string fileDestination, ApiResponse<Stream> apiResponse)
    {
      using MemoryStream ms = new();
      await apiResponse.Content.CopyToAsync(ms);
      byte[] bytes = ms.ToArray();

      using FileStream savedFile = new($"{fileDestination}\\{apiResponse.Headers.First(_ => _.Key == "x-bz-file-name").Value.First().Split('/').Last()}", FileMode.Create);
      await savedFile.WriteAsync(bytes);
      savedFile.Close();
    }

    private string GenerateContentSha1(byte[] bytes)
    {
      StringBuilder sb = new();

      using SHA1 sha1 = SHA1.Create();
      byte[] hashData = sha1.ComputeHash(bytes, 0, bytes.Length);
      foreach (byte b in hashData)
      {
        sb.Append(b.ToString("x2"));
      }

      return sb.ToString();
    }

    private async Task<(List<byte[]> Parts, List<string> PartsSha1, long FileSize)> GeneratePartsAndPartsSha1Async(string filePath)
    {
      FileStream fileStream = File.OpenRead(filePath);
      long fileSize = fileStream.Length;
      long totalBytesParted = 0;
      List<string> partsSha1 = new();
      List<byte[]> parts = new();

      while (totalBytesParted < fileSize)
      {
        long partBytesToSend = AbsoluteMinimumPartSize;

        if (fileSize - totalBytesParted < AbsoluteMinimumPartSize)
        {
          partBytesToSend = fileSize - totalBytesParted;
        }

        byte[] data = new byte[partBytesToSend];
        fileStream.Seek(totalBytesParted, SeekOrigin.Begin);
        await fileStream.ReadAsync(data);

        parts.Add(data);
        partsSha1.Add(GenerateContentSha1(data));

        totalBytesParted += partBytesToSend;
      }

      fileStream.Close();

      return (parts, partsSha1, fileSize);
    }

    private async Task<(IBackBlazeUploadApi BackBlazeUploadApi, string AuthorizationToken, string uploadDestination)> GetBackBlazeUploadApiInformationsAsync(string bucketId)
    {
      // upload url authorization
      UploadUrlResponse uploadUrlResponse = await BackBlazeApi.GetUploadUrlAsync(new UploadUrlRequest
      {
        BucketId = bucketId
      });

      Uri uri = new(uploadUrlResponse.UploadUrl);
      return (RestService.For<IBackBlazeUploadApi>($"{uri.Scheme}://{uri.Host}/{Constants.B2ApiPath}/{Constants.ApiVersion}"), uploadUrlResponse.AuthorizationToken, uri.Segments.Last());
    }

    private async Task<(IBackBlazeLargeUploadApi BackBlazeLargeUploadApi, string AuthorizationToken, string bucketIdAndCvt, string secondPart)> GetBackBlazeUploadPartApiInformationsAsync(string fileId)
    {
      // upload url authorization
      UploadPartUrlResponse uploadPartUrlResponse = await BackBlazeApi.GetUploadPartUrlAsync(new UploadPartUrlRequest
      {
        FileId = fileId
      });

      Uri uri = new(uploadPartUrlResponse.UploadUrl);
      return (RestService.For<IBackBlazeLargeUploadApi>($"{uri.Scheme}://{uri.Host}/{Constants.B2ApiPath}/{Constants.ApiVersion}"),
        uploadPartUrlResponse.AuthorizationToken,
        uri.Segments[^2].TrimEnd(new char[] { '/' }),
        uri.Segments.Last());
    }
  }
}
