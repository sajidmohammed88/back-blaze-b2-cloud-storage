using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Entities.Files;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Abstractions
{
  public interface IBackBlazeLargeUploadApi
  {
    [Post("/b2_upload_part/{bucketIdAndCvt}/{secondPart}")]
    public Task<FilePartInformation> UploadLargeFileAsync(string bucketIdAndCvt, string secondPart, [Body] ByteArrayContent byteArrayContent, [HeaderCollection] IDictionary<string, string> headers);
  }
}
