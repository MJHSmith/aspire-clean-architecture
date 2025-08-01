using Aspire.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace EndToEndTests;


[CollectionDefinition("AspireApp")]
public class AspireAppCollection : ICollectionFixture<AspireAppFixture>
{
    // This class is used to define a collection fixture for the AspireAppFixture.
    // It allows sharing the same instance of AspireAppFixture across multiple test classes.
}

public class AspireAppFixture : IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    public DistributedApplication? Application { get; private set; }
    public HttpClient UnAuthorizedApiClient { get; private set; }
    public HttpClient AuthorizedApiClient { get; private set; }
    public HttpClient WebClient { get; private set; }
    

    public async ValueTask InitializeAsync()
    {
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Application = await SetupDistributedApplication(cancellationToken);
        WebClient = await GetHttpClientForAspireResource("web-react", Application, cancellationToken);
        UnAuthorizedApiClient = await GetHttpClientForAspireResource("web-api", Application, cancellationToken);
        AuthorizedApiClient = await GetHttpClientForAspireResource("web-api", Application, cancellationToken);
        await LoginAndUseBearerToken(AuthorizedApiClient, cancellationToken);
    }
    public async ValueTask DisposeAsync()
    {
        if (Application is not null)
        {
            await Application.DisposeAsync();
        }
    }

    private static async Task<HttpClient> GetHttpClientForAspireResource(string aspireResourceName, DistributedApplication app, CancellationToken cancellationToken)
    {
        await app.ResourceNotifications.WaitForResourceHealthyAsync(aspireResourceName, cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        return app.CreateHttpClient(aspireResourceName);
    }

    private static async Task LoginAndUseBearerToken(HttpClient httpClient, CancellationToken cancellationToken)
    {
        HttpResponseMessage loginResponse = await httpClient.PostAsJsonAsync(
            "/users/login", new { Email = "a@b.com", Password = "5Babylon!" }, cancellationToken);
        loginResponse.EnsureSuccessStatusCode();
        string tokenJson = await loginResponse.Content.ReadAsStringAsync(cancellationToken);
        string token = JsonSerializer.Deserialize<string>(tokenJson) ?? throw new InvalidOperationException("Token is null");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private static async Task<DistributedApplication> SetupDistributedApplication(CancellationToken cancellationToken)
    {
        IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Aspire_AppHost>(cancellationToken);
        appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            // Override the logging filters from the app's configuration
            logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
            // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
        });

        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        DistributedApplication app = await appHost.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        await app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        return app;
    }
}
