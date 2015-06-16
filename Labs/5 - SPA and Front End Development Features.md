## Adding Bower and Node modules
1. Open `bower.json` and add references to the `Angular` and `Angular-Route` packages. Your edited `bower.json` file will appear as follows:

  ```json
{
  "name": "ASP.NET",
  "private": true,
  "dependencies": {
    "bootstrap": "3.0.0",
    "jquery": "1.10.2",
    "jquery-validation": "1.11.1",
    "jquery-validation-unobtrusive": "3.2.2",
    "hammer.js": "2.0.4",
    "bootstrap-touch-carousel": "0.8.0",
    "angular": "^1.3.12",
    "angular-route": "^1.3.12"
  }
}
  ```
  
2. Save the file. Expand the `Dependencies/Bower` node in Solution Explorer and verify that Angular and Angular-Route have been installed.
3. Edit your package.json file and add references to `todomvc-common` and `todomvc-app-css`:

  ```json
{
  "name": "ASP.NET",
  "version": "0.0.0",
  "devDependencies": {
        "gulp": "3.8.11",
        "rimraf": "2.2.8",
        "todomvc-app-css": "^2.0.0", 
        "todomvc-common": "^1.0.0"
  }
}
  ```
4. Save the file and verify that the packages have been installed in `Dependencies/NPM`.

## Modifying the Gulp copy task to copy the new packages
1. There is an existing Gulp task that copies Bower packages, so we'll start with the easy part: adding the new Bower packages to the existing task. Edit the `bower` declaration and add the new Bower packages:

  ```javascript
    var bower = {
        "bootstrap": "bootstrap/dist/**/*.{js,map,css,ttf,svg,woff,eot}",
        "bootstrap-touch-carousel": "bootstrap-touch-carousel/dist/**/*.{js,css}",
        "hammer.js": "hammer.js/hammer*.{js,map}",
        "jquery": "jquery/jquery*.{js,map}",
        "jquery-validation": "jquery-validation/jquery.validate.js",
        "jquery-validation-unobtrusive": "jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
        "angular": "angular/angular.js",
        "angular-route": "angular-route/angular-route.js"
    }
  ```
2. Modify the `paths` section to include an `npm` path:

  ```javascript
  var paths = {
    bower: "./bower_components/",
    npm: "./node_modules/",
    lib: "./" + project.webroot + "/lib/"
  };
  ```
  
3. Add the following code directly after the for loop at the end of the file, which will loop through the new `npm` modules and copy them over to `lib` as well:  

  ```javascript
    var npm = {
        "todomvc-app-css": "todomvc-app-css/*.css",
        "todomvc-common": "todomvc-common/*.{js,css}"
    }

    for (var destinationDir in npm) {
        gulp.src(paths.npm + npm[destinationDir])
          .pipe(gulp.dest(paths.lib + destinationDir));
    }
  ```
  > *Note: This code will appear directly before the ending line: `});`*
3. Open the `Task Runner Explorer` and execute the `Copy` task. Verify that the new packages have been added to `wwwroot/lib`.

## Running the Todos Application
1. Open `Controllers/TodosController.cs` and take a look at how it's set up. This is a simple implementation of the [TodoMvc specification] (https://github.com/tastejs/todomvc-api/blob/master/todos.apib) from the [TodoMvc](http://todomvc.com) project.
2. Run the application and verify that you are able to add new tasks. You can click on Active and Completed to view your tasks.
