# Adding ASP.NET MVC to an Empty Web Application
1. Create a new ASP.NET 5 application using the "Empty" template.
2. Open `project.json` and add a reference to the dependencies section so that it appears as below:

  ```json
"dependencies": {
  "Microsoft.AspNet.Server.IIS": "1.0.0-beta4",
  "Microsoft.AspNet.Server.WebListener": "1.0.0-beta4",
  "Microsoft.AspNet.Mvc": "6.0.0-beta4"
},
  ```

  > *Note: Make sure you remember the comma at the end of the previous line.*

3. In `Startup.cs`, modify the `ConfigureServices()` function to include `services.AddMvc()`:

  ```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
}
  ```

4. Replace the code in the `Configure()` method with `app.UseMvc();` like this:

  ```csharp
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller}/{action}/{id?}",
        defaults: new { controller = "Home", action = "Index" });
});
  ```

5. Right-click on the project and select "Add / New Folder" and name the folder "Controllers".
6. Right-click on the `Controllers` folder and select "Add / New Item". Select "MVC Controller Class" from the list and leave the default name ("HomeController").
7. Right-click on the project and select "Add / New Folder" and name the folder "Views".
8. Right-click the `Views` folder and select "Add / New Folder" and name the folder "Home".
9. Right-click the `Views\Home` folder and select "Add / New Item". Select "MVC View Page" from the list and leave the default ("Index.cshtml").
10. Add some simple text (e.g. "Hello") to the bottom of the `Index.cshtml` and run the application. You should see your text in the browser.

# Creating a new ASP.NET MVC 6 application
1. Create a new project. This time, select the "Web Site" template instead of "Empty".
2. Take a look around the solution, especially the `project.json` and `Startup.cs` files to see the code you added in the previous lab.
3. Run the application to see the default project template design.
4. Stop the application but Leave this solution open, you'll use it in the next lab.

# Adding a Web API controller to the ASP.NET MVC 6 application
1. Right-click the `Controllers` folder, select "Add New Item" and add a new "MVC Controller". Name the controller "ExampleController.cs".
2. Delete the `Index()` action. You will now have an empty `ExampleController` class.
3. Add a new `Get()` method that returns a string, like this:

  ```csharp
public class ExampleController : Controller
{
    public string Get()
    {
        return "Well hello there!";
    }
}
  ```

4. Add the following attribute above the ExampleController declaration:

  ```
[Route("Example")]
public class ExampleController : Controller
  ```

5. Run the application and browse to /Example, e.g. http://localhost:40292/Example. You should see "Well hello there!" in your browser.

## Returning a complex type
1. Modify the `Get()` method to return an `IEnumerable<string>` like this:

  ```csharp
[Route("Example")]
public class ExampleController : Controller
{
    public IEnumerable<string> Get()
    {
        return new string[] {"Well hello there!", "Another value"};
    }
}
  ```

2. Launch the application and browse to /Example again. The controller will return a json object containing the above values.
  > *Note: If you're viewing the site in Internet Explorer, you'll be prompted to download the json file. You can save it an open it in notepad.*

## Changing the route to use the [controller] token
1. Replace the `[Route("Example")]` attribute with `[Route("[controller]")]`:

  ```csharp
[Route("[controller]")]
public class ExampleController : Controller
{
    public IEnumerable<string> Get()
    {
        return new string[] { "Well hello there!", "Another value" };
    }
}
  ```

2. Run the application again and browse to /Example again. Note that the `[controller]` route token automatically maps the URL based on the controller name.
 
## Converting to a POCO controller
1. Change the `ExampleController` class to no longer inherits from Controller by deleting `: Controller` from the end of the class declaration:

  ```csharp
public class ExampleController
  ```
  
2. Run the application one more time to verify that /Example still returns the json values. In ASP.NET MVC 6, controllers are not required to inherit from the `Controller` base class.
