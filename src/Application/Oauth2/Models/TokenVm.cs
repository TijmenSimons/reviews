namespace Template.Application.Oauth2.Models;

// OAuth 2.0 token response specified in https://tools.ietf.org/html/rfc6749#section-5.1
public class TokenVm
{
	public string AccessToken { get; set; } = default!;
	public TokenType TokenType { get; set; } = default!;
	public int ExpiresIn { get; set; }
	public string RefreshToken { get; set; } = default!;
	public IList<string> Scope { get; set; } = new List<string>();
	public IList<string> Roles { get; set; } = new List<string>();
}
