## Open and Explore the Unicorn Store
1. Open the "Begin" project found in this repo under `/Labs/EF/Begin` and open in Visual Studio 2015.
2. Run the application and browse through the store.
3. In Solution Explorer, expand the "References / DNX Core 5.0" node. Notice that EntityFramework.SqlServer is brought in via NuGet, and SQL Client is brought in as a dependency.

## Logging
1. Look at the `Logging/SqlLogger.cs` class. This is a simple class that implements `ILogger`, dumping queries to a file located at `C:\temp\DatabaseLog.sql`, but it's not turned on yet.
2. Open `Logging/SqlLoggingProvider.cs`. Modify the `CreateLogger` method so it returns an instance of SqlLogger:
```csharp
if(_whitelist.Contains(name))
{
    return new SqlLogger();
}
```
3. Open `Startup.cs` and scroll down to the `Configure` method. Uncomment the following line:
```csharp
loggerfactory.AddProvider(new SqlLoggerProvider());
```
4. Run the application and browse through the store.
5. Open the SQL Log file found in `C:\temp\DatabaseLog.sql`. Keep this file open, as you'll be using it in the following steps.

## Composing Inline SQL with LINQ
1. The site includes the start of a search feature, but it hasn't been implemented yet. Open the `Controllers/ShopController.cs` file and scroll to the `Search` action.
2. Replace the the first line (which creates an empty Products array) with the following:
```csharp
var products = db.Products
    .FromSql("SELECT * FROM [dbo].[SearchProducts] (@p0)", term)
    .ToList();
```
> *Note: The above code is calling a table valued function which implements the search query. Now search will work (you can verify by running the application and searching for "Shirt"), but results aren't ordered. We want to order results by CurrentPrice, but want to implent this using LINQ.* 
3. Modify the above query to order by CurrentPrice as follows:
```csharp
var products = db.Products
    .FromSql("SELECT * FROM [dbo].[SearchProducts] (@p0)", term)
    .OrderByDescending(p => p.CurrentPrice)
    .ToList();
```
4. Run the application and search for "Shirt". View the database log file and scroll to the end. You should see the following:
```sql
SELECT [p].[CategoryId], [p].[CurrentPrice], [p].[Description], [p].[DisplayName], [p].[ImageUrl], [p].[MSRP], [p].[ProductId]
FROM (
    SELECT * FROM [dbo].[SearchProducts] (@p0)
) AS [p]
ORDER BY [p].[CurrentPrice] DESC
```
> *Note: The above query results demonstrate that we were able to run a SQL query, add a LINQ query, and EF7 composed the two into an intellligent SQL statement.*

## Using a Helper Property in the Search Query
The above search results are just sorted by price. Instead, we want to order by savings. We'll utilize a helper property to calculate savings in the model.
1. Open the `Models/UnicornStore/Products.cs` file. Notice that we have a `Savings` helper property which is calculated by subtracting `CurrentPrice` from `MSRP`.
2. Modify the `Search` action in the `ShopController` to replace `CurrentPrice` with `Savings` as follows:
```csharp
var products = db.Products
    .FromSql("SELECT * FROM [dbo].[SearchProducts] (@p0)", term)
    .OrderByDescending(p => p.Savings)
    .ToList();
```
3. Run the application and verify that the results are now sorted by product savings.

> *Note: In the past, it was not possible to compose client-computed properties with database executed queries. EF7 is able to integrate the two, running the SQL query on the server and ordering the results on the client.*

## Multi-level Include With ThenInclude
EF has supported multi-level includes since EF4.1, but the syntax has not been obvious or discoverable. EF7 now includes a simple syntax for multi-level includes with the `ThenInclude` operator. 
> *Note: For more information, see the [Multi-level Include syntax](https://github.com/aspnet/EntityFramework/wiki/Design-Meeting-Notes:-January-8,-2015) discussion in the EF meeting notes.*
1. Open the `OrdersController` and view the `Index` method. This currently shows Order information for your placed orders.
2. Run the application, add some items to your card, and place the order.
3. From header, select the dropdown from your name and click on the Orders list. Note that details for the products aren't shown.
4. Modify the `OrdersController` to include the product details using the following query:
```csharp
var orders = db.Orders
    .Include(o => o.Lines).ThenInclude(l => l.Product)
    .Where(o => o.Username == User.GetUserName())
    .Where(o => o.State != OrderState.CheckingOut);
```
5. Run the application and verify that products are included in the orders list.