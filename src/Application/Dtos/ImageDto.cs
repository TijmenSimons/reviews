using AutoMapper;
using Template.Application.Common.Mappings;
using Template.Domain.Entities;

namespace Template.Application.Dtos;

public class ImageDto : IMapFrom<Image>
{
	public Guid Id { get; init; }

	public string Url { get; init; } = default!;

	public void Mapping(Profile profile)
	{
		profile.CreateMap<Image, ImageDto>()
			.ForMember(imageDto => imageDto.Url, member => member.MapFrom(image => $"/images/{image.Id}{image.Extension}"));
	}
}
