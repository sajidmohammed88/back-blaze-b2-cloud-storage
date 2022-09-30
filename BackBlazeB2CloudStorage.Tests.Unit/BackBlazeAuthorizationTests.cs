using BackBlazeB2CloudStorage.AuthorizationAggregate.Abstractions;
using BackBlazeB2CloudStorage.AuthorizationAggregate.ValuesObject;

namespace BackBlazeB2CloudStorage.Tests.Unit
{
  public class BackBlazeAuthorizationTests
  {
    private readonly IBackBlazeAuthorization _backBlazeAuthorization;

    public BackBlazeAuthorizationTests()
    {
      _backBlazeAuthorization = RestService.For<IBackBlazeAuthorization>(Constants.AuthorizationBaseUrl, new RefitSettings
      {
        AuthorizationHeaderValueGetter = GenerateAuthorizationToken("0045d88ceb180d80000000003", "K004QQfvmYM6a0863V61GNCbsPjsFHI")
      });
    }

    [Fact]
    public async Task Should_return_bad_request_When_request_is_null()
    {
      // Act && Assert
      var result = await Assert.ThrowsAsync<ApiException>(async () => await _backBlazeAuthorization.AuthorizeUserAsync(null));

      Assert.NotNull(result);
      Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
      Assert.Equal("Response status code does not indicate success: 400 ().", result.Message);
    }

    [Fact]
    public async Task Should_return_ok_When_application_id_is_not_wrong_and_request_is_valid()
    {
      // Act
      var result = await _backBlazeAuthorization.AuthorizeUserAsync(new AuthorizationRequest());

      // Assert
      Assert.NotNull(result);
    }
    [Fact]
    public async Task Should_return_unauthorized_When_request_is_valid_and_application_key_is_null()
    {
      // arrange
      var backBlazeAuthorization = RestService.For<IBackBlazeAuthorization>(Constants.AuthorizationBaseUrl, new RefitSettings
      {
        AuthorizationHeaderValueGetter = GenerateAuthorizationToken(null, "K004QQfvmYM6a0863V61GNCbsPjsFHI")
      });

      // Act && Assert
      var result = await Assert.ThrowsAsync<ApiException>(async () => await backBlazeAuthorization.AuthorizeUserAsync(new AuthorizationRequest()));
      Assert.NotNull(result);
      Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
      Assert.Equal("Response status code does not indicate success: 401 ().", result.Message);
    }

    protected static Func<Task<string>> GenerateAuthorizationToken(string applicationKeyId, string applicationKey)
    {
      return () => Task.FromResult(Convert.ToBase64String(Encoding.ASCII.GetBytes($"{applicationKeyId}:{applicationKey}")));
    }
  }
}
