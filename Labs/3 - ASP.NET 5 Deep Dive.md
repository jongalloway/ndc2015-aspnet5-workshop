# Creating an inline middleware
1. Start with the application you created in the first lab, or just create a new empty ASP.NET 5 application
1. Open `Startup.cs`
1. Create an inline middleware that runs **before** the hello world delegate that sets the culture for the current request from the query string:
``` C#
public void Configure(IApplicationBuilder appBuilder)
{
  app.Use((context, next) =>
  {
    var cultureQuery = context.Request.Query["culture"];
    if (!string.IsNullOrWhiteSpace(cultureQuery))
    {
      var culture = new CultureInfo(cultureQuery);
#if !DNXCORE50
      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;
#else
      CultureInfo.CurrentCulture = culture;
      CultureInfo.CurrentUICulture = culture;
#endif
    }
    
    // Call the next delegate/middleware in the pipeline
    return next();
  });
  
  app.Run(async (context) =>
  {
      await context.Response.WriteAsync($"Hello {CultureInfo.CurrentCulture.DisplayName}");
  });
}
```
1. Run the app now and set the culture via the query string, e.g. http://localhost/?culture=no

# Add configuration
1. Add a constructor to the application's `Startup.cs`
1. Create a new `Configuration` object in the constructor and assign it to a new private class field `IConfiguration _configuration`
1. Add a reference to the `Microsoft.Framework.ConfigurationModel.Json` package in the application's `project.json` file
1. Back in the `Startup.cs`, add a call to `.AddJsonFile("config.json")` immediately after the creation of the `Configuration` object (inline, chained method). It should now look like the following:
``` C#
  public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup()
        {
            var configuration = new Configuration()
                .AddJsonFile("config.json");

            _configuration = configuration;
        }
        ...
```
1. Add a new JSON file to the project called `config.json`
1. Add a new key/value pair to the `config.json` file: `"message": "Hello from config.json!"`
1. Change the code in `Startup.cs` to use the message from the configuration system:
``` C#
  app.Run(async (context) =>
  {
      await context.Response.WriteAsync(_configuration["message"]);
  });
```
1. Run the application and the message from `config.json` should be returned
1. Change the message in the `config.json` file and refresh the page (without changing any other code). Note that the message hasn't changed as the configuration was only read when the application was started.
1. Go back to Visual Studio and touch and save the `Startup.cs` file to force the process to restart
1. Go back to the browser now and refresh the page and it should show the updated message

## Override configuration from environment variables
