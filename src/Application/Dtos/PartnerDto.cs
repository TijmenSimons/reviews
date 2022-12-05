using Template.Application.Common.Mappings;
using Template.Domain.Entities;

namespace Template.Application.Dtos;

public class PartnerDto : IMapFrom<Partner>
{
	public Guid Id { get; init; }

	public string Name { get; init; } = default!;

	public string LogoPath { get; init; } = default!;

	public string? Url { get; init; }
}
