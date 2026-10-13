using Microsoft.Extensions.Configuration;

namespace SeleniumPortfolio.Tests.Configuration;

/// <summary>
/// Resolution order (last wins): appsettings.json, appsettings.{TEST_ENV}.json, environment variables
/// (BASE_URL, API_BASE_URL, BROWSER, HEADLESS, WAIT_TIMEOUT_SECONDS).
/// </summary>
public sealed class TestSettings
{
    public string Environment { get; init; } = "default";
    public string BaseUrl { get; init; } = "https://www.saucedemo.com/";
    public string ApiBaseUrl { get; init; } = "https://restful-booker.herokuapp.com/";
    public string Browser { get; init; } = "chrome";
    public bool Headless { get; init; }
    public int WaitTimeoutSeconds { get; init; } = 10;

    private static readonly Lazy<TestSettings> Cached = new(Build);

    public static TestSettings Load() => Cached.Value;

    private static TestSettings Build()
    {
        var environment = System.Environment.GetEnvironmentVariable("TEST_ENV");

        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false);

        if (!string.IsNullOrWhiteSpace(environment))
            builder.AddJsonFile($"appsettings.{environment}.json", optional: false);

        var section = builder.Build().GetSection("TestSettings");
        var defaults = new TestSettings();

        return new TestSettings
        {
            Environment = string.IsNullOrWhiteSpace(environment) ? defaults.Environment : environment,
            BaseUrl = Env("BASE_URL") ?? section["BaseUrl"] ?? defaults.BaseUrl,
            ApiBaseUrl = Env("API_BASE_URL") ?? section["ApiBaseUrl"] ?? defaults.ApiBaseUrl,
            Browser = Env("BROWSER") ?? section["Browser"] ?? defaults.Browser,
            Headless = bool.TryParse(Env("HEADLESS") ?? section["Headless"], out var headless) ? headless : defaults.Headless,
            WaitTimeoutSeconds = int.TryParse(Env("WAIT_TIMEOUT_SECONDS") ?? section["WaitTimeoutSeconds"], out var timeout)
                ? timeout
                : defaults.WaitTimeoutSeconds
        };
    }

    private static string? Env(string name)
    {
        var value = System.Environment.GetEnvironmentVariable(name);
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
