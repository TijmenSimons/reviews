namespace Template.Domain.Entities;

public class Content : BaseAuditableEntity
{
	public string Title { get; set; } = default!;

	public string Description { get; set; } = default!;

	public ICollection<Category> Categories { get; set; } = new List<Category>();

	public Video? Video { get; set; }

	public bool IsActive { get; set; } = true;
}
