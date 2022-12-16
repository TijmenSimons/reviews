using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
	private readonly ILogger<ApplicationDbContextInitialiser> _logger;
	private readonly ApplicationDbContext _context;
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
	{
		_logger = logger;
		_context = context;
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task InitialiseAsync()
	{
		try
		{
			if (_context.Database.IsNpgsql())
			{
				await _context.Database.MigrateAsync();
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while initialising the database.");
			throw;
		}
	}

	public async Task SeedAsync()
	{
		try
		{
			await TrySeedAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while seeding the database.");
			throw;
		}
	}

	public async Task TrySeedAsync()
	{
		// Default roles
		var administratorRole = new IdentityRole("Administrator");

		if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
		{
			await _roleManager.CreateAsync(administratorRole);
		}

		// Default users
		var administrator = new User { UserName = "administrator@localhost", Email = "administrator@localhost" };

		if (_userManager.Users.All(u => u.UserName != administrator.UserName))
		{
			await _userManager.CreateAsync(administrator, "Administrator1!");
			await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
		}

		if (!_context.Clients.Any())
		{
			_context.Clients.Add(new Client
			{
				Id = new Guid("902e3558-ca65-48aa-baec-bddcb0047117"),
				Secret = "Test",
				Name = "Web"
			});
			await _context.SaveChangesAsync();
		}
	}
}
