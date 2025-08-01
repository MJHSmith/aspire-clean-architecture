using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Application.Animals.Create;
using Application.Animals.Delete;
using Application.Animals.Get;
using Application.Animals.GetAll;
using NSubstitute;
using SharedKernel;
using Shouldly;
using Xunit;
using GetAllAnimalResponse = global::Application.Animals.GetAll.AnimalResponse;
using GetAnimalResponse = global::Application.Animals.Get.AnimalResponse;

namespace Specs.Presentation.Animals;

public class EndpointSpecs : IClassFixture<CustomWebApplicationFactory>
{

    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public EndpointSpecs(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_Animals_Endpoint_Exists_And_Returns_Expected_Schema()
    {
        // Arrange
        var expected = new List<GetAllAnimalResponse>
        {
            new GetAllAnimalResponse(Guid.NewGuid(), "AnimalType", true, "Name", "Sound")
        };
        _factory.GetAnimalsHandlerMock
            .Handle(Arg.Any<GetAnimalsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success(expected)));

        // Act
        HttpResponseMessage response = await _client.GetAsync("/animals", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        List<GetAllAnimalResponse>? animals = await response.Content.ReadFromJsonAsync<List<GetAllAnimalResponse>>(TestContext.Current.CancellationToken);
        animals.ShouldNotBeNull();
        animals.ShouldAllBe(a => 
            a.Id == expected[0].Id 
            && a.Name == expected[0].Name 
            && a.IsMammel == expected[0].IsMammel 
            && a.Sound == expected[0].Sound);
    }

    [Fact]
    public async Task Get_Animal_By_Type_And_Id_Endpoint_Exists_And_Returns_Expected_Schema()
    {
        // Arrange
        var expected = new GetAnimalResponse(Guid.NewGuid(), "AnimalType", true, "Name", "Sound", "HairColourDescription", 10);
        _factory.GetAnimalHandlerMock
            .Handle(Arg.Any<GetAnimalQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success(expected)));

        // Act
        HttpResponseMessage response = await _client.GetAsync($"/animal/animalType/{Guid.NewGuid()}", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        GetAnimalResponse detail = await response.Content.ReadFromJsonAsync<GetAnimalResponse>(TestContext.Current.CancellationToken);

        // Assert
        detail.Id.ShouldBe(expected.Id);
        detail.Type.ShouldBe(expected.Type);
        detail.Name.ShouldBe(expected.Name);
        detail.Sound.ShouldBe(expected.Sound);
        detail.HairColourDescription.ShouldBe(expected.HairColourDescription);
        detail.WingspanInCentimeters.ShouldBe(expected.WingspanInCentimeters);
    }

    [Fact]
    public async Task CreateCat_Endpoint_Exists_And_Accepts_Expected_Input()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        _factory.CreateCatHandlerMock
            .Handle(Arg.Any<CreateCatCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success(expectedId)));
        var request = new { Name = "TestCat", HairColourDescription = "Black" };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/cat", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        Guid id = await response.Content.ReadFromJsonAsync<Guid>(TestContext.Current.CancellationToken);
        id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task CreateDog_Endpoint_Exists_And_Accepts_Expected_Input()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        _factory.CreateDogHandlerMock
            .Handle(Arg.Any<CreateDogCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success(expectedId)));
        var request = new { Name = "TestDog", HairColour1 = "Brown", HairColour2 = (string?)null, HairPattern = (string?)null };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/v1/dog", request, TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        Guid id = await response.Content.ReadFromJsonAsync<Guid>(TestContext.Current.CancellationToken);
        id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task CreateDogV2_Endpoint_Exists_And_Accepts_Expected_Input()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        _factory.CreateDogHandlerMock
            .Handle(Arg.Any<CreateDogCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success(expectedId)));
        var request = new { Name = "TestDog", HairColour1 = "Brown", HairColour2 = (string?)null, HairPattern = (string?)null };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/v2/dog", request, TestContext.Current.CancellationToken);
        //using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/dog") { Content = JsonContent.Create(request) };
        //requestMessage.Headers.Add("x-api-version", "2.0");
        //HttpResponseMessage response = await _client.SendAsync(requestMessage, TestContext.Current.CancellationToken);

        // Assert - should be 2 tests: returns 201 created, returns guid
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        Guid id = await response.Content.ReadFromJsonAsync<Guid>(TestContext.Current.CancellationToken);
        id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task CreateBird_Endpoint_Exists_And_Accepts_Expected_Input()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        _factory.CreateBirdHandlerMock
            .Handle(Arg.Any<CreateBirdCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success(expectedId)));
        var request = new { Name = "TestBird", WingspanInCentimters = 30u };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/bird", request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        Guid id = await response.Content.ReadFromJsonAsync<Guid>(TestContext.Current.CancellationToken);
        id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Delete_Endpoint_Exists_And_Accepts_Expected_Input()
    {
        // Arrange
        _factory.DeleteAnimalHandlerMock
            .Handle(Arg.Any<DeleteAnimalCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Result.Success()));
        
        // Act
        HttpResponseMessage response = await _client.DeleteAsync($"/animal/animalType/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    // TODO: validate non-success handler responses generate expected problem details
    // -- [endpoint]
    //    -- Exists
    //    -- Returns expected schema
    //    -- Returns problem details on unexpected failure 
    //    -- Returns problem details on bad requests (cant contruct value objects)
    //    -- Returns problem details on application validation failure (handler validation errors)
    //    -- Returns problem details on domain validation (cant do x because of y)
}
