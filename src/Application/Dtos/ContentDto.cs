using Template.Application.Common.Mappings;
using Template.Domain.Entities;

namespace Template.Application.Dtos;

public class ContentDto : IMapFrom<Content>
{
	public Guid Id { get; init; }

	public string Title { get; set; } = default!;

	public string Description { get; set; } = default!;

	public ICollection<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

	public VideoDto? Video { get; set; }
}
