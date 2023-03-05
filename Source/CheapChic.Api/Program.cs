using CheapChic.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.ConfigureConfiguration();
builder.Services.ConfigureServices(builder.Configuration);

var application = builder.Build();
application.ConfigureApplication();

application.Run();