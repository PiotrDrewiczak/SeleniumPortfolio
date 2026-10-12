# SeleniumPortfolio — C# UI Automation Framework

[![UI automation tests](https://github.com/PiotrDrewiczak/SeleniumPortfolio/actions/workflows/dotnet-tests.yml/badge.svg)](https://github.com/PiotrDrewiczak/SeleniumPortfolio/actions/workflows/dotnet-tests.yml)

A portfolio-ready Selenium WebDriver framework built with C#, NUnit and the Page Object Model. The tests target the public demo shop at https://www.saucedemo.com/.

## Features

- Page Object Model (POM)
- Centralized browser creation with Selenium Manager (Chrome / Firefox)
- Configuration via `appsettings.json` and environment variables
- Explicit waits; no fixed sleeps
- NUnit test fixtures and assertions
- Screenshot capture on test failure, attached to the Allure report
- Allure test report published to GitHub Pages
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

Run a subset by category (`Smoke`, `Login`, `Inventory`, `Cart`, `Checkout`):

```bash
dotnet test --filter "Category=Smoke"
```

The `Smoke` tests cover the critical path (login, product list, checkout) and finish quickly; the other categories form the wider regression suite.

Configuration precedence is environment variable first, then `appsettings.json`.
Supported variables: `BASE_URL`, `BROWSER` (`chrome` or `firefox`), `HEADLESS`, `WAIT_TIMEOUT_SECONDS`.

## Test scenarios

- Valid login
- Invalid login: wrong password, unknown user, wrong letter case, locked-out user, missing username or password
- A failed login keeps the user on the login page
- The inventory page cannot be opened without logging in
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
├── appsettings.ci.json
└── SeleniumPortfolio.Tests.csproj
.github/workflows/dotnet-tests.yml
```

## Continuous integration

GitHub Actions runs on every push and pull request (`.github/workflows/dotnet-tests.yml`):

1. **smoke**: runs the `Smoke` tests headless in Chrome on Ubuntu.
2. **regression**: runs the full suite, only if the smoke job passed.
3. **report**: on `main`, builds the Allure report and publishes it to GitHub Pages.

Each test job uploads its `.trx` results, failure screenshots and raw Allure results as build artifacts (Actions run page, *Artifacts* section). The CI profile (`appsettings.ci.json`) is selected with `TEST_ENV=ci`.

## Test report

The latest Allure report is published at https://piotrdrewiczak.github.io/SeleniumPortfolio/. Tests are grouped by suite (Login, Inventory, Cart, Checkout) and failed tests include a screenshot.

To build the report locally, run the tests and then, with the [Allure CLI](https://allurereport.org/docs/install/) installed:

```bash
allure serve SeleniumPortfolio.Tests/bin/Debug/net8.0/allure-results
```

## Next improvements

1. Add API tests and contract checks.
2. Add test data builders and a clearer separation between test data and page objects.
3. Add retry policy only for known infrastructure instability (avoid hiding flaky tests).
4. Run tests in a container or a browser matrix in CI.
5. Add a short architecture diagram.

## Portfolio note

Before sharing this repository, run the tests locally, confirm the current demo site's behaviour, and update the README with the results and any known limitations. A green CI run is stronger evidence than a claim that tests are stable.
