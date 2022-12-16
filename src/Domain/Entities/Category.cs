namespace Template.Domain.Entities;

public class Category : BaseEntity
{
	public string Name { get; set; } = default!;

	public string Description { get; set; } = default!;

	public Image? Image { get; set; }

	public Partner? Partner { get; set; }

	public bool IsRoot { get; set; } = false;

	public bool IsActive { get; set; } = true;

	public DefaultCategory? DefaultValue { get; set; }

	public Category? ParentCategory { get; set; }

	public ICollection<Category> ChildCategories { get; set; } = new List<Category>();

	public bool HasDefaults { get; set; } = false;

	public ICollection<Content> Content { get; set; } = new List<Content>();
}
