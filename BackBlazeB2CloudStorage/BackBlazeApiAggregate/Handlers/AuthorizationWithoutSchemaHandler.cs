namespace BackBlazeB2CloudStorage.BackBlazeApiAggregate.Handlers
{
  public class AuthorizationWithoutSchemaHandler : DelegatingHandler
  {
    private readonly string _token;

    public AuthorizationWithoutSchemaHandler(string token) : base(new HttpClientHandler())
    {
      _token = token ?? throw new ArgumentNullException(nameof(token));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      request.Headers.TryAddWithoutValidation("Authorization", _token);

      return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
  }
}
