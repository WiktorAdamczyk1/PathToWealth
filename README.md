# PathToWealth

 A Single Page Fullstack Application for financial analysis, investment strategy visualization, and retirement planning based on the principles presented by [JL Collins' "The Simple Path to Wealth"](https://www.amazon.com/Simple-Path-Wealth-financial-independence/dp/1533667926).

## Functionalities

- Perform financial analysis without logging in.
- Remember user information between sessions adn devices by registering an account.
- User authentication with JWT Tokens and HTTPCookies.
- Customise the possible variables or use the default values based on the book.
- Predict the amount of money you may withdraw yearly when following the 4% rule.

### API Endpoints

* `/login` POST
* `/logout` POST
* `/register` POST
* `/refresh-token` POST
* `/revoke-token` POST
* `/userfinancialdata` GET
* `/userfinancialdata` PUT
* `/delete-account` DELETE
* `/change-password` POST

## Technologies

* Backend: [.NET 8.0 ASP.NET Core Web API with Minimal API architecture](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio)
* Frontend: [Vue3](https://vuejs.org/) with TypeScript
* CSS: [Element Plus](https://element-plus.org/)
* Database: [MS SQL Server](https://www.microsoft.com/en-us/sql-server/), [Entity Framework](https://learn.microsoft.com/en-us/ef/)
* Additional: Single Page Application, [JWT Authentication](https://jwt.io/introduction), [FluentValidation](https://fluentvalidation.net/)

