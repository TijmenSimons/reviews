namespace Template.Application.Common.Interfaces;

public interface ITokenService
{
	Task<string> CreateAccessTokenAsync(string username);
	Task<string> CreateRefreshTokenAsync(string username);
}
