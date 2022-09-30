using BackBlazeB2CloudStorage.AuthorizationAggregate.Entities;

namespace BackBlazeB2CloudStorage.AuthorizationAggregate.Abstractions
{
  public interface IBackBlazeAuthorizationService
  {
    Task<AuthorizationResponse> AuthorizeUserAsync();
    bool IsTokenValid();
  }
}
