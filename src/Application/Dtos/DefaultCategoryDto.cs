using Template.Application.Common.Mappings;
using Template.Domain.Entities;

namespace Template.Application.Dtos;

public class DefaultCategoryDto : IMapFrom<DefaultCategory>
{
	public Guid Id { get; init; }

	public string Name { get; init; } = default!;

	public string Description { get; init; } = default!;

	public ImageDto? Image { get; init; }

	public PartnerDto? Partner { get; init; }

	public DefaultCategoryParentDto? ParentCategory { get; init; } = default!;

	public List<DefaultCategoryChildDto>? ChildrenCategories { get; init; } = new List<DefaultCategoryChildDto>();

	public bool IsRoot { get; init; } = false;
}

