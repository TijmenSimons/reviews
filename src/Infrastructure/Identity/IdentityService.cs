using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Domain.Entities;

namespace Template.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
	private readonly UserManager<User> _userManager;
	private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
	private readonly IAuthorizationService _authorizationService;
	private readonly IApplicationDbContext _context;

	public IdentityService(
		UserManager<User> userManager,
		IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
		IAuthorizationService authorizationService,
		IApplicationDbContext context)
	{
		_userManager = userManager;
		_userClaimsPrincipalFactory = userClaimsPrincipalFactory;
		_authorizationService = authorizationService;
		_context = context;
	}

	public async Task<string> GetUserNameAsync(string userId)
	{
		var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

		return user.UserName;
	}

	public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
	{
		var user = new User
		{
			UserName = userName,
			Email = userName,
		};

		var result = await _userManager.CreateAsync(user, password);

		return (result.ToApplicationResult(), user.Id);
	}

	public async Task<bool> CheckPasswordAsync(string username, string password)
	{
		var user = await _userManager.FindByNameAsync(username);

		return user is not null && await _userManager.CheckPasswordAsync(user, password);
	}

	public async Task<bool> CheckRefreshTokenAsync(string username, string token)
	{
		var refreshToken = await _context.RefreshTokens
			.Include(rt => rt.User)
			.FirstOrDefaultAsync(rt => rt.Token.Equals(token));

		return refreshToken is not null && refreshToken.User.NormalizedUserName.Equals(username.ToUpper());
	}

	public async Task<IList<string>> GetRolesAsync(string username)
	{
		var user = await _userManager.FindByNameAsync(username);

		return user is not null
			? await _userManager.GetRolesAsync(user)
			: new List<string>();
	}

	public async Task<bool> IsInRoleAsync(string userId, string role)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

		return user is not null && await _userManager.IsInRoleAsync(user, role);
	}

	public async Task<bool> AuthorizeAsync(string userId, string policyName)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

		if (user == null)
		{
			return false;
		}

		var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

		var result = await _authorizationService.AuthorizeAsync(principal, policyName);

		return result.Succeeded;
	}

	public async Task<Result> DeleteUserAsync(string userId)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

		return user is not null
			? await DeleteUserAsync(user)
			: Result.Success();
	}

	public async Task<Result> DeleteUserAsync(User user)
	{
		var result = await _userManager.DeleteAsync(user);

		return result.ToApplicationResult();
	}
}
