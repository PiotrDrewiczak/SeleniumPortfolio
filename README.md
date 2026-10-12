# SeleniumPortfolio — C# UI Automation Framework

A portfolio-ready Selenium WebDriver framework built with C#, NUnit and the Page Object Model. The tests target the public demo shop at https://www.saucedemo.com/.

## Features

- Page Object Model (POM)
- Centralized browser creation with Selenium Manager (Chrome / Firefox)
- Configuration via `appsettings.json` and environment variables
- Explicit waits; no fixed sleeps
- NUnit test fixtures and assertions
- Screenshot capture on test failure
- GitHub Actions CI workflow
- Positive and negative scenarios for login, cart, sorting and checkout

## Requirements

- .NET 8 SDK
- Google Chrome or Mozilla Firefox
- Internet access to reach SauceDemo and restore NuGet packages

Selenium Manager, included with Selenium 4, normally handles browser-driver resolution automatically.

## Run

From the repository root:

```bash
dotnet restore
dotnet test
```

Run in headless mode:

**PowerShell**
```powershell
$env:HEADLESS="true"
dotnet test
```

**bash**
```bash
HEADLESS=true dotnet test
```

Select a browser:

```bash
BROWSER=firefox dotnet test
```

Configuration precedence is environment variable first, then `appsettings.json`.
Supported variables: `BASE_URL`, `BROWSER` (`chrome` or `firefox`), `HEADLESS`, `WAIT_TIMEOUT_SECONDS`.

## Test scenarios

- Valid login
- Invalid login
- Inventory page loads and shows products
- Products can be sorted by price
- A product can be added to and removed from the cart
- Checkout completes with valid customer information
- Checkout form validation rejects missing required information

Demo credentials are public credentials supplied by SauceDemo for its demo application:
- Username: `standard_user`
- Password: `secret_sauce`

Do not use real credentials or production systems without explicit authorization.

## Architecture

```text
SeleniumPortfolio.Tests/
├── Configuration/
│   └── TestSettings.cs
├── Core/
│   ├── DriverFactory.cs
│   └── TestBase.cs
├── Pages/
│   ├── BasePage.cs
│   ├── LoginPage.cs
│   ├── InventoryPage.cs
│   ├── CartPage.cs
│   └── CheckoutPage.cs
├── Tests/
│   ├── LoginTests.cs
│   ├── InventoryTests.cs
│   ├── ShoppingCartTests.cs
│   └── CheckoutTests.cs
├── appsettings.json
└── SeleniumPortfolio.Tests.csproj
└── .github/workflows/dotnet-tests.yml
```

## Next improvements

1. Add Allure or another report generator.
2. Add API tests and contract checks.
3. Add test data builders and a clearer separation between test data and page objects.
4. Add retry policy only for known infrastructure instability (avoid hiding flaky tests).
5. Run tests in a container or a browser matrix in CI.
6. Add a short architecture diagram and a CI badge to the GitHub repository.

## Portfolio note

Before sharing this repository, run the tests locally, confirm the current demo site's behaviour, and update the README with the results and any known limitations. A green CI run is stronger evidence than a claim that tests are stable.
