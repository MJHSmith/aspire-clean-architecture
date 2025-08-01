# Specs (Unit tests)

Use the terminology "Specs" to refer to unit tests as they specify the functionality of the solution.

Folders match the folders/organization of the solution.
Test method names describe the specification.

## Domain

Test/Specify the behaviour of domain aggregates, entities and value objects.

## Application

Test/Specify the behaviour of application services (e.g validation), commands and queries.

## Presentation

Test/Specify the behaviour of presentation components, e.g. endpoints.

These tests use WebApplicationFactory to run the application in a test server, 
so the endpoint is being directly executed.

Services in the WebApplicationFactory can be mocked or changed for example:  
 - Use in-memory database  
 - Use a containerized real database
 - Adapt authentication to pass a test user