namespace Template.Domain.Entities;
public class Partner : BaseEntity
{
	public string Name { get; set; } = default!;

	public string LogoPath { get; set; } = default!;

	public string? Url { get; set; }
}
