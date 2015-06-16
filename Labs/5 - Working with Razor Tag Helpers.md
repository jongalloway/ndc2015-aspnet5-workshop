# Look at Tag Helpers in the project template
1. Create a new ASP.NET 5 project using the "Web Site" template
1. Make sure the project is set to use 1.0.0-beta4 in its `global.json` otherwise the Tag Helper tooling won't work (remember to restart VS if you have to change `global.json`)
1. Open the view `Views/Account/Register.cshtml`
1. Look at the Tag Helpers being used in this view (they're colored purple and in bold) and play around with setting their attributes and exploring the IntelliSense offered for the different attribute types
1. Run the application and see the HTML output by the Tag Helpers
1. Look at the other views in `Views/Account/` folder to see how they use Tag Helpers
1. Open the file `Views/Shared/_Layout.cshtml`
1. Look at the Tag Helpers being used in the `<head>` element and at the end of the page to render CSS stylesheets and JavaScript files and compare it to the generated HTML output

