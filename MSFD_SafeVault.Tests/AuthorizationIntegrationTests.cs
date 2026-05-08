using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MSFD_SafeVault.Tests;

public class AuthorizationIntegrationTests
{
    [Test]
    public async Task AdminRoute_UnauthenticatedUser_RedirectsToLogin()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost")
        });

        var response = await client.GetAsync("/Admin");

        Assert.That(response.StatusCode == HttpStatusCode.Redirect);
        Assert.That(response.Headers.Location?.OriginalString?.Contains("/Account/Login") == true);
    }

    [Test]
    public async Task AdminRoute_AuthenticatedNonAdminUser_RedirectsToAccessDenied()
    {
        await using var factory = CreateFactoryWithRole("User");
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost")
        });

        var response = await client.GetAsync("/Admin");

        Assert.That(response.StatusCode == HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task AdminRoute_AdminUser_ReturnsSuccess()
    {
        await using var factory = CreateFactoryWithRole("Admin");
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost")
        });

        var response = await client.GetAsync("/Admin");

        Assert.That(response.StatusCode == HttpStatusCode.OK);
    }

    private static WebApplicationFactory<Program> CreateFactoryWithRole(string role)
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                        options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                        options.DefaultForbidScheme = TestAuthHandler.SchemeName;
                    }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        TestAuthHandler.SchemeName,
                        options => { });

                    services.Configure<TestAuthOptions>(options =>
                    {
                        options.Role = role;
                    });
                });
            });
    }

    private sealed class TestAuthOptions
    {
        public string Role { get; set; } = "User";
    }

    private sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "TestAuth";
        private readonly TestAuthOptions _testOptions;

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IOptions<TestAuthOptions> testOptions)
            : base(options, logger, encoder)
        {
            _testOptions = testOptions.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "test-user"),
                new Claim(ClaimTypes.Role, _testOptions.Role)
            };

            var identity = new ClaimsIdentity(claims, SchemeName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return Task.CompletedTask;
        }
    }
}