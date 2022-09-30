using BackBlazeB2CloudStorage.AuthorizationAggregate.Abstractions;
using BackBlazeB2CloudStorage.AuthorizationAggregate.Entities;
using BackBlazeB2CloudStorage.BackBlazeApiAggregate.Handlers;

namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Abstractions
{
  public abstract class BackBlazeApiBaseService
  {
    private readonly IBackBlazeAuthorizationService _backBlazeAuthorizationService;
    private bool _initialized;
    protected BackBlazeApiBaseService(IBackBlazeAuthorizationService backBlazeAuthorizationService)
    {
      _backBlazeAuthorizationService = backBlazeAuthorizationService ?? throw new ArgumentNullException(nameof(backBlazeAuthorizationService));
    }

    protected int AbsoluteMinimumPartSize { get; private set; }
    protected string AccountId { get; private set; }
    protected IBackBlazeApi BackBlazeApi { get; private set; }
    protected IBackBlazeDownloadApi BackBlazeDownloadApi { get; private set; }
    protected string DownloadAuthorizationToken { get; private set; }
    protected async Task Initialize()
    {
      if (!_initialized || !_backBlazeAuthorizationService.IsTokenValid())
      {
        AuthorizationResponse authorizationResponse = await _backBlazeAuthorizationService.AuthorizeUserAsync();
        AccountId = authorizationResponse.AccountId;
        AbsoluteMinimumPartSize = authorizationResponse.AbsoluteMinimumPartSize;
        DownloadAuthorizationToken = authorizationResponse.AuthorizationToken;

        BackBlazeApi = RestService.For<IBackBlazeApi>(new HttpClient(new AuthorizationWithoutSchemaHandler(authorizationResponse.AuthorizationToken))
        {
          BaseAddress = new Uri($"{authorizationResponse.ApiUrl}/{Constants.B2ApiPath}/{Constants.ApiVersion}")
        });

        BackBlazeDownloadApi = RestService.For<IBackBlazeDownloadApi>(authorizationResponse.DownloadUrl);

        _initialized = true;
      }
    }
  }
}
