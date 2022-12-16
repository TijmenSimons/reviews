namespace Template.Domain.Entities;

public class Client
{
	public Guid Id { get; set; }
	public string Secret { get; set; } = default!;
	public string Name { get; set; } = default!;
}
