1. Visual Studio RC Install
2. Create a new empty ASP.NET 5 project and run it (Ctrl+F5)
3. Look at the various files in the project and familiarize yourself with them
4. Look at the References node and explore the package graph
5. Select the launch profile for "web" and run the app using Web Listener (self-host)
6. Run the app using the Core CLR
7. Fix the bad PATH settings that VS2015 RC created
8. Install DNVM from github.com/aspnet/home
9. Run `dnvm` and look at the commands
10. Install latest DNX using DNVM from the unstable feed

  ```
  dnvm upgrade -u
  dnvm install default -r coreclr -u
  ```

11. Set the current DNX back to beta4
12. Run the app you created before from the command line

  ```
  dnx . web
  ```

13. Delete the project.lock.json file from the application
14. Recreate the file by running a package restore

  ```
  dnu restore
  ```

15. Publish the application to a folder

  ```
  dnu publish
  ```

16. Inspect the publish output
17. Delete the publish output
18. Publish the application again with pre-compilation

  ```
  dnu publish --no-source
  ```

19. Inspect the publish output again and note the additional "application package"
20. Delete the publish output
21. Publish the application again and include the .NET Core runtime as well

  ```
  dnu publish --no-source --runtime dnx-coreclr-win-x86.1.0.0-beta4
  ```
