namespace Template.Domain.Entities;

public class Bookmark : BaseEntity
{
	public User User { get; set; } = default!;

	public Content Content{ get; set; } = default!;

	public DateTime Created { get; set; } = DateTime.Now;
}
