# BulkyBook
BulkyBook is an e-commerce book shop. See [demo](https://my-eventify.herokuapp.com/).

## Technology used
The app is built using .NET 6: 
- ASP.NET Core, 
- EF Core,
- ASP.NET Core Identity

For styling used:
- Bootstrap
- [Bootswatch Lux](https://bootswatch.com/lux/)

For data presentation on necessary occasions used [DataTables](https://datatables.net/) Jquery plug-in.

For payments processing used [Stripe API](https://stripe.com/docs/payments/payment-methods) in "test mode".

Configured alternative FaceBook login.

## N-tier Architecture
 Solution was structured with n-tier architecture concept in mind.
 - BulkyBook.DataAccess layer contains code for accessing database. Impemented **Repository and UnitOfWork pattern** to isolate data accessing logic.
 - BulkyBook.Models layer contains core entities of the application, and ViewModels.
 - BulkyBookWeb is main project (i.e. entry point with Startup.cs file) contains business logic and configurations: it accepts and responds to HTTP requests, 
 presents data in Views.
 - BulkyBook.Utility layer contains helping tools(e.g. EmailSender implementation, settings).

## Database
During development SQLite database engine used, then switched to PostgreSQL for deployment. 
 

