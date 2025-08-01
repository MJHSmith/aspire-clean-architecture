# ReadMe.md for AspireBase solution

## TODO:

1. Repeat CreateCatCommandValidator logic in other commands

consider moving to ErrorOr:

| **ErrorOr Method**      | **Functional Equivalent** | **Purpose**                            |
| ----------------------- | ------------------------- | -------------------------------------- |
| `Then`, `ThenAsync`     | `bind` / `flatMap`        | Transform on success, propagate errors |
| `ThenDo`, `ThenDoAsync` | `tap` / `tee`             | Perform side-effect on success         |
| `Match`, `MatchFirst`   | `match` / `fold`          | Pattern-match on success or error      |
| `Switch`, `SwitchFirst` | `match` (void-returning)  | Do side-effects depending on state     |
| `FailIf`                | `filter` / `guard`        | Conditionally fail based on predicate  |
| `Else`                  | `orElse` / `recover`      | Provide fallback when error occurs     |

Consider using ValueOf

## Running the solution

### Pre-requsisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Volta](https://volta.sh) - for managing Node.js versions
  - Install Volta, then run `volta install node` to install the required version of Node.js
  - This will also install npm, which is required for the React front-end project
- **Optional**: [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

### Running the application

If using Visual Studio, open the solution file `AspireBase.sln` and launch/debug the `Aspire.AppHost` project.

If using the .NET CLI, run `dotnet run` from the `Aspire.AppHost` directory.  
Follow the link to the aspire dashboard in the console output.

> [!WARNING]  
> On first run, it will take a while to download the SQL Server container image

### Navigating application
The Aspire dashboard will be displayed when the applicaiton starts.

#### Front end
> [!TIP]  
> Click on URL for the **web-react** resource.

The application will automatically log in using default username/password seeded into the database.  
A table of animals will be shown.

#### Test APIs
> [!TIP]  
> Click on the URL for the **web-api** resource.

1. The Swagger UI dashbaord will be displayed
1. Login using default user: a@b.com / 5Babylon!
1. Copy the bearer token response
1. Click **Authorize** button at top
1. Paste bearer token into _Value_ input, and click **Authorize**
1. Select any endpoint, click **Try it out**, enter inputs (if required) click **Execute**

## Mark's notes

This is my prototype solution where I practice new concepts and approaches from training.
I have adapted the domain objects, CQRS use-cases, API endpoints and Tests to match the tech-test 
requirements, and to demonstrate knowledge of various concepts.  

> [!IMPORTANT]  
> This is **intentionally over-engineered** for the scope/complexity of the tech-test requirements.
> Particularly CQRS which adds much complexity for this simple CRUD scenario.

I am still learning react, so that project is very basic to simply meet requirements of tech-test.  
(It was created from a Visual Studio react project template, which is the same as the CLI command
`npm init --yes vite@latest react -- --template=react-ts`.)

## Concepts to highlight

- Solution/Folder configuration
  - `Directory.Build.props` to manage common build settings across projects
  - `Directory.Packages.props` to manage common package versions across solution

- **Clean Architecture** (or **Onion Architecture**)
  - Separation of concerns, each layer has its own responsibility.
	  - Domain layer is at the core, it contains business logic and domain objects.
	  - Application layer contains use cases and application logic.
		- Translate queries/commands with primitive properties to domain value objects and entities.
	  - Infrastructure layer contains external dependencies (e.g. database, authentication).
	  - Presentation layer (e.g. ASP.NET Core Minimal APIs) is the outermost layer.
		- Translates HTTP requests to application CQRS requests
		- Translates CQRS request response to HTTP JSON responses containing data or problem details.

- **DDD** (Domain driven design)
  - Domain object
	- Domain objects are responsible for creating themsleves. 
	- All logic for the state remains inside the domain object.
  - Domain events
	- Decouple the core domain logic from side effects (e.g., sending emails, updating read models, publishing messages)
	- New functionality can be added by subscribing to events, without modifying existing domain logic

- **ValueObject**
  - ValueObjects provide type satety in sea of strings and guids (primitive obsession)
  - Self-define/enforce their valid state 
  - Reduces need for domain validation because object cannot exist in invalid state

- Exceptions are exceptional -> Use of **Result Pattern** for known/expected failures
  - Result pattern will be 1st class citizen in c#14 with [Discriminated Unions](https://github.com/dotnet/csharplang/blob/b6d1f4b3d132ce648299c75f3b9e2961151996b0/proposals/TypeUnions.md)
  - Explicit workflow (No hidden exception flows, clearly see if a method can fail)
  - No misuse of exceptions for control flow: improves performnce

- **CQRS** _(Exceptional overkill for this scenario)_
  - Separation of concerns, can evolve read and write models independently.
  - In this example commands interact with rich domain models, querys return simple DTOs.

- **Observability**
  - OpenTelemetry for distributed tracing
  - Use of `ILogger` for structured logging
  - Can be viewed in the Aspire dashboard, could be exported to a 3rd party tool

- **Testing**
  - Architecture testing to ensure layers are not dependent on each other
  - Unit tests ("Specs") for domain logic, application logic, and presentation logic
  - End-to-End tests to ensure the application works as expected from the user's perspective

## Original requirements

- Cat is an animal
- Dog is an animal
- Both are mammals
- Bird is an animal, but it’s not a mammal
- All of them make different sounds
- I’d like see a set of objects and interfaces created in c# to represent these things
  - [x] Objects/interfaces in namespace `Domain.Animals`
- Store the animals in a SQL database and query it using EF
  - [x] EF configuration in namespace `Infrastructure.Database.Configuration`
  - [x] Queries in Handlers, namespace `Application.Animals` Get{XXX}
- All put into a single array or list
  - [x] Single list in of animals constructed in `Application.Animals.Get.GetAnimalsQueryHandler`
- loop the array and examine different types in the list
  - [x] Loop in extra code at bottom of `Application.Animals.Get.GetAnimalsQueryHandler`
- query the array using LINQ
  - [x] Query in extra code at bottom of `Application.Animals.Get.GetAnimalsQueryHandler`
- serialise the array or list to JSON or XML
  - [x] ASP.NET Core framework automatically serialises to JSON
    - uses `System.Text.Json` by default
    - JSON options can be configured for minimal APIs with `Configure<JsonOptions>()` on the `IServiceCollection` in `Program.cs` 
      (or for Controllers when adding `AddControllers()` with `AddJsonOptions()`)
 
 
 ## What's included in the solution?

- SharedKernel project with common Domain-Driven Design abstractions.
- Domain layer
	- Domain objects (encapsulating business requiremnts/rules)
	- ValueObjects (encapsulating business requiremnts/rules)
- Application layer
  - Applicaiton logic in example use cases
  - Abstractions for:
	- Authentication
	- Data access
    - CQRS 
	- Logging + Validation cross-cutting concerns (a.k.a. "behaviours" in MediatR)  
- Infrastructure layer
  - Authentication / Permission authorization
  - EF Core, SqlServer
  - Domain event dispatcher
- Presentation layer
  - ASP.NET Core Minimal APIs
  - React front-end application
- Testing projects
- Aspire orchestration project
