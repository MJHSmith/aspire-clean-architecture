namespace EndToEndTests;

[Collection("AspireApp")]
public class WebApiEndpointTests
{
    
    private readonly AspireAppFixture _fixture;

    public WebApiEndpointTests(AspireAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Get_Animals_Returns_Unauthorized_StatusCode()
    {
        // Arrange
        

        // Act
        HttpResponseMessage response = await _fixture.UnAuthorizedApiClient.GetAsync("/animals", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_Animals_Returns_Ok_StatusCode()
    {
        // Arrange

        // Act
        HttpResponseMessage response = await _fixture.AuthorizedApiClient.GetAsync("/animals", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // TODO: more complex workflows where you create, update and delete
    // TODO: validated non-happy paths, such as validation errors, etc.
}
