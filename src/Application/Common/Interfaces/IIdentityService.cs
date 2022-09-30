using Template.Application.Common.Models;

namespace Template.Application.Common.Interfaces;

public interface IIdentityService
{
	Task<string> GetUserNameAsync(string userId);

	Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

	Task<bool> CheckPasswordAsync(string username, string password);

	Task<bool> CheckRefreshTokenAsync(string username, string token);

	Task<IList<string>> GetRolesAsync(string username);

	Task<bool> IsInRoleAsync(string userId, string role);

	Task<bool> AuthorizeAsync(string userId, string policyName);

	Task<Result> DeleteUserAsync(string userId);
}
