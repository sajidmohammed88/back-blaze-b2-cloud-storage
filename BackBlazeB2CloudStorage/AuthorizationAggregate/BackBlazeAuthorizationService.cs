using BackBlazeB2CloudStorage.AuthorizationAggregate.Abstractions;
using BackBlazeB2CloudStorage.AuthorizationAggregate.Entities;
using BackBlazeB2CloudStorage.AuthorizationAggregate.ValuesObject;

namespace BackBlazeB2CloudStorage.AuthorizationAggregate
{
  public class BackBlazeAuthorizationService : IBackBlazeAuthorizationService
  {
    private readonly IBackBlazeAuthorization _backBlazeAuthorization;
    private AuthorizationResponse _authorizationResponse;

    public BackBlazeAuthorizationService(string applicationKeyId, string applicationKey)
    {
      _backBlazeAuthorization = RestService.For<IBackBlazeAuthorization>(Constants.AuthorizationBaseUrl, new RefitSettings
      {
        AuthorizationHeaderValueGetter = GenerateAuthorizationToken(applicationKeyId, applicationKey)
      });
    }

    public async Task<AuthorizationResponse> AuthorizeUserAsync()
    {
      if (!IsTokenValid())
      {
        _authorizationResponse = await _backBlazeAuthorization.AuthorizeUserAsync(new AuthorizationRequest());
      }

      return _authorizationResponse;
    }

    public bool IsTokenValid()
    {
      return _authorizationResponse != null && DateTime.Compare(_authorizationResponse.ExpirationDate, DateTime.UtcNow) > 0;
    }

    private Func<Task<string>> GenerateAuthorizationToken(string applicationKeyId, string applicationKey) =>
       () => Task.FromResult(Convert.ToBase64String(Encoding.ASCII.GetBytes($"{applicationKeyId}:{applicationKey}")));
  }
}
