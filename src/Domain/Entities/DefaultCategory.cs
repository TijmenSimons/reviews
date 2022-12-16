namespace Template.Domain.Entities;

public class DefaultCategory : BaseEntity
{
	public string Name { get; set; } = default!;

	public string Description { get; set; } = default!;

	public Image? Image { get; set; }

	public Partner? Partner { get; set; }

	public bool IsRoot { get; set; } = false;

	public DefaultCategory? ParentCategory { get; set; } = default!;

	public ICollection<DefaultCategory> ChildCategories { get; set; } = new List<DefaultCategory>();
}
