namespace Template.Domain.Entities;

public class Video : BaseAuditableEntity
{
	public User Creator { get; set; } = default!;

	public string Extension { get; set; } = default!;
}
