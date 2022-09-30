using Microsoft.AspNetCore.Identity;

namespace Template.Domain.Entities;

public class User : IdentityUser
{
	public bool IsDeleted { get; set; } = false;
	public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
