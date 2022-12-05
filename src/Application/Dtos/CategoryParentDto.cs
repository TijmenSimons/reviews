using AutoMapper;
using Template.Application.Common.Mappings;
using Template.Domain.Entities;

namespace Template.Application.Dtos;
public class CategoryParentDto : IMapFrom<Category>
{
	public Guid Id { get; init; }

	public string Name { get; init; } = default!;

	public string Description { get; init; } = default!;

	public ImageDto? Image { get; init; }

	public PartnerDto? Partner { get; init; }

	public CategoryParentDto? ParentCategory { get; init; } = default!;

	public bool IsRoot { get; init; }

	public bool IsActive { get; init; }

	public bool HasDefaults { get; init; }

	public void Mapping(Profile profile)
	{
		profile.CreateMap<Category, CategoryParentDto>()
			.ForMember(categoryDto => categoryDto.Name, member => member.MapFrom(category => category.DefaultValue == null ? category.Name : category.DefaultValue.Name))
			.ForMember(categoryDto => categoryDto.Description, member => member.MapFrom(category => category.DefaultValue == null ? category.Description : category.DefaultValue.Description))
			.ForMember(categoryDto => categoryDto.Image, member => member.MapFrom(category => category.DefaultValue == null ? category.Image : category.DefaultValue.Image))
			.ForMember(categoryDto => categoryDto.Partner, member => member.MapFrom(category => category.DefaultValue == null ? category.Partner : category.DefaultValue.Partner));
	}
}
