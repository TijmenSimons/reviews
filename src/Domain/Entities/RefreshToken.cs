namespace Template.Domain.Entities;

public class RefreshToken
{
	public string Token { get; set; } = default!;
	public DateTime IssuedAt { get; set; }
	public User User { get; set; } = default!;
	//public Client Client { get; set; } = default!;
}
