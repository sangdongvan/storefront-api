using System.Collections.Frozen;
using Storefront.DbContext;
using Storefront.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Client;

namespace Storefront.Controllers;

[Route("api/[controller]/[action]")]
public class AuthController(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    OpenIddictClientService openIddictClientService)
    : Controller
{
    [HttpPost]
    [ProducesResponseType<CreateUserResponse>(200)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var email = request.Email;
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var createUserResult = await userManager.CreateAsync(user, request.Password);

        if (!createUserResult.Succeeded)
        {
            if (TryClassifyCreateUserError(createUserResult, out var invalidEmailError, out var passwordTooWeakError))
            {
                if (invalidEmailError != null)
                {
                    return Problem(invalidEmailError, statusCode: 400);
                }

                if (passwordTooWeakError != null)
                {
                    return Problem(passwordTooWeakError, statusCode: 400);
                }
            }

            return Problem("Internal error");
        }

        var createdUser = await userManager.FindByEmailAsync(email);

        var authenticationRequest = new OpenIddictClientModels.PasswordAuthenticationRequest
        {
            Username = request.Email,
            Password = request.Password,
            Scopes = ["offline_access"]
        };
        var result = await openIddictClientService.AuthenticateWithPasswordAsync(authenticationRequest);

        return Ok(new CreateUserResponse
        {
            Id = createdUser.Id,
            Email = createdUser.Email,
            Token = new GetUserTokenResponse
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            }
        });
    }

    [HttpPost]
    [ProducesResponseType<GetUserTokenResponse>(200)]
    public async Task<IActionResult> GetUserToken([FromBody] GetUserTokenRequest request)
    {
        await PopulateSampleData();

        var authenticationRequest = new OpenIddictClientModels.PasswordAuthenticationRequest
        {
            Username = request.Email,
            Password = request.Password,
            Scopes = ["offline_access"]
        };
        try
        {
            var result = await openIddictClientService.AuthenticateWithPasswordAsync(authenticationRequest);
            return Ok(new GetUserTokenResponse
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken!
            });
        }
        catch (OpenIddictExceptions.ProtocolException ex)
        {
            if (ex.Error == "invalid_grant")
            {
                return Problem("Invalid credentials.", statusCode: 400);
            }

            return Problem(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType<GetUserTokenResponse>(200)]
    public async Task<IActionResult> RefreshUserToken([FromBody] RefreshUserTokenRequest request)
    {
        await PopulateSampleData();

        var authenticationRequest = new OpenIddictClientModels.RefreshTokenAuthenticationRequest
        {
            RefreshToken = request.RefreshToken,
        };
        var result = await openIddictClientService.AuthenticateWithRefreshTokenAsync(authenticationRequest);

        return Ok(new GetUserTokenResponse
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken
        });
    }

    private async Task PopulateSampleData()
    {
        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        var sampleUser = new ApplicationUser
        {
            UserName = "abc@domain.com",
            Email = "abc@domain.com"
        };
        await userManager.AddPasswordAsync(sampleUser, "abcABC@123");
        await userManager.CreateAsync(sampleUser);
    }

    private static bool TryClassifyCreateUserError(
        IdentityResult result,
        out string? invalidEmailError,
        out string? passwordTooWeakError)
    {
        foreach (var error in result.Errors)
        {
            // If password doesn't conform the rule, just simplify error message, and ignore all other errors.
            if (WeakPasswordMapping.ContainsKey(error.Code))
            {
                invalidEmailError = null;
                passwordTooWeakError = "Password too weak";
                return true;
            }

            if (DuplicatedUsernameMapping.ContainsKey(error.Code))
            {
                invalidEmailError = "Email duplicated";
                passwordTooWeakError = null;
                return true;
            }

            if (InvalidUsernameMapping.ContainsKey(error.Code))
            {
                invalidEmailError = "Invalid email";
                passwordTooWeakError = null;
                return true;
            }
        }

        invalidEmailError = null;
        passwordTooWeakError = null;
        return false;
    }

    private static readonly FrozenDictionary<string, bool> WeakPasswordMapping = new[]
    {
        KeyValuePair.Create<string, bool>(nameof(IdentityErrorDescriber.PasswordTooShort), true),
        KeyValuePair.Create<string, bool>(nameof(IdentityErrorDescriber.PasswordRequiresLower), true),
        KeyValuePair.Create<string, bool>(nameof(IdentityErrorDescriber.PasswordRequiresUpper), true),
        KeyValuePair.Create<string, bool>(nameof(IdentityErrorDescriber.PasswordRequiresDigit), true),
        KeyValuePair.Create<string, bool>(nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric), true),
        KeyValuePair.Create<string, bool>(nameof(IdentityErrorDescriber.PasswordRequiresUniqueChars), true)
    }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

    private static readonly FrozenDictionary<string, bool> DuplicatedUsernameMapping = new[]
    {
        KeyValuePair.Create<string, bool>(nameof(IdentityErrorDescriber.DuplicateUserName), true),
    }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

    private static readonly FrozenDictionary<string, bool> InvalidUsernameMapping = new[]
    {
        KeyValuePair.Create<string, bool>(nameof(IdentityErrorDescriber.InvalidUserName), true)
    }.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
}

public class CreateUserRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}