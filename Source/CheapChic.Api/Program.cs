using CheapChic.Api.Configuration;
using CheapChic.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.ConfigureConfiguration();
builder.Services.ConfigureServices(builder.Configuration);

var application = builder.Build();
application.ConfigureApplication();

InitializeDatabase(application);

application.Run();


static void InitializeDatabase(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
    scope?.ServiceProvider.GetRequiredService<CheapChicContext>().Database.Migrate();
}