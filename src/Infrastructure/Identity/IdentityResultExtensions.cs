using Microsoft.AspNetCore.Identity;
using Template.Application.Common.Models;

namespace Template.Infrastructure.Identity;

public static class IdentityResultExtensions
{
	public static Result ToApplicationResult(this IdentityResult result)
	{
		return result.Succeeded
			? Result.Success()
			: Result.Failure(result.Errors.Select(e => e.Description));
	}
}
