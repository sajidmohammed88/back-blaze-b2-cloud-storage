namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Abstractions
{
  public interface IBackBlazeDownloadApi
  {
    [Get($"/{Constants.B2ApiPath}/{Constants.ApiVersion}/b2_download_file_by_id?fileId={{fileId}}")]
    public Task<ApiResponse<Stream>> DownloadFileByIdAsync(string fileId, [Header("Authorization")] string authorization);

    [Get("/file/{bucketName}/{fileName}")]
    public Task<ApiResponse<Stream>> DownloadFileByNameAsync(string bucketName, string fileName, [Header("Authorization")] string authorization);
  }
}
