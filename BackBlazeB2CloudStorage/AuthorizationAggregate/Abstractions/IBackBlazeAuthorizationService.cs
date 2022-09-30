using BackBlazeB2CloudStorage.AuthorizationAggregate.Entities;
using BackBlazeB2CloudStorage.AuthorizationAggregate.ValuesObject;

namespace BackBlazeB2CloudStorage.AuthorizationAggregate.Abstractions
{
  public interface IBackBlazeAuthorization
  {
    [Post("/b2_authorize_account")]
    [Headers("Authorization:Basic")]
    public Task<AuthorizationResponse> AuthorizeUserAsync(AuthorizationRequest request);
  }
}
