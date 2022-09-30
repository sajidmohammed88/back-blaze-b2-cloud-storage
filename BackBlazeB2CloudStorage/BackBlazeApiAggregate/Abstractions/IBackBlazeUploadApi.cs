using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Files;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Abstractions
{
  public interface IBackBlazeUploadApi
  {
    [Post("/b2_upload_file/{bucketId}/{uploadDestination}")]
    public Task<FileInformation> UploadFileAsync(string bucketId, string uploadDestination, [Body] ByteArrayContent byteArrayContent, [HeaderCollection] IDictionary<string, string> headers);
  }
}
