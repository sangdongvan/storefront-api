using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Client;
using OpenIddict.Validation.AspNetCore;
using StoreFront.Config;
using Storefront.DbContext;
using Storefront.Swagger;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddSimpleSwagger();

var globalConfig = services.AddGlobalConfig(builder.Configuration);

services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite("Data Source=app.db");

    // Register the entity sets needed by OpenIddict.
    options.UseOpenIddict();
});

// Local Identity
services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager();

services.AddOpenIddict()
    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        // Configure OpenIddict to use the Entity Framework Core stores and models.
        // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
        options.UseEntityFrameworkCore()
            .UseDbContext<ApplicationDbContext>();
    })
    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        // Enable the token endpoint.
        options.SetTokenEndpointUris("_connect/token");
        options.SetConfigurationEndpointUris("_connect/openid-configuration");
        options.SetCryptographyEndpointUris("_connect/jwks");

        options.SetAccessTokenLifetime(TimeSpan.FromMinutes(30));
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

        // Allow grant_type=password to be negotiated.
        options.AllowPasswordFlow()
            .AllowRefreshTokenFlow();

        // Disable token storage, which is not necessary for non-interactive flows like
        // grant_type=password, grant_type=client_credentials or grant_type=refresh_token.
        options.DisableTokenStorage();
        options.DisableAuthorizationStorage();

        // Disable token encryption. Just use plain JWT.
        options.DisableAccessTokenEncryption();

        // Accept anonymous clients (i.e clients that don't send a client_id).
        options.AcceptAnonymousClients();

        // Register the signing and encryption credentials.
        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        var openIddictServerBuilder = options.UseAspNetCore()
            .EnableTokenEndpointPassthrough();

        // This server is deployed behind a HTTPS proxy, it's technically hosted under http protocol.
        openIddictServerBuilder.DisableTransportSecurityRequirement();
    })
    // Register the OpenIddict validation components.
    .AddValidation(options =>
    {
        // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    })
    // Register the OpenIddict client components.
    .AddClient(options =>
    {
        // Allow grant_type=password to be negotiated.
        options.AllowPasswordFlow()
            .AllowRefreshTokenFlow();

        // Disable token storage, which is not necessary for non-interactive flows like
        // grant_type=password, grant_type=client_credentials or grant_type=refresh_token.
        options.DisableTokenStorage();

        // Register the signing and encryption credentials.
        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        // Register the System.Net.Http integration and use the identity of the current
        // assembly as a more specific user agent, which can be useful when dealing with
        // providers that use the user agent as a way to throttle requests (e.g Reddit).
        options.UseSystemNetHttp()
            .SetProductInformation(typeof(Program).Assembly);

        // Add a client registration without a client identifier/secret attached.
        options.AddRegistration(new OpenIddictClientRegistration
        {
            Issuer = new Uri(globalConfig.StorefrontAuthUrl, UriKind.Absolute),
            ConfigurationEndpoint = new Uri(globalConfig.StorefrontAuthConfigurationUrl, UriKind.Absolute)
        });
    });

// This configuration is essential to host OpenIddict on cloud, behind an HTTPS proxy.
// Check it out https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer
//
// You may want to forward real ip of client to apply throttling policy, if so, you need to configure ForwardedHeaders
// flag like so, options.ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor;
services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
});

// Authentication
services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});
services.AddAuthorization();

services.AddControllers();

// Allow access from SPA
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = globalConfig.AllowOrigins.Split(',');
        if (origins.Length != 0)
        {
            policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
        }
    });
});

var app = builder.Build();
app.UseSimpleSwagger();

// Your app is deployed behind a HTTPS proxy, you need to configure forwarded headers.
// https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer
app.UseForwardedHeaders();

app.UseDeveloperExceptionPage();
app.UseStatusCodePagesWithReExecute("/error");

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();

_ = Task.Run(async () =>
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
});

app.Run();