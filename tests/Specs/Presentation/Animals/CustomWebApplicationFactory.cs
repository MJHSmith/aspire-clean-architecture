using System.Security.Claims;
using System.Text.Encodings.Web;
using Application.Abstractions.Messaging;
using Application.Animals.Create;
using Application.Animals.Delete;
using global::Application.Animals.Get;
using global::Application.Animals.GetAll;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using GetAllAnimalResponse = global::Application.Animals.GetAll.AnimalResponse;
using GetAnimalResponse = global::Application.Animals.Get.AnimalResponse;

namespace Specs.Presentation.Animals;

/// <summary>
/// Mocks the handlers for the animal commands and queries, and sets up a test authentication scheme so that all authentication checks pass.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ICommandHandler<CreateBirdCommand, Guid> CreateBirdHandlerMock { get; } = Substitute.For<ICommandHandler<CreateBirdCommand, Guid>>();
    public ICommandHandler<CreateCatCommand, Guid> CreateCatHandlerMock { get; } = Substitute.For<ICommandHandler<CreateCatCommand, Guid>>();
    public ICommandHandler<CreateDogCommand, Guid> CreateDogHandlerMock { get; } = Substitute.For<ICommandHandler<CreateDogCommand, Guid>>();
    public ICommandHandler<DeleteAnimalCommand> DeleteAnimalHandlerMock { get; } = Substitute.For<ICommandHandler<DeleteAnimalCommand>>();
    public IQueryHandler<GetAnimalQuery, GetAnimalResponse> GetAnimalHandlerMock { get; } = Substitute.For<IQueryHandler<GetAnimalQuery, GetAnimalResponse>>();
    public IQueryHandler<GetAnimalsQuery, List<GetAllAnimalResponse>> GetAnimalsHandlerMock { get; } = Substitute.For<IQueryHandler<GetAnimalsQuery, List<GetAllAnimalResponse>>>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("UnitTesting");
        builder.ConfigureServices(services =>
        {
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            services.RemoveAll(typeof(ICommandHandler<,>));
            services.AddSingleton(CreateBirdHandlerMock);
            services.AddSingleton(CreateCatHandlerMock);
            services.AddSingleton(CreateDogHandlerMock);
            services.RemoveAll(typeof(ICommandHandler<>));
            services.AddSingleton(DeleteAnimalHandlerMock);
            services.RemoveAll(typeof(IQueryHandler<,>));
            services.AddSingleton(GetAnimalHandlerMock);
            services.AddSingleton(GetAnimalsHandlerMock);

            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });
        });
    }

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Claim[] claims = new[] { new Claim(ClaimTypes.Name, "TestUser") };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
