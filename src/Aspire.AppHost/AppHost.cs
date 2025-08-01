using Aspire.AppHost;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> password = 
    builder.AddParameter("password", "YourStrong!Passw0rd", secret: true);

IResourceBuilder<SqlServerDatabaseResource> database = builder
    .AddSqlServer("database", password)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("database-data")
    .AddDatabase("tech-demo");

IResourceBuilder<ProjectResource> webapi = builder.AddProject<Projects.Web_Api>("web-api")
    .WithEnvironment("ConnectionStrings__Database", database)
    .WithReference(database)
    .WaitFor(database)
    .WithSwaggerUI()
    .WithScalar()
    .WithReDoc();

IResourceBuilder<NodeAppResource> webreact = builder.AddNpmApp("web-react", "../React")
    .WithReference(webapi) // Vite only exposes variables exposed with VITE_ to client site code 
    .WithEnvironment("VITE_API_BASE_HTTPS", webapi.GetEndpoint("https"))
    .WithEnvironment("VITE_API_BASE_HTTP", webapi.GetEndpoint("http"))
    .WaitFor(webapi)
    .WithHttpEndpoint(env: "PORT")
    .PublishAsDockerFile();

webapi.WithEnvironment("FRONTEND_ORIGIN", webreact.GetEndpoint("http"));

builder.Build().Run();
