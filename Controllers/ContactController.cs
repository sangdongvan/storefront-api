using Storefront.DbContext;
using Storefront.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

namespace Storefront.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[Route("api/[controller]/[action]")]
public class ContactController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public ContactController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType<FindContactsResponse>(200)]
    public async Task<ActionResult> Find([FromQuery] FindContactsRequest request)
    {
        await PopulateSampleData();

        var q = request.Q is null || request.Q.Trim().Length == 0
            ? null
            : request.Q.Trim();

        var queries = q is null
            ? _dbContext.Contacts
            : _dbContext.Contacts.Where(c => c.FirstName.Contains(q) || c.LastName.Contains(q));

        var contacts = await queries
            .OrderBy(c => c.FirstName)
            .ToListAsync();

        var res = new FindContactsResponse
        {
            Contacts = contacts.Select(c => new FindOneContactResponse
            {
                Id = c.Id,
                Avatar = c.Avatar,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Twitter = c.Twitter,
                Notes = c.Notes,
                Favorite = c.Favorite,
            }).ToList()
        };

        return Ok(res);
    }

    [HttpGet]
    [ProducesResponseType<FindOneContactResponse>(200)]
    public async Task<ActionResult> FindOne([FromQuery] FindOneContactRequest request)
    {
        await PopulateSampleData();

        var findResult = await _dbContext.Contacts
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync();

        if (findResult is null)
        {
            return NoContent();
        }

        var res = new FindOneContactResponse
        {
            Id = findResult.Id,
            Avatar = findResult.Avatar,
            FirstName = findResult.FirstName,
            LastName = findResult.LastName,
            Twitter = findResult.Twitter,
            Notes = findResult.Notes,
            Favorite = findResult.Favorite
        };

        return Ok(res);
    }

    [HttpPost]
    [ProducesResponseType<FindOneContactResponse>(200)]
    public async Task<ActionResult> CreateEmpty()
    {
        var newContactId = Guid.NewGuid().ToString();
        await _dbContext.Contacts.AddAsync(new Contact { Id = newContactId });
        await _dbContext.SaveChangesAsync();

        // Find back.
        var newContact = await _dbContext.Contacts
            .Where(c => c.Id == newContactId)
            .FirstOrDefaultAsync();

        if (newContact is null)
        {
            return NoContent();
        }

        var res = new FindOneContactResponse
        {
            Id = newContact.Id,
            Avatar = newContact.Avatar,
            FirstName = newContact.FirstName,
            LastName = newContact.LastName,
            Twitter = newContact.Twitter,
            Notes = newContact.Notes,
            Favorite = newContact.Favorite
        };

        return Ok(res);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<ActionResult> DeleteOne([FromBody] DeleteOneContactRequest request)
    {
        var success = await _dbContext.Contacts
            .Where(c => c.Id == request.Id)
            .ExecuteDeleteAsync();

        if (success == 0)
        {
            return NoContent();
        }

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType<FindOneContactResponse>(200)]
    public async Task<ActionResult> UpdateOne([FromBody] UpdateOneContactRequest request)
    {
        var findResult = await _dbContext.Contacts
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync();

        if (findResult is null)
        {
            return NoContent();
        }

        if (request.Avatar is not null)
        {
            findResult.Avatar = request.Avatar;
        }

        if (request.FirstName is not null)
        {
            findResult.FirstName = request.FirstName;
        }

        if (request.LastName is not null)
        {
            findResult.LastName = request.LastName;
        }

        if (request.Twitter is not null)
        {
            findResult.Twitter = request.Twitter;
        }

        if (request.Notes is not null)
        {
            findResult.Notes = request.Notes;
        }

        if (request.Favorite is not null)
        {
            findResult.Favorite = request.Favorite ?? false;
        }

        await _dbContext.SaveChangesAsync();

        // Find back.
        var updatedResult = await _dbContext.Contacts
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync();

        if (updatedResult is null)
        {
            return NoContent();
        }

        var res = new FindOneContactResponse
        {
            Id = updatedResult.Id,
            Avatar = updatedResult.Avatar,
            FirstName = updatedResult.FirstName,
            LastName = updatedResult.LastName,
            Twitter = updatedResult.Twitter,
            Notes = updatedResult.Notes,
            Favorite = updatedResult.Favorite
        };

        return Ok(res);
    }

    [HttpPost]
    [ProducesResponseType<FindOneContactResponse>(200)]
    public async Task<ActionResult> MarkAsFavorite([FromBody] MarkAsFavoriteContactRequest request)
    {
        var findResult = await _dbContext.Contacts
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync();

        if (findResult is null)
        {
            return NoContent();
        }

        findResult.Favorite = request.Favorite;

        await _dbContext.SaveChangesAsync();

        // Find back.
        var updatedResult = await _dbContext.Contacts
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync();

        if (updatedResult is null)
        {
            return NoContent();
        }

        var res = new FindOneContactResponse
        {
            Id = updatedResult.Id,
            Avatar = updatedResult.Avatar,
            FirstName = updatedResult.FirstName,
            LastName = updatedResult.LastName,
            Twitter = updatedResult.Twitter,
            Notes = updatedResult.Notes,
            Favorite = updatedResult.Favorite
        };

        return Ok(res);
    }

    private async Task PopulateSampleData()
    {
        if (await _dbContext.Contacts.AnyAsync())
        {
            return;
        }

        //Pre - populate with sample data
        await _dbContext.Contacts.AddRangeAsync(SampleData.Contacts());
        await _dbContext.SaveChangesAsync();
    }
}